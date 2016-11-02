using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAgent
{
    class Model
    {
        public ManagementObjectCollection GetWmiList(string wmiQuery)
        {
            ManagementObjectSearcher moSearch;

            moSearch = new ManagementObjectSearcher(wmiQuery);

            moSearch.Options.ReturnImmediately = true;

            return moSearch.Get();

        }
    }
}
