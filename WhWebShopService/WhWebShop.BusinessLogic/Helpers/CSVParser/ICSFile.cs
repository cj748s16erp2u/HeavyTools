namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers.CSVParser;

public interface ICSFile <THead, TLine>
{
    public THead GetHead();
    public List<TLine> GetLines();
    public Type GetHeadType();
    public Type GetLineType();

}
