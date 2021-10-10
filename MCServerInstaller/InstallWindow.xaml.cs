using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

namespace MCServerInstaller
{
    /// <summary>
    /// Interaction logic for InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {
        public InstallWindow()
        {
            InitializeComponent();
            Loaded += install;
        }

        private string getJavaVersion()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "java.exe";
                psi.Arguments = " -version";
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;

                Process pr = Process.Start(psi);
                string strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");

                return (strOutput);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ("");
            }
        }
        private async void install(object sender, RoutedEventArgs e)
        {
            Name name = new Name();
            name.ShowDialog();
            if (MainWindow.selectedSoftware == "Spigot")
            {
                Directory.CreateDirectory("build");
                StatusTxt.Content = "Downloading BuildTools...";
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new System.Uri(MainWindow.fileUrls + "BuildTools.jar"), "build\\BuildTools.jar");
                }
                File.Create("build\\build.bat").Close();
                using (StreamWriter sw = new StreamWriter("build\\build.bat"))
                {
                    sw.WriteLine(@"cd /d " + MainWindow.startupPath + @"\build");
                    sw.WriteLine("java -jar BuildTools.jar --rev " + MainWindow.selectedVersion);
                }
                StatusTxt.Content = "Building jar, this will take a while!";
                await Task.Delay(100);
                Process p2 = new Process();
                p2.StartInfo.FileName = "build\\build.bat";
                p2.Start();
                p2.WaitForExit();
                Directory.CreateDirectory("Servers\\" + MainWindow.name);
                foreach (string file in Directory.GetFiles("build"))
                {
                    if (file.Contains("spigot") && file.Contains(".jar"))
                    {
                        File.Move(file, "Servers\\" + MainWindow.name + "\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        File.Create("Servers\\" + MainWindow.name + "\\StartServer.bat").Close();
                        using (StreamWriter sw = new StreamWriter("Servers\\" + MainWindow.name + "\\StartServer.bat"))
                        {
                            sw.WriteLine(@"cd /d " + MainWindow.startupPath + @"\servers\" + MainWindow.name);
                            sw.WriteLine("java -Xmx2048M -Xms2048M -jar " + file.Split('\\')[file.Split('\\').Length - 1] + " nogui");
                        }
                    }
                    if (file.Contains("craftbukkit") && file.Contains(".jar"))
                    {
                        File.Delete(file);
                    }
                }
            }
            if (MainWindow.selectedSoftware == "CraftBukkit")
            {
                Directory.CreateDirectory("build");
                StatusTxt.Content = "Downloading BuildTools...";
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new System.Uri(MainWindow.fileUrls + "BuildTools.jar"), "build\\BuildTools.jar");
                }
                File.Create("build\\build.bat").Close();
                using (StreamWriter sw = new StreamWriter("build\\build.bat"))
                {
                    sw.WriteLine(@"cd /d " + MainWindow.startupPath + @"\build");
                    sw.WriteLine("java -jar BuildTools.jar --rev " + MainWindow.selectedVersion);
                }
                StatusTxt.Content = "Building jar, this will take a while!";
                await Task.Delay(100);
                Process p2 = new Process();
                p2.StartInfo.FileName = "build\\build.bat";
                p2.Start();
                p2.WaitForExit();
                Directory.CreateDirectory("Servers\\" + MainWindow.name);
                foreach (string file in Directory.GetFiles("build"))
                {
                    if (file.Contains("craftbukkit") && file.Contains(".jar"))
                    {
                        File.Move(file, "Servers\\" + MainWindow.name + "\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        File.Create("Servers\\" + MainWindow.name + "\\StartServer.bat").Close();
                        using (StreamWriter sw = new StreamWriter("Servers\\" + MainWindow.name + "\\StartServer.bat"))
                        {
                            sw.WriteLine(@"cd /d " + MainWindow.startupPath + @"\servers\" + MainWindow.name);
                            sw.WriteLine("java -Xmx2048M -Xms2048M -jar " + file.Split('\\')[file.Split('\\').Length - 1] + " nogui");
                        }
                    }
                    if (file.Contains("spigot") && file.Contains(".jar"))
                    {
                        File.Delete(file);
                    }
                }
            }
            if (MainWindow.selectedSoftware == "Forge" || MainWindow.selectedSoftware == "Fabric" || MainWindow.selectedSoftware == "Paper" || MainWindow.selectedSoftware == "Magma" || MainWindow.selectedSoftware == "Mohist")
            {
                StatusTxt.Content = "Downloading " + MainWindow.selectedSoftware + "...";
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new System.Uri(MainWindow.fileUrls + MainWindow.selectedVersion), "temp\\server.zip");
                }
                ZipFile.ExtractToDirectory("temp\\server.zip", "Servers\\" + MainWindow.name);
                File.Delete("temp\\server.zip");
                File.Create("Servers\\" + MainWindow.name + "\\StartServer.bat").Close();
                StatusTxt.Content = "Creating batch file...";
                using (StreamWriter sw = new StreamWriter("Servers\\" + MainWindow.name + "\\StartServer.bat"))
                {
                    sw.WriteLine(@"cd /d " + MainWindow.startupPath + @"\Servers\" + MainWindow.name);
                    string server = null;
                    foreach (string file in Directory.GetFiles("Servers\\" + MainWindow.name))
                    {
                        if (file.Split('\\')[file.Split('\\').Length - 1].Contains("forge") || file.Split('\\')[file.Split('\\').Length - 1].Contains("fabric") || file.Split('\\')[file.Split('\\').Length - 1].Contains("paper") || file.Split('\\')[file.Split('\\').Length - 1].Contains("Magma") || file.Split('\\')[file.Split('\\').Length - 1].Contains("Mohist") || file.Split('\\')[file.Split('\\').Length - 1].Contains("mohist") && file.Contains(".jar"))
                        {
                            server = file.Split('\\')[file.Split('\\').Length - 1];
                        }
                    }
                    if (server == null)
                    {
                        MessageBox.Show("An unknown error occured!");
                        this.Hide();
                        return;
                    }
                    sw.WriteLine(@"java -Xmx4G -Xms4G -jar " + server + " nogui");
                }
            }
            File.Create("Servers\\" + MainWindow.name + "\\eula.txt").Close();
            using (StreamWriter sw = new StreamWriter("Servers\\" + MainWindow.name + "\\eula.txt"))
            {
                sw.WriteLine("eula=true");
            }
            Process p = new Process();
            p.StartInfo.FileName = "Servers\\" + MainWindow.name + "\\StartServer.bat";
            p.Start();
            if (MainWindow.selectedVersion == "Magma-1.12.2.zip")
            {
                p.WaitForExit();
                p.Start();
            }
            await Task.Delay(3000);
            this.Hide();
        }
    }
}
