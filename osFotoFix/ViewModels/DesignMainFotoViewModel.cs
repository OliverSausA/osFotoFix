using System;
using System.IO;
namespace osFotoFix.ViewModels;

using Avalonia.Media;
using osFotoFix.Models;
using osFotoFix.Services;

public class DesignMainFotoViewModel : MainFotoViewModel
{
  public DesignMainFotoViewModel() : base( new FotoInfoService() )
  {
    SourcePath = "Source/Path/To/Fotos";

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

  protected override void CreateMenuItems()
  {
    MainMenuItems.Add(new MainMenuItemVM() { 
      Title="Save", IconName="Save", IconColor=Colors.Green.ToString() } );
    MainMenuItems.Add(new MainMenuItemVM() { 
      Title="Trash", IconName="Delete", IconColor=Colors.Yellow.ToString() } );
  }
}