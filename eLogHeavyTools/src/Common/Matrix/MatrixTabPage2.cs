using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.Base.Masters.Cost;
using eLog.Base.Masters.Item;
using eLog.Base.Masters.PriceTable;
using eLog.Base.Setup.Warehouse;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.Item.Model;
using eLog.HeavyTools.Masters.Item.Season;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Rights;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.UI;
using eProjectWeb.Framework.UI.Commands;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Common.Matrix
{
    abstract class MatrixTabPage2<ItemModelSeasonBL, RootEntity, RowEntityCollection, ColEntityCollection> : TabPage2 
            where RootEntity : Entity
            where RowEntityCollection : EntityCollection
            where ColEntityCollection : EntityCollection
    {
        private MatrixTabPageData<ItemModelSeasonBL, RootEntity, RowEntityCollection, ColEntityCollection> MatrixTabPageData =
                             new MatrixTabPageData<ItemModelSeasonBL, RootEntity, RowEntityCollection, ColEntityCollection>();

        protected abstract ColEntityCollection SetupCols(RootEntity rootEntity);
        protected abstract RowEntityCollection SetupRows(RootEntity rootEntity);

        protected abstract MatrixStoredEntity GetStoredEntityCollection(Key key);


        static string RootKeySessionID = "RootKeySessionID";
        protected MatrixStoredEntity GetStoredEntityCollectionInternal()
        {
            if (rootEntity == null)
            {
                var k = new Key();
                k.Add(MatrixTabPageData.RootKey.Name, Session.Current[RootKeySessionID]);
                return GetStoredEntityCollection(k);

            } else
            {
                var k = new Key();
                var keySessionID = rootEntity[MatrixTabPageData.RootKey];
                Session.Current[RootKeySessionID] = keySessionID;
                k.Add(MatrixTabPageData.RootKey.Name, keySessionID);
                return GetStoredEntityCollection(k);
            } 
        }

        private RootEntity GetRootEntity(PageUpdateArgs args)
        { 
            if (!args.PageData.ContainsKey(Consts.RootEntityKey)) {
                return null;
            }
            var rek = (Dictionary<string, object>)args.PageData[Consts.RootEntityKey];
            var rootkey = (int)(long)rek[MatrixTabPageData.RootKey.Name];
            return Entity<RootEntity>.Load(new Key { { MatrixTabPageData.RootKey.Name, rootkey } });
        }

        public abstract void InitalizeMatrix(MatrixTabPageData<ItemModelSeasonBL, RootEntity, RowEntityCollection, ColEntityCollection> matrixTabPageData);
         
        string matrixTitle;
        string matrixGroupTitle;
        string matrixsingTitle;
        string rootTitle;
        string MatrixResultTitle;
        string MatrixResult;
        public int MatrixFirstCellWidth = 135;

        public abstract string GetMatrixTitle();

        public abstract string GetMatrixGroupTitle();

        public abstract string GetMatrixsingTitle();
         
        public abstract string GetTootTitle();

        public abstract string GetResultTitle();
        public abstract string GetMatrixResult();

 
        private LayoutTable matrixSubItemsLayoutTable;
        private LayoutTable matrixTopItemsLayoutTable;
        private LayoutTable matrixTableItems;
        private LayoutTable matrixTableItemsHeader;

        RootEntity rootEntity;


        private Button buildButton;
        private Button saveChangeButton;
        private Button noSaveButton;

        private DialogBox dialogBox;


        protected override void Initialize(string itemmatrix)
        {
            base.Initialize(itemmatrix);
            var layoutHolder = this;
            var columns = new[]
                              {
                                  new TableColumn(150),
                                  new TableColumn(200),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(48),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(48),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(48),
                                  new TableColumn(160)
                              };
            var layout = new LayoutTable(columns)
            {
                ID = "EditGroup3",
                LabelId = rootTitle,
                ControlGroup = itemmatrix
            };
            layoutHolder.Add(layout);

            SetupHeader(layout);
      
            buildButton = new Button("build");
            buildButton.SetOnClick(OnBuildClick);
            buildButton.Shortcut = ShortcutKeys.Key_F5;
            AddCmd(buildButton);
          
            saveChangeButton = new Button("savechange") { ColsToSpan = -1 };
            saveChangeButton.SetOnClick(OnSaveClick);
            saveChangeButton.Shortcut = ShortcutKeys.Key_F10;
            AddCmd(saveChangeButton);      
             
            noSaveButton = new Button("nosave") { ColsToSpan = -1 };
            noSaveButton.SetOnClick(OnNoSaveClick);
            noSaveButton.Shortcut = ShortcutKeys.Key_ESC;
            AddCmd(noSaveButton);
         

            //BuildMatrix(null, true, true);

            dialogBox = new DialogBox(DialogBoxType.Ok);
            RegisterDialog(dialogBox);
            OnPageTaskActivate += OnOnPageTaskActivate;
            OnPageLoad += MatrixControlFix;


            matrixTitle = GetMatrixTitle();
            matrixGroupTitle = GetMatrixGroupTitle();

            matrixsingTitle = GetMatrixsingTitle();

            rootTitle = GetTootTitle();

            MatrixResultTitle = GetResultTitle();
            MatrixResult = GetMatrixResult();
        }

        protected void MatrixControlFix(PageUpdateArgs args)
        {
            var l = MatrixControlHideList.GetAll();
            foreach (var name in l)
            {
                args.AddPostExecCommand(string.Format("{0}.first().hide()", name));
            }
            args.AddPostExecCommand("$('#MatrixTableItems').css('margin-top',0).css('border-top',0).css('padding-top',0);");
            args.AddPostExecCommand("$('#MatrixTableItems > span').remove();");
            args.AddPostExecCommand("$('#MatrixCheckBox').css('border-bottom',0).css('padding-bottom',0);");
             
        }
         

        protected abstract void SetupHeader(LayoutTable layout);

        private void LoadDataFromDB(PageUpdateArgs args)
        {
            if (MatrixTabPageData.RootKey == null)
            {
                InitalizeMatrix(MatrixTabPageData);
            }

            MatrixTabPageData.SetupCols(SetupCols(rootEntity));
            MatrixTabPageData.SetupRows(SetupRows(rootEntity));
            rootEntity = GetRootEntity(args);

            MatrixTabPageData.SetStoredEntityCollection(GetStoredEntityCollectionInternal()); 
        }

        private void OnOnPageTaskActivate(PageUpdateArgs args)
        {
            LoadDefaultValues(args);
            NoSave(args);
            RenderControls(args);
        }
         

        private void LoadDefaultValues(PageUpdateArgs args)
        {
            var isRenderControls = false;
            if (args.PageData.ContainsKey(Consts.RootEntityKey))
            {
                if (rootEntity == null)
                {
                    rootEntity = GetRootEntity(args);
                } 
                if (rootEntity != null)
                //if (rek.ContainsKey(MatrixTabPageData.RootKey.Name))
                { 
                    SetupRootKey(rootEntity);

                    if (args.PageData.ContainsKey(RootKey))
                    {
                        var m = ConvertUtils.ToInt32(args.PageData[RootKey]);
                        if (m != ConvertUtils.ToInt32(rootEntity[MatrixTabPageData.RootKey.Name]))
                        {
                            NoSave(args);
                            isRenderControls = true;
                        }
                    }

                    if (isRenderControls)
                        RenderControls(args);
                }
            }

        }

        protected abstract void SetupRootKey(RootEntity rootentity);
          
        private void OnNoSaveClick(PageUpdateArgs args)
        {
            NoSave(args); 
            RenderControls(args);
        }

        private void NoSave(PageUpdateArgs args)
        {
            ClearMatrix(args);
        }

        private void ClearMatrix(PageUpdateArgs args)
        {
            if (matrixTableItems != null)
            {
                this.Remove(matrixTableItems);
                this.Remove(matrixTableItemsHeader);
                this.Remove(matrixSubItemsLayoutTable);
                this.Remove(matrixTopItemsLayoutTable);

                matrixTableItems = null;
                matrixTableItemsHeader = null;
                matrixSubItemsLayoutTable = null;
                matrixTopItemsLayoutTable = null;
            }
        }

        public override void LoadState(ControlState state)
        {
            BuildMatrix(PageUpdateArgs.Current, false, false);
            base.LoadState(state);
        }

        private void OnBuildClick(PageUpdateArgs args)
        {
            BuildMatrix(args, true, true);
            MatrixControlFix(args);
        }

        private const string AllControlId = "AllControlId";
        private const string SubControlId = "SubControlId";

        private void BuildMatrix(PageUpdateArgs args, bool save, bool renderControls)
        {
            BeforeBuildMatrix(args, save, renderControls);
            LoadDataFromDB(args);

            if (matrixTableItems!=null)
            {
                this.Remove(matrixTableItems);
                this.Remove(matrixTableItemsHeader);

                if (matrixSubItemsLayoutTable!=null)
                    this.Remove(matrixSubItemsLayoutTable);
                if (matrixTopItemsLayoutTable!=null)
                    this.Remove(matrixTopItemsLayoutTable); 
            } 
            
            CreateLayout();

            matrixTableItemsHeader.AddControl(new Empty());
            
            foreach (Entity colentity in MatrixTabPageData.Cols)
            {
                AddTitle(matrixTableItemsHeader, ConvertUtils.ToString(colentity[MatrixTabPageData.ColKey.Name]));
            }
            matrixTableItems.AddControl(new ForceNextRow());

            Dictionary<string, bool> FieldsEquals = new Dictionary<string, bool>();
            Dictionary<string, object> FieldsValueEquals = new Dictionary<string, object>();


            foreach (Entity row in MatrixTabPageData.Rows)
            {
                var rKey = MatrixTabPageData.RowKey.Key.Name;

                var rowId = row[rKey];
                MatrixTabPageData.MatrixData.AddRowKey(rowId);

                // Mátrix jelölők
                AddTitle(matrixTableItems, BeforeColNameShow(row[MatrixTabPageData.RowKey.Name].ToString()));
                 
                foreach (Entity col in MatrixTabPageData.Cols)
                { 
                    var eqKey = MatrixTabPageData.ColKey.Key.Name;
                    var colrId = col[eqKey];

                    MatrixTabPageData.MatrixData.AddColKey(colrId);

                    if (!FieldsEquals.ContainsKey(eqKey))
                    {
                        FieldsEquals.Add(eqKey, true);
                    }
                     

                    var value = ConvertUtils.ToBoolean(MatrixTabPageData.GetMatrixData(
                        row[MatrixTabPageData.RowKey.Key.Name],
                        col[MatrixTabPageData.ColKey.Key.Name],
                        MatrixTabPageData.RootKey
                        ));

                    var found = value.HasValue && value.Value;


                    var c = new Checkbox(string.Format("cp_{0}_{1}", row[MatrixTabPageData.RowKey.Key], col[MatrixTabPageData.ColKey.Key]))
                    {
                        Checked = found,
                        Disabled = found,
                        LabelId = "$xx"
                    };

                    matrixTableItems.AddControl(c);
                }

                matrixTableItems.AddControl(new ForceNextRow());

                matrixSubItemsLayoutTable.AddControl(new ForceNextRow());
                var rowkey = row[MatrixTabPageData.RowKey.Name].ToString();
                AddTitle(matrixSubItemsLayoutTable, BeforeColNameShow(rowkey));

                var rowkeIdy = row[MatrixTabPageData.RowKey.Key.Name].ToString();

                var cc = 0;
                //  Alsó controlok
                foreach (var item in MatrixTabPageData.EditItems)
                {
                    var c = item.GetControl(SubControlId+ rowkeIdy);
                    c.Disabled = !MatrixTabPageData.MatrixData.IsEqual2(rowId, item.GetField());

                    c.Value = MatrixTabPageData.GetMatrixData(
                       row[MatrixTabPageData.RowKey.Key.Name],
                       item.GetField()
                       );
                    if (item.GetReadOnly())
                    {
                       if (c.Value != null)
                        {
                            c.Disabled = true;
                        }
                    }
                    matrixSubItemsLayoutTable.AddControl(c);
                    cc++;
                    if (cc == 3)
                    {
                        matrixSubItemsLayoutTable.AddControl(new ForceNextRow());
                        matrixSubItemsLayoutTable.AddControl(new Empty());
                        cc = 0;
                    }
                } 
            }

            var ccc = 0;
            matrixTopItemsLayoutTable.AddControl(new Empty());
            // Összevont színtű adatok
            foreach (var item in MatrixTabPageData.EditItemsTop)
            {
                var c = item.GetControl(AllControlId);
                c.Disabled = !MatrixTabPageData.MatrixData.IsEqual2(item.GetField());
                c.Value = MatrixTabPageData.GetMatrixData(item.GetField());
                if (item.GetReadOnly())
                {
                    if (c.Value != null)
                    {
                        c.Disabled = true;
                    }
                }

                matrixTopItemsLayoutTable.AddControl(c);
                ccc++;
                if (ccc == 3)
                {
                    ccc = 0;
                    matrixTopItemsLayoutTable.AddControl(new ForceNextRow());
                    matrixTopItemsLayoutTable.AddControl(new Empty());
                }
            }

            if (MatrixTabPageData.EditItemsTop.Count == 0)
            {
                this.Remove(matrixTopItemsLayoutTable);
            }

            if (MatrixTabPageData.EditItems.Count == 0)
            {
                this.Remove(matrixSubItemsLayoutTable);
            }
            
            if (/*renderControls && */args != null)
            {
                RenderControls(args);
            }
        }

        protected virtual void BeforeBuildMatrix(PageUpdateArgs args, bool save, bool renderControls)
        {
            
        }

        protected virtual string BeforeColNameShow(string rowkey)
        {
            return rowkey;
        }

        private void CreateLayout()
        { 
            var defwidth = 65;
            var columnsHeader = new[]
                               {
                                        new TableColumn(MatrixFirstCellWidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth), new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth), new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth), new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(defwidth),
                                       new TableColumn(1, TableColumnFlags.None),
                                       new TableColumn(55),
                                    };
            var columnsHeader2 = new[]
                         {
                                         new TableColumn(125),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth), new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth), new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth), new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(defwidth),
                                        new TableColumn(1, TableColumnFlags.None),
                                        new TableColumn(144),
                                        new TableColumn(0, TableColumnFlags.Empty),
                                   };
            var columns = new[]
                              {
                                  new TableColumn(120),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(80),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(80),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                              };

            var columns2 = new[]
                    {
                                  new TableColumn(160),
                                  new TableColumn(1),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(100),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(100),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                                  new TableColumn(100),
                                  new TableColumn(160),
                                  new TableColumn(10, TableColumnFlags.Empty),
                              };

            matrixTableItemsHeader = new LayoutTable(columnsHeader2) { ID = "MatrixTableItemsHeader", LabelId= matrixTitle, ControlGroup = "g1" }; 
            matrixTableItems = new LayoutTable(columnsHeader) { ID = "MatrixTableItems", ControlGroup = "g2" };
            matrixTopItemsLayoutTable = new LayoutTable(columns2) {ID = "MatrixTopItemsLayoutTable", LabelId = matrixGroupTitle, ControlGroup = "g3" };
            matrixSubItemsLayoutTable = new LayoutTable(columns2) { ID = "MatrixSubItemsLayoutTable", LabelId = matrixsingTitle, ControlGroup = "g4" };

            this.Add(matrixTableItemsHeader);
            this.Add(matrixTableItems);
            this.Add(matrixTopItemsLayoutTable);
            this.Add(matrixSubItemsLayoutTable);
            

            MatrixControlHideList.Add(@"$('#MatrixCheckBox2 span')");
             

        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void AddTitle(LayoutTable matrixTableItems, string title)
        {
            var c = new Textbox(RandomString(10)) { LabelId = title, Disabled=true};
            matrixTableItems.AddControl(c);
            MatrixControlHideList.AddByID(c.ID);
            return;
   /*
            var t = new Table() { ID = RandomString(10) };
            matrixTableItems.AddRenderable(t);
            return;

            var r = new TableRow();
            t.AddRow(r);

            var c = new TableCell(title); 
            r.AddCell(c);
         
            var c = new CustomControl
            {
                Content = title,
                ID= RandomString(10)
            }; 
            matrixTableItems.AddRenderable(c); */
        }

        private const string ColorData = "ColorData";
        private const string RootKey = "RootKey";
        private const string SeasonData = "SeasonData";
        private const string MainGroupData = "MainGroupData";
        
        private Control CreatePropCombo(string preId, int propgrpid, object value, bool disabled)
        {
            var combo = new Combo("prop" + preId + "_" + propgrpid, ItemPropList.ID)
            {
                FilterKey = propgrpid,
                Disabled = disabled,
                LabelId = "$prop" + propgrpid
            };
            if (!combo.Disabled && value != null) combo.Value = value;
            return combo;
        }

        public static bool SkipRootKey = false;

        private void OnSaveClick(PageUpdateArgs args)
        { 
            int updatedCount = 0;
            int createdCount = 0;
            using (DB db = DB.GetConn(DB.Main, Transaction.Use))
            {
                LoadDataFromDB(args);

                foreach (Entity row in MatrixTabPageData.Rows)
                {
                    var rKey = MatrixTabPageData.RowKey.Key.Name;
                    var rowId = row[rKey];
                    var checkedCount = 0;

                    foreach (Entity col in MatrixTabPageData.Cols)
                    {
                        var eqKey = MatrixTabPageData.ColKey.Key.Name;
                        var colrId = col[eqKey];
 
                        var c = FindRenderable<Checkbox>(string.Format("cp_{0}_{1}", row[MatrixTabPageData.RowKey.Key.Name], col[MatrixTabPageData.ColKey.Key.Name]));

                        if (c.Checked)
                        {
                            checkedCount++;
                            var stored = MatrixTabPageData.StoredEntitiesFinder(row, col);
                            if (stored == null)
                            {
                                var rek = (Dictionary<string, object>)args.PageData[Consts.RootEntityKey];
                                var rootkey = (int)(long)rek[MatrixTabPageData.RootKey.Name];

                                MatrixStoredEntityValues msev = MatrixTabPageData.CreateNew();

                                var fs = new List<Field>();

                                fs.Add(MatrixTabPageData.RowKey.Key);
                                fs.Add(MatrixTabPageData.ColKey.Key);
                                if (!SkipRootKey)
                                {
                                    fs.Add(MatrixTabPageData.RootKey); 
                                }
                                 
                                var e = msev.FindEntity(fs.ToArray()); // new[] { MatrixTabPageData.RootKey, MatrixTabPageData.RowKey.Key, MatrixTabPageData.ColKey.Key });
                                if (!SkipRootKey)
                                {
                                    e[MatrixTabPageData.RootKey.Name] = rootkey;
                                }

                                e[MatrixTabPageData.RowKey.Key.Name] = row[MatrixTabPageData.RowKey.Key.Name];
                                e[MatrixTabPageData.ColKey.Key.Name] = col[MatrixTabPageData.ColKey.Key.Name];

                                MatrixTabPageData.Add(msev);
                                createdCount++;
                            }
                        }

                        var rowkey = row[MatrixTabPageData.RowKey.Key.Name].ToString();

                        var isChanged = false;
                        //  Alsó controlok
                        foreach (var item in MatrixTabPageData.EditItems)
                        {
                            var cc = (Control)FindRenderable(SubControlId + rowkey + item.GetField().Name);
                             
                            if (checkedCount > 0)
                            {
                                MandatoryCheck(item, cc.Value, BeforeColNameShow(row[MatrixTabPageData.RowKey.Name].ToString()));


                            }
                            MatrixTabPageData.MatrixData.SetMatrixData(row[MatrixTabPageData.RowKey.Key], item, cc.Value, cc.Disabled);
                            
                        }
                        if (isChanged)
                        {
                            updatedCount++;
                        }
                    }

                    // Összevont színtű adatok
                    foreach (var item in MatrixTabPageData.EditItemsTop)
                    {
                        var cc = (Control)FindRenderable(AllControlId + item.GetField().Name);
                       
                        MandatoryCheck(item, cc.Value, null);
                             
                        MatrixTabPageData.MatrixData.SetMatrixData(item, cc.Value, cc.Disabled);    
                     
                    }
                }
                MatrixTabPageData.UpdateStoreValues();

                foreach (var se in MatrixTabPageData.GetStoreItems())
                {
                    var ch = 0;
                    foreach (var item in se.Entities)
                    {
                        if (item.Value.State == DataRowState.Modified)
                        {
                            foreach (var f in item.Value.Schema.Fields)
                            {
                                var o = item.Value[f, DataRowVersion.Original];
                                var c = item.Value[f, DataRowVersion.Current];

                                if (c !=null || o!=null)
                                {
                                    if (c==null && o != null)
                                    {
                                        ch++;
                                        break;
                                    }
                                    if (c != null && o == null)
                                    {
                                        ch++;
                                        break;
                                    }
                                    if (!o.Equals(c))
                                    {
                                        ch++;
                                        break;
                                    }
                                }
                            } 
                        }
                    }
                    if (ch > 0)
                    {
                        updatedCount++;
                    }
                    var map = MatrixTabPageData.GetBLObjectMap(se.GetMapDefaultEntity());
                    se.FillNonDefaultEntity(map); 
                    Presave(map); 
                    MatrixTabPageData.Save(map);
                    Postsave(map);
                }
                db.Commit();
                BuildMatrix(args, true, true);
                args.ShowDialog(dialogBox, MatrixResultTitle,
                                Translator.Translate(MatrixResult, updatedCount, createdCount));
            }
            MatrixControlFix(args);
        }
        private void MandatoryCheck(IMatrixEditItem imei, object value, string rownem)
        {
            if (imei.GetMandatory())
            {
                if (value == null)
                {
                    var f = Translator.Translate("$" + imei.GetField().Name);
                    if (!string.IsNullOrEmpty(rownem))
                    {
                        f = rownem + " / " + f;
                    }
                    var et = Translator.TranslateNspace("$validation_error_mandatory", "eProjectWeb", f);
                    throw new MessageException(et);
                }
            }
        }

        protected virtual void Postsave(BLObjectMap map)
        {
        }

        protected abstract void Presave(BLObjectMap map);

    }
    public class MatrixControlHideList
    {
        private const string sessionid = "MatrixControlHideListSessionId";

        public static void Add(string id)
        {
            var h = Session.Current[sessionid] as List<string>;
            if (h== null)
            {
                h = new List<string>();
            }
            h.Add(id);
            Session.Current[sessionid] = h;
        }
        public static string[] GetAll()
        {
            var h = Session.Current[sessionid] as List<string>;
            if (h == null)
            {
                h = new List<string>();
            }
            var a = h.ToArray();
            h.Clear();
            Session.Current[sessionid] = h;
            return a;
        }

        internal static void AddByID(object id)
        {
            Add(string.Format("$('#{0}')", id));
        }
    }
}