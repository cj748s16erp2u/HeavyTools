using eProjectWeb.Framework.Data;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class MSSRSViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var sid = Request.QueryString["sid"];
                try
                {
                    eProjectWeb.Framework.Session.SetCurrentSession(sid);

                    var rid = Request.QueryString["rid"];
                    var xmlTypeName = Request.QueryString["xmltypename"];
                    var reportFilename = Request.QueryString["repfilename"];
                    var reportProcname = Request.QueryString["repprocname"];

                    m_xmlTypeName = xmlTypeName;
                    m_reportFilename = GetReportFilename(reportFilename);
                    // load sql proc params
                    m_procParams = new eProjectWeb.Framework.Reports.ProcParams();
                    LoadXml();

                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = m_reportFilename;

                    var dbConnID = m_procParams.GetConnIDOfProc(reportProcname);

                    var reportParamNames = m_procParams.GetParamNamesOfProc(reportProcname, eProjectWeb.Framework.Reports.ProcParamRange.Report, true);
                    var reportParamValues = LoadParamValues(rid, reportParamNames);
                    var parameters = reportParamValues.Select(p => new ReportParameter(p.Key, $"{p.Value}", false)).ToList();

                    var sqlParamNames = m_procParams.GetParamNamesOfProc(reportProcname, eProjectWeb.Framework.Reports.ProcParamRange.SQL, true);
                    var sqlParamValues = LoadParamValues(rid, sqlParamNames);
                    var dt = ExecuteReport(dbConnID, reportProcname, sqlParamValues);

                    ReportViewer1.LocalReport.EnableHyperlinks = true;
                    var dataSourceNames = ReportViewer1.LocalReport.GetDataSourceNames();
                    foreach (var str8 in dataSourceNames)
                    {
                        if (!string.IsNullOrWhiteSpace(str8))
                        {
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(str8, dt));
                        }
                    }

                    ReportViewer1.LocalReport.SetParameters(parameters);
                }
                catch(System.Data.SqlClient.SqlException ex)
                {
                    ReportViewer1.Visible = false;
                    msg.Text = ex.ToString();
                    msg.Style.Add("white-space", "pre");
                    msg.Style.Add("font-family", "monospace");
                    msg.Style.Add("display", "block");
                }
                finally
                {
                    eProjectWeb.Framework.Session.EndCurrentSession();
                }
            }
        }

        private System.Data.DataTable ExecuteReport(DBConnID dbConnID, string reportProcname, IDictionary<string, object> paramValues)
        {
            object realReportFileName;
            if (paramValues.TryGetValue("realreportfilename", out realReportFileName))
            {
                paramValues["report_name"] = realReportFileName;
                paramValues.Remove("realreportfilename");
            }

            var sql = $@"exec {reportProcname} {string.Join(", ", paramValues?.Select(p => $"@{p.Key} = {eProjectWeb.Framework.Utils.SqlToString(p.Value)}").ToArray())}";

            using (var db = DB.GetConn(dbConnID))
            using (var da = db.CreateDA(sql))
            {
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        class Param
        {
            public string name { get; set; }
            public object value { get; set; }
        }

        private IDictionary<string, object> LoadParamValues(string rid, string[] paramNames)
        {
            var sql = $@"select r.name, r.value
from cfw_repparams r (nolock)
where r.repcallguid = {eProjectWeb.Framework.Utils.SqlToString(rid)}
  and r.name in ({eProjectWeb.Framework.Utils.SqlInToString<string>(paramNames)})
order by r.id
";
            var values = SqlDataAdapter.GetList2<Param>(DB.Main, sql);

            return values.ToDictionary(v => v.name, v => v.value);
        }

        private string m_xmlTypeName;
        private string m_reportFilename;
        private eProjectWeb.Framework.Reports.ProcParams m_procParams;


        private void LoadXml()
        {
            string fileName = null;
            var reader = eProjectWeb.Framework.Xml.XmlCache.Get(m_xmlTypeName + "." + Path.GetFileNameWithoutExtension(m_reportFilename), out fileName);
            try
            {
                eProjectWeb.Framework.Xml.XmlUtils.SetCurrentFileName(fileName);
                reader.ReadStartElement();
                //ReadLabelsFromXml(reader);
            }
            finally
            {
                eProjectWeb.Framework.Xml.XmlUtils.SetCurrentFileName(String.Empty);
                reader.Close();
                reader = null;
                GC.Collect();
            }
            reader = eProjectWeb.Framework.Xml.XmlCache.Get(m_xmlTypeName + ".ProcParams", out fileName);
            try
            {
                eProjectWeb.Framework.Xml.XmlUtils.SetCurrentFileName(fileName);
                reader.ReadStartElement();
                ReadProcParamsFromXml(reader);
            }
            finally
            {
                eProjectWeb.Framework.Xml.XmlUtils.SetCurrentFileName(String.Empty);
                reader.Close();
                reader = null;
                GC.Collect();
            }
        }

        private void ReadProcParamsFromXml(System.Xml.XmlReader reader)
        {
            eProjectWeb.Framework.Xml.XmlUtils.ReadXml(m_procParams, reader, (System.Xml.XmlReader r, string localName, out object createdObj, out bool requireAttributes) =>
                eProjectWeb.Framework.Reports.ProcParams.ProcParamProcessElement(m_procParams, r, localName, out createdObj, out requireAttributes));
        }

        private string GetReportFilename(string reportFilename)
        {
            var repFilename = eProjectWeb.Framework.Utils.GetFirstFileFromDirs(eProjectWeb.Framework.Globals.ReportsFolder, reportFilename + ".rdl");
            if (string.IsNullOrWhiteSpace(repFilename) || !File.Exists(repFilename))
            {
                throw new eProjectWeb.Framework.Reports.ReportException($"Invalid report file: {repFilename} (report folders: {eProjectWeb.Framework.Globals.ReportsFolder}, report file name: {reportFilename}.rdl");
            }

            return repFilename;
        }

    }
}