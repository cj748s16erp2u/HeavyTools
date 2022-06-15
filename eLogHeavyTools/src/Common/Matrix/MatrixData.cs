using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common.Matrix
{ 

    public class Fields : Dictionary<Field, object> {
        public Dictionary<Field, object> GetFields()
        {
            return this;
        }
        List<Field> Changed = new List<Field>();

        internal object GetValue(Field f)
        {
            foreach (var item in this)
            {
                if (item.Key.Name == f.Name)
                {
                    return item.Value;
                }
            }
            return null;
        }
        internal void SetChanged(Field f)
        {
            foreach (var item in Changed)
            {
                if (item.Name == f.Name)
                {
                    return;
                }
            } 
            Changed.Add(f);
        }

        internal bool AddFields(Field f, object value, bool disabled)
        {
            foreach (var item in this)
            {
                if (item.Key.Name == f.Name)
                {
                    if (disabled)
                    {
                        if (this[f] == null)
                        {
                            return false;
                        }
                    }
                    this[f] = value;
                    return true;
                }
            }
            this.Add(f, value);
            return true;
        }

        internal Dictionary<Field, object> GetChangedValue()
        {
            var c = new Dictionary<Field, object>();

            foreach (var field in Changed)
            {
                c.Add(field, GetValue(field));
            }

            return c;
        }
    }

    public class Colls : Dictionary<object, Fields>
    {
        public Dictionary<object, Fields> GetColls()
        {
            return this;
        }
    }

    public class Rows : Dictionary<object, Colls>
    {
        public Dictionary<object, Colls> GetRows()
        {
            return this;
        }
    }


    public class MatrixData
    {
        private List<object> ColKeys = new List<object>();
        private List<object> RowKeys = new List<object>();

        private Rows Data = new Rows();

        internal void AddColKey(object key)
        {
            foreach (var row in RowKeys)
            {
                var colls = Data[row];
                if (!colls.ContainsKey(key))
                {
                    colls.Add(key, new Fields());
                } 
            }
        }

        internal void AddRowKey(object key)
        {
            RowKeys.Add(key);
            if (!Data.ContainsKey(key))
            {
                Data.Add(key, new Colls());
            }
        }

        internal void AddValue(object rowId, object colId, Field f, object value)
        {  if (!Data.ContainsKey(rowId))
            {
                Data.Add(rowId, new Colls());
            }
            var dataRow = Data[rowId];

            if (!dataRow.ContainsKey(colId))
            {
                dataRow.Add(colId, new Fields());
            }
            var dataCol = dataRow[colId];

            dataCol.AddFields(f, value, false); ;
        }

        internal bool IsEqual(Field field)
        {
            foreach (var item in Data.Keys)
            {
                var e = IsEqual(item, field);
                if (!e)
                {
                    return false;
                }
            }

            return true;
        }
        internal bool IsEqual2(Field field)
        {
            foreach (var item in Data.Keys)
            {
                var e = IsEqual2(item, field);
                if (!e)
                {
                    return false;
                }
            }

            return true;
        }

        internal bool IsEqual(object rowId, Field field)
        {
            object lasto = null;
            if (Data.ContainsKey(rowId))
            {
                foreach (var item in Data[rowId])
                {
                    var v = item.Value.GetValue(field);
                     
                    if (lasto == null)
                    {
                        lasto = v;
                    }
                    if (!IsEqual(lasto, v))
                    {
                        return false;
                    }
                } 
            } 

            return true;
        }
        internal bool IsEqual2(object rowId, Field field)
        {
            object lasto = null;
            if (Data.ContainsKey(rowId))
            {
                foreach (var item in Data[rowId])
                {
                    var v = item.Value.GetValue(field);

                    if (lasto == null)
                    {
                        lasto = v;
                    }
                    if (!IsEqual2(lasto, v))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public static bool IsEqual(object value1, object value2)
        {
            if (value1 == null && value2 == null)
            {
                return true;
            }
            if (value1 == null || value2 == null)
            {
                return false;
            }
            if (value1.ToString() != value2.ToString())
            {
                return false;
            }
            return true;
        }


        public static bool IsEqual2(object value1, object value2)
        {
            if (value1 == null || value2 == null)
            {
                return true;
            }
            if (value1.ToString() != value2.ToString())
            {
                return false;
            }
            return true;
        }


        internal object GetMatrixData(object rowKey, object colKey, Field field)
        {
            if (Data.ContainsKey(rowKey))
            {
                var row = Data[rowKey];
                if (row.GetColls().ContainsKey(colKey))
                {
                    var cols = row[colKey];

                    var v = cols.GetValue(field);
                    return v;
                } 
            }

            return null;
        }
         

        internal object GetMatrixData(object rowKey, Field field)
        {
            bool eq = true;
            object v = null;
            if (Data.ContainsKey(rowKey))
            {
                var row = Data[rowKey];
                 
                foreach (var item in row.GetColls())
                {
                    var cols = item.Value;
                    var vv = cols.GetValue(field);
                    if (v == null)
                    {
                        v = vv;
                    } else
                    {
                        if (!IsEqual2(v, vv))
                        {
                            eq = false;
                        }
                    }
                } 
            }
            if (!eq)
            {
                return null;
            }
            return v;
        }

        internal object GetMatrixData(Field field)
        {
            //összevont
            bool eq = true;
            object v = null;

            foreach (var rw in Data.GetRows())
            {
                var row = rw.Value;

                foreach (var item in row.GetColls())
                {
                    var cols = item.Value;
                    var vv = cols.GetValue(field);
                    if (v == null)
                    {
                        v = vv;
                    }
                    else
                    {
                        if (!IsEqual2(v, vv))
                        {
                            eq = false;
                        }
                    }
                }
            }
            if (!eq)
            {
                return null;
            }
            return v;
        }



        public void SetMatrixData(object rowKey, IMatrixEditItem imei, object value, bool disabled)
        {
            var row = Data[rowKey];
            foreach (var item in row.GetColls())
            {
                var cols2 = item.Value;
                if (cols2.AddFields(imei.GetField(), value, disabled))
                {
                    cols2.SetChanged(imei.GetField());
                }
            }
        }

        public void SetMatrixData(IMatrixEditItem imei, object value, bool disabled)
        {
            //összevont
            foreach (var rw in Data.GetRows())
            {
                var row = rw.Value;

                foreach (var item in row.GetColls())
                {
                    var cols = item.Value;
                    if (cols.AddFields(imei.GetField(), value, disabled))
                    {
                        cols.SetChanged(imei.GetField());
                    }
                }
            }
        }

        internal List<ChangedValues> GetChangedValues()
        {
            var vs = new List<ChangedValues>();

            foreach (var row in Data.GetRows())
            {
                foreach (var col in row.Value.GetColls())
                {
                    var changed = col.Value.GetChangedValue();

                    foreach (var changedItem in changed)
                    {
                        var cv = new ChangedValues
                        {
                            RowKey = row.Key,
                            ColKey = col.Key,
                            Field = changedItem.Key,
                            Value = changedItem.Value
                        };
                        vs.Add(cv);
                    } 
                }
            }
            return vs;
        }
    }

    public class ChangedValues
    {
        public object RowKey;
        public object ColKey;
        public Field Field;
        public object Value;

    }
}
