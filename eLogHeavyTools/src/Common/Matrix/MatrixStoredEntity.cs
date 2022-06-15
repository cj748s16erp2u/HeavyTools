using eLog.HeavyTools.Masters.Item.ItemMatrix;
using eLog.HeavyTools.Masters.Item.Model;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common.Matrix
{
    public class MatrixStoredEntity
    {
        List<MatrixStoredEntityValues> StoredEntity;

        public readonly Type MapDefaultEntity; 
        public readonly Type MatrixDefaultEntity;

        private List<Type> Types = new List<Type>();

        private List<Entity> EntityType = new List<Entity>();
         
        public MatrixStoredEntity(MatrixStoredEntityConverter msec)
        {
            MapDefaultEntity = msec.MapDefaultEntity;
            MatrixDefaultEntity = msec.MatrixDefaultEntity;
            StoredEntity = new List<MatrixStoredEntityValues>();
            StoredEntity.AddRange(msec.GetMatrixStoredEntityValues());
            Types.AddRange(msec.GetTypes());

            foreach (var t in Types)
            {
                EntityType.Add(CreateEntity(t));
            }
        }

        public List<MatrixStoredEntityValues> GetMatrixStoredEntityValues()
        {
            return StoredEntity;
        }
        
        internal IEnumerable<FindStoredEntitiesResult> FindStoredEntities(Field[] fields)
        {
            var res = new List<FindStoredEntitiesResult>();

            foreach (var e in EntityType)
            {
                var c = 0;
                foreach (var f in fields)
                {
                    try
                    {
                        //Megnézzük, van-e ilyen mező benne
                        var v = e[f.Name];
                        c++;
                    }
                    catch (Exception)
                    {
                        break;
                    }

                }
                if (c == fields.Count())
                { 
                    foreach (var item in StoredEntity)
                    {
                        var ne = new FindStoredEntitiesResult();

                        ne.ResultEntity = item.Entities[e.GetType()];
                        ne.KeyEntity = item.Entities[item.MatrixDefaultEntity];
                        res.Add(ne);
                    }
                    break;
                }
            }
            return res;
        }

        internal void CreateNew(MatrixStoredEntityValues matrixStoredEntityValues)
        {
            matrixStoredEntityValues.MapDefaultEntity = MapDefaultEntity;
            matrixStoredEntityValues.MatrixDefaultEntity = MatrixDefaultEntity;

            foreach (var t in Types)
            {
                var e = CreateEntity(t);

                matrixStoredEntityValues.Entities.Add(t, e);
            } 
        }

        private Entity CreateEntity(Type t)
        {
            return (Entity)t.GetMethod("CreateNew",
                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).
                       Invoke(null, null);
        }

        internal void Add(MatrixStoredEntityValues msev)
        {
            StoredEntity.Add(msev);
        } 
    }

    public class FindStoredEntitiesResult
    {
        public Entity ResultEntity;
        public Entity KeyEntity;
    }

    public class MatrixStoredEntityItem
    {
        /* kapcsolat a olc táblával */
        public Type RootEntity; 
        public Field RootField;
         
        public EntityCollection StoredEntityCollection;
        public Type StoredEntity; 

        public MatrixStoredEntityItem(EntityCollection ec, Type entity)
        {
            StoredEntityCollection = ec;
            StoredEntity = entity;
        }

        public MatrixStoredEntityItem(EntityCollection ec, Type entity, Type rootEntity, Field rootField) : this(ec, entity)
        {
            StoredEntityCollection = ec;
            StoredEntity = entity;
            RootEntity = rootEntity;
            RootField = rootField;
        }

         

        internal IEnumerable<Entity> GetEntitiesList()
        {
            var l = new List<Entity>();

            foreach (Entity e in StoredEntityCollection.AllRows)
            {
                l.Add(e);
            }

            return l;
        }
    }


    public class MatrixStoredEntityValues
    {
        public Dictionary<Type, Entity> Entities = new Dictionary<Type, Entity>();
         
        public Type MapDefaultEntity;
        public Type MatrixDefaultEntity;


        public MatrixStoredEntityValues(Type mapdefaultentity, Type matrixdefaultentity)
        {
            MapDefaultEntity = mapdefaultentity;
            MatrixDefaultEntity = matrixdefaultentity;
        }

        internal Entity FindEntity(Field[] fields)
        {
            foreach (var e in Entities)
            {
                var c = 0;
                foreach (var f in fields)
                {
                    try
                    {
                        //Megnézzük, van-e ilyen mező benne
                        var v=e.Value[f.Name];
                        c++;
                    }
                    catch (Exception)
                    {
                        break;
                    }

                }
                if (c == fields.Count())
                {
                    return e.Value;
                }
            }
            throw new ArgumentException();
        }

        public Entity GetMapDefaultEntity()
        {
            return Entities[MapDefaultEntity];
        }

        internal void FillNonDefaultEntity(BLObjectMap map)
        {
            foreach (var item in Entities)
            {
                if (item.Key!= MapDefaultEntity)
                { 
                    map.Add(item.Key.Name, item.Value);
                }
            }
        }

        internal void CreateNew(MatrixStoredEntity storedEntityCollection)
        {
            storedEntityCollection.CreateNew(this);
        }
    }


    public class MatrixStoredEntityConverter
    {
        public List<MatrixStoredEntityItem> StoredEntity = new List<MatrixStoredEntityItem>();

        public Type MatrixDefaultEntity;
        public Type MapDefaultEntity;

        public List<Type> Types = new List<Type>();

        public List<MatrixStoredEntityValues> GetMatrixStoredEntityValues()
        { 
            var l = new List<MatrixStoredEntityValues>();
            foreach (var se in StoredEntity)
            {
                if (se.StoredEntity == MatrixDefaultEntity)
                {
                    foreach (Entity row in se.StoredEntityCollection.AllRows)
                    {
                        var i = new MatrixStoredEntityValues(MapDefaultEntity, MatrixDefaultEntity);
                        i.Entities.Add(se.StoredEntity, row);
                        l.Add(i);
                    }
                } else
                {
                    foreach (Entity e in se.StoredEntityCollection.AllRows)
                    {
                        var rootkey = e[se.RootField];

                        foreach (var item in l)
                        {
                            var re = item.Entities[se.RootEntity];
                            if (re == null)
                            {
                                throw new Exception("MissingRootElement");
                            }
                            var eKey = re[se.RootField];

                            if (MatrixData.IsEqual(rootkey, eKey))
                            {
                                item.Entities.Add(e.GetType(), e);
                            }
                        }
                    }
                }
            }

            return l;
        }

        internal void Add(MatrixStoredEntityValues msev)
        { 
            foreach (var e in msev.Entities)
            {
                foreach (var item in StoredEntity)
                {
                    if (item.StoredEntity == e.Key)
                    {
                        item.StoredEntityCollection.AddRow(e.Value);
                    }
                } 
            } 
        }
        internal void Add(EntityCollection ec, Type entity, Type rootEntity, Field rootField)
        {
            Types.Add(entity);
            StoredEntity.Add(new MatrixStoredEntityItem(ec, entity, rootEntity, rootField));
        }
        internal void Add(EntityCollection ec, Type entity)
        {

            Types.Add(entity);
            StoredEntity.Add(new MatrixStoredEntityItem(ec, entity));
        }

        internal IEnumerable<Type> GetTypes()
        {
            return Types;
        }

        public MatrixStoredEntityConverter(Type matrixdefaultentity, Type mapdefaultentity)
        {
            MatrixDefaultEntity = matrixdefaultentity;
            MapDefaultEntity = mapdefaultentity;
        }
    }

}
