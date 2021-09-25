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

        public MainWindow()
        {
            InitializeComponent();
            Loaded += onLoad;
        }

        private async void onLoad(object sender, RoutedEventArgs e)
        {
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
                    AvailableListBox.Items.Add(line);
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
            Process.Start(@"D:\VisualStudioRepos\MCServerInstaller\MCServerInstaller\bin\Debug\Servers\" + InstalledListBox.SelectedItem.ToString() + "\\StartServer.bat");
        }

        private async void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableListBox.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select something!");
                return;
            }
            Name nameWindow = new Name();
            nameWindow.ShowDialog();
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
            Directory.Move("Temp\\" + System.IO.Path.ChangeExtension(AvailableListBox.SelectedItem.ToString(), null), "Servers\\" + name);
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
                sw.WriteLine(@"cd /d D:\VisualStudioRepos\MCServerInstaller\MCServerInstaller\bin\Debug\Servers\" + name);
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
            arrLine[2] = "eula=true";
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
    }
}
