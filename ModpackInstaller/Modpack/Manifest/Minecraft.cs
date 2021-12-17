using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackCreator.Modpack
{
    public class Minecraft
    {
        public string version { get; set; } = "";
        public Modloader[] modLoaders { get; set; } = new Modloader[1];
    }
}
