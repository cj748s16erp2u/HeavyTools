using eLog.HeavyTools.Common.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{ 
    [Common.Json.JsonObjectAttributes("")]
    public class CalcJsonResultDto : ResultDto
    {
        [JsonFieldAttribute(true)]
        public string Curid { get; set; } = null;
        [JsonFieldAttribute(true)]
        public List<CalcItemJsonResultDto> Items2 { get; set; } = new List<CalcItemJsonResultDto>();
        [JsonFieldAttribute(true)]
        public bool FreeShipping { get; set; } = false;
        [JsonFieldAttribute(true)]
        public bool FreeFee { get; set; } = false;
    
    }

    public class CalcItemJsonResultDto
    {
        [JsonFieldAttribute(true)]
        public int? CartId { get; set; } = null;
        
        [JsonFieldAttribute(true)]
        public int? Itemid { get; set; } = null;

        [JsonFieldAttribute(true)]
        public string ItemCode { get; set; } = null;
        [JsonFieldAttribute(true)]
        public int? Quantity { get; set; } = null;
        [JsonFieldAttribute(true)]
        /// <summary>
        /// Eredeti nettó egységár
        /// </summary>
        public decimal? OrignalSelPrc { get; set; } = null;

        [JsonFieldAttribute(true)]
        /// <summary>
        /// Eredeti bruttó egységár
        /// </summary>
        public decimal? OrignalGrossPrc { get; set; } = null;
        [JsonFieldAttribute(true)]
        /// <summary>
        /// Eredeti bruttó érték
        /// </summary>
        public decimal? OrignalTotVal { get; set; } = null;
        [JsonFieldAttribute(true)]
        /// <summary>
        /// Nettó egységár
        /// </summary>
        public decimal? SelPrc { get; set; } = null;
        [JsonFieldAttribute(true)]
        /// <summary>
        /// Bruttó egységár
        /// </summary>
        public decimal? GrossPrc { get; set; } = null;

        [JsonFieldAttribute(true)]
        /// <summary>
        /// Nettó érték  (Nettó egységár * Mennyiség)
        /// </summary>
        public decimal? NetVal { get; set; } = null;

        [JsonFieldAttribute(true)]
        /// <summary>
        /// Áfa érték
        /// </summary>
        public decimal? TaxVal { get; set; } = null;
        
        [JsonFieldAttribute(true)]
        /// <summary>
        /// Bruttó érték
        /// </summary>
        public decimal? TotVal { get; set; } = null;



        [JsonFieldAttribute(true)]
        /// <summary>
        /// Kedvezmény az.
        /// </summary>
        public int? Aid { get; set; } = null;

        [JsonFieldAttribute(true)]
        /// <summary>
        /// Kedvezmény név
        /// </summary>
        public string AidName { get; set; } = null;


    }


}
