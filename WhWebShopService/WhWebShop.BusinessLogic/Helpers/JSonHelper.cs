﻿using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
  
public class JsonRegexp
{
  
    public static string Integer = "^[0-9]*$";
    public static string Decimal = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
    public static string Date = "^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$";

}
public class JsonParser
{/*
    internal static T Parse<T>(JObject order, string root)
    { 
        
        var o = (T)Activator.CreateInstance(typeof(T))!;
        Parse(o, order, root);
        return o; 
    }*/

    protected static object Parse(Type t, JObject order, string root, bool isRoot)
    {
        var o = Activator.CreateInstance(t)!;
        ParseInternal(t, o, order, root, isRoot);
        return o;
    }


    internal static void ParseInternal(Type tt, object o, JObject jo, string root, bool isRoot)
    {
        var properties = tt.GetProperties();

        foreach (var item in properties)
        {
            try
            {
                foreach (var a in item.GetCustomAttributes(false))
                {
                    var ja = a as JsonFieldAttribute;
                    if (ja != null)
                    {
                        var found = jo.ContainsKey(item.Name);

                        if (isRoot)
                        {
                            if (ja.Mandotary && !found)
                            {
                                throw new Exception("Missing item(Mandotary)");
                            }
                        } else
                        {

                            if (ja.Mandotarysub && !found)
                            {
                                throw new Exception("Missing item(MandotarySub)");
                            }
                        }

                        if (found)
                        {
                            Type t = item.PropertyType;
                            object v = null!;

                            if (t == typeof(string))
                            {
                                v = jo[item.Name]!.Value<string>()!;
                            }
                            else if (t == typeof(int))
                            {
                                v = jo[item.Name]!.Value<int>()!;
                            }
                            else if (t == typeof(int?))
                            {
                                v = jo[item.Name]!.Value<int?>()!;
                            }
                            else if (t == typeof(decimal))
                            {
                                v = jo[item.Name]!.Value<decimal>()!;
                            }
                            else if (t == typeof(decimal?))
                            {
                                v = jo[item.Name]!.Value<decimal?>()!;
                            }
                            else if (t == typeof(DateTime?))
                            {
                                v = jo[item.Name]!.Value<DateTime?>()!;
                            }
                            else if (t == typeof(string[]))
                            {
                                v = jo[item.Name]!.Values<string>()!.ToArray();
                            }
                            else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                var subitem = jo[item.Name]!.ToArray();
                                Type itemType = t.GetGenericArguments()[0];

                                var m = t.GetMethod("Add");

                                foreach (var ai in subitem)
                                {
                                    var oo = Parse(itemType, ai.ToObject<JObject>()!, root + "/" + item.Name, false);

                                    m!.Invoke(item.GetValue(o, null), new[] { oo });
                                }
                                continue;
                            }
                            else if (t.BaseType == typeof(object))
                            {
                                var subitem = jo[item.Name]!.ToObject<JObject>();
                                v = Parse(t, subitem!, root + "/" + item.Name, false);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid field value" + item.Name);
                            }
                            item.SetValue(o, v);
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ArgumentException(root + "/" + item.Name + ": " + e.Message);
            }

        }
    }

    internal static T ParseObject<T>(JObject parms)
    {
        if (parms == null)
        {
            throw new Exception("Json parse error");
        }
        foreach (var a in typeof(T).GetCustomAttributes(false))
        {
            var ja = a as JsonObjectAttributes;
            if (ja != null)
            {
                var jsonRoot = ja.ObjectName;
 
                var order = parms[jsonRoot];

                return (T)Parse(typeof(T), order!.ToObject<JObject>()!, jsonRoot, true);
            }
            
        }
        throw new Exception("Missing JsonObjectAttributes attributes");
    }
}