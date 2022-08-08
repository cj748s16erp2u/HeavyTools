using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers.CSVParser
{
    internal class CSVAttribute : Attribute
    {
        public int index;

        public CSVAttribute(int index)
        {
            this.index = index;
        }
    }
}