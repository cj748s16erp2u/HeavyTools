using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;



[Table("olc_b2bpartnertmp")]
public class B2BPartnerTmp : BusinessEntity
{

    [Key]
    [Column("id", TypeName = "bigint")]
    public int? Id { get; set; }= null!;

    [Column("partncode", TypeName = "varchar(200)")]
    public string? Partncode { get; set; }= null!;

    [Column("name", TypeName = "varchar(200)")]
    public string? Name { get; set; }= null!;

    [Column("selname", TypeName = "varchar(200)")]
    public string? selname { get; set; }= null!;

    [Column("selcountry", TypeName = "varchar(200)")]
    public string? Selcountry { get; set; }= null!;

    [Column("seladdrdistrict", TypeName = "varchar(200)")]
    public string? Seladdrdistrict { get; set; }= null!;

    [Column("seladdrplace", TypeName = "varchar(200)")]
    public string? Seladdrplace { get; set; }= null!;

    [Column("seladdrplacetype", TypeName = "varchar(200)")]
    public string? Seladdrplacetype { get; set; }= null!;

    [Column("seladdrhnum", TypeName = "varchar(200)")]
    public string? Seladdrhnum { get; set; }= null!;

    [Column("seladdrbuilding", TypeName = "varchar(200)")]
    public string? Seladdrbuilding { get; set; }= null!;

    [Column("seladdrstairway", TypeName = "varchar(200)")]
    public string? Seladdrstairway { get; set; }= null!;

    [Column("seladdrfloor", TypeName = "varchar(200)")]
    public string? Seladdrfloor { get; set; }= null!;

    [Column("seladdrdoor", TypeName = "varchar(200)")]
    public string? Seladdrdoor { get; set; }= null!;

    [Column("sinvname", TypeName = "varchar(200)")]
    public string? Sinvname { get; set; }= null!;

    [Column("sinvcountry", TypeName = "varchar(200)")]
    public string? Sinvcountry { get; set; }= null!;

    [Column("sinvaddrdistrict", TypeName = "varchar(200)")]
    public string? Sinvaddrdistrict { get; set; }= null!;

    [Column("sinvaddrplace", TypeName = "varchar(200)")]
    public string? Sinvaddrplace { get; set; }= null!;

    [Column("sinvaddrplacetype", TypeName = "varchar(200)")]
    public string? Sinvaddrplacetype { get; set; }= null!;

    [Column("sinvaddrhnum", TypeName = "varchar(200)")]
    public string? Sinvaddrhnum { get; set; }= null!;

    [Column("sinvaddrbuilding", TypeName = "varchar(200)")]
    public string? Sinvaddrbuilding { get; set; }= null!;

    [Column("sinvaddrstairway", TypeName = "varchar(200)")]
    public string? Sinvaddrstairway { get; set; }= null!;

    [Column("sinvaddrfloor", TypeName = "varchar(200)")]
    public string? Sinvaddrfloor { get; set; }= null!;

    [Column("sinvaddrdoor", TypeName = "varchar(200)")]
    public string? Sinvaddrdoor { get; set; }= null!;

    [Column("tel", TypeName = "varchar(200)")]
    public string? Tel { get; set; }= null!;

    [Column("wsemail", TypeName = "varchar(200)")]
    public string? Wsemail { get; set; }= null!;

    [Column("loyaltycardno", TypeName = "varchar(200)")]
    public string? Loyaltycardno { get; set; }= null!;

    [Column("vatnum", TypeName = "varchar(200)")]
    public string? Vatnum { get; set; }= null!;

    [Column("vatnumeu", TypeName = "varchar(200)")]
    public string? Vatnumeu { get; set; }= null!;

    [Column("groupvatnum", TypeName = "varchar(200)")]
    public string? Groupvatnum { get; set; }= null!;


}
