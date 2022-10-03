using eLog.Base.Common;
using eLog.HeavyTools.Common.Matrix;
using eLog.HeavyTools.Masters.Item.Color;
using eLog.HeavyTools.Masters.Item.Season;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;

namespace eLog.HeavyTools.Masters.Item.Model
{ 
    internal class OlcItemModelSeasonRules : TypedBaseRuleSet<OlcItemModelSeason>
    {
        public OlcItemModelSeasonRules()
            : base(true)
        {
            ERules["imsid"].Mandatory = false; //PK 
        }
    }
  
    internal class OlcItemModelSeasonBL : DefaultBL1<OlcItemModelSeason, OlcItemModelSeasonRules>
    {
        public static readonly string ID = typeof(OlcItemModelSeasonBL).FullName;

        protected OlcItemModelSeasonBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
             
        }

        public static OlcItemModelSeasonBL New()
        {
            return (OlcItemModelSeasonBL)ObjectFactory.New(typeof(OlcItemModelSeasonBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemModelSeason).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemModelSeasonRules));

        }
    }

    internal class OlcItemModelSeasonSearchTab: MatrixTabPage2<OlcItemModelSeasonBL, OlcItemModel, OlcItemColors, OlcItemSeasons>
    {
        public static OlcItemModelSeasonSearchTab New()
        {
            var t = (OlcItemModelSeasonSearchTab)ObjectFactory.New(typeof(OlcItemModelSeasonSearchTab));
            t.Initialize("OlcItemModelSeason");
            return t;
        }

        public override void InitalizeMatrix(MatrixTabPageData<OlcItemModelSeasonBL, OlcItemModel, OlcItemColors, OlcItemSeasons> matrixTabPageData)
        { 
            matrixTabPageData.Setup(OlcItemModel.FieldImid,
                    new MatrixField(OlcItemColor.FieldIcid, OlcItemColor.FieldName),
                    new MatrixField(OlcItemSeason.FieldIsid, OlcItemSeason.FieldName),
                    OlcItemModelSeasonBL.ID
                    ); 
        } 

        protected override OlcItemSeasons SetupCols(OlcItemModel im)
        {
            return OlcItemSeasons.LoadOrderBy(new Key() { { OlcItemSeason.FieldDelstat.Name, 0 } });
        }
        
        protected override OlcItemColors SetupRows(OlcItemModel im)
        {
            return OlcItemColors.Load(new Key() { { OlcItemColor.FieldDelstat.Name, 0 } });
        }
          
        Textbox codetextbox = new Textbox("code") { Disabled = true };
        Textbox nametextbox = new Textbox("name") { Disabled = true };
        protected override void SetupHeader(LayoutTable layout)
        {
            layout.AddControl(codetextbox);
            layout.AddControl(nametextbox);
        }
        protected override void SetupRootKey(OlcItemModel rootentity)
        {
            codetextbox.Value = rootentity.Code;
            nametextbox.Value = rootentity.Name;

            if (rootentity.Isimported == 1)
            {
                buildButton.Disabled = true;
                saveChangeButton.Disabled = true;
                noSaveButton.Disabled = true;
            }  else
            {
                buildButton.Disabled = false;
                saveChangeButton.Disabled = false;
                noSaveButton.Disabled = false;
            }
        }


        protected override MatrixStoredEntity GetStoredEntityCollection(Key key)  {

            key.Add(OlcItemModelSeason.FieldDelstat.Name, 0);

            var msec = new MatrixStoredEntityConverter(typeof(OlcItemModelSeason), typeof(OlcItemModelSeason));
            msec.Add(OlcItemModelSeasons.Load(key), typeof(OlcItemModelSeason));

            return new MatrixStoredEntity(msec);
        }

        protected override bool Presave(BLObjectMap map, MatrixStoredEntityValues se)
        {
            return true;
        }

        #region Title
        public override string GetMatrixTitle()
        {
            return @"$modelsessionmatrixtitle";
        }

        public override string GetMatrixGroupTitle()
        {
            return @"$modelsessiongrouptitle";
        }

        public override string GetMatrixsingTitle()
        {
            return @"$modelsessionmatrixsingtitle";
        }

        public override string GetTootTitle()
        {
            return @"$modelsessiontoottitle";
        }

        public override string GetResultTitle()
        {
            return @"$modelMatrixResultTitle";
        }

        public override string GetMatrixResult()
        {
            return @"$modelMatrixResult";
        }
         

        #endregion


    }
}