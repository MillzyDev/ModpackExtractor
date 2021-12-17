using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackCreator.Modpack
{
    public class Mod
    {
        public int projectID { get; set; } = 0;
        public int fileID { get; set; } = 0;
        public bool required { get; set; } = true;
    }
}
