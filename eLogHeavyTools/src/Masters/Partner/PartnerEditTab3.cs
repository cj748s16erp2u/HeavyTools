using System;
using System.Collections.Generic;
using System.Linq;
using CodaInt.Base.Masters.Partner;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnerEditTab3 : PartnerEditTab2, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(PartnerEditTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        protected virtual IEnumerable<Control> OlcControls => this.EditGroup1?.ControlArray
            .Where(c => c.CustomData == "olc");

        public override string XmlObjectName => baseType.Name;
        #endregion

        protected Control ctrlDebcredsumvalue;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrlDebcredsumvalue = this.EditGroup1["debcredsumvalue"];
        }

        protected override eLog.Base.Masters.Partner.Partner DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID != ActionID.New)
            {
                var olc = OlcPartner.Load(e.Partnid);
                if (olc != null)
                {
                    foreach (var c in this.OlcControls)
                    {
                        c.DataBind(olc, false);
                    }
                }
            }

            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, eLog.Base.Masters.Partner.Partner e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Partnid.HasValue ? OlcPartner.Load(e.Partnid) : null) ?? OlcPartner.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            map.Add(olc);

            OlcPartnCmps partnCmps = SaveOlcCmpValuesToData(e.PK);
            map.Add(partnCmps.GetType().Name, partnCmps);

            return map;
        }

        protected override void LoadCmpValues(Key partnerKey)
        {
            int? partnId = null;
            if (partnerKey != null)
                partnId = ConvertUtils.ToInt32(partnerKey[eLog.Base.Masters.Partner.Partner.FieldPartnid.Name]);

            PartnCmps pcmps = (partnId.HasValue ? PartnCmps.LoadWithSessionCompanies(partnId.Value) : PartnCmps.New());
            OlcPartnCmps opcmps = (partnId.HasValue ? OlcPartnCmps.LoadWithSessionCompanies(partnId.Value) : OlcPartnCmps.New());

            foreach (Layout l in CmpGroups)
            {
                int cmpid = GetCmpIdByCmpLayoutPanel(l);
                if (cmpid < 1)
                    continue;

                string prefix = CmpGroupControlIdPrefix(cmpid);
                PartnCmp pc = pcmps.Find(cmpid);
                OlcPartnCmp olcpc = opcmps.Find(cmpid);

                if (pc == null)
                {
                    pc = pcmps.AddEntity();
                    pc.Cmpid = cmpid;
                }

                if (olcpc == null)
                {
                    olcpc = opcmps.AddEntity();
                    olcpc.Cmpid = cmpid;
                }

                foreach (Control c in l.Controls)
                {
                    string id = c.ID;
                    if (id.StartsWith(prefix))
                    {
                        string fieldName = id.Substring(prefix.Length);

                        if (fieldName.Equals(OlcPartnCmp.FieldSecpaymid.Name))
                        {
                            Field field = OlcPartnCmp.GetSchema().Fields.Get(fieldName);
                            if (field != null)
                                c.Value = olcpc[field];
                        }
                        else
                        {
                            Field field = PartnCmp.GetSchema().Fields.Get(fieldName);
                            if (field != null)
                                c.Value = pc[field];
                        }
                    }
                }

                AfterLoadCmpValuesForCmp(cmpid, prefix, pc, l);
            }
        }

        protected OlcPartnCmps SaveOlcCmpValuesToData(Key partnerKey)
        {
            OlcPartnCmps partnCmps = OlcPartnCmps.New();
            int partnerId = Convert.ToInt32(partnerKey[Base.Masters.Partner.Partner.FieldPartnid.Name]);
            foreach (Layout l in CmpGroups)
            {
                int cmpid = GetCmpIdByCmpLayoutPanel(l);
                if (cmpid < 1)
                    continue;

                string prefix = CmpGroupControlIdPrefix(cmpid);
                OlcPartnCmp pc = GetOlcPartnCmp(partnCmps, partnerId, cmpid);

                foreach (Control c in l.Controls)
                {
                    string id = c.ID;
                    if (id.StartsWith(prefix))
                    {
                        string fieldName = id.Substring(prefix.Length);
                        Field field = OlcPartnCmp.GetSchema().Fields.Get(fieldName);
                        if (field != null)
                            pc[field] = c.Value;
                    }
                }
            }
            return partnCmps;
        }

        private OlcPartnCmp GetOlcPartnCmp(OlcPartnCmps partnCmps, int partnerId, int cmpId)
        {
            OlcPartnCmp pc = partnCmps.Find(cmpId);
            if (pc == null)
            {
                pc = partnCmps.AddEntity();
                pc.Partnid = partnerId;
                pc.Cmpid = cmpId;
            }
            return pc;
        }

        protected override void MakeControlsReadOnly(bool clearValue, PageUpdateArgs args)
        {
            base.MakeControlsReadOnly(clearValue, args);

            this.ctrlDebcredsumvalue.SetDisabled(true);
            if (clearValue)
            {
                this.ctrlDebcredsumvalue.SetValue(null, args: args);
            }
        }
    }
}