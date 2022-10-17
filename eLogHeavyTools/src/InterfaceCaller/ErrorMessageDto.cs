using eLog.Base.Common;
using eLog.HeavyTools.Common.Json;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{
    public class ErrorMessageDto
    {
        [JsonFieldAttribute(false)]
        public MessageExceptionResultDto ErrorMessage { get; set; }

        internal void CheckResult()
        {
            if (ErrorMessage != null)
            {
                if (ErrorMessage.ErrKey == null)
                {
                    if (ErrorMessage.NameSpace != null)
                    {
                        if (ErrorMessage.Param == null)
                        {
                            var msg = Translator.TranslateNspace(ErrorMessage.Id, ErrorMessage.NameSpace);
                            throw new MessageException(msg); 

                        } else
                        {
                            var msg = Translator.TranslateNspace(ErrorMessage.Id, ErrorMessage.NameSpace, ErrorMessage.Param);
                            throw new MessageException(msg);
                        }
                    } else
                    {
                        if (ErrorMessage.Param == null)
                        {
                            var msg = Translator.Translate(ErrorMessage.Id);
                            throw new MessageException(msg);
                        } else
                        {
                            var msg = Translator.Translate(ErrorMessage.Id, ErrorMessage.Param);
                            throw new MessageException(msg);
                        }
                    }

                }
                else
                {
                    throw new MessageException(Utils.ListToString(SqlFunctions.GetErrLog(ErrorMessage.ErrKey)));
                }
            }
        }
    }
}
