using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Warehouse.WhZone.Common
{
    internal static class WhZTranUtils
    {
        /// <summary>
        /// Basic authentikacius kulcs eloallitasa
        /// </summary>
        /// <returns>Basic authentikacios kulcs</returns>
        public static string CreateAuthentication()
        {
            var userId = GetCustomSettingsValue("WhZoneService-UserID");
            var password = GetCustomSettingsValue("WhZoneService-Password");
            if (password.StartsWith("*"))
            {
                password = password.Substring(1);
            }
            else
            {
                // TODO decrypt
            }

            var auth = $"{userId}:{password}";
            var authBase64 = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            return authBase64;
        }

        /// <summary>
        /// Szolgaltatas url kiolvasasa a beallitasokbol
        /// </summary>
        /// <returns>A szolgaltatas eleresi utvonala</returns>
        public static string GetServiceUrl()
        {
            return GetCustomSettingsValue("WhZoneService-ServiceUrl");
        }

        /// <summary>
        /// Parameter kiolvasasa es validalasa a CustomSettings-bol
        /// </summary>
        /// <param name="key">Paremter kulcs</param>
        /// <returns>Parameter ertek</returns>
        /// <exception cref="MessageException">A parameter nincs megadva vagy nincs erteke</exception>
        public static string GetCustomSettingsValue(string key)
        {
            var value = CustomSettings.GetString(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new MessageException("$err_whzone_optionmissing".eLogTransl(key));
            }

            return value;
        }

        /// <summary>
        /// Hibauzenet formazasa
        /// </summary>
        /// <param name="message">Hibauzenet</param>
        /// <returns>Formazott uzenet</returns>
        public static string ParseErrorResponse(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || message.Length < 2)
            {
                return message;
            }

            message = message.Substring(1, message.Length - 2);
            message = message.Replace("\\r", "\r").Replace("\\n", "\n");

            return message;
        }
    }
}
