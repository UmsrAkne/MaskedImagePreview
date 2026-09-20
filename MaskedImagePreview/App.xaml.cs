using System.Windows;
using MaskedImagePreview.Utils;
using MaskedImagePreview.Views;

namespace MaskedImagePreview;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        var appSettings = AppSettings.Load();
        containerRegistry.RegisterInstance(appSettings);
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }
}