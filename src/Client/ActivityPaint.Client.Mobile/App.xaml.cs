namespace ActivityPaint.Client.Mobile;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        if (window is not null)
        {
            window.Title = "Activity Paint";
        }

        return window!;
    }
}
