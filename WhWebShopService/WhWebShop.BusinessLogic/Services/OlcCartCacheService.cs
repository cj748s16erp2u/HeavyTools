using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using System.Collections.Concurrent; 
using System.Runtime.Serialization;
using System.Xml; 
using System.Security.Cryptography;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using System.Reflection;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOlcCartCacheService))]
public class OlcCartCacheService : IOlcCartCacheService
{
    /// <summary>
    /// Kosár adatok
    /// </summary>
    public static ConcurrentDictionary<string, CartCacheItem>
        carthashs = new ConcurrentDictionary<string, CartCacheItem>();

    /// <summary>
    /// Akciók és kosár összerendezés
    /// </summary>
    public static ConcurrentDictionary<int, string> aidHashs = new ConcurrentDictionary<int, string>();
     
    void IOlcCartCacheService.Add(CalcJsonParamsDto cart, CalcJsonResultDto res)
    {
        var hash = GenerateHashKey(cart);
        carthashs.TryAdd(hash, new CartCacheItem() { CalcJsonResultDto = res });
        foreach (var item in res.Items)
        {
            if (item.Aid.HasValue)
            {
                aidHashs.TryAdd(item.Aid.Value, hash);
            }
        }
    }
    public static string GenerateHashKey(ICloneable oObjectOriginal)
    {
        var oObject = oObjectOriginal.Clone() as CalcJsonParamsDto;

        if (oObject == null)
        {
            return string.Empty;
        }

        foreach (var item in oObject.Items)
        {
            foreach (PropertyInfo p in item.GetType().GetProperties())
            {
                object[] attrs = p.GetCustomAttributes(true);

                foreach (var attr in attrs)
                {
                    var co = attr as JsonFieldAttribute;
                    if (co != null)
                    {
                        if (co.DeleteAnotherfield)
                        {
                            var v = p.GetValue(item);
                            if (v != null)
                            {
                                SetValueToNull<CartItemJson>(item, co.Condition);
                            }
                        }
                    }
                }
            } 
        }

        var serializer = new DataContractSerializer(typeof(CalcJsonParamsDto));
        using (var memoryStream = new MemoryStream())
        {
            XmlWriter writer = XmlDictionaryWriter.CreateBinaryWriter(memoryStream);
            serializer.WriteObject(memoryStream, oObject);
            byte[] serializedData = memoryStream.ToArray();

            var SHA = new SHA512CryptoServiceProvider();
            byte[] hash = SHA.ComputeHash(serializedData);

            var h = Convert.ToBase64String(hash);
            return h;
        }
    }

    private static void SetValueToNull<T>(object oObject, string field)
    {
        foreach (var p in typeof(T).GetFields())
        {
            if (p.Name == field)
            {
                p.SetValue(oObject, null);
            }
        }
    }

    void IOlcCartCacheService.RemoveCartByAction(int aid)
    {
        if (aidHashs.TryGetValue(aid, out var hash))
        {
            carthashs.TryRemove(hash, out var res);
        }
        aidHashs.TryRemove(aid, out var h);
    }

    bool IOlcCartCacheService.TryGet(CalcJsonParamsDto cart, out CalcJsonResultDto outcjrd)
    {
        var hash = GenerateHashKey(cart);
        if (carthashs.TryGetValue(hash, out var cjrd))
        {
            if (cjrd != null)
            {
                if (cjrd.Expire > DateTime.Now)
                {
                    outcjrd = cjrd.CalcJsonResultDto;
                    return true;
                } else
                {
                    carthashs.TryRemove(hash, out var c);
                }
            }
        }
        outcjrd = null!;
        return false;
    }

    public void Reset()
    {
        carthashs = new ConcurrentDictionary<string, CartCacheItem>();
        aidHashs = new ConcurrentDictionary<int, string>();

    }
}
