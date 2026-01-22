using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using osFotoFix.Services;
using osFotoFix.Models;

namespace osFotoFix.Services.Tests
{
  public class FotoInfoServiceTest : IDisposable
  {
    private readonly string _testPath;
    private readonly FotoInfoService _service;

    public FotoInfoServiceTest()
    {
      _testPath = Path.Combine(Path.GetTempPath(), $"FotoInfoServiceTest_{Guid.NewGuid()}");
      Directory.CreateDirectory(_testPath);
      _service = new FotoInfoService();
    }

    [Fact]
    public async Task ReadFotoInfos_NonExistentDirectory_ReturnsTrue()
    {
      var dir = new DirectoryInfo(Path.Combine(_testPath, "nonexistent"));
      var result = await _service.ReadFotoInfos(dir, CancellationToken.None);
      Assert.True(result);
    }

    [Fact]
    public async Task ReadFotoInfos_HiddenDirectory_ReturnsTrue()
    {
      var hiddenDir = new DirectoryInfo(Path.Combine(_testPath, "hidden"));
      hiddenDir.Create();
      hiddenDir.Attributes |= FileAttributes.Hidden;

      var result = await _service.ReadFotoInfos(hiddenDir, CancellationToken.None);
      Assert.True(result);

      hiddenDir.Attributes &= ~FileAttributes.Hidden;
    }

    [Fact]
    public async Task ReadFotoInfos_CancellationRequested_ReturnsFalse()
    {
      var dir = new DirectoryInfo(_testPath);
      var cts = new CancellationTokenSource();
      cts.Cancel();

      var result = await _service.ReadFotoInfos(dir, cts.Token);
      Assert.False(result);
    }

    [Fact]
    public async Task ReadFotoInfos_WithValidJpgFile_RaisesEvent()
    {
      var dir = new DirectoryInfo(_testPath);
      var jpgFile = Path.Combine(_testPath, "test.jpg");
      File.WriteAllText(jpgFile, "test");

      FotoInfoEventArgs receivedArgs = null;
      _service.FotoInfoReadEvent += (sender, args) => receivedArgs = args;

      await _service.ReadFotoInfos(dir, CancellationToken.None);

      Assert.NotNull(receivedArgs);
      Assert.NotNull(receivedArgs.FotoInfo);
    }

    [Fact]
    public async Task ReadFotoInfos_WithInvalidExtension_SkipsFile()
    {
      var dir = new DirectoryInfo(_testPath);
      var txtFile = Path.Combine(_testPath, "test.txt");
      File.WriteAllText(txtFile, "test");

      FotoInfoEventArgs receivedArgs = null;
      _service.FotoInfoReadEvent += (sender, args) => receivedArgs = args;

      await _service.ReadFotoInfos(dir, CancellationToken.None);

      Assert.Null(receivedArgs);
    }

    [Fact]
    public async Task ReadFotoInfos_RecursiveDirectories_ReturnsTrue()
    {
      var subDir = Directory.CreateDirectory(Path.Combine(_testPath, "subdir"));
      var jpgFile = Path.Combine(subDir.FullName, "test.jpg");
      File.WriteAllText(jpgFile, "test");

      var result = await _service.ReadFotoInfos(new DirectoryInfo(_testPath), CancellationToken.None);

      Assert.True(result);
    }

    [Fact]
    public async Task FotoFixItAsync_WithValidFiles_CopiesFilesSuccessfully()
    {
      var sourceFile = Path.Combine(_testPath, "test.jpg");
      File.WriteAllText(sourceFile, "test content");
      
      var targetDir = Path.Combine(_testPath, "target");
      Directory.CreateDirectory(targetDir);
      
      var fotoInfo = new FotoInfo(
        new FileInfo(sourceFile),
        DateTime.Now,
        ETypeOfCreationDate.Filesystem)
      {
        TargetPath = targetDir,
        NewFileName = Path.Combine(targetDir, "test_copy.jpg")
      };
      
      await _service.FotoFixItAsync(new[] { fotoInfo }, null, CancellationToken.None);
      
      Assert.True(File.Exists(fotoInfo.NewFileName));
    }

    [Fact]
    public async Task FotoFixItAsync_WithMultipleFiles_ProcessesAll()
    {
      var file1 = Path.Combine(_testPath, "test1.jpg");
      var file2 = Path.Combine(_testPath, "test2.jpg");
      File.WriteAllText(file1, "content1");
      File.WriteAllText(file2, "content2");
      
      var targetDir = Path.Combine(_testPath, "target");
      Directory.CreateDirectory(targetDir);
      
      var fotoInfos = new[]
      {
        new FotoInfo(new FileInfo(file1), DateTime.Now, ETypeOfCreationDate.Filesystem)
        {
          TargetPath = targetDir,
          NewFileName = Path.Combine(targetDir, "file1.jpg")
        },
        new FotoInfo(new FileInfo(file2), DateTime.Now, ETypeOfCreationDate.Filesystem)
        {
          TargetPath = targetDir,
          NewFileName = Path.Combine(targetDir, "file2.jpg")
        }
      };
      
      await _service.FotoFixItAsync(fotoInfos, null, CancellationToken.None);
      
      Assert.True(File.Exists(fotoInfos[0].NewFileName));
      Assert.True(File.Exists(fotoInfos[1].NewFileName));
    }

    [Fact]
    public async Task FotoFixItAsync_CancellationRequested_ThrowsOperationCanceledException()
    {
      var sourceFile = Path.Combine(_testPath, "test.jpg");
      File.WriteAllText(sourceFile, new string('x', 10000));
      
      var targetDir = Path.Combine(_testPath, "target");
      Directory.CreateDirectory(targetDir);
      
      var fotoInfo = new FotoInfo(new FileInfo(sourceFile), DateTime.Now, ETypeOfCreationDate.Filesystem)
      {
        TargetPath = targetDir,
        NewFileName = Path.Combine(targetDir, "test.jpg")
      };
      
      var cts = new CancellationTokenSource();
      cts.Cancel();
      
      await Assert.ThrowsAsync<OperationCanceledException>(
        () => _service.FotoFixItAsync(new[] { fotoInfo }, null, cts.Token));
    }

    [Fact]
    public async Task FotoFixItAsync_WithProgress_ReportsProgress()
    {
      var sourceFile = Path.Combine(_testPath, "test.jpg");
      File.WriteAllText(sourceFile, new string('x', 1000));
      
      var targetDir = Path.Combine(_testPath, "target");
      Directory.CreateDirectory(targetDir);
      
      var fotoInfo = new FotoInfo(new FileInfo(sourceFile), DateTime.Now, ETypeOfCreationDate.Filesystem)
      {
        TargetPath = targetDir,
        NewFileName = Path.Combine(targetDir, "test.jpg")
      };
      
      var progressValues = new List<double>();
      var progress = new Progress<double>(p => progressValues.Add(p));
      
      await _service.FotoFixItAsync(new[] { fotoInfo }, progress, CancellationToken.None);
      
      Assert.NotEmpty(progressValues);
      Assert.True(progressValues.Last() > 0);
    }
    public void Dispose()
    {
      try
      {
        if (Directory.Exists(_testPath))
          Directory.Delete(_testPath, true);
      }
      catch { }
    }
  }
}

