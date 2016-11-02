using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryAgent
{
    class Controller
    {
        Model myModel = new Model();

        public void ListSoftware(ListBox listbox)
        {
            string query = "Select * from Win32_Product";

            ManagementObjectCollection moReturn;

            moReturn = myModel.GetWmiList(query);

            listbox.Items.Clear();

            foreach (ManagementObject mo in moReturn)
            {
                try
                {
                    listbox.Items.Add(mo["Name"].ToString());
                }
                catch (Exception)
                {
                    continue;
                }
            }

        }

        public void ListPrinters(ListBox listBox)
        {
            string query = "Select * From Win32_Printer";

            ManagementObjectCollection moReturn;

            moReturn = myModel.GetWmiList(query);

            listBox.Items.Clear();

            foreach (ManagementObject mo in moReturn)
            {
                try
                {
                    listBox.Items.Add(mo["Name"].ToString());
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
