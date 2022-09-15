using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterBL : DefaultBL1<ReminderLetterItem, ReminderLetterRules>
    {
        public static readonly string ID = typeof(ReminderLetterBL).FullName;

        public static T New<T>()
            where T : ReminderLetterBL
        {
            return ObjectFactory.New<T>();
        }

        public static ReminderLetterBL New()
        {
            return New<ReminderLetterBL>();
        }

        public ReminderLetterBL() : base(DefaultBLFunctions.Basic)
        {
        }

        public static string REMINDERLETTER_TEXT_XCVCODE = "PartnReminderLetterText";

        protected IList<ReminderLetterItem> GetList()
        {
            var prov = (ReminderLetterSearchProvider)SearchServer.Get(ReminderLetterSearchProvider.ID);
            var args = new MSPCreateListArgs(new Dictionary<string, object>(), MSPCreateListCallType.Search);
            var list = prov.CreateList(args);

            return list;
        }

        protected override Entity GetEntityInternal(Key k)
        {
            if ((Functions & DefaultBLFunctions.GetEntity) == 0)
                throw new InvalidOperationException();

            var list = this.GetList();

            var newKey = new Key();
            foreach (var key in k.Keys.ToArray())
            {
                newKey[key.ToLowerInvariant()] = k[key];
            }

            var e = list.FirstOrDefault(l => newKey.Equals(l.PK));

            return e;
        }

        protected Common.xcval.OfcXcval LoadOfcXcval(ReminderLetterItem entity, bool createIfNotFound = true)
        {
            var ofcXcval = entity.Xcvid.HasValue ? Common.xcval.OfcXcval.Load(entity.Xcvid) : null;
            if (ofcXcval == null)
            {
                var k = new Key
                {
                    { Common.xcval.OfcXcval.FieldXcvcode.Name, REMINDERLETTER_TEXT_XCVCODE },
                    { Common.xcval.OfcXcval.FieldXcvextcode.Name, null }
                };

                ofcXcval = Common.xcval.OfcXcval.Load(k);
            }

            if (ofcXcval == null && createIfNotFound)
            {
                ofcXcval = Common.xcval.OfcXcval.CreateNew();
                ofcXcval.Xcvcode = REMINDERLETTER_TEXT_XCVCODE;
            }

            return ofcXcval;
        }

        protected void SaveOfcXcval(Common.xcval.OfcXcval ofcXcval)
        {
            var map = new BLObjectMap();
            map.SysParams.ActionID = ofcXcval.State == DataRowState.Added ? ActionID.New : ActionID.Modify;
            map.Default = ofcXcval;

            var ofcXcvalBL = Common.xcval.OfcXcvalBL.New();
            ofcXcvalBL.Save(map);
        }

        public void SaveXml(ReminderLetterItem entity)
        {
            var ofcXcval = this.LoadOfcXcval(entity);

            ofcXcval.Xmldata = this.AddOrUpdateEntityInXml(ofcXcval.Xmldata, entity);

            this.SaveOfcXcval(ofcXcval);
        }

        public void DeleteXml(ReminderLetterItem entity)
        {
            var ofcXcval = this.LoadOfcXcval(entity, false);

            if (ofcXcval != null)
            {
                ofcXcval.Xmldata = this.DeleteEntityInXml(ofcXcval.Xmldata, entity);

                this.SaveOfcXcval(ofcXcval);
            }
        }

        protected string AddOrUpdateEntityInXml(string xmldata, ReminderLetterItem entity)
        {
            var xDoc = new XDocument();
            if (!string.IsNullOrWhiteSpace(xmldata))
            {
                xDoc = XDocument.Parse(xmldata);
            }
            else
            {
                xDoc.Add(new XElement("items"));
            }

            var root = xDoc.Element("items");
            var items = root.Elements("item");
            if (!entity.Seqno.HasValue)
            {
                var seqnos = items.Select(i => ConvertUtils.ToInt32(i.Element("seqno")?.Value)).Where(x => x.HasValue).Select(x => x.Value);
                if (seqnos.Any())
                {
                    entity.Seqno = seqnos.Max() + 1;
                }
                else
                {
                    entity.Seqno = 1;
                }
            }

            var seqno = ConvertUtils.ToString(entity.Seqno);
            var item = items.Where(i => string.Equals(i.Element("seqno")?.Value, seqno, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (item == null)
            {
                item = new XElement("item");
                root.Add(item);

                this.SetXItemValue(item, "seqno", entity.Seqno);
            }

            this.SetXItemValue(item, "company", entity.Company);
            this.SetXItemValue(item, "ledger", entity.Ledger);
            this.SetXItemValue(item, "severity", entity.Severity);
            this.SetXItemValue(item, "lettertype", entity.Lettertype);
            this.SetXItemValue(item, "template", entity.Template);

            return xDoc.ToString(SaveOptions.DisableFormatting);
        }

        protected string DeleteEntityInXml(string xmldata, ReminderLetterItem entity)
        {
            var xDoc = new XDocument();
            if (!string.IsNullOrWhiteSpace(xmldata))
            {
                xDoc = XDocument.Parse(xmldata);
            }
            else
            {
                xDoc.Add(new XElement("items"));
            }

            var root = xDoc.Element("items");
            var items = root.Elements("item");

            var seqno = ConvertUtils.ToString(entity.Seqno);
            var item = items.Where(i => string.Equals(i.Element("seqno")?.Value, seqno, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            item?.Remove();

            return xDoc.ToString(SaveOptions.DisableFormatting);
        }

        private void SetXItemValue(XElement item, string key, object value)
        {
            var remove = value == null || string.IsNullOrWhiteSpace(ConvertUtils.ToString(value));
            var e = item.Element(key);
            if (e == null && !remove)
            {
                item.Add(new XElement(key, value));
            }
            else if (!remove)
            {
                e.Value = ConvertUtils.ToString(value);
            }
            else if (e != null)
            {
                e.Remove();
            }
        }

        public override void Delete(Key k)
        {
            using (DB x = DB.GetConn(DBConnID, Transaction.Use))
            {
                string reason = null;
                if (!IsDeletePossible(k, out reason))
                {
                    throw new MessageException(reason);
                }

                PreDelete(k);
                var e = this.GetEntity(k);
                this.DeleteXml(e);
                PostDeleteFunc?.Invoke(e);

                x.Commit();
            }
        }
    }
}
