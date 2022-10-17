using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;

public class MessageException : Exception
{
    public string Id { get; set; }
    public string NameSpace { get; set; }
    public object[] Param { get; set; }
 
    public string ErrKey { get; set; }

    public MessageException(string errkey)
    {
        ErrKey = errkey;
        Id = null!;
        NameSpace = null!;
        Param = null!;
    }


    public MessageException(string id, string nspace, params object[] param)
    {
        Id = id;
        NameSpace = nspace;
        Param = param;
        ErrKey = null!;
    }
    public MessageExceptionResultDto GetDto()
    {
        return new MessageExceptionResultDto()
        {
            Id = Id,
            NameSpace = NameSpace,
            Param = Param,
            ErrKey = ErrKey
        };
    } 
}
