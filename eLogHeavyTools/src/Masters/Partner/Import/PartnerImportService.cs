using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.ImportBase.Xlsx;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using Utils = eProjectWeb.Framework.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace eLog.HeavyTools.Masters.Partner.Import
{
    internal class PartnerImportService : ImportServiceBase<PartnerImportResultSet, PartnerImportResultSets, PartnerRowContext>
    {
        private Base.Masters.Partner.PartnerBL partnerBL;
        private Base.Masters.Partner.PartnAddrBL partnAddrBL;
        private Base.Masters.Partner.PartnBankBL partnBankBL;
        private Base.Masters.Partner.EmployeeBL employeeBL;

        private IDictionary<int, int> addrIdTranslateDict = new Dictionary<int, int>();

        public PartnerImportService() : base()
        {
        }

        public override ProcessResult Import(string importDescrFileName, string importXlsxFileName)
        {
            return this.Import("Partner", importDescrFileName, importXlsxFileName);
        }

        protected override int SaveImport(PartnerImportResultSets results)
        {
            Base.Masters.Partner.PartnerRules.ValidateExtcodeUnique = false;
            Base.Masters.Partner.PartnerRules.ValidateExtcodeEditable = false;

            this.partnerBL = Base.Masters.Partner.PartnerBL.New();
            this.partnAddrBL = Base.Masters.Partner.PartnAddrBL.New();
            this.partnBankBL = Base.Masters.Partner.PartnBankBL.New();
            this.employeeBL = ObjectFactory.New<Base.Masters.Partner.EmployeeBL>();

            var success = 0;

            using (new NS(typeof(Base.Masters.Partner.Partner).Namespace))
            {
                var count = results.Count();
                var i = 0;
                foreach (var r in results)
                {
                    var ret = this.SaveImport(r, ++i, count);
                    if (ret)
                    {
                        success++;
                    }

                    if (i % 100 == 0)
                    {
                        this.logger.FlushFiles();
                    }
                }

                this.logger.FlushFiles();
            }

            return success;
        }

        private bool SaveImport(PartnerImportResultSet result, int? pos = null, int? count = null)
        {
            var partnCode = result.OlcPartner?.Entity["oldcode"]
                ?? result.Partner?.Entity["partnid"]
                ?? result.PartnAddrs?.FirstOrDefault()?.Entity["partnid"]
                ?? result.PartnBanks?.FirstOrDefault()?.Entity["partnid"]
                ?? result.Employees?.FirstOrDefault()?.Entity["partnid"];

            if (pos != null && count != null)
            {
                var percnt = Math.Round(pos.Value / (count.Value * 1M) * 100, 2, MidpointRounding.AwayFromZero);
                this.logger.Log($"Saving partner [{pos} / {count} ({percnt:#.00}%)]: {partnCode} ");
            }
            else
            {
                this.logger.Log($"Saving partner: {partnCode} ");
            }

            var logText = new StringBuilder(result.LogText);

            string entityType = null;
            try
            {
                using (var db = DB.GetConn(DB.Main, Transaction.Use))
                {

                    entityType = typeof(Base.Masters.Partner.Partner).Name;
                    var partnerIsHidden = this.SavePartner(result);
                    entityType = typeof(Base.Masters.Partner.PartnAddr).Name;
                    this.SavePartnAddr(result);
                    entityType = typeof(Base.Masters.Partner.PartnBank).Name;
                    this.SavePartnBank(result);
                    entityType = typeof(Base.Masters.Partner.Employee).Name;
                    this.SaveEmployee(result);

                    if (partnerIsHidden == true)
                    {
                        this.partnerBL.Hide(result.Partner.Entity.PK);
                    }
                    else if (partnerIsHidden == false)
                    {
                        this.partnerBL.Unhide(result.Partner.Entity.PK);
                    }

                    db.Commit();

                    this.logger.LogLine("...done");

                    return true;
                }
            }
            catch (eProjectWeb.Framework.Rules.ValidateException ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Partner: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ToTextValidate(ex);
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }
            catch (Exception ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Partner: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ex.Message;
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }

            result.LogText = logText.ToString();

            return false;
        }

        protected override IEnumerable<ImportSheet> ParseImportDescr(string descrText, params Newtonsoft.Json.JsonConverter[] converters)
        {
            var converterList = new List<Newtonsoft.Json.JsonConverter>();
            if (converters != null)
            {
                converterList.AddRange(converters);
            }

            converterList.Add(new ImportConditionalConverter());

            return base.ParseImportDescr(descrText, converterList.ToArray());
        }

        private string GetEntityName(string entityType)
        {
            if (string.IsNullOrWhiteSpace(entityType))
            {
                return null;
            }

            return $"'{$"${entityType.ToLowerInvariant()}".eLogTransl()}'";
        }

        /// <summary>
        /// Human readable console format
        /// </summary>
        /// <returns></returns>
        private string ToTextValidate(eProjectWeb.Framework.Rules.ValidateException ex)
        {
            string s = "";

            if (ex.Errors.Count > 0)
            {
                s += $"{"$errors".eLogTransl()}:\r\n-------\r\n";
                foreach (var er in ex.Errors)
                {
                    if (!string.IsNullOrEmpty(er.FieldName))
                        s += $"[{$"${er.FieldName}".eLogTransl()}]: ";
                    s += Translator.Translate(er.Text).Replace("\r\n", "  \r\n") + "\r\n";
                }
            }
            if (ex.Exceptions.Count > 0)
            {
                if (s != "")
                    s += "\r\n";
                s += $"{"$exceptions".eLogTransl()}:\r\n-------\r\n";
                foreach (var e in ex.Exceptions)
                {
                    s += Translator.Translate(e.ToString()).Replace("\r\n", "  \r\n") + "\r\n";
                }
            }

            return s;
        }

        private void FixPartnerVatnum(Base.Masters.Partner.Partner partner)
        {
            if (!string.IsNullOrWhiteSpace(partner?.Vatnum))
            {
                var pattern = @"(\d{8})-?(\d)-?(\d{2})";
                var matches = System.Text.RegularExpressions.Regex.Match(partner.Vatnum, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
                if (matches.Success)
                {
                    var part1 = matches.Groups[1].Value;
                    var part2 = matches.Groups[2].Value;
                    var part3 = matches.Groups[3].Value;

                    var isValidBaseNumber = this.ValidateVatnumBaseNumber(part1);
                    if (isValidBaseNumber)
                    {
                        var vatnum = $"{part1}-{part2}-{part3}";
                        var vatnumType = ConvertUtils.ToInt32(part2);
                        if (vatnumType == 5)
                        {
                            partner.Vatnum = null;
                            partner.Groupvatnum = vatnum;
                        }
                        else
                        {
                            partner.Vatnum = vatnum;
                        }
                    }
                }
            }
        }

        private bool ValidateVatnumBaseNumber(string vatnumBase)
        {
            var id = vatnumBase.Substring(0, 7);
            var csref = "" + vatnumBase[7];

            var cs = 0;
            var x = new int[] { 9, 7, 3, 1 };
            for (var i = 0; i < id.Length; i++)
            {
                cs += x[i % 4] * int.Parse("" + id[i]);
            }

            cs %= 10;
            if (cs != 0)
            {
                cs = 10 - cs;
            }

            return string.Equals(csref, cs.ToString());
        }

        private bool? SavePartner(PartnerImportResultSet result)
        {
            if (result.Partner != null)
            {
                var map = new BLObjectMap();
                map.SysParams.ActionID = ActionID.New;

                Base.Masters.Partner.Partner origPartner = null;
                OlcPartner origOlcPartner = null;

                var oldcode = ConvertUtils.ToString(result.OlcPartner.Entity[OlcPartner.FieldOldcode.Name]);
                if (!string.IsNullOrWhiteSpace(oldcode))
                {
                    var partnerKey = new Key
                    {
                        [OlcPartner.FieldOldcode.Name] = result.OlcPartner.Entity[OlcPartner.FieldOldcode.Name]
                    };

                    origOlcPartner = OlcPartner.Load(partnerKey);
                }

                int? delstat = null;

                if (origOlcPartner != null)
                {
                    origPartner = Base.Masters.Partner.Partner.Load(origOlcPartner.Partnid);
                    if (origPartner != null)
                    {
                        origPartner.MergeTo(result.Partner.Entity);
                        result.Partner.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;
                        this.logger.Log(" [modify] ");

                        var newDelstat = ConvertUtils.ToInt32(result.Partner.Entity[Base.Masters.Partner.Partner.FieldDelstat]);
                        if (newDelstat != origPartner.Delstat)
                        {
                            delstat = newDelstat;
                            result.Partner.Entity[Base.Masters.Partner.Partner.FieldDelstat] = 0;
                        }

                        // a rejtett partnereket nem lehet módosítani
                        this.partnerBL.Unhide(result.Partner.Entity.PK);
                    }
                    else
                    {
                        this.logger.Log(" [create] ");

                        delstat = ConvertUtils.ToInt32(result.Partner.Entity[Base.Masters.Partner.Partner.FieldDelstat]);
                        result.Partner.Entity[Base.Masters.Partner.Partner.FieldDelstat] = 0;
                    }
                }
                else
                {
                    this.logger.Log(" [create] ");

                    delstat = ConvertUtils.ToInt32(result.Partner.Entity[Base.Masters.Partner.Partner.FieldDelstat]);
                    result.Partner.Entity[Base.Masters.Partner.Partner.FieldDelstat] = 0;
                }

                var partner = result.Partner.Entity as Base.Masters.Partner.Partner;
                this.FixPartnerVatnum(partner);

                map.Default = result.Partner.Entity;

                if (result.OlcPartner != null)
                {
                    if (origPartner != null)
                    {
                        if (origOlcPartner != null)
                        {
                            origOlcPartner.MergeTo(result.OlcPartner.Entity);
                            result.OlcPartner.Entity.State = DataRowState.Modified;
                        }

                        result.OlcPartner.Entity["partnid"] = origPartner["partnid"];
                    }

                    map.Add(result.OlcPartner.Entity);
                }

                var origPartnCmps = origPartner != null ? Base.Masters.Partner.PartnCmps.Load(origPartner.PK) : null;
                var origOlcPartnCmps = origPartnCmps != null ? OlcPartnCmps.Load(origPartner.PK) : null;
                var partnCmps = Base.Masters.Partner.PartnCmps.New();
                var olcPartnCmps = OlcPartnCmps.New();
                foreach (var pc in result.PartnCmps.Where(pc => pc.Valid))
                {
                    var origPc = origPartnCmps?.FirstOrDefault(x => Utils.Equals(x["cmpid"], pc.Entity["cmpid"]));
                    if (origPc != null)
                    {
                        origPc.MergeTo(pc.Entity);
                        pc.Entity["partnid"] = origPc["partnid"];
                        pc.Entity.State = DataRowState.Modified;
                    }

                    partnCmps.AddRow(pc.Entity);

                    var olcPc = result.OlcPartnCmps.FirstOrDefault(x => x.Valid && Utils.Equals(x.Entity["cmpid"], pc.Entity["cmpid"]));
                    var origOlcPc = (OlcPartnCmp)origOlcPartnCmps?.FirstOrDefault(x => Utils.Equals(x["cmpid"], pc.Entity["cmpid"]));
                    if (olcPc != null)
                    {
                        if (origOlcPc != null)
                        {
                            origOlcPc.MergeTo(olcPc.Entity);
                            olcPc.Entity["partnid"] = origOlcPc["partnid"];
                            olcPc.Entity.State = DataRowState.Modified;
                        }

                        if (olcPc.Entity["secpaycid"] == null)
                        {
                            olcPc.Entity["secpaymid"] = null;
                        }

                        olcPartnCmps.AddRow(olcPc.Entity);
                    }
                    else if (origOlcPc != null)
                    {
                        origOlcPc.State = DataRowState.Deleted;
                        origOlcPc.Save();
                    }
                }

                map.Add(partnCmps);
                map.Add(olcPartnCmps);

                this.partnerBL.Save(map);

                if (delstat != null)
                {
                    return delstat != 0;
                }
            }

            return null;
        }

        private void SavePartnAddr(PartnerImportResultSet result)
        {
            if (result.PartnAddrs?.Any(pa => pa.Valid) == true)
            {
                var map = new BLObjectMap();

                foreach (var pa in result.PartnAddrs.Where(pa => pa.Valid))
                {
                    map.SysParams.ActionID = ActionID.New;

                    var addrid = ConvertUtils.ToInt32(pa.Entity["addrid"]);
                    var olcPA = result.OlcPartnAddrs?.FirstOrDefault(e => e.Valid && Utils.Equals(e.Entity["addrid"], addrid));
                    var oldcode = ConvertUtils.ToString(olcPA?.Entity["oldcode"]);

                    var partnId = result.Partner?.Entity["partnid"] ?? pa.Entity["partnid"];

                    OlcPartnAddr origOlcPA = null;

                    Key partnAddrKey;
                    if (!string.IsNullOrWhiteSpace(oldcode))
                    {
                        partnAddrKey = new Key
                        {
                            ["e." + Base.Masters.Partner.PartnAddr.FieldPartnid.Name] = partnId,
                            ["c." + OlcPartnAddr.FieldOldcode.Name] = oldcode
                        };

                        var sql = $"select c.* from olc_partnaddr c (nolock) join ols_partnaddr e (nolock) on e.addrid = c.addrid where {partnAddrKey.ToSql()}";
                        try
                        {
                            origOlcPA = OlcPartnAddr.New();
                            SqlDataAdapter.FillSingleRow(DB.Main, origOlcPA, sql);
                        }
                        catch (RecordNotFoundException)
                        {
                            origOlcPA = null;
                        }
                    }

                    Base.Masters.Partner.PartnAddr origAddr = null;
                    if (origOlcPA != null)
                    {
                        origAddr = Base.Masters.Partner.PartnAddr.Load(origOlcPA.Addrid);
                    }

                    pa.Entity["partnid"] = partnId;
                    pa.Entity["addrid"] = null;

                    if (olcPA != null)
                    {
                        olcPA.Entity["addrid"] = null;
                    }

                    if (origAddr != null)
                    {
                        origAddr.MergeTo(pa.Entity);
                        pa.Entity["addrid"] = origAddr["addrid"];
                        pa.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;

                        if (origOlcPA != null)
                        {
                            origOlcPA.MergeTo(olcPA.Entity);
                            olcPA.Entity["addrid"] = pa.Entity["addrid"];
                            olcPA.Entity.State = DataRowState.Modified;
                        }
                    }

                    map.Default = pa.Entity;

                    map.Remove<OlcPartnAddr>();
                    if (olcPA != null)
                    {
                        map.Add((OlcPartnAddr)olcPA.Entity);
                    }

                    map.Remove<Base.Masters.Partner.PartnAddrCmps>();
                    if (addrid.HasValue && result.PartnAddrCmps != null)
                    {
                        var origPartnAddrCmps = origAddr != null ? Base.Masters.Partner.PartnAddrCmps.Load(origAddr.PK) : null;
                        var partnAddrCmps = Base.Masters.Partner.PartnAddrCmps.New();
                        foreach (var pac in result.PartnAddrCmps.Where(pac => pac.Valid && ConvertUtils.ToInt32(pac.Entity["addrid"]) == addrid.Value))
                        {
                            pac.Entity["addrid"] = null;

                            var origPac = origPartnAddrCmps?.FirstOrDefault(x => Utils.Equals(x["cmpid"], pac.Entity["cmpid"]));
                            if (origPac != null)
                            {
                                origPac.MergeTo(pac.Entity);
                                pac.Entity["addrid"] = origPac["addrid"];
                                pac.Entity.State = DataRowState.Modified;
                            }

                            partnAddrCmps.AddRow(pac.Entity);
                        }

                        map.Add(partnAddrCmps);
                    }

                    this.partnAddrBL.Save(map);

                    if (addrid.HasValue)
                    {
                        this.addrIdTranslateDict[addrid.Value] = ConvertUtils.ToInt32(pa.Entity["addrid"]).GetValueOrDefault(0);
                    }
                }
            }
        }

        private void SavePartnBank(PartnerImportResultSet result)
        {
            if (result.PartnBanks?.Any(pb => pb.Valid) == true)
            {
                var map = new BLObjectMap();

                foreach (var pb in result.PartnBanks.Where(pb => pb.Valid))
                {
                    map.SysParams.ActionID = ActionID.New;

                    var partnId = result.Partner?.Entity["partnid"] ?? pb.Entity["partnid"];
                    var partnBankKey = new Key
                    {
                        [Base.Masters.Partner.PartnBank.FieldPartnid.Name] = partnId,
                        ["__OR__"] = new Key.OrAtToSql(new[]
                        {
                            new Key
                            {
                                [Base.Masters.Partner.PartnBank.FieldAccno.Name] = pb.Entity["accno"]
                            },
                            new Key
                            {
                                [Base.Masters.Partner.PartnBank.FieldIban.Name] = pb.Entity["iban"]
                            }
                        })
                    };

                    var bankid = ConvertUtils.ToInt32(pb.Entity["bankid"]);
                    pb.Entity["partnid"] = partnId;
                    pb.Entity["bankid"] = null;

                    var origBank = Base.Masters.Partner.PartnBank.Load(partnBankKey);
                    if (origBank != null)
                    {
                        origBank.MergeTo(pb.Entity);
                        pb.Entity["bankid"] = origBank["bankid"];
                        pb.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;
                    }

                    map.Default = pb.Entity;

                    map.Remove<Base.Masters.Partner.PartnBankCmps>();
                    if (bankid.HasValue && result.PartnBankCmps != null)
                    {
                        var origPartnBankCmps = origBank != null ? Base.Masters.Partner.PartnBankCmps.Load(origBank.PK) : null;
                        var partnBankCmps = Base.Masters.Partner.PartnBankCmps.New();
                        foreach (var pbc in result.PartnBankCmps.Where(pbc => pbc.Valid && ConvertUtils.ToInt32(pbc.Entity["bankid"]) == bankid.Value))
                        {
                            pbc.Entity["bankid"] = null;

                            var origPbc = origPartnBankCmps?.FirstOrDefault(x => Utils.Equals(x["cmpid"], pbc.Entity["cmpid"]));
                            if (origPbc != null)
                            {
                                origPbc.MergeTo(pbc.Entity);
                                pbc.Entity["bankid"] = origPbc["bankid"];
                                pbc.Entity.State = DataRowState.Modified;
                            }

                            partnBankCmps.AddRow(pbc.Entity);
                        }

                        map.Add(partnBankCmps);
                    }

                    this.partnBankBL.Save(map);
                }
            }
        }

        private void SaveEmployee(PartnerImportResultSet result)
        {
            if (result.Employees?.Any(e => e.Valid) == true)
            {
                var map = new BLObjectMap();

                foreach (var emp in result.Employees.Where(e => e.Valid))
                {
                    map.SysParams.ActionID = ActionID.New;

                    var empid = ConvertUtils.ToInt32(emp.Entity["empid"]);
                    var olcEmp = result.OlcEmployees?.FirstOrDefault(e => e.Valid && Utils.Equals(e.Entity["empid"], empid));
                    var oldcode = ConvertUtils.ToString(olcEmp?.Entity["oldcode"]);

                    var partnId = result.Partner?.Entity["partnid"] ?? emp.Entity["partnid"];
                    var addrId = ConvertUtils.ToInt32(emp.Entity["addrid"]);
                    if (addrId.HasValue && this.addrIdTranslateDict.ContainsKey(addrId.Value))
                    {
                        addrId = this.addrIdTranslateDict[addrId.Value];
                    }

                    var defaultAddrIfEmpty = emp.DefaultIfExists.Any(f => string.Equals(f.Field, "addrid", StringComparison.InvariantCultureIgnoreCase));
                    if (defaultAddrIfEmpty)
                    {
                        var addrKey = new Key
                        {
                            [Base.Masters.Partner.PartnAddr.FieldPartnid.Name] = partnId,
                            [Base.Masters.Partner.PartnAddr.FieldDef.Name] = 1,
                        };
                        var defAddr = Base.Masters.Partner.PartnAddr.Load(addrKey);
                        if (defAddr != null)
                        {
                            addrId = defAddr.Addrid;
                        }
                    }

                    OlcEmployee origOlcEmp = null;

                    Key empKey;
                    if (!string.IsNullOrWhiteSpace(oldcode))
                    {
                        empKey = new Key
                        {
                            ["e." + Base.Masters.Partner.Employee.FieldPartnid.Name] = partnId,
                            ["e." + Base.Masters.Partner.Employee.FieldAddrid.Name] = addrId,
                            ["c." + OlcEmployee.FieldOldcode.Name] = oldcode
                        };

                        var sql = $"select c.* from olc_employee c (nolock) join ols_employee e (nolock) on e.empid = c.empid where {empKey.ToSql()}";
                        try
                        {
                            origOlcEmp = OlcEmployee.New();
                            SqlDataAdapter.FillSingleRow(DB.Main, origOlcEmp, sql);
                        }
                        catch (RecordNotFoundException)
                        {
                            origOlcEmp = null;
                        }
                    }

                    Base.Masters.Partner.Employee origEmp = null;
                    if (origOlcEmp != null)
                    {
                        origEmp = Base.Masters.Partner.Employee.Load(origOlcEmp.Empid);
                    }

                    emp.Entity["partnid"] = partnId;
                    emp.Entity["addrid"] = addrId;
                    emp.Entity["empid"] = null;

                    if (olcEmp != null)
                    {
                        olcEmp.Entity["empid"] = null;
                    }

                    if (origEmp != null)
                    {
                        origEmp.MergeTo(emp.Entity);
                        emp.Entity["empid"] = origEmp["empid"];
                        emp.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;

                        if (origOlcEmp != null)
                        {
                            origOlcEmp.MergeTo(olcEmp.Entity);
                            olcEmp.Entity["empid"] = emp.Entity["empid"];
                            olcEmp.Entity.State = DataRowState.Modified;
                        }
                    }

                    map.Default = emp.Entity;

                    map.Remove<OlcEmployee>();
                    if (olcEmp != null)
                    {
                        map.Add((OlcEmployee)olcEmp.Entity);
                    }

                    this.employeeBL.Save(map);
                }
            }
        }

        protected override PartnerImportResultSet CreateResultSet(PartnerRowContext rowContext)
        {
            return new PartnerImportResultSet
            {
                Partner = rowContext.Partner,
                OlcPartner = rowContext.OlcPartner,
                PartnCmps = rowContext.PartnCmps.ToList(),
                OlcPartnCmps = rowContext.OlcPartnCmps.ToList(),
                PartnAddrs = rowContext.PartnAddrs.ToList(),
                OlcPartnAddrs = rowContext.OlcPartnAddrs.ToList(),
                PartnAddrCmps = rowContext.PartnAddrCmps.ToList(),
                PartnBanks = rowContext.PartnBanks.ToList(),
                PartnBankCmps = rowContext.PartnBankCmps.ToList(),
                Employees = rowContext.Employees.ToList(),
                OlcEmployees = rowContext.OlcEmployees.ToList(),

                LogText = rowContext.LogText,
                Row = rowContext.Row,
                LogCol = rowContext.LogCol,
            };
        }

        protected override void CreateEntity(PartnerRowContext rowContext)
        {
            var alias = rowContext.CurrentTable.Alias;
            if (string.IsNullOrWhiteSpace(alias))
            {
                alias = rowContext.CurrentTable.Table;
            }

            TableEntry entry;
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_partner":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.Partner.GetSchema(),
                        Entity = Base.Masters.Partner.Partner.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.Partner = entry;
                    break;
                case "olc_partner":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcPartner.GetSchema(),
                        Entity = OlcPartner.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.OlcPartner = entry;
                    break;
                case "ols_partncmp":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.PartnCmp.GetSchema(),
                        Entity = Base.Masters.Partner.PartnCmp.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PartnCmps.Add(entry);
                    break;
                case "olc_partncmp":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcPartnCmp.GetSchema(),
                        Entity = OlcPartnCmp.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.OlcPartnCmps.Add(entry);
                    break;
                case "ols_partnaddr":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.PartnAddr.GetSchema(),
                        Entity = Base.Masters.Partner.PartnAddr.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PartnAddrs.Add(entry);
                    break;
                case "olc_partnaddr":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcPartnAddr.GetSchema(),
                        Entity = OlcPartnAddr.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.OlcPartnAddrs.Add(entry);
                    break;
                case "ols_partnaddrcmp":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.PartnAddrCmp.GetSchema(),
                        Entity = Base.Masters.Partner.PartnAddrCmp.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PartnAddrCmps.Add(entry);
                    break;
                case "ols_partnbank":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.PartnBank.GetSchema(),
                        Entity = Base.Masters.Partner.PartnBank.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PartnBanks.Add(entry);
                    break;
                case "ols_partnbankcmp":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.PartnBankCmp.GetSchema(),
                        Entity = Base.Masters.Partner.PartnBankCmp.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PartnBankCmps.Add(entry);
                    break;
                case "ols_employee":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Partner.Employee.GetSchema(),
                        Entity = Base.Masters.Partner.Employee.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.Employees.Add(entry);
                    break;
                case "olc_employee":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcEmployee.GetSchema(),
                        Entity = OlcEmployee.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.OlcEmployees.Add(entry);
                    break;
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.CreateEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        protected override void RemoveEntity(PartnerRowContext rowContext)
        {
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_partner":
                    rowContext.Partner = null;
                    break;
                case "olc_partner":
                    rowContext.OlcPartner = null;
                    break;
                case "ols_partncmp":
                    rowContext.PartnCmps.Remove(rowContext.CurrentEntry);
                    break;
                case "olc_partncmp":
                    rowContext.OlcPartnCmps.Remove(rowContext.CurrentEntry);
                    break;
                case "ols_partnaddr":
                    rowContext.PartnAddrs.Remove(rowContext.CurrentEntry);
                    break;
                case "olc_partnaddr":
                    rowContext.OlcPartnAddrs.Remove(rowContext.CurrentEntry);
                    break;
                case "ols_partnaddrcmp":
                    rowContext.PartnAddrCmps.Remove(rowContext.CurrentEntry);
                    break;
                case "ols_partnbank":
                    rowContext.PartnBanks.Remove(rowContext.CurrentEntry);
                    break;
                case "ols_partnbankcmp":
                    rowContext.PartnBankCmps.Remove(rowContext.CurrentEntry);
                    break;
                case "ols_employee":
                    rowContext.Employees.Remove(rowContext.CurrentEntry);
                    break;
                case "olc_employee":
                    rowContext.OlcEmployees.Remove(rowContext.CurrentEntry);
                    break;
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.RemoveEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        protected override object DetermineSelfLookupValue(object value, PartnerRowContext rowContext)
        {
            var alias = rowContext.CurrentField.Lookup.Alias;

            Entity entity = null;

            if (string.Equals(rowContext.Partner?.Alias, alias, StringComparison.InvariantCultureIgnoreCase))
            {
                entity = rowContext.Partner.Entity;
            }

            if (entity == null)
            {
                var entry = rowContext.PartnAddrs.FirstOrDefault(pa => string.Equals(pa.Alias, alias, StringComparison.InvariantCultureIgnoreCase));
                entity = entry?.Entity;
            }

            if (entity == null)
            {
                var entry = rowContext.PartnBanks.FirstOrDefault(pb => string.Equals(pb.Alias, alias, StringComparison.InvariantCultureIgnoreCase));
                entity = entry?.Entity;
            }

            if (entity == null)
            {
                var entry = rowContext.Employees.FirstOrDefault(e => string.Equals(e.Alias, alias, StringComparison.InvariantCultureIgnoreCase));
                entity = entry?.Entity;
            }

            if (entity == null)
            {
                entity = this.DetermineSelfLookupPartnAddrPartnidValueSpec(rowContext, alias, value);
            }

            if (entity != null)
            {
                return entity[rowContext.CurrentField.Lookup.ValueField];
            }

            //var retValue = this.DetermineSelfLookupEmployeeAddridValue(rowContext, alias, value);
            //if (retValue != null)
            //{
            //    return retValue;
            //}

            return base.DetermineSelfLookupValue(value, rowContext);
        }

        private Base.Masters.Partner.Partner DetermineSelfLookupPartnAddrPartnidValueSpec(PartnerRowContext rowContext, string alias, object value)
        {
            if (string.Equals(alias, "ols_partner", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(rowContext.CurrentField.Lookup.KeyField) &&
                rowContext.CurrentEntry.Schema.Fields[rowContext.CurrentField.Lookup.KeyField] != null)
            {
                var keyValue = rowContext.CurrentEntry.Entity[rowContext.CurrentField.Lookup.KeyField];
                if (keyValue != null)
                {
                    var key = new Key
                    {
                        [rowContext.CurrentField.Lookup.KeyField] = keyValue
                    };

                    return Base.Masters.Partner.Partner.Load(key);
                }
            }

            return null;
        }

        //        private object DetermineSelfLookupEmployeeAddridValue(PartnerRowContext rowContext, string alias, object value)
        //        {
        //            if (string.Equals(alias, "CustomPartnAddrId", StringComparison.InvariantCultureIgnoreCase) && value != null)
        //            {
        //                var sql = $@"select top 2 [pa].[addrid]
        //from [olc_partner] [p] (nolock)
        //  join [ols_partnaddr] [pa] (nolock) on [pa].[partnid] = [p].[partnid]
        //where [p].[oldcode] = {Utils.SqlToString(value)}
        //  and [pa].[def] = 1
        //";

        //                var list = new List<int>();
        //                Base.Common.SqlFunctions.QueryData(DB.Main, sql, r =>
        //                {
        //                    list.Add(Convert.ToInt32(r.GetValue(0)));
        //                });

        //                if (list.Count == 1)
        //                {
        //                    return list[0];
        //                }
        //            }

        //            return null;
        //        }

        protected override bool ProcessCustomConditionals(PartnerRowContext rowContext, ImportConditional cond, object value)
        {
            if ((PartnerImportConditionalType)cond.Type == PartnerImportConditionalType.CheckCompanyHierarchy)
            {
                return this.ProcessCompanyHierarchyConditionals(rowContext, cond, value);
            }
            else if ((PartnerImportConditionalType)cond.Type == PartnerImportConditionalType.CheckCompanyHierarchyNot)
            {
                return !this.ProcessCompanyHierarchyConditionals(rowContext, cond, value);
            }

            return base.ProcessCustomConditionals(rowContext, cond, value);
        }

        private bool ProcessCompanyHierarchyConditionals(PartnerRowContext rowContext, ImportConditional cond, object value)
        {
            if (value == null)
            {
                return true;
            }

            if (cond is PartnerImportConditional partnCond)
            {
                var cacheKey = $"COMPH|{value}";
                if (!lookupCache.TryGetValue(cacheKey, out var o))
                {
                    var sql = $@"select top 2 [mainkey]
from [imp_companyhierarchy]
where [key] = {Utils.SqlToString(value)}";

                    var list = new List<int?>();
                    Base.Common.SqlFunctions.QueryData(DB.Main, sql, r =>
                    {
                        var obj = ConvertUtils.ToInt32(r.GetValue(0));
                        list.Add(obj);
                    });

                    // ha 1-nel tobb talalat van, akkor az nem egyertelmu talalat
                    o = null;
                    if (list.Count == 1)
                    {
                        o = list[0];
                    }

                    if (o == null)
                    {
                        this.logger.LogErrorLine($"CompanyHierarchy value is not found: {value}");
                    }
                }

                if (o == null || Utils.Equals(o, 0))
                {
                    return true;
                }

                var oldCode = rowContext.Sheet.GetCellValue(rowContext.Row, partnCond.RefColumnIndex.Value);

                return ConvertUtils.ToInt32(oldCode) == ConvertUtils.ToInt32(o);
            }

            return false;
        }

        protected override void ResolveColumnNames(XlsxWorksheet worksheet, IEnumerable<Tuple<string, string>> cellObjs, ImportConditional cond)
        {
            base.ResolveColumnNames(worksheet, cellObjs, cond);

            if (cond.Valid && cond is PartnerImportConditional partnCond)
            {
                if (string.IsNullOrWhiteSpace(partnCond.RefColumn) && !string.IsNullOrWhiteSpace(partnCond.RefColumnName))
                {
                    var obj = cellObjs.FirstOrDefault(c => string.Equals(c.Item1, partnCond.RefColumnName, StringComparison.InvariantCultureIgnoreCase));
                    partnCond.RefColumn = obj?.Item2;
                    partnCond.Valid = !string.IsNullOrWhiteSpace(partnCond.RefColumn);
                }

                if (!string.IsNullOrWhiteSpace(partnCond.RefColumn))
                {
                    partnCond.RefColumnIndex = XlsxWorksheet.GetColumnIndex(partnCond.RefColumn);
                }
            }
        }
    }
}
