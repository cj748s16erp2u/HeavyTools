using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using static eProjectWeb.Framework.Data.Key;

namespace eLog.HeavyTools.Masters.Item.Import
{
    public class ColorException
    {

        private static ImpColorExceptions list = null;
        public ColorException() {
            list = ImpColorExceptions.Load(new Key { { ImpColorException.FieldColor1.Name, new NotNullAtToSql() } });
          
            foreach (ImpColorException item in list.AllRows)
            {

                var sql = string.Format(@"select tl1.value c1, tl2.value c2, tl3.value c3, tl4.value s1, tl5.value s2
  from imp_colorexception e
  left join ols_typeline tl1 on tl1.typegrpid=507 and tl1.name=e.color1
  left join ols_typeline tl2 on tl2.typegrpid=507 and tl2.name=e.color2
  left join ols_typeline tl3 on tl3.typegrpid=507 and tl3.name=e.color3
  left join ols_typeline tl4 on tl4.typegrpid=501 and tl4.str1=e.sample1
  left join ols_typeline tl5 on tl5.typegrpid=501 and tl5.str1=e.sample2
  where e.ice={0}", item.Ice);



                foreach (var cs in SqlDataAdapter.Query(DB.Main, sql).AllRows)
                {
                    item.ColorValue1 = ConvertUtils.ToInt32(cs["c1"]);
                    item.ColorValue2 = ConvertUtils.ToInt32(cs["c2"]);
                    item.ColorValue3 = ConvertUtils.ToInt32(cs["c3"]);
                    item.SampleValue1 = ConvertUtils.ToInt32(cs["s1"]);
                    item.SampleValue2 = ConvertUtils.ToInt32(cs["s2"]);

                }
            }
        }

        internal bool IsColorException(string isidCode, string imidCode, out ImpColorException ice)
        {
            foreach (ImpColorException ce in list.AllRows)
            {
                if (ce.Modelnumber== isidCode+ imidCode)
                {
                    ice = ce;
                    return true;
                }
            }
            ice = null;
            return false;
        }
    }
}