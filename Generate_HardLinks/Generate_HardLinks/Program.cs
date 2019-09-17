using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generate_HardLinks
{
    class Program
    {
        static void Main(string[] args)
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
                foreach (var file in notfound)
                    txt.WriteLine(file);
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
