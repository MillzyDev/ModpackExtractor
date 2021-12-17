using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackCreator.Modpack
{
    public class Manifest
    {
        private static Manifest _instance = null;
        public static Manifest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Manifest();
                }
                return _instance;
            }
            set 
            {
                _instance = value;
            }
        }

        public Minecraft minecraft { get; set; } = new Minecraft();
        public string manifestType { get; set; } = "minecraftModpack";
        public int manifestVersion { get; set; } = 1;
        public string name { get; set; } = "Modpack Name";
        public string version { get; set; } = "";
        public string author { get; set; } = "";
        public Mod[] files { get; set; } = Array.Empty<Mod>();
        public string overrides { get; set; } = "overrides";
    }
}
