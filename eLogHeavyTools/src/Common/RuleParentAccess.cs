using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common
{
    internal class RuleParentAccess
    {
        private readonly Dictionary<string, object> _dict = new Dictionary<string, object>();

        private readonly RuleValidateContext _ctx;

        public RuleParentAccess(RuleValidateContext ctx)
        {
            _ctx = ctx;
            ctx.InternalCustomData = _dict;
        }

        public void AddTable(string name)
        {
            Entity entity = null;
            object parent = _ctx.GetParentObject();
            var iparent = parent as INamedObjectCollection;
            if (iparent != null)
            {
                entity = (Entity)iparent.Get(name);
            }
            if (entity == null && _dict.ContainsKey(name))
                _dict.Remove(name);

            if (entity != null)
                _dict[name] = entity;
        }

        private Entity GetParent(string name)
        {
            var dict = _ctx.InternalCustomData as Dictionary<string, object>;
            if (dict != null)
            {
                object h;
                if (dict.TryGetValue(name, out h))
                    return h as Entity;
            }
            return null;
        }

        public T GetParent<T>() where T : Entity
        {
            var e = GetParent(typeof(T).Name);
            return (T)e;
        }
    }
}