using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(ICuponNumberGenerator))]
public class CuponNumberGeneratorService : ICuponNumberGenerator
{
    public string GenerateCupon(int id)
    {
        return CouponCode(Crypt((uint)id));
    }
     
    const string ALPHABET = "ER2TA9SDJHN5FGCVBUZIO8PLK1346M70";
    
    static string CouponCode(uint number)
    {
        var b = new StringBuilder();
        for (int i = 0; i < 6; ++i)
        {
            b.Append(ALPHABET[(int)number & ((1 << 5) - 1)]);
            number >>= 5;
        }
        return b.ToString();
    }

    const int BITCOUNT = 30;
    const int BITMASK = (1 << BITCOUNT / 2) - 1;

    static uint RoundFunction(uint number)
    {
        return (((number ^ 47894) + 25) << 1) & BITMASK;
    }

    static uint Crypt(uint number)
    {
        uint left = number >> (BITCOUNT / 2);
        uint right = number & BITMASK;
        for (int round = 0; round < 10; ++round)
        {
            left = left ^ RoundFunction(right);
            uint temp = left; left = right; right = temp;
        }
        return left | (right << (BITCOUNT / 2));
    }
}
