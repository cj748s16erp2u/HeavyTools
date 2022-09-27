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
using System.Text;
using System.Runtime.Serialization.Formatters.Binary; 

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
     
    void IOlcCartCacheService.Add(string hash, CalcJsonResultDto res)
    {
        carthashs.TryAdd(hash, new CartCacheItem() { CalcJsonResultDto = res });
        foreach (var item in res.Items)
        {
            if (item.Aid.HasValue)
            {
                aidHashs.TryAdd(item.Aid.Value, hash);
            }
        }
    }

    public static byte[] ObjectToByteArray(object obj)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(obj);
        return Encoding.UTF8.GetBytes(json);
    }

    void IOlcCartCacheService.RemoveCartByAction(int aid)
    {
        if (aidHashs.TryGetValue(aid, out var hash))
        {
            carthashs.TryRemove(hash, out var res);
        }
        aidHashs.TryRemove(aid, out var h);
    }

    bool IOlcCartCacheService.TryGet(string hash, out CalcJsonResultDto outcjrd)
    {
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

    public string GenerateHash(CalcJsonParamsDto cart)
    {
        byte[] serializedData = ObjectToByteArray(cart);
         
        var h = ComputeSHA256Hash(serializedData);
        return h;
    }

    public static string ComputeSHA256Hash(byte[] bytes)
    {
        using (var sha256 = new SHA256Managed())
        {
            return BitConverter.ToString(sha256.ComputeHash(bytes)).Replace("-", "");
        }
    }
}
