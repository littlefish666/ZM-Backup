using System;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace ZmodelerBackup
{

    class IniFile   // revision 11
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Zmodeler3 Backup";
            // Some random checks and declarations and ini stuff
            var settings = new IniFile("Settings.ini");

            string reset = settings.Read("Reset", "Files").ToString();

            string trueVal = "True";

            if (reset == trueVal)
            {
                File.Delete("Settings.ini");
                settings.Write("Reset", "False", "Plugin");
                settings.Write("Transfer Delay", "10000", "Plugin");
                settings.Write("SaveInterval", "5", "Files");
                //settings.Write(".Z3D Compression", "True", "Files");
                Environment.Exit(0);
            }

            
            

            //Zmodeler Profile Work and other stuff

            string[] profileXml = File.ReadAllLines("ZModeler3_profile.xml");
            string saveInterval = settings.Read("saveInterval", "Files");
            int intervalMin = Convert.ToInt32(saveInterval);


            string delay = settings.Read("Transfer Delay", "Plugin");
            int delayStr = Convert.ToInt32(delay);
            int intervalMil = (intervalMin * 60000) + delayStr;

            string compression = settings.Read(".Z3D Compression", "Files");
            if (compression == "False")
            {
                int counter = 0;
                string line;

                // Read the file line by line
                StreamReader file = new System.IO.StreamReader("ZModeler3_profile.xml");
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(@"              <zmp:key name=""Use Compression"" type=""l"" value=""1""/>"))
                    {
                        var lines = File.ReadAllLines("ZModeler3_profile.xml");
                        lines[counter] = @"              <zmp:key name=""Use Compression"" type=""l"" value=""0""/>";
                        File.WriteAllLines("profile.xml", lines);
                    }

                    counter++;
                }
                file.Close();
            }

            else if(compression == "True")
            {
                int counter = 0;
                string line;

                // Read the file and display it line by line.
                StreamReader file = new StreamReader("ZModeler3_profile.xml");
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(@"              <zmp:key name=""Use Compression"" type=""l"" value=""0""/>"))
                    {
                        string[] lines = File.ReadAllLines("ZModeler3_profile.xml");
                        lines[counter] = @"              <zmp:key name=""Use Compression"" type=""l"" value=""1""/>";
                        File.WriteAllLines("profile.xml", lines);
                    }

                    counter++;
                }
                file.Close();
            }

            else
            {
                Console.WriteLine("Z3D Compression was not a valid value (True or False)");
            }

            File.Delete("ZModeler3_profile.xml");
            string[] toWrite = File.ReadAllLines("profile.xml");
            File.WriteAllLines("ZModeler3_profile.xml", toWrite);




            string saveIntervalINI = settings.Read("SaveInterval", "Files");
            Convert.ToInt32(saveIntervalINI);
            int parsedTicket;
            if (int.TryParse(saveIntervalINI, out parsedTicket))
            {
                int counter = 0;
                string line;
                StreamReader file = new System.IO.StreamReader("ZModeler3_profile.xml");
                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith(@"              <zmp:key name=""Autosave Interval(minutes)"""))
                    {
                        var lines = File.ReadAllLines("ZModeler3_profile.xml");
                        string half1 = @"              <zmp:key name=""Autosave Interval(minutes)"" type=""l"" value=";
                        string half2 = @"""/>";
                        string autosaveSetting = half1 + saveIntervalINI + half2;
                        lines[counter] = autosaveSetting;
                        File.WriteAllLines("profile.xml", lines);
                    }

                    counter++;
                }
                file.Close();
            }           

            else
            {
                Console.WriteLine("Save interval was not an integer");
                Environment.Exit(0);
            }

            File.Delete("ZModeler3_profile.xml");
            string[] toWrite2 = File.ReadAllLines("profile.xml");
            File.WriteAllLines("ZModeler3_profile.xml", toWrite);
            File.Delete("profile.xml");












            File.Delete("zmodPlugin.log");
            string zmodLog = "zmodPlugin.log";

            if(!File.Exists("ZModeler3.exe"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Zmodeler3 not found in this folder, please locate this exe to the same folder as Zmodeler3");
                Thread.Sleep(5000);
                Console.ReadKey();
                System.Environment.Exit(0);
                string dateTime0 = DateTime.Now.ToString();
                File.WriteAllText(zmodLog, dateTime0 + @" Zmodeler 3 exe was not found in this folder, either rename it to ""ZModeler3.exe"" or move the plugin into same folder " + System.Environment.NewLine);
            }

            string shortDate = DateTime.Now.ToShortDateString();
            string sdFix = shortDate.Replace(@"\", ".");
            Console.WriteLine("Any key to start program, Zmodeler3 will be automatically launched");
            Console.ReadKey();               

            Console.WriteLine("Scanning to find Zmodeler3 process");
            foreach (System.Diagnostics.Process zmodProc in System.Diagnostics.Process.GetProcesses())
            {
                
                if (zmodProc.ProcessName == "ZModeler3")
                {
                    string dateTime2 = DateTime.Now.ToString();
                    zmodProc.Kill();
                    File.AppendAllText(zmodLog, dateTime2 + ": Zmodeler process found and closed" + System.Environment.NewLine);
                }

                else
                {
                    string dateTime2 = DateTime.Now.ToString();
                    File.AppendAllText(zmodLog, dateTime2 + ": Zmodeler3 not open/process was not found" + System.Environment.NewLine);
                }
            }

            File.AppendAllText(zmodLog, "#############################################################" + System.Environment.NewLine);




            //System.Diagnostics.Process.Start("ZModeler3.exe");

            Console.WriteLine("Conducting basic searches...");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("LOG");


            DateTime now2 = DateTime.Now;

            //Declaring target directories
            string currentdirFull = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string currentDir = currentdirFull.Substring(0, currentdirFull.Length - 18);

            
            Timer time = new System.Threading.Timer(onTick, null, 0, intervalMil); // 5 minutes  = 300000, to take into account saving & compression - 330000
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }
        

        static void onTick(object o)
        {
            DateTime autosave = DateTime.Now;
            Console.WriteLine("Autosave.z3d moved to backup folder:   " + autosave);
            File.AppendAllText("zmodPlugin.log", "autosave.z3d moved to folder: " + autosave + System.Environment.NewLine);
            File.AppendAllText("zmodPlugin.log", "" + System.Environment.NewLine);
            string saveFolderShort = DateTime.Now.ToShortDateString();
            string pluginFolder = @"SAVES\";
            string folderUpdate = pluginFolder + DateTime.Now.ToString("yyyy-MM-dd---HH.mm");
            Directory.CreateDirectory(folderUpdate);
            Thread.Sleep(100);
            string toCopyTo =  @"SAVES\" + DateTime.Now.ToString(@"yyyy-MM-dd---HH.mm") + @"\" + "autosave.z3d";            
            //Console.WriteLine(toCopyTo);
            File.Copy("autosave.z3d", toCopyTo, true);
        }

          



            
        
    }
}