using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    partial class OlcAction
    {
        [NotMapped]
        public ActionType ActionTypeEnum
        {
            get { return (ActionType)this.Actiontype; }
            set { this.Actiontype = (int)value; }
        }

        [NotMapped]
        public Discounttype? DiscounttypeEnum
        {
            get {
                return (Discounttype?)this.Discounttype ?? null;
            }
            set { 
                this.Discounttype = (int?)(value??null); }
        }

        [NotMapped]
        public Discountcalculationtype? DiscountcalculationtypeEnum
        {
            get { return (Discountcalculationtype?)this.Discountcalculationtype; }
            set { this.Discountcalculationtype = (int?)value; }
        }

        [NotMapped]
        public Purchasetype PurchasetypeEnum
        {
            get { return (Purchasetype)this.Purchasetype; }
            set { this.Purchasetype = (int)value; }
        }



        [NotMapped]
        public FilterCustomersType? FilterCustomersTypeEnum
        {
            get { return (FilterCustomersType?)this.Filtercustomerstype; }
            set { this.Filtercustomerstype = (int?)value; }
        }
         
    }
}
