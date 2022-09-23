using eLog.Base.Common;
using eLog.HeavyTools.Common.Transate;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    partial class OlcItemMainGroup : ITranslateController
    {
        public override void SetDefaultValues()
        {
            Maingrouplastnum = 0;
        }

        public override void PostSave()
        {
            base.PostSave();
            TranslateController.PostSave(this);
        }

        public void InitalizeTranslateController(TranslateController tc)
        {
            tc.Add(_TableName, FieldImgid, "EditGroup1", "name_eng", "en-us");
        }

        public string name_eng;
    }
}
