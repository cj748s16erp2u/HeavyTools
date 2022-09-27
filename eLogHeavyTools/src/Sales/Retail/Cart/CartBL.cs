using eLog.HeavyTools.InterfaceCaller;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eProjectWeb.Framework.Data.Key;

namespace eLog.HeavyTools.Sales.Retail.Cart
{
    internal class CartBL
    {
        private CartData CartData = null;
        private readonly Numberbox originalValue;
        private readonly Numberbox discValue;
        private readonly Numberbox totValue;
        private readonly Numberbox payValue;
        private readonly Numberbox missingValue;

        public CartBL(Numberbox originalValue, Numberbox discValue, Numberbox totValue, Numberbox payValue, Numberbox missingValue)
        {
            LoadFromDB();
            this.originalValue = originalValue;
            this.discValue = discValue;
            this.totValue = totValue;
            this.payValue = payValue;
            this.missingValue = missingValue;
        }

        private void LoadFromDB()
        {
            CartData = new CartData
            {
                Wid = "com",
                FirstPurchase = false,
                Curid = "EUR",
                CountryId = "HU"
            };

            var carts = OlcCarts.Load(new Key() { { OlcCart.FieldAddusrid.Name, Session.Current.UserID } });
            foreach (OlcCart c in carts.AllRows)
            {
                if (c.Itemid.HasValue)
                {
                    CartData.Items.Add(c);
                }
                if (!string.IsNullOrEmpty(c.LoyaltyCardNo))
                {
                    CartData.LoyaltyCardNo = c.LoyaltyCardNo;
                }
                if (!string.IsNullOrEmpty(c.Cupon))
                {
                    CartData.Cupons.Add(c.Cupon);
                }
            }
        }

        internal void AddNewItem(int itemid)
        {
            var c = OlcCart.CreateNew();
            c.Itemid = itemid;
            c.IsHandPrice = 0;
            c.Save();
            CartData.Items.Add(c);
            Recalc();
        }

        private void FillData(OlcCart c, CalcItemJsonResultDto item)
        {
            c.Itemid = item.Itemid;
            c.IsHandPrice = 0;
            c.OrignalSelPrc = item.OrignalSelPrc;
            c.OrignalGrossPrc = item.OrignalGrossPrc;
            c.OrignalTotVal = item.OrignalTotVal;
            c.SelPrc = item.SelPrc;
            c.GrossPrc = item.GrossPrc;
            c.NetVal = item.NetVal;
            c.TaxVal = item.TaxVal;
            c.TotVal = item.TotVal;
            c.Aid = item.Aid;
        }

        internal void Recalc()
        {
            var d = new InterfaceCallerBL().CartCalucate(CartData);
            foreach (var item in d.Items2)
            {
                if (item.CartId.HasValue)
                {
                    var cc = OlcCart.Load(item.CartId);
                    FillData(cc, item);
                    cc.Save();
                }
                else
                {
                    var cc = OlcCart.CreateNew();
                    cc.Itemid = item.Itemid;
                    cc.IsHandPrice = 0;
                    cc.Save();
                    FillData(cc, item);
                    cc.Save();
                }
                if (item.Quantity > 1)
                {
                    for (int i = 1; i < item.Quantity; i++)
                    {
                        var cc = OlcCart.CreateNew();
                        cc.Itemid = item.Itemid;
                        cc.IsHandPrice = 0;
                        cc.Save();
                        FillData(cc, item);
                        cc.Save();
                    }
                }
            }
            FillTotal();
        }

        public void FillTotal()
        {
            var ct = GetCartTotal();
            originalValue.Value = ct.GetOriginalValue();
            discValue.Value = ct.GetDiscValue();
            totValue.Value = ct.GetTotValue();
            payValue.Value = ct.GetPayValue();
            missingValue.Value = ct.GetMissingValue();
        }

        public CartTotal GetCartTotal()
        {
            return new CartTotal(Session.Current.UserID, CartData.Curid);
        }

        internal void AddCupon(string cupon)
        {
            var cc = OlcCart.CreateNew();
            cc.Cupon = cupon;
            cc.IsHandPrice = 0;
            cc.Save();
            LoadFromDB();
            Recalc();
        }

        internal void SetLoyaltyCardNo(string loyaltycardno)
        {
            var cc = OlcCart.Load(
                new Key() {
                    { OlcCart.FieldAddusrid.Name, Session.Current.UserID },
                    {OlcCart.FieldLoyaltyCardNo.Name, new NotNullAtToSql() }
                });
            if (cc== null)
            {
                cc = OlcCart.CreateNew();
            }
            cc.LoyaltyCardNo = loyaltycardno;
            cc.Save();
            LoadFromDB();
            Recalc();
        }
    }
    public class CartData
    {
        public List<string> Cupons { get; set; } = new List<string>();
        public string Wid { get; internal set; }
        public bool FirstPurchase { get; internal set; }
        public string Curid { get; internal set; }
        public string LoyaltyCardNo { get; internal set; }
        public string CountryId { get; internal set; }
        public List<OlcCart> Items { get; set; } = new List<OlcCart>();
    } 
}
