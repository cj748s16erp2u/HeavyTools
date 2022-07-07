using eLog.Base.Common;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace eLog.HeavyTools.Common.Transate
{
    public class TranslateController
    {
        public List<TranslateControllerData> Controls = new List<TranslateControllerData>();
 
        internal void Add(string tableName, Field field, string layoutTableID, string xmlFieldName, string langid)
        {
            Controls.Add(new TranslateControllerData(tableName, field, layoutTableID, xmlFieldName, langid));
        }

        internal static void PostSave(ITranslateController e)
        {
            TranslateController tc = new TranslateController();
            e.InitalizeTranslateController(tc);
        }

        internal static void DefaultPageLoad(TabPage2 tab, ITranslateController e)
        {
            var entity = (Entity)e;

            var tc = new TranslateController();
            e.InitalizeTranslateController(tc);
             
            foreach (var control in tc.Controls)
            {
                var t = DBTranslate.Load(control.GetKey(entity));
                if (t != null)
                {
                    var lt=tab.FindRenderable<LayoutTable>(control.LayoutTableID);
                    var c = (Control)lt.FindRenderable(control.XmlFieldName);
                    if (c != null)
                    {
                        c.Value = t.Txt;
                    }
                }
            }

        }
 
        internal static void SaveControlsToBLObjectMap(TabPage2 tab, ITranslateController e)
        {
            var entity = (Entity)e;

            var tc = new TranslateController();
            e.InitalizeTranslateController(tc);

            foreach (var control in tc.Controls)
            {
                var t = DBTranslate.Load(control.GetKey(entity));
                if (t == null)
                {
                    t = DBTranslate.CreateNew();
                    t.Trlgrp = control.TableName;
                    t.Trlkey = control.GetTrlkey(entity);
                    t.Trlitm = control.XmlFieldName;
                    t.Langid = control.Langid;
                }

                var lt = tab.FindRenderable<LayoutTable>(control.LayoutTableID);
                var c = (Control)lt.FindRenderable(control.XmlFieldName);
                if (c != null)
                {
                    t.Txt = ConvertUtils.ToString(c.Value);
                } else
                {
                    t.Txt = null;
                }

                t.Save();
            }
        }

        internal static string UpdateQueryString(string sql, ITranslateController e, string fieldText, string joinText)
        {
            var tc = new TranslateController();
            e.InitalizeTranslateController(tc);

            var field = "";
            var join = "";
            for (int i = 0; i < tc.Controls.Count; i++)
            {
                var item = tc.Controls[i];
                field += ", ot1.txt " + item.XmlFieldName;

                join += string.Format(@" outer apply(
	select txt
	  from ols_translate (nolock)
	  where trlgrp='{0}' and trlkey='''{1}='''+convert(varchar, {1}) and langid='{2}'
  ) ot1", item.TableName, item.Field.Name, item.Langid);
            }

            return sql.Replace(fieldText, field).Replace(joinText, join);
        }
    }
    public class TranslateControllerData
    {
        public string TableName;
        public Field Field;
        public string XmlFieldName;
        public string LayoutTableID;
        public string Langid; 

        public TranslateControllerData(string tableName, Field field, string layoutTableID, string xmlFieldName, string langid)
        {
            this.TableName = tableName;
            this.Field = field;
            this.XmlFieldName = xmlFieldName;
            this.LayoutTableID = layoutTableID;
            this.Langid = langid;
        }

        internal Key GetKey(Entity entity)
        {
           return new Key
                {
                    { DBTranslate.FieldTrlgrp.Name, TableName },
                    { DBTranslate.FieldTrlkey.Name, GetTrlkey(entity) },
                    { DBTranslate.FieldTrlitm.Name, XmlFieldName },
                    { DBTranslate.FieldLangid.Name, Langid }
                };
        }

        internal string GetTrlkey(Entity entity)
        {
            return "'" + Field.Name + "='" + entity[Field.Name];
        }
    }
}
