using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Import
{
    public class ItemImportMessageException : MessageException
    { 

        public ItemImportMessageException(string msg) :base(msg)
        {

        }
    }
}
