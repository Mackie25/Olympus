using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympusExample.Workers
{
    public interface INetworkLocations
    {
        void CreateFolderStructure();

        //bool CreateDirectory(string directory);

        //bool CreateFile(string file);
    }
}
