using ModpackExtractor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ModpackExtractor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ModItem> Items { get; set; } = new ObservableCollection<ModItem>(new List<ModItem>());

        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
        public string Author { get; set; } = "";
        public string MinecraftVersion { get; set; } = "";
        public string Modloader { get; set; } = "";
    }
}
