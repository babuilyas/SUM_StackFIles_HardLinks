using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generate_HardLinks
{
    class Program
    {
        static string js = @"
// https://github.com/babuilyas/SUM_StackFIles_HardLinks
// build array of files missing
var a = [ 
<FILES>
];

// todo: display all items by paging down
// todo: select all items and run following script

$('td').find('input').each(function(indx){console.log(this.checked)});
a.forEach(function(item){
console.log(item);
sap.ui.getCore().byId($(""td:contains('""+item+""')"").closest('td').prev('td').children(0)[0].id).fireSelect();
});
$('td').find('input').each(function(indx){ console.log(this.checked)});
";

        static void Main(string[] args)
        {           

            var xDocument = System.Xml.Linq.XDocument.Load("stackfiles.xls");
            // XML elements are in the "ss" namespace.
            System.Xml.Linq.XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";
            var rows = xDocument.Root.Element(ss + "Worksheet")
                            .Element(ss + "Table").Elements(ss + "Row");
            var files = from c in rows                        
                        select c.Elements(ss + "Cell").ElementAt(1).Element(ss + "Data").Value;
            var notfound = new List<string>();
            int i = 1;
            for (var j = 5; j < files.Count(); j++)
            {
                var file = files.ElementAt(j);
                Console.WriteLine("{0}/{1} {2}", i++, files.Count() - 5, file);
                var xfiles = new List<System.IO.FileInfo>();
                if (file.Length > 0)
                {
                    foreach (var dir in new System.IO.DirectoryInfo(Environment.CurrentDirectory).Root.GetDirectories())
                    {
                        try
                        {
                            var xfiles2 = dir.GetFiles(new System.IO.FileInfo(file).Name, System.IO.SearchOption.AllDirectories);
                            xfiles.AddRange(xfiles2.ToList<System.IO.FileInfo>());
                        }
                        catch { continue; }
                    }
                }
                if (xfiles.Count > 0)
                    CreateHardLink(Environment.CurrentDirectory + "\\" + file, xfiles[0].FullName, IntPtr.Zero);
                else
                    notfound.Add(file);
            }
            if (notfound.Count > 0)
            {
                var txt = System.IO.File.CreateText("notfound.txt");
                var sb = new StringBuilder();
                foreach (var file in notfound)
                    sb.AppendFormat("'{0}',", file);
                sb.Remove(sb.Length - 1, 1);
                js = js.Replace("<FILES>", sb.ToString());
                txt.WriteLine(js);
                txt.Close();
            }
        }
        static void Main2(string[] args)
        {
            var files = System.IO.File.ReadAllLines("stackfiles.txt");
            var notfound = new List<string>();
            int i = 1;
            foreach (var file in files)
            {
                Console.WriteLine("{0}/{1} {2}", i++, files.Count(), file);
                var xfiles = new List<System.IO.FileInfo>();
                if (file.Length > 0)
                {
                    foreach (var dir in new System.IO.DirectoryInfo(Environment.CurrentDirectory).Root.GetDirectories())
                    {
                        try
                        {
                            var xfiles2 = dir.GetFiles(new System.IO.FileInfo(file).Name, System.IO.SearchOption.AllDirectories);
                            xfiles.AddRange(xfiles2.ToList<System.IO.FileInfo>());
                        }
                        catch { continue; }
                    }
                }
                if (xfiles.Count > 0)
                    CreateHardLink(Environment.CurrentDirectory + "\\" + file, xfiles[0].FullName, IntPtr.Zero);
                else
                    notfound.Add(file);
            }
            if (notfound.Count > 0)
            {
                var txt = System.IO.File.CreateText("notfound.txt");
                var sb = new StringBuilder();
                foreach (var file in notfound)
                    sb.AppendFormat("'{0}',", file);
                sb.Remove(sb.Length - 1, 1);
                js = js.Replace("<FILES>", sb.ToString());
                txt.WriteLine(js);
                txt.Close();
            }
        }
        [System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern bool CreateHardLink(
         string lpFileName,
         string lpExistingFileName,
         IntPtr lpSecurityAttributes
         );
    }
}
