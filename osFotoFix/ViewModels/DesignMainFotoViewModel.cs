using System;
using System.IO;
namespace osFotoFix.ViewModels;

using osFotoFix.Models;

public class DesignMainFotoViewModel : MainFotoViewModel
{
  public DesignMainFotoViewModel()
  {
    SourcePath = "Source/Path/To/Fotos";
    MainMenuItems.Add(new MainMenuItemVM() { Title="Save", IconName="Save" } );
    MainMenuItems.Add(new MainMenuItemVM() { Title="Trash", IconName="Delete" } );

    FileInfo fileInfo = new FileInfo("Test.jpg");
    var fotoInfo = new FotoInfo(fileInfo, DateTime.Now, ETypeOfCreationDate.Filesystem);
    var FotoInfoViewModel = new FotoInfoViewModel(fotoInfo);
    FotoInfoList.Add( FotoInfoViewModel);
    FotoSelected = FotoInfoViewModel;

    fileInfo = new FileInfo("Test2.jpg");
    fotoInfo = new FotoInfo(fileInfo, DateTime.Now, ETypeOfCreationDate.Filesystem);
    FotoInfoViewModel = new FotoInfoViewModel(fotoInfo);
    FotoInfoList.Add( FotoInfoViewModel);
  }
}