using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common
{
    public class MagyarPostaCheckQRCode
    {
        public MagyarPostaCheckQRResult Generate(MagyarPostaCheckQRDataList lst)
        {
            var propContent = eProjectWeb.Framework.CustomSettings.GetString("MagyarPostaCheckQRCode-PropContent");
            if (string.IsNullOrEmpty(propContent))
                throw new Exception("Missing property content");
            propContent = eProjectWeb.Framework.Utils.UnescapeString(propContent);
            var rsaQrGenPath = eProjectWeb.Framework.CustomSettings.GetString("MagyarPostaCheckQRCode-RsaQrGen");
            if (string.IsNullOrEmpty(rsaQrGenPath))
                throw new Exception("Missing RsaQrGenPath");
            if (!System.IO.Path.IsPathRooted(rsaQrGenPath))
                rsaQrGenPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(eProjectWeb.Framework.Globals.SiteRoot, rsaQrGenPath));
            if (!System.IO.File.Exists(rsaQrGenPath))
                throw new Exception("Missing file: " + rsaQrGenPath);

            var pcLines = new List<string>(propContent.Replace("\\r", "").Split('\n'));
            for (int i = 0; i < pcLines.Count; i++)
            {
                var line = pcLines[i];
                if (line.StartsWith("keyFileName=")) // ha a keyfile relativ eleresi uttal van megadva, akkor a Site gyokerehez kepest ertjuk
                {
                    var keyFileName = line.Substring(12);
                    if (!System.IO.Path.IsPathRooted(keyFileName))
                    {
                        keyFileName = System.IO.Path.GetFullPath(System.IO.Path.Combine(eProjectWeb.Framework.Globals.SiteRoot, keyFileName));
                        pcLines[i] = "keyFileName=" + keyFileName;
                    }
                }
            }
            propContent = string.Join(Environment.NewLine, pcLines);

            var r = new MagyarPostaCheckQRResult();

            var enc = Encoding.GetEncoding("ISO-8859-2");

            var propName = System.IO.Path.GetRandomFileName();
            var outName = System.IO.Path.GetRandomFileName();
            var logName = propName + ".txt";
            propName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), propName);
            logName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), logName);
            outName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), outName);
            propContent += "\r\nlogFileName="+ logName;
            System.IO.File.WriteAllText(propName, propContent, enc);

            lst.Save();
            try
            {
                var stdOutput = new List<string>();
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = rsaQrGenPath;
                p.StartInfo.Arguments = "\"" + propName + "\" \"" + lst.Path + "\" \"" + outName + "\"";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += (/*object */sender, /*System.Diagnostics.DataReceivedEventArgs */e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        stdOutput.Add(e.Data.Replace('ï', 'ő'));
                    else
                        stdOutput.Add("");
                };
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
                p.Close();

                if (System.IO.File.Exists(logName + ".finished"))
                {
                    System.IO.File.Delete(logName + ".finished");
                    var lines = System.IO.File.ReadAllLines(outName, enc);
                    System.IO.File.Delete(outName);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(';');
                        if (parts.Length != 3)
                            throw new Exception(parts.Length.ToString());
                        var id = parts[0];
                        var item = lst.FirstOrDefault(x => x.tetelAzonosito == id);

                        var qrCode = parts[1];
                        if (qrCode.StartsWith("\"") && qrCode.EndsWith("\""))
                            qrCode = qrCode.Substring(1, qrCode.Length - 2);
                        var pngHexData = parts[2];
                        if (pngHexData.StartsWith("\"") && pngHexData.EndsWith("\""))
                            pngHexData = pngHexData.Substring(1, pngHexData.Length - 2);
                        var pngData = Hex2Bytes(pngHexData);

                        r.Items.Add(new MagyarPostaCheckQRResultItem() { Data = item, QRCode = qrCode, PngImageBytes = pngData });
                    }
                }
                else
                {
                    if (System.IO.File.Exists(logName))
                    {
                        r.Log = System.IO.File.ReadAllLines(logName);
                        System.IO.File.Delete(logName);
                    }
                    else
                        r.Log = stdOutput.ToArray();
                }
            }
            finally
            {
                if (System.IO.File.Exists(propName))
                    System.IO.File.Delete(propName);
                lst.Delete();
            }

            return r;
        }

        protected static byte[] Hex2Bytes(string hexData)
        {
            if (string.IsNullOrEmpty(hexData))
                return new byte[0];

            var b = new byte[hexData.Length / 2];
            for (int i = 0; i < b.Length; i++)
                b[i] = Convert.ToByte(hexData.Substring(i * 2, 2), 16);
            return b;
        }
    }

    public class MagyarPostaCheckQRDataList : List<MagyarPostaCheckQRData>
    {
        public string Path { get; private set; }

        public void Save(string path = null)
        {
            if (path == null)
            {
                path = System.IO.Path.GetRandomFileName();
                path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), path);
            }

            using (var sw = new System.IO.StreamWriter(path, false, Encoding.GetEncoding("ISO-8859-2")))
            {
                sw.WriteLine(@"""tetelAzonosito"";""strukturaAzonosito"";""szamlaAzonosito"";""szamlaKelte"";""fizetesiHatarido"";""befizetoAzonosito"";""osszeg"";""devizanem"";""szamlaszam"";""tranzakcioKod"";""gyartoKod"";""bizonylatAzonosito"";""outputKod"";""kozlemeny""");
                foreach (var line in this.Select(x => x.GetCSVLine()))
                    sw.WriteLine(line);
                sw.Flush();
            }
            Path = path;
        }

        public void Delete()
        {
            if (string.IsNullOrEmpty(Path))
                return;
            if (System.IO.File.Exists(Path))
                System.IO.File.Delete(Path);
        }
    }

    public class MagyarPostaCheckQRData
    {
        public string tetelAzonosito { get; set; }
        public string strukturaAzonosito { get; set; }
        public string szamlaAzonosito { get; set; }
        public DateTime? szamlaKelte { get; set; }
        public DateTime? fizetesiHatarido { get; set; }
        public string befizetoAzonosito { get; set; }
        public int osszeg { get; set; }
        public string devizanem { get; set; }
        public string szamlaszam { get; set; }
        public string tranzakcioKod { get; set; }
        public string gyartoKod { get; set; }
        public string bizonylatAzonosito { get; set; }
        public string outputKod { get; set; }
        public string kozlemeny { get; set; }

        public string GetCSVLine()
        {
            var lst = new List<string>();
            lst.Add(tetelAzonosito); // 1
            lst.Add(strukturaAzonosito); // 2
            lst.Add(szamlaAzonosito); // 3
            lst.Add(szamlaKelte?.ToString("yyyy'.'MM'.'dd")); // 4
            lst.Add(fizetesiHatarido?.ToString("yyyy'.'MM'.'dd")); // 5
            lst.Add(befizetoAzonosito); // 6
            lst.Add(osszeg.ToString(System.Globalization.CultureInfo.InvariantCulture)); // 7
            lst.Add(devizanem); // 8
            lst.Add(szamlaszam); // 9
            lst.Add(tranzakcioKod); // 10
            lst.Add(gyartoKod); // 11
            lst.Add(bizonylatAzonosito); // 12
            lst.Add(outputKod); // 13
            lst.Add(kozlemeny); // 14

            var result = string.Join(";", lst.Select(i => !string.IsNullOrEmpty(i) ? "\"" + i.Replace("\"", "\"\"") + "\"" : ""));
            return result;
        }
    }

    public class MagyarPostaCheckQRResult
    {
        public List<MagyarPostaCheckQRResultItem> Items { get; private set; }
        public string[] Log { get; set; }

        public MagyarPostaCheckQRResult()
        {
            Items = new List<MagyarPostaCheckQRResultItem>();
        }
    }

    public class MagyarPostaCheckQRResultItem
    {
        public MagyarPostaCheckQRData Data { get; set; }
        public string QRCode { get; set; }
        public byte[] PngImageBytes { get; set; }
    }
}
