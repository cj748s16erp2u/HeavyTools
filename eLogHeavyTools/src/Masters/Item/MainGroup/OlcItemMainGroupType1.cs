using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    partial class OlcItemMainGroupType1
    {
        public override void SetDefaultValues()
        {
            this.Grouplastnum = 0;
        }
        public override void PreSave()
        {
            base.PreSave();
           
            if (!string.IsNullOrEmpty(Imgt1id.Value))
            {
                Imgt1id = Imgt1id.Value.ToUpper();
            }
            
        }
    }
}
