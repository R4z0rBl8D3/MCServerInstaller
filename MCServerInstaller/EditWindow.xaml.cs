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
using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;
using System.Net;
using System.IO.Compression;

namespace MCServerInstaller
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public static List<string> properties = new List<string>();
        public static string[] saveProperties = { };
        
        public EditWindow()
        {
            InitializeComponent();
            Loaded += onLoad;
            ModsListBox.Drop += modDrop;
            DatapacksListBox.Drop += datapackDrop;
            PluginsListBox.Drop += pluginDrop;
        }

        private void modDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    MessageBox.Show(file + " already exists! Skipping");
                }
                else
                {
                    ModsListBox.Items.Add(file.Split('\\')[file.Split('\\').Length - 1]);
                    File.Copy(file, MainWindow.editPath + "\\plugins\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                }
            }
        }

        private void datapackDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    MessageBox.Show(file + " already exists! Skipping");
                }
                else
                {
                    DatapacksListBox.Items.Add(file.Split('\\')[file.Split('\\').Length - 1]);
                    File.Copy(file, MainWindow.editPath + "\\plugins\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                }
            }
        }

        private void pluginDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    MessageBox.Show(file + " already exists! Skipping");
                }
                else
                {
                    PluginsListBox.Items.Add(file.Split('\\')[file.Split('\\').Length - 1]);
                    File.Copy(file, MainWindow.editPath + "\\plugins\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                }
            }
        }

        private void load()
        {
            if (!File.Exists(MainWindow.editPath + "\\server.properties"))
            {
                StatusTxt.Content = "server.properties failed to load!";
                MessageBox.Show("Failed to load!");
                this.Hide();
                return;
            }
            if (File.Exists(MainWindow.editPath + "\\Java.txt"))
            {
                using (StreamReader sr = new StreamReader(MainWindow.editPath + "\\Java.txt"))
                {
                    string version = sr.ReadLine();
                    if (version == "8")
                    {
                        Java8.IsChecked = true;
                    }
                    else
                    {
                        if (version == "11")
                        {
                            Java11.IsChecked = true;
                        }
                        else
                        {
                            if (version == "16")
                            {
                                Java16.IsChecked = true;
                            }
                            else
                            {
                                MessageBox.Show("Failed to load!");
                                this.Hide();
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                NoJava.IsChecked = true;
            }
            StatusTxt.Content = "Loading properties...";
            properties.Clear();
            using (StreamReader sr = new StreamReader(MainWindow.editPath + "\\server.properties"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    properties.Add(line);
                    line = sr.ReadLine();
                }
            }
            foreach (string property in properties)
            {
                if (property.StartsWith("allow-flight"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        AllowFlightCheckbox.IsChecked = true;
                    }
                    else
                    {
                        AllowFlightCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("allow-nether"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        AllowNetherCheckbox.IsChecked = true;
                    }
                    else
                    {
                        AllowNetherCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("force-gamemode"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        ForceGamemodeCheckbox.IsChecked = true;
                    }
                    else
                    {
                        ForceGamemodeCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("hardcore"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        HardcoreCheckbox.IsChecked = true;
                    }
                    else
                    {
                        HardcoreCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("online-mode"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        OnlineModeCheckbox.IsChecked = true;
                    }
                    else
                    {
                        OnlineModeCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("pvp"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        PVPCheckbox.IsChecked = true;
                    }
                    else
                    {
                        PVPCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("spawn-animals"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        SpawnAnimalsCheckbox.IsChecked = true;
                    }
                    else
                    {
                        SpawnAnimalsCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("spawn-monsters"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        SpawnMonstersCheckbox.IsChecked = true;
                    }
                    else
                    {
                        SpawnMonstersCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("spawn-npcs"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        SpawnNPCsCheckbox.IsChecked = true;
                    }
                    else
                    {
                        SpawnNPCsCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("enable-command-block"))
                {
                    if (property.Split('=')[1] == "true")
                    {
                        EnableCommandBlocksCheckbox.IsChecked = true;
                    }
                    else
                    {
                        EnableCommandBlocksCheckbox.IsChecked = false;
                    }
                }
                if (property.StartsWith("difficulty"))
                {
                    DifficultyBox.Text = property.Split('=')[1];
                }
                if (property.StartsWith("server-ip"))
                {
                    ServerIPBox.Text = property.Split('=')[1];
                }
                if (property.StartsWith("server-port"))
                {
                    ServerPortBox.Text = property.Split('=')[1];
                }
                if (property.StartsWith("motd"))
                {
                    MOTDBox.Text = property.Split('=')[1];
                }
                StatusTxt.Content = "";
            }
            if (!File.Exists(MainWindow.editPath + "\\StartServer.bat"))
            {
                MessageBox.Show("Server was not set up properly");
            }
            using (StreamReader sr = new StreamReader(MainWindow.editPath + "\\StartServer.bat"))
            {
                sr.ReadLine();
                string line = sr.ReadLine();
                try
                {
                  MemoryBox.Text = line.Split(' ')[1].Split('x')[1];
                }
                catch
                {
                    MessageBox.Show("Server was not set up properly");
                }
            }
            using (StreamReader sr = new StreamReader(MainWindow.editPath + "\\StartServer.bat"))
            {
                string line = sr.ReadLine();
                bool gui = true;
                while (line != null)
                {
                    if (line.Contains("nogui"))
                    {
                        gui = false;
                    }
                    line = sr.ReadLine();
                }
                GuiCheckbox.IsChecked = gui;
            }
            if (Directory.Exists(MainWindow.editPath + "\\mods"))
            {
                StatusTxt.Content = "Loading mods...";
                ModsListBox.Items.Clear();
                string[] mods = Directory.GetFiles(MainWindow.editPath + "\\mods");
                foreach (string mod in mods)
                {
                    ModsListBox.Items.Add(mod.Split('\\')[mod.Split('\\').Length - 1]);
                }
            }
            if (Directory.Exists(MainWindow.editPath + "\\plugins"))
            {
                StatusTxt.Content = "Loading plugins...";
                PluginsListBox.Visibility = Visibility.Visible;
                PluginsDeleteBtn.Visibility = Visibility.Visible;
                NoPluginsLbl.Visibility = Visibility.Collapsed;
                NoPluginsLbl2.Visibility = Visibility.Collapsed;
                PluginsListBox.Items.Clear();
                string[] plugins = Directory.GetFiles(MainWindow.editPath + "\\plugins");
                foreach (string plugin in plugins)
                {
                    PluginsListBox.Items.Add(plugin.Split('\\')[plugin.Split('\\').Length - 1]);
                }
            }
            else
            {
                PluginsListBox.Visibility = Visibility.Collapsed;
                PluginsDeleteBtn.Visibility = Visibility.Collapsed;
                NoPluginsLbl.Visibility = Visibility.Visible;
                NoPluginsLbl2.Visibility = Visibility.Visible;
            }
            if (Directory.Exists(MainWindow.editPath + "\\world\\datapacks"))
            {
                StatusTxt.Content = "Loading datapacks...";
                DatapacksListBox.Visibility = Visibility.Visible;
                DatapacksDeleteBtn.Visibility = Visibility.Visible;
                NoDatapacksLbl.Visibility = Visibility.Collapsed;
                NoDatapacksLbl2.Visibility = Visibility.Collapsed;
                DatapacksListBox.Items.Clear();
                string[] datapacks = Directory.GetFiles(MainWindow.editPath + "\\world\\datapacks");
                foreach (string datapack in datapacks)
                {
                    DatapacksListBox.Items.Add(datapack.Split('\\')[datapack.Split('\\').Length - 1]);
                }
            }
            else
            {
                DatapacksListBox.Visibility = Visibility.Collapsed;
                DatapacksDeleteBtn.Visibility = Visibility.Collapsed;
                NoDatapacksLbl.Visibility = Visibility.Visible;
                NoDatapacksLbl2.Visibility = Visibility.Visible;
            }
        }

        private void onLoad(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void lineChanger(string newText, string temp, int line_to_edit)
        {
            saveProperties[line_to_edit] = newText;
        }

        private void save()
        {
            int i = 0;
            saveProperties = new string[properties.Count()];
            properties.CopyTo(saveProperties);
            foreach (string property in properties)
            {
                if (property.StartsWith("allow-flight"))
                {
                    //property.Split('=')[0] + "=" + AllowFlightCheckbox.IsChecked.ToString().ToLower()
                    saveProperties[i] = property.Split('=')[0] + "=" + AllowFlightCheckbox.IsChecked.ToString().ToLower();
                }
                if (property.StartsWith("allow-nether"))
                {
                    lineChanger(property.Split('=')[0] + "=" + AllowNetherCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("force-gamemode"))
                {
                    lineChanger(property.Split('=')[0] + "=" + ForceGamemodeCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("hardcore"))
                {
                    lineChanger(property.Split('=')[0] + "=" + HardcoreCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("online-mode"))
                {
                    lineChanger(property.Split('=')[0] + "=" + OnlineModeCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("pvp"))
                {
                    lineChanger(property.Split('=')[0] + "=" + PVPCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("spawn-animals"))
                {
                    lineChanger(property.Split('=')[0] + "=" + SpawnAnimalsCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("spawn-npcs"))
                {
                    lineChanger(property.Split('=')[0] + "=" + SpawnNPCsCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("spawn-monsters"))
                {
                    lineChanger(property.Split('=')[0] + "=" + SpawnMonstersCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("enable-command-block"))
                {
                    lineChanger(property.Split('=')[0] + "=" + EnableCommandBlocksCheckbox.IsChecked.ToString().ToLower(), MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("difficulty"))
                {
                    lineChanger(property.Split('=')[0] + "=" + DifficultyBox.Text, MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("server-ip"))
                {
                    lineChanger(property.Split('=')[0] + "=" + ServerIPBox.Text, MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("server-port"))
                {
                    lineChanger(property.Split('=')[0] + "=" + ServerPortBox.Text, MainWindow.editPath + "\\server.properties", i);
                }
                if (property.StartsWith("motd"))
                {
                    lineChanger(property.Split('=')[0] + "=" + MOTDBox.Text, MainWindow.editPath + "\\server.properties", i);
                }
                i++;
            }
            File.WriteAllLines(MainWindow.editPath + "\\server.properties", saveProperties);
            string serverPath = null;
            using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\StartServer.bat"))
            {
                string[] files = Directory.GetFiles(MainWindow.editPath);
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
            }
            string startup = null;
            if (NoJava.IsChecked == false)
            {
                startup = @"Java\bin\java.exe -Xmx" + MemoryBox.Text + " -Xms" + MemoryBox.Text + " -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true";
            }
            else
            {
                startup = "java -Xmx" + MemoryBox.Text + " -Xms" + MemoryBox.Text + " -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true";
            }
            if (GuiCheckbox.IsChecked == false)
            {
                startup = startup + " nogui";
            }
            File.Delete(MainWindow.editPath + "\\StartServer.bat");
            File.Create(MainWindow.editPath + "\\StartServer.bat").Close();
            using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\StartServer.bat"))
            {
                sw.WriteLine(@"cd /d " + MainWindow.startupPath +  "\\" + MainWindow.editPath);
                sw.WriteLine(startup);
            }
            if (NoJava.IsChecked == true && Directory.Exists(MainWindow.editPath + "\\Java"))
            {
                try
                {
                    File.Delete(MainWindow.editPath + "\\Java.txt");
                    Directory.Delete(MainWindow.editPath + "\\Java", true);
                    startup = "java -Xmx" + MemoryBox.Text + " -Xms" + MemoryBox.Text + " -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true";
                    File.Delete(MainWindow.editPath + "\\StartServer.bat");
                    File.Create(MainWindow.editPath + "\\StartServer.bat").Close();
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\StartServer.bat"))
                    {
                        sw.WriteLine(@"cd /d " + MainWindow.startupPath + "\\" + MainWindow.editPath);
                        sw.WriteLine(startup);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            if (File.Exists(MainWindow.editPath + "\\Java.txt"))
            {
                using (StreamReader sr = new StreamReader(MainWindow.editPath + "\\Java.txt"))
                {
                    string version = sr.ReadLine();
                    if (Java8.IsChecked == true && version != "8")
                    {
                        Directory.Delete(MainWindow.editPath + "\\Java");
                        File.Delete(MainWindow.editPath + "\\Java.txt");
                    }
                    if (Java11.IsChecked == true && version != "11")
                    {
                        Directory.Delete(MainWindow.editPath + "\\Java");
                        File.Delete(MainWindow.editPath + "\\Java.txt");
                    }
                    if (Java16.IsChecked == true && version != "16")
                    {
                        Directory.Delete(MainWindow.editPath + "\\Java");
                        File.Delete(MainWindow.editPath + "\\Java.txt");
                    }
                }
            }
            if (Java8.IsChecked == true && !Directory.Exists(MainWindow.editPath + "\\Java"))
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(new System.Uri(MainWindow.fileUrls + "Java8.zip"), MainWindow.editPath + "\\Java.zip");
                    }
                    ZipFile.ExtractToDirectory(MainWindow.editPath + "\\Java.zip", MainWindow.editPath + "\\Java");
                    File.Delete(MainWindow.editPath + "\\Java.zip");
                    File.Create(MainWindow.editPath + "\\Java.txt");
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\Java.txt"))
                    {
                        sw.WriteLine("8");
                    }
                }
                catch
                {
                    MessageBox.Show("Could not download Java 8!");
                    startup = "java -Xmx" + MemoryBox.Text + " -Xms" + MemoryBox.Text + " -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true";
                    File.Delete(MainWindow.editPath + "\\StartServer.bat");
                    File.Create(MainWindow.editPath + "\\StartServer.bat").Close();
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\StartServer.bat"))
                    {
                        sw.WriteLine(@"cd /d " + MainWindow.startupPath + "\\" + MainWindow.editPath);
                        sw.WriteLine(startup);
                    }
                    if (File.Exists(MainWindow.editPath + "\\Java.txt"))
                    {
                        File.Delete(MainWindow.editPath + "\\Java.txt");
                    }
                }
            }
            if (Java11.IsChecked == true && !Directory.Exists(MainWindow.editPath + "\\Java"))
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(new System.Uri(MainWindow.fileUrls + "Java11.zip"), MainWindow.editPath + "\\Java.zip");
                    }
                    ZipFile.ExtractToDirectory(MainWindow.editPath + "\\Java.zip", MainWindow.editPath + "\\Java");
                    File.Delete(MainWindow.editPath + "\\Java.zip");
                    File.Create(MainWindow.editPath + "\\Java.txt");
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\Java.txt"))
                    {
                        sw.WriteLine("11");
                    }
                }
                catch
                {
                    MessageBox.Show("Could not download Java 11!");
                    startup = "java -Xmx" + MemoryBox.Text + " -Xms" + MemoryBox.Text + " -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true";
                    File.Delete(MainWindow.editPath + "\\StartServer.bat");
                    File.Create(MainWindow.editPath + "\\StartServer.bat").Close();
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\StartServer.bat"))
                    {
                        sw.WriteLine(@"cd /d " + MainWindow.startupPath + "\\" + MainWindow.editPath);
                        sw.WriteLine(startup);
                    }
                    if (File.Exists(MainWindow.editPath + "\\Java.txt"))
                    {
                        File.Delete(MainWindow.editPath + "\\Java.txt");
                    }
                }
            }
            if (Java16.IsChecked == true && !Directory.Exists(MainWindow.editPath + "\\Java"))
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(new System.Uri(MainWindow.fileUrls + "Java16.zip"), MainWindow.editPath + "\\Java.zip");
                    }
                    ZipFile.ExtractToDirectory(MainWindow.editPath + "\\Java.zip", MainWindow.editPath + "\\Java");
                    File.Delete(MainWindow.editPath + "\\Java.zip");
                    File.Create(MainWindow.editPath + "\\Java.txt");
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\Java.txt"))
                    {
                        sw.WriteLine("16");
                    }
                }
                catch
                {
                    MessageBox.Show("Could not download Java 16!");
                    startup = "java -Xmx" + MemoryBox.Text + " -Xms" + MemoryBox.Text + " -jar " + serverPath.Split('\\')[serverPath.Split('\\').Length - 1] + " true";
                    File.Delete(MainWindow.editPath + "\\StartServer.bat");
                    File.Create(MainWindow.editPath + "\\StartServer.bat").Close();
                    using (StreamWriter sw = new StreamWriter(MainWindow.editPath + "\\StartServer.bat"))
                    {
                        sw.WriteLine(@"cd /d " + MainWindow.startupPath + "\\" + MainWindow.editPath);
                        sw.WriteLine(startup);
                    }
                    if (File.Exists(MainWindow.editPath + "\\Java.txt"))
                    {
                        File.Delete(MainWindow.editPath + "\\Java.txt");
                    }
                }
            }
            new ToastContentBuilder()
                 .AddText("Minecraft Server Installer")
                 .AddText("Save completed!")
                 .Show();
            load();
        }

        private void ModsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedList = new List<string>();
            foreach (var item in ModsListBox.SelectedItems)
            {
                selectedList.Add(item.ToString());
            }
            if (selectedList.Count() == 0) { return; }
            MessageBoxResult messageResult = MessageBox.Show(string.Join(Environment.NewLine, selectedList), "Are you sure you want to delete", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                foreach (var item in ModsListBox.SelectedItems)
                {
                    File.Delete(MainWindow.editPath + "\\mods\\" + item.ToString());
                }
                ModsListBox.Items.Clear();
                string[] mods = Directory.GetFiles(MainWindow.editPath + "\\mods");
                foreach (string mod in mods)
                {
                    ModsListBox.Items.Add(mod.Split('\\')[mod.Split('\\').Length - 1]);
                }
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
            //MessageBox.Show("Are you sure you want to delete the selected items: " + Environment.NewLine +
                    //string.Join(Environment.NewLine, selectedList));
        }

        private void PropertiesBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", MainWindow.editPath + "\\server.properties");
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            save();
            this.Hide();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", MainWindow.editPath);
        }

        private void DatapacksDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedList = new List<string>();
            foreach (var item in ModsListBox.SelectedItems)
            {
                selectedList.Add(item.ToString());
            }
            if (selectedList.Count() == 0) { return; }
            MessageBoxResult messageResult = MessageBox.Show(string.Join(Environment.NewLine, selectedList), "Are you sure you want to delete", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                foreach (var item in ModsListBox.SelectedItems)
                {
                    File.Delete(MainWindow.editPath + "\\world\\datapacks\\" + item.ToString());
                }
                ModsListBox.Items.Clear();
                string[] datapacks = Directory.GetFiles(MainWindow.editPath + "\\world\\datapacks");
                foreach (string datapack in datapacks)
                {
                    DatapacksListBox.Items.Add(datapack.Split('\\')[datapack.Split('\\').Length - 1]);
                }
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
        }

        private void PluginsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedList = new List<string>();
            foreach (var item in ModsListBox.SelectedItems)
            {
                selectedList.Add(item.ToString());
            }
            if (selectedList.Count() == 0) { return; }
            MessageBoxResult messageResult = MessageBox.Show(string.Join(Environment.NewLine, selectedList), "Are you sure you want to delete", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                foreach (var item in ModsListBox.SelectedItems)
                {
                    File.Delete(MainWindow.editPath + "\\plugins\\" + item.ToString());
                }
                ModsListBox.Items.Clear();
                string[] plugins = Directory.GetFiles(MainWindow.editPath + "\\plugins");
                foreach (string plugin in plugins)
                {
                    PluginsListBox.Items.Add(plugin.Split('\\')[plugin.Split('\\').Length - 1]);
                }
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
