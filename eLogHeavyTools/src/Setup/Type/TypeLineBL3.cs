using eProjectWeb.Framework;
using eProjectWeb.Framework.BL; 

namespace eLog.HeavyTools.Setup.Type
{
    public  class TypeLineBL3 : DefaultBL1<Base.Setup.Type.TypeLine, Base.Setup.Type.TypeLineRules>
    {
        public static readonly string ID = typeof(TypeLineBL3).FullName;

        protected TypeLineBL3()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }
        public static TypeLineBL3 New()
        {
            return (TypeLineBL3)ObjectFactory.New(typeof(TypeLineBL3));
        }
    }
}
