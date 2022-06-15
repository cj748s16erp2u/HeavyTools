using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Script;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Summary description for WebAppInit
/// </summary>
public class WebAppInit
{
    private static bool Inited = false;
    //public static void Initialize(string serverPath)
    public static void Initialize()
    {
        if (Inited)
            return;

        try
        {

            Log.Initialize(Path.Combine(Globals.LogFolder, "webapp.log"));
            Log.Status("Log started");
        }
        catch(Exception e)
        {
            System.Diagnostics.EventLog.WriteEntry("eProjectWeb", "Error in Application startup - couldn't create log file: " + e.Message, System.Diagnostics.EventLogEntryType.Error);
        }
        try
        {
            DBConfig.Load(Path.Combine(Globals.SiteRoot, "databases.config"));

            // Initialize ChangeLog
            ChangeLog.LoadConfig(Path.Combine(Globals.SiteRoot, "TableChangeLog.config"));

            // Load modules
            string[] paths = Globals.XmlFolder.Split(new char[]{';'});
            foreach(string path in paths)
            {
                string moduleConfigFileName = Path.Combine(path, "ModuleConfig.xml");
                if (!File.Exists(moduleConfigFileName))
                    continue;

                ModuleRegistry.LoadModuleConfig(moduleConfigFileName);
                break;
            }
            ModuleRegistry.PreloadModules();

            // Load menu
            //eProjectWeb.Framework.UI.Menu.Menu.Initialize(eProjectWeb.Framework.Globals.XmlFolder + "Menu.xml");

            Inited = true;
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            throw;
        }
    }

    // A szukseges <script src=...> tag-eket rendereli az oldalba.
    // Attol fuggoen, hogy a Web.config-ban az EmbedResources hogy van beallitva, a /Scripts konyvtarbol,
    // vagy a dll-be resource-kent beleforditott scriptekre hivatkozik.
    public static void RenderScriptIncludes(HttpResponse Response)
    {
        if (Globals.ScriptsFolder != null)
        {
            if (Globals.ScriptsCompress)
            {
                // Tomoritett es 1 file-ba osszerakott scriptek (production environment-ben ajanlott)
                Response.Write("<script type='text/javascript' language='javascript' src='");
                Response.Write("AllScripts.jsx");
                Response.Write("'></script>");
                return;
            }

            string[] paths = Globals.ScriptsFolder.Split(new char[] { ';' });

            Dictionary<string, string> allScriptFiles = new Dictionary<string, string>();

            foreach (string path in paths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fis = di.GetFiles("*.js");
                foreach (FileInfo fi in fis)
                {
                    string fnWithoutExt = Path.GetFileNameWithoutExtension(fi.Name);

                    // Ez a file sajnos mindenhova fel van telepitve a scripts konyvtarba
                    // Pedig egy nagyon regi, debugolashoz hasznalt script, amit ha betoltunk
                    // attol meghal az eLog. 
                    // Inkabb bedrotoztam ide, hogy ilyen file nevvel ne lehessen scriptet
                    // betenni, igy a regi eLog telepitesek sem fognak meghalni akkor sem,
                    // ha elfelejtenenk letorolni ezt a file-t a scripts konyvtarbol
                    if (fnWithoutExt == "FakeAsync")
                        continue;

                    allScriptFiles[fnWithoutExt] = fnWithoutExt;
                }
            }
            List<string> scriptFilesSorted = new List<string>(allScriptFiles.Keys);
            scriptFilesSorted.Sort();

            foreach (string fn in scriptFilesSorted)
            {
                Response.Write("<script type='text/javascript' language='javascript' src='");
                Response.Write(fn);
                Response.Write(".jsx'></script>");
            }

            return;
        }




        // Obsolete: a Framework-be embeddelt js file-okat terkepezi fel, es azokat fogja betoltetni az oldallal
        // Megtartjuk visszafele kompatibilitas miatt (igy a regi modon telepitett eLog-ok is mukodni fognak)
        System.Reflection.Assembly[] asms = eProjectWeb.Framework.ModuleRegistry.GetLoadedModuleAssemblies();
        foreach (System.Reflection.Assembly asm in asms)
        {
            if (asm == null)
                continue;
            string[] scriptFiles = asm.GetManifestResourceNames();
            Array.Sort(scriptFiles);
            string asmPrefix = asm.GetName().Name + ".Scripts";
            foreach (string s in scriptFiles)
            {
                int n = s.IndexOf(".js");
                if (n != -1)
                {
                    string s2 = s.Substring(0, n);
                    s2 = s2.Substring(asmPrefix.Length + 1);
                    Response.Write("<script type='text/javascript' language='javascript' src='Scripts/");
                    Response.Write(s2);
                    Response.Write(".js'></script>");
                }
            }
        }      

    }
}
