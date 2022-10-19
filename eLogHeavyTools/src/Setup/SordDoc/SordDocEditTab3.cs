using eLog.Base.Setup.SordDoc;
using eLog.Base.Setup.StDoc;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.SordDoc
{
    internal class SordDocEditTab3 : SordDocEditTab
    {
        protected eProjectWeb.Framework.UI.PageParts.LayoutTable EditGroupCustom;

        public SordDocEditTab3()
        {
        }

        Combo type;
        Combo frameordersorddocid;


        protected override void CreateBase()
        {
            base.CreateBase();

            EditGroupCustom = (eProjectWeb.Framework.UI.PageParts.LayoutTable)this["EditGroupCustom"];


            type = FindRenderable<Combo>("type");
            type.SetOnChanged(OnTypeChanged);
            frameordersorddocid = FindRenderable<Combo>("frameordersorddocid");
        }

        private void OnTypeChanged(PageUpdateArgs args)
        {
            var s = false;

            var t = ConvertUtils.ToInt32(type.Value);

            if (t.HasValue && t.Value==2)
            {
                s = true;
            }
            frameordersorddocid.Visible = s;
        }

        protected override Base.Setup.SordDoc.SordDoc DefaultPageLoad_LoadEntity(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad_LoadEntity(args);

            var c = OlcSordDoc.Load(e.PK);
            if (c != null)
            {
                if (EditGroupCustom != null)
                    EditGroupCustom.DataBind(c, false);
            }
            return e;
        }



        protected override eProjectWeb.Framework.BL.BLObjectMap SaveControlsToBLObjectMap(eProjectWeb.Framework.PageUpdateArgs args, Base.Setup.SordDoc.SordDoc e)
        {
            eProjectWeb.Framework.BL.BLObjectMap map = base.SaveControlsToBLObjectMap(args, e);
             

            var c = OlcSordDoc.Load(e.PK);

            if (c == null)
            {
                c = OlcSordDoc.CreateNew();
                c.SetKey(e.PK);
            }

            if (EditGroupCustom != null)
                EditGroupCustom.DataBind(c, true);

            map.Add(c);

            return map;
        }
    }
}
