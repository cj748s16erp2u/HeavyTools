using eLog.Base.Setup.Warehouse;
using eLog.HeavyTools.Masters.Item.ItemMatrix;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.Item.Model;
using eLog.HeavyTools.Masters.Item.Season;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Commands;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common.Matrix
{
    public interface IMatrixEditItem
    {
        Field GetField();
        Control GetControl(string nameprefix);
        bool GetMandatory();

        bool GetReadOnly();
    }

    public class MatrixEditItem <C> : IMatrixEditItem where C : Control 
    {
        private Field Field; 
        Action<C> ControlSetup;

        public MatrixEditItem(Field field)
        { 
            Field = field;
            ControlSetup = null;
            Mandatory = false;
        }

        public MatrixEditItem(Field field, Action<C> controlsetup)
        { 
            Field = field;
            ControlSetup = controlsetup;
            Mandatory = false;
        }

        public bool Mandatory { get; internal set; }
        public bool ReadOnly { get; internal set; } 
        public MatrixEditItem<Combo> WriteValueFromComboOnChange { get; internal set; }

        public Control GetControl(string nameprefix)
        {
            var control = (C)ObjectFactory.New(typeof(C));
            if (ControlSetup != null)
            {
                ControlSetup(control);
            }
            control.ID = nameprefix + Field.Name;
            control.LabelId = "$" + Field.Name;

           /* if (WriteValueFromComboOnChange != null)
            {
                control.SetOnChanged(delegate (PageUpdateArgs args) {
                    var c = (Control)args.TabPage.FindRenderable(WriteValueFromComboOnChange.GetField().Name);
                    args.Control.Value = c.Value;
                });
                 
            }*/

            return control;
        }
         

        public Field GetField()
        {
            return Field; 
        }

        public bool GetMandatory()
        {
            return Mandatory;
        }

        public bool GetReadOnly()
        {
            return ReadOnly;
        }
    }

    public class MatrixField
    {
        /* Kulcs mező */
        public Field Key;

        /* Képernyőre irható név*/
        public Field Name;
        public MatrixField(Field key, Field name)
        {
            Key = key;
            Name = name;
        }
    }

    public class MatrixTabPageData<StoredEntityBL, RootEntity, RowEntityCollection, ColEntityCollection>
            where RootEntity : Entity
            where RowEntityCollection : EntityCollection
            where ColEntityCollection : EntityCollection
    {
        public Field RootKey;
        public MatrixField RowKey;
        public MatrixField ColKey;
         

        public List<IMatrixEditItem> EditItems = new List<IMatrixEditItem>();
        public List<IMatrixEditItem> EditItemsTop = new List<IMatrixEditItem>();

        public RowEntityCollection Rows;
        public ColEntityCollection Cols;


         
         
        private StoredEntityBL bl;


        public MatrixData MatrixData = new MatrixData();
  
        public void SetupRows(RowEntityCollection rows)
        {
            Rows = rows;
        }
        public void SetupCols(ColEntityCollection cols)
        {
            Cols = cols;
        }

        internal void Setup(Field rootkey, MatrixField rowkey, MatrixField colkey, string businessLogicID)
        {
            RootKey = rootkey;
            RowKey = rowkey;
            ColKey = colkey;
            bl = (StoredEntityBL)BusinessLogicServer.Create(businessLogicID);
        }


        MatrixStoredEntity StoredEntityCollection;
        
        internal void SetStoredEntityCollection(MatrixStoredEntity storeEntityCollection)
        {
            StoredEntityCollection = storeEntityCollection;

            foreach (MatrixStoredEntityValues msev in storeEntityCollection.GetMatrixStoredEntityValues())
            {
                var e = msev.FindEntity(new[] { RowKey.Key, ColKey.Key }); 
                var rowPk = e[RowKey.Key.Name];
                var colPk = e[ColKey.Key.Name];
     
                MatrixData.AddValue(rowPk, colPk, RootKey, true);
              
                foreach (var item in EditItems)
                {
                    var f = item.GetField();
                    var ee = msev.FindEntity(new[] { f }); 
                    var vv = ee[f.Name];  
                    MatrixData.AddValue(rowPk, colPk, f, vv); 
                }
                foreach (var item in EditItemsTop)
                {
                    var f = item.GetField();
                    var ee = msev.FindEntity(new[] { f });
                    var vv = ee[f.Name];
                    MatrixData.AddValue(rowPk, colPk, f, vv);
                }
            } 
        }

        internal Entity StoredEntitiesFinder(Entity row, Entity col)
        {
            foreach (FindStoredEntitiesResult item in FindStoredEntities(new[] { RowKey.Key, ColKey.Key  }))
            {
                if (MatrixData.IsEqual(item.KeyEntity[RowKey.Key.Name], row[RowKey.Key.Name]))
                {
                    if (MatrixData.IsEqual(item.KeyEntity[ColKey.Key.Name], col[ColKey.Key.Name]))
                    {
                        return (Entity)item.ResultEntity;
                    }
                }
            }

            return null;
        }

        internal BLObjectMap GetBLObjectMap(Entity se)
        {
            Type t = bl.GetType();
            var m = t.GetMethod("CreateBLObjects"); 
            var map = (BLObjectMap)m.Invoke(bl, new object[0]); 
            map.Default = se;
            return map;
        }

        internal void Save(BLObjectMap map)
        {
            Type t = bl.GetType();

            foreach (var m in t.GetMethods())
            {
                if (m.Name == "Save")
                {
                    var ps = m.GetParameters();
                    if (ps.Length == 1)
                    {
                        if (ps[0].ParameterType == typeof(BLObjectMap))
                        { 
                            m.Invoke(bl, new object[] { map });
                            return;
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }

        internal object GetMatrixData(object rowKey, object colKey, Field field)
        {
            return MatrixData.GetMatrixData(rowKey, colKey, field);
        }

        internal IEnumerable<FindStoredEntitiesResult> FindStoredEntities(Field[] fields)
        {
            return StoredEntityCollection.FindStoredEntities(fields);
        }

        internal MatrixStoredEntityValues CreateNew()
        {
            var mapdefaultentity = StoredEntityCollection.MapDefaultEntity;
            var matrixdefaultentity = StoredEntityCollection.MatrixDefaultEntity;

            var msev = new MatrixStoredEntityValues(mapdefaultentity, matrixdefaultentity);
            msev.CreateNew(StoredEntityCollection);
            return msev;
        }

        internal IEnumerable<MatrixStoredEntityValues> GetStoreItems()
        {
            return StoredEntityCollection.GetMatrixStoredEntityValues();
        }

        internal void Add(MatrixStoredEntityValues msev)
        {
            StoredEntityCollection.Add(msev);
        }

        internal object GetMatrixData(object rowKey, Field field)
        {
            return MatrixData.GetMatrixData(rowKey, field);
        }

        internal object GetMatrixData(Field field)
        {
            return MatrixData.GetMatrixData(field);
        }

        internal void UpdateStoreValues()
        {
            var cvs = MatrixData.GetChangedValues();

            foreach (var cv in cvs)
            {
                var es = StoredEntityCollection.FindStoredEntities(new[] { cv.Field });
                foreach (var item in es)
                {
                    if (MatrixData.IsEqual(item.KeyEntity[RowKey.Key.Name], cv.RowKey))
                    {
                        if (MatrixData.IsEqual(item.KeyEntity[ColKey.Key.Name], cv.ColKey))
                        {
                            item.ResultEntity[cv.Field.Name] = cv.Value;
                        }
                    }
                }
            }
        } 
    }
}
