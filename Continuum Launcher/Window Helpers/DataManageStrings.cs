using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class DataManageStrings
    {
        public List<string> dataStringList, dataSizeList, dataIdList;
        public DataManageStrings()
        {
            dataStringList = new List<string>();
            dataSizeList = new List<string>();
            dataIdList = new List<string>();
        }
        public void Clear()
        {
            dataStringList.Clear();
            dataSizeList.Clear();
            dataIdList.Clear();
        }
    }
}
