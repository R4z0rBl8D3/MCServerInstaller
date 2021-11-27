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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using Microsoft.Toolkit.Uwp.Notifications;

namespace MCServerInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string fileUrls = "https://github.com/R4z0rBl8D3/MCServerInstaller/releases/download/AppData/";
        public static int selectedInstalled = -1;
        public static int selectedAvailable = -1;
        public static string name = null;
        public static string editPath = null;
        public static string startupPath = Environment.CurrentDirectory;
        public static string selectedSoftware = null;
        public static string selectedVersion = null;
        public static float version = 1.4f;
        public static string releaseNotes = "-Added a feature to copy everything from a CurseForge modpack in case it does not work";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += onLoad;
            SoftwareListBox.Items.Add("Vanilla");
            SoftwareListBox.Items.Add("CraftBukkit");
            SoftwareListBox.Items.Add("Fabric");
            SoftwareListBox.Items.Add("Forge");
            SoftwareListBox.Items.Add("Magma");
            SoftwareListBox.Items.Add("Mohist");
            SoftwareListBox.Items.Add("Paper");
            SoftwareListBox.Items.Add("Spigot");
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

        private void update(float lVersion)
        {
            MessageBoxResult messageResult = MessageBox.Show("There is a new version of MCServerInstaller, do you want to install it? Current version: v" + version + " Latest version: v" + lVersion, "Update", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                //gud
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile("https://github.com/R4z0rBl8D3/AppUpdater/releases/download/v1.0/Release.zip", "updater.zip");
            }
            ZipFile.ExtractToDirectory("updater.zip", "Updater");
            File.Delete("updater.zip");
            File.Create("Updater\\Startup.txt").Close();
            using (StreamWriter sw = new StreamWriter("Updater\\Startup.txt"))
            {
                sw.WriteLine("MCServerInstaller.exe");
            }
            File.Create("Updater\\Update.txt").Close();
            using (StreamWriter sw = new StreamWriter("Updater\\Update.txt"))
            {
                sw.WriteLine("https://github.com/R4z0rBl8D3/MCServerInstaller/releases/download/v" + lVersion + "/Release.zip");
                sw.WriteLine("Servers");
            }
            Process.Start("Updater\\AppUpdater.exe");
            this.Close();
        }

        private async void onLoad(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists("Updater"))
            {
                if (!File.Exists("Updater\\Log.txt"))
                {
                    MessageBox.Show("Update failed!");
                }
                else
                {
                    using (StreamReader sr = new StreamReader("Updater\\Log.txt"))
                    {
                        string log = sr.ReadLine();
                        if (log == "Successful")
                        {
                            MessageBox.Show("Update successful!");
                            MessageBox.Show(releaseNotes, "Version v" + version);
                        }
                        else
                        {
                            MessageBox.Show("Update failed!");
                        }
                    }
                }
                Directory.Delete("Updater", true);
            }
            if (File.Exists("latest.txt"))
            {
                File.Delete("latest.txt");
            }
            using (var client = new WebClient())
            {
                client.DownloadFile(fileUrls + "latest.txt", "latest.txt");
            }
            float lVersion = 0;
            using (StreamReader sr = new StreamReader("latest.txt"))
            {
                lVersion = float.Parse(sr.ReadLine());
            }
            if (lVersion > version)
            {
                update(lVersion);
            }
            if (File.Exists("versions.txt"))
            {
                File.Delete("versions.txt");
            }
            using (var client = new WebClient())
            {
                client.DownloadFile(fileUrls + "versions.txt", "versions.txt");
            }
            while (true)
            {
                selectedAvailable = AvailableListBox.SelectedIndex;
                selectedInstalled = InstalledListBox.SelectedIndex;
                reload();
                AvailableListBox.SelectedIndex = selectedAvailable;
                InstalledListBox.SelectedIndex = selectedInstalled;
                await Task.Delay(1000);
            }
        }

        private void reload()
        {
            if (!Directory.Exists("Servers"))
            {
                Directory.CreateDirectory("Servers");
            }
            if(!Directory.Exists("Temp"))
            {
                Directory.CreateDirectory("Temp");
            }
            InstalledListBox.Items.Clear();
            AvailableListBox.Items.Clear();
            string[] files = Directory.GetDirectories("Servers");
            foreach (string file in files)
            {
                InstalledListBox.Items.Add(file.Split('\\')[1]);
            }
            using (StreamReader sr = new StreamReader("versions.txt"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (SoftwareListBox.SelectedIndex == -1)
                    {
                        return;
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Forge")
                    {
                        if (line.Contains("Forge"))
                        {
                            AvailableListBox.Items.Add(line);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Spigot")
                    {
                        if (line.Contains("Spigot"))
                        {
                            AvailableListBox.Items.Add(line.Split('-')[1]);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Paper")
                    {
                        if (line.Contains("paper"))
                        {
                            AvailableListBox.Items.Add(line);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Magma")
                    {
                        if (line.Contains("Magma"))
                        {
                            AvailableListBox.Items.Add(line);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Mohist")
                    {
                        if (line.Contains("Mohist"))
                        {
                            AvailableListBox.Items.Add(line);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "CraftBukkit")
                    {
                        if (line.Contains("Bukkit"))
                        {
                            AvailableListBox.Items.Add(line.Split('-')[1]);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Fabric")
                    {
                        if (line.Contains("Fabric"))
                        {
                            AvailableListBox.Items.Add(line);
                        }
                    }
                    if (SoftwareListBox.SelectedItem.ToString() == "Vanilla")
                    {
                        if (line.Contains("Vanilla"))
                        {
                            AvailableListBox.Items.Add(line);
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InstalledListBox.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select something!");
                return;
            }
            Process p = new Process();
            //p.StartInfo.FileName = "Servers\\" + InstalledListBox.SelectedItem.ToString() + "\\StartServer.bat";
            //p.Start();
            p.StartInfo.UseShellExecute = false;
            Process.Start(startupPath + @"\Servers\" + InstalledListBox.SelectedItem.ToString() + "\\StartServer.bat");
        }

        private async void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableListBox.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select something!");
                return;
            }
            selectedSoftware = SoftwareListBox.SelectedItem.ToString();
            selectedVersion = AvailableListBox.SelectedItem.ToString();
            InstallWindow installWindow = new InstallWindow();
            installWindow.ShowDialog();
            return;
            Name nameWindow = new Name();
            nameWindow.ShowDialog();
            if (SoftwareListBox.SelectedItem.ToString() == "Bukkit" || SoftwareListBox.SelectedItem.ToString() == "Spigot")
            {
                Directory.CreateDirectory("build");
                using (var client = new WebClient())
                {
                    client.DownloadFile(fileUrls + "BuildTools.jar", "build\\BuildTools.jar");
                }
                File.Create("build\\build.bat");
                using (StreamWriter sw = new StreamWriter("build\\build.bat"))
                {
                    sw.WriteLine(@"cd /d " + startupPath + @"\build");
                    sw.WriteLine("java -jar BuildTools.jar --rev " + InstalledListBox.SelectedItem);
                }
                Process pro = new Process();
                pro.StartInfo.FileName ="build\\BuildTools.jar";
                pro.Start();
                pro.WaitForExit();
            }
            else
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(fileUrls + AvailableListBox.SelectedItem, name + ".zip");
                }
                if (Directory.Exists("Temp\\" + name))
                {
                    Directory.Delete("Temp\\" + name);
                }
                ZipFile.ExtractToDirectory(name + ".zip", "Temp");
                File.Delete(name + ".zip");
                if (Directory.Exists("Temp\\" + System.IO.Path.ChangeExtension(AvailableListBox.SelectedItem.ToString(), null)))
                {
                    //good
                }
                else
                {
                    Directory.CreateDirectory("Temp\\" + System.IO.Path.ChangeExtension(AvailableListBox.SelectedItem.ToString(), null));
                    File.Move("Temp\\" + System.IO.Path.ChangeExtension(AvailableListBox.SelectedItem.ToString(), null) + ".jar", "Temp\\" + System.IO.Path.ChangeExtension(AvailableListBox.SelectedItem.ToString(), null) + "\\" + AvailableListBox.SelectedItem.ToString());
                }
                Directory.Move("Temp\\" + System.IO.Path.ChangeExtension(AvailableListBox.SelectedItem.ToString(), null), "Servers\\" + name);
            }
            string folderPath = "Servers\\" + name;
            using (StreamWriter sw = new StreamWriter("Servers\\" + name + "\\StartServer.bat"))
            {
                string serverPath = null;
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    if (file.Contains("forge"))
                    {
                        serverPath = file;
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("minecraft"))
                        {
                            serverPath = file;
                        }
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("craftbukkit"))
                        {
                            serverPath = file;
                        }
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("fabric"))
                        {
                            serverPath = file;
                        }
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("Magma"))
                        {
                            serverPath = file;
                        }
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("Mohist"))
                        {
                            serverPath = file;
                        }
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("paper"))
                        {
                            serverPath = file;
                        }
                    }
                }
                if (serverPath == null)
                {
                    foreach (string file in files)
                    {
                        if (file.Contains("spigot"))
                        {
                            serverPath = file;
                        }
                    }
                }
                sw.WriteLine(@"cd /d " + startupPath + @"\Servers\" + name);
                sw.WriteLine("java -Xmx2048M -Xms2048M -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true nogui");
                sw.Close();
            }
            Process.Start("Servers\\" + name + "\\StartServer.bat");
            while (!File.Exists("Servers\\" + name + "\\eula.txt"))
            {
                await Task.Delay(100);
            }
            await Task.Delay(1000);
            string[] arrLine = File.ReadAllLines("Servers\\" + name + "\\eula.txt");
            int i = 0;
            foreach (string line in arrLine)
            {
                if (line.Contains("eula"))
                {
                    arrLine[i] = "eula=true";
                }
                i++;
            }
            File.WriteAllLines("Servers\\" + name + "\\eula.txt", arrLine);
            Process p = new Process();
            p.StartInfo.FileName = "Servers\\" + name + "\\StartServer.bat";
            p.Start();
            while (!Directory.Exists("Servers\\" + name + "\\world"))
            {
                await Task.Delay(100);
            }
            p.Close();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to delete '"  + InstalledListBox.SelectedItem + "'?", "Confirm Delete", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                try
                {
                    Directory.Delete("Servers\\" + InstalledListBox.SelectedItem, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InstalledListBox.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select something!");
                return;
            }
            editPath = "Servers\\" + InstalledListBox.SelectedItem.ToString();
            EditWindow editWindow = new EditWindow();
            editWindow.ShowDialog();
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("latest.txt"))
            {
                File.Delete("latest.txt");
            }
            using (var client = new WebClient())
            {
                client.DownloadFile(fileUrls + "latest.txt", "latest.txt");
            }
            float lVersion = 0;
            using (StreamReader sr = new StreamReader("latest.txt"))
            {
                lVersion = float.Parse(sr.ReadLine());
            }
            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to download the latest version of MCServerInstaller? Current version: v" + version + " Latest version: v" + lVersion, "Do you want to update?", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                //gud
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile("https://github.com/R4z0rBl8D3/AppUpdater/releases/download/v1.0/Release.zip", "updater.zip");
            }
            ZipFile.ExtractToDirectory("updater.zip", "Updater");
            File.Delete("updater.zip");
            File.Create("Updater\\Startup.txt").Close();
            using (StreamWriter sw = new StreamWriter("Updater\\Startup.txt"))
            {
                sw.WriteLine("MCServerInstaller.exe");
            }
            File.Create("Updater\\Update.txt").Close();
            using (StreamWriter sw = new StreamWriter("Updater\\Update.txt"))
            {
                sw.WriteLine("https://github.com/R4z0rBl8D3/MCServerInstaller/releases/download/v" + lVersion + "/Release.zip");
                sw.WriteLine("Servers");
            }
            Process.Start("Updater\\AppUpdater.exe");
            this.Close();
        }

        private void aVersionBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Current app version is: v" + version);
        }

        private void jVersionBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Your Java version is: " + getJavaVersion());
        }

        private void rNotesBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(releaseNotes, "Version v" + version);
        }
    }
}
