using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ForgedCurse;
using ForgedCurse.Json;
using ModpackCreator.Modpack;
using ModpackExtractor.Models;
using ModpackExtractor.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModpackExtractor.Views
{
    public partial class MainWindow : Window
    {
        private Button installButton;
        private Button extractButton;

        public MainWindow()
        {
            OpenFileDialog openModpack = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Open modpack...",
                Filters =
                {
                    new FileDialogFilter
                    {
                        Extensions = { "zip" },
                        Name = "Zip Archive"
                    }
                },
            };
            string[] modpackPath = openModpack.ShowAsync(this).GetAwaiter().GetResult();
            if (modpackPath == null || modpackPath == Array.Empty<string>())
            {
                this.Close();
                return;
            }

            InitializeComponent();

            extractButton = this.FindControl<Button>("extractButton");

            var tempDir = Path.Combine(Path.GetTempPath(), @"ModpackExtractor");
            Directory.CreateDirectory(tempDir);

            ZipFile.ExtractToDirectory(modpackPath[0], tempDir, true);

            string manifest = File.ReadAllText(Path.Combine(tempDir, @"manifest.json"));
            Manifest.Instance = JsonSerializer.Deserialize<Manifest>(manifest);

            LoadModpack(Manifest.Instance);
            //extractButton.IsEnabled = true;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task LoadModpack(Manifest manifest)
        {
            DataContext = new MainWindowViewModel
            {
                Name = manifest.name,
                Version = manifest.version,
                Author = manifest.author,
                MinecraftVersion = manifest.minecraft.version,
                Modloader = manifest.minecraft.modLoaders[0].id
            };
            
            var client = new ForgeClient();
            
            foreach (Mod mod in manifest.files)
            {
                Release modRelease = await client.Files.RetrieveRelease(mod.projectID, mod.fileID);
                Addon addon = await client.Addons.RetriveAddon(mod.projectID);

                List<string> authorList = new List<string>();
                foreach (Author author in addon.Authors)
                {
                    authorList.Add(author.Name);
                }

                var viewModel = (MainWindowViewModel)DataContext;
                viewModel.Items.Add(new ModItem
                {
                    ModName = addon.Name,
                    ModAuthors = string.Join(", ", authorList),
                    ModUrl = addon.Website,
                    DownloadURL = modRelease.DownloadUrl
                });
            }

            extractButton.IsEnabled = true;
        }

        public void OpenWebsite(object sender, RoutedEventArgs e)
        {
            string url = ((Button)e.Source).Name;
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
           
        }

        public async void ExtractMods(object sender, RoutedEventArgs e)
        {
            Button src = (Button)e.Source;
            src.IsEnabled = false;

            OpenFolderDialog folderDialog = new OpenFolderDialog
            {
                Title = "Extract Mods",

            };
            string dirPath = await folderDialog.ShowAsync(this);
            if (dirPath == null || dirPath == "")
            {
                src.IsEnabled = true;
                return;
            }

            var dataContext = (MainWindowViewModel?)DataContext;
            foreach (ModItem item in dataContext.Items)
            {
                using WebClient webClient = new WebClient();
                webClient.DownloadFile(item.DownloadURL, Path.Combine(dirPath, Path.GetFileName(item.DownloadURL)));
            }
            src.IsEnabled = true;
        }

        public async void InstallModpack(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog folderDialog = new OpenFolderDialog
            {
                Title = "Select Minecraft instance folder",
                DefaultDirectory = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), @".minecraft")
            };
            string minecraftDir = await folderDialog.ShowAsync(this);
            if (minecraftDir == string.Empty || minecraftDir == null)
                return;
        }
    }
}
