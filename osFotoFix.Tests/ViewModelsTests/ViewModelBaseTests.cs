using osFotoFix.ViewModels;

namespace osFotoFix.Tests.ViewModelsTests;

public class ViewModelBaseTests
{
    [Fact]
    public async Task InitializeAsync_InitializesOnlyOnce()
    {
        var viewModel = new TestViewModel();

        var firstInitialization = viewModel.InitializeAsync();
        var secondInitialization = viewModel.InitializeAsync();

        Assert.Same(firstInitialization, secondInitialization);
        await firstInitialization;
        Assert.Equal(1, viewModel.InitializationCount);
    }

    private sealed class TestViewModel : ViewModelBase
    {
        public int InitializationCount { get; private set; }

        protected override Task OnInitializedAsync()
        {
            InitializationCount++;
            return Task.CompletedTask;
        }
    }
}
