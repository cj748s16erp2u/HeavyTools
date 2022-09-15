using System.Globalization;
using System.Reflection;
using System.Text;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers.CSVParser;

internal class CSVParser
{
    internal static void Parse2<THead, Tline>(CSFile<THead, Tline> o, string csv)
        where THead : class
        where Tline : class
    {

        var csvReader = new CsvReader(new StringReader(csv), "|");


        var line = 0;

        var headColumns = GetColumnsFromType(o.GetHeadType());
        var lineColumns = GetColumnsFromType(o.GetLineType());


        while (csvReader.Read())
        {
            if (line == 0)
            {
                var head = o.GetHead();
                foreach (var col in headColumns)
                {
                    col.Key.SetValue(head, csvReader[col.Value.index]);
                }
            }

            var newwLine = o.GetNewLine();
            foreach (var col in lineColumns)
            {
                col.Key.SetValue(newwLine, csvReader[col.Value.index]);
            }
            line++;
        }

        if (line == 0) { 
            throw new Exception("CannotReadCSV");
        } 
    }

    private static Dictionary<PropertyInfo, CSVAttribute> GetColumnsFromType(Type type)
    {
        var columns = new Dictionary<PropertyInfo, CSVAttribute>();
       
       foreach (System.Reflection.PropertyInfo p in type.GetProperties())
       {
           object[] attrs = p.GetCustomAttributes(true);

           foreach (var attr in attrs)
           {
               CSVAttribute? cSVAttribute = attr as CSVAttribute;
               if (cSVAttribute != null)
               {
                    columns.Add(p, cSVAttribute);
               }
           }
       }

        return columns;
    }
}
public enum CSVLineType
{
    Head = 0,
    Line = 1
}

public class CSFile<THead, Tline> : ICSFile<THead, Tline> where THead : class where Tline: class
{
    public CSFile()
    {
        Head = (THead)Activator.CreateInstance(GetHeadType())!;
        Lines = new List<Tline>();
    }

    private THead Head { get; set; }

    private List<Tline> Lines { get; set; } = new List<Tline>();

    internal static CSFile<THead, Tline> ParseCSVText(string csv)
    {
        var o = new CSFile<THead, Tline>();

        CSVParser.Parse2(o, csv);
        return o;
    }

    public THead GetHead()
    {
        return Head;

    }

    public Type GetHeadType()
    {
        return typeof(THead);
    }

    public List<Tline> GetLines()
    {
        return Lines;
    }

    public Type GetLineType()
    {
        return typeof(Tline);
    }

    internal Tline GetNewLine()
    {
        var l = (Tline)Activator.CreateInstance(GetLineType())!;
        Lines.Add(l);
        return l;
    }
}