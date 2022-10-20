using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomBL : DefaultBLAbstract
    {
        public static T New<T>() where T : ReceivingStLocCustomBL
        {
            return ObjectFactory.New<T>();
        }

        public static ReceivingStLocCustomBL New()
        {
            return New<ReceivingStLocCustomBL>();
        }

        protected ReceivingStLocCustomBL() { }

        public override void Assign(Key parentEntityKey, List<Key> keysToAssign, out IEnumerable<Key> resultList)
        {
            throw new InvalidOperationException();
        }

        public override BLObjectMap CreateBLObjects()
        {
            throw new InvalidOperationException();
        }

        public override void Delete(List<Key> keys)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Key k)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Key> Hide(ICollection<Key> keys)
        {
            throw new InvalidOperationException();
        }

        public override Key Hide(Key key)
        {
            throw new InvalidOperationException();
        }

        public override bool IsDeletePossible(Key k, out string reason)
        {
            throw new NotImplementedException();
        }

        public override Key Save(BLObjectMap objects)
        {
            throw new NotImplementedException();
        }

        public override Key Save(BLObjectMap objects, string langnamespace)
        {
            throw new NotImplementedException();
        }

        public override Key Save(BLObjectMap objects, string langnamespace, bool skipMerge)
        {
            throw new NotImplementedException();
        }

        public override void Unassign(Key parentEntityKey, List<Key> keysToUnassign)
        {
            throw new InvalidOperationException();
        }

        public override ICollection<Key> Unhide(ICollection<Key> keys)
        {
            throw new InvalidOperationException();
        }

        public override Key Unhide(Key key)
        {
            throw new InvalidOperationException();
        }

        public override void Validate(BLObjectMap objects)
        {
            throw new NotImplementedException();
        }

        protected override Entity GetEntityInternal(Key k)
        {
            throw new NotImplementedException();
        }

        protected override Entity GetNewEntityInternal()
        {
            throw new NotImplementedException();
        }

        protected override bool HasDuplicateInternal(Entity e)
        {
            throw new InvalidOperationException();
        }
    }
}
