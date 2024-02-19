namespace osFotoFix.Tests.ServicesTests
{
  using osFotoFix.Services;

  public class UserSettingsServiceTests
  {
    [Fact]
    public void GetServiceInstance()
    {
      var service = UserSettingsService.GetInstance;
      
    }
  }

};