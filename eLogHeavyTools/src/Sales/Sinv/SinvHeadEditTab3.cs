using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.coda.eassets.schemas.asset;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Sales.Sinv
{
    public class SinvHeadEditTab3 : CodaInt.Base.Sales.Sinv.SinvHeadEditTab2
    {
        protected List<Control> m_ctrlCustomGroupSeparators;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.m_ctrlCustomGroupSeparators = this.EditGroup1.ControlArray.Where(c => c.CustomData != null && c.DataField.StartsWith("gscustom")).GroupBy(c => c.DataField).Select(g => g.FirstOrDefault()).ToList();
            this.m_ctrlCustomGroupSeparators.ForEach(c => c.Visible = false);
        }

        protected override void OnPartnerChanged(PageUpdateArgs args)
        {
            base.OnPartnerChanged(args);

            int? partnId = ConvertUtils.ToInt32(m_partncode?.Value);
            SetCustomGroupSeparatorsVisibility(args, partnId);
        }

        protected void SetCustomGroupSeparatorsVisibility(PageUpdateArgs args, int? partnId)
        {
            bool visible = SalesSqlFunctions.IsSinvCustomPartner(partnId);
            m_ctrlCustomGroupSeparators.ForEach(delegate (Control c)
            {
                c.Visible = visible;
            });
        }


    }
}
