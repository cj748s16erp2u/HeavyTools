namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;

public class PrcItem
{
    public decimal? Prc { get; set; }
    public string Curid { get; set; }
    public int Ptid { get; set; }
    public int Prctype { get; set; }
    public string Wid { get; set; }
    public int Order { get; set; }
    public int? Imid { get; set; }
}

public class ModelSeasonItem
{
    public int? Imid { get; set; }
    public int? Imsid { get; set; }
}

public class OlcItemItem
{
    public int? Imsid { get; set; }
    public int? Itemid { get; set; }
}
