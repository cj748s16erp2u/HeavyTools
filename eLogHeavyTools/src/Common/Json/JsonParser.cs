using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common.Json
{

    public static class JObjectExtensions {
        public static bool ContainsKey(this JObject jo, string key, out JToken val)
        {             
            if (jo.TryGetValue(key, StringComparison.InvariantCultureIgnoreCase ,out var value))
            {
                val = value;
                return true;
            }
            val = null;
            return false; 
        }
    }

    public class JsonParser
    {
        protected static object Parse(Type t, JObject order, string root, int deep = -1)
        {
            deep++;
            var o = Activator.CreateInstance(t);
            ParseInternal(t, o, order, root, deep);
            return o;
        }


        internal static void ParseInternal(Type tt, object o, JObject jo, string root, int deep)
        {
            var properties = tt.GetProperties();

            foreach (var item in properties)
            {
                var vvt = "???";
                try
                {
                    foreach (var a in item.GetCustomAttributes(false))
                    {
                        var ja = a as JsonFieldAttribute;
                        if (ja != null)
                        {
                            JToken outjk = null;

                            if (item.Name=="Items")
                            {
                                var p = 1;
                            }

                            var found = jo.ContainsKey(item.Name, out outjk);

                            if (ja.IsMandotary(deep) && !found)
                            {
                                var itemidFound = false;
                                // Ha itemid és nincs itemcode, akkor nem állunk meg
                                if (item.Name == "ItemCode")
                                {
                                    foreach (var p2 in properties)
                                    {
                                        if (p2.Name == "itemid")
                                        {
                                            var vvv = "";
                                           
                                            var vv = outjk;
                                            if (vv != null)
                                            {
                                                vvv = vv.ToString();
                                            }
                                           
                                            if (!string.IsNullOrEmpty(vvv))
                                            {
                                                itemidFound = true;
                                            }

                                        }
                                    }
                                }
                                if (!itemidFound)
                                {
                                    throw new Exception($"Missing item(Mandotary) deep={deep}");
                                }
                            }

                            if (found)
                            {
                                var vv = outjk;
                                if (vv != null)
                                {
                                    vvt = vv.ToString();
                                }
                                Type t = item.PropertyType;
                                object v = null;

                                if (t == typeof(string))
                                {
                                    v = outjk.Value<string>();
                                }
                                else if (t == typeof(bool))
                                {
                                    v = outjk.Value<bool>();
                                }
                                else if (t == typeof(int))
                                {
                                    v = outjk.Value<int>();
                                }
                                else if (t == typeof(int?))
                                {
                                    v = outjk.Value<int?>();
                                }
                                else if (t == typeof(decimal))
                                {
                                    v = outjk.Value<decimal>();
                                }
                                else if (t == typeof(decimal?))
                                {
                                    v = outjk.Value<decimal?>();
                                }
                                else if (t == typeof(DateTime?))
                                {
                                    v = outjk.Value<DateTime?>();
                                }
                                else if (t == typeof(string[]))
                                {
                                    v = outjk.Values<string>().ToArray();
                                }
                                else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    var subitem = outjk.ToArray();
                                    Type itemType = t.GetGenericArguments()[0];

                                    var m = t.GetMethod("Add");

                                    foreach (var ai in subitem)
                                    {
                                        var oo = Parse(itemType, ai.ToObject<JObject>(), root + "/" + item.Name, deep);
                                        m.Invoke(item.GetValue(o, null), new[] { oo });
                                    }
                                    continue;
                                }
                                else if (t.BaseType == typeof(object))
                                {
                                    var subitem = outjk.ToObject<JObject>();
                                    v = Parse(t, subitem, root + "/" + item.Name, deep);
                                } else if (t.BaseType == typeof(Array))
                                {
                                    var subitem = outjk.ToObject<JArray>();

                                    var m = t.GetMethod("Add");
                                    Type itemType = t.GetGenericArguments()[0];

                                    foreach (var item2 in subitem)
                                    {
                                        var oo = Parse(itemType, item2.ToObject<JObject>(), root + "/" + item.Name, deep);
                                        m.Invoke(item.GetValue(o, null), new[] { oo });
                                    }
                                    continue;
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
                    throw new ArgumentException(root + "/" + item.Name + ": '" + vvt + "', error: " + e.Message);
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
                    if (!string.IsNullOrEmpty(jsonRoot))
                    {
                        var order = parms[jsonRoot];
                        if (order == null)
                        {
                            throw new Exception("Missing root element");
                        }
                        return (T)Parse(typeof(T), order.ToObject<JObject>(), jsonRoot);
                    } else
                    {
                        return (T)Parse(typeof(T), parms, "");
                    }
                }

            }
            throw new Exception("Missing JsonObjectAttributes attributes");
        }
    }
}