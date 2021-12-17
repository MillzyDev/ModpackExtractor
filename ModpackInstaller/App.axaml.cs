using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ModpackExtractor.ViewModels;
using ModpackExtractor.Views;

namespace ModpackExtractor
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                    Width = 800,
                    Height = 450,
                    CanResize = false
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
