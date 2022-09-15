using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    partial class OlcActionext
    {
        [NotMapped]
        public Discounttype? DiscounttypeEnum
        {
            get
            {
                return (Discounttype?)this.Discounttype ?? null;
            }
            set
            {
                this.Discounttype = (int?)(value ?? null);
            }
        }


        [NotMapped]
        public Discountcalculationtype? DiscountcalculationtypeEnum
        {
            get
            {
                return (Discountcalculationtype?)this.Discountcalculationtype ?? null;
            }
            set
            {
                this.Discountcalculationtype = (int?)(value ?? null);
            }
        }
         
    }
}
