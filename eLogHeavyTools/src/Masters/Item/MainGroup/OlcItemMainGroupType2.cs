using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    partial class OlcItemMainGroupType2
    {
        public override void SetDefaultValues()
        {
        }
        public override void PreSave()
        {
            base.PreSave();

            if (!string.IsNullOrEmpty(Imgt2id.Value))
            {
                Imgt2id = Imgt2id.Value.ToUpper();
            }

        }
    }
}
