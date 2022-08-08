using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options
{
    public class SordOptions
    {
        public const string NAME = "Sord";
        public string? SordDocId { get; set; }
        public int? Cmpid { get; set; }
        public int? SordType { get; set; }
        public int? PartnId { get; set; }
        public int? AddrId { get; set; }
        public int? Gen { get; set; }
        public string? AddUsrId { get; set; }
    }
}