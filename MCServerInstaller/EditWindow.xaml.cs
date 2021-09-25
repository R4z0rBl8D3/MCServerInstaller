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

namespace MCServerInstaller
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public static List<string> properties = new List<string>();
        
        public EditWindow()
        {
            InitializeComponent();
            Loaded += onLoad;
        }

        private void load()
        {
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
            }
            using (StreamReader sr = new StreamReader(MainWindow.editPath + "\\StartServer.bat"))
            {
                sr.ReadLine();
                string line = sr.ReadLine();
                MemoryBox.Text = line.Split(' ')[1].Split('x')[1];
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
            if (Directory.Exists(MainWindow.editPath + "\\scripts"))
            {
                StatusTxt.Content = "Loading scripts...";
                ScriptsListBox.Items.Clear();
                string[] scripts = Directory.GetFiles(MainWindow.editPath + "\\scripts");
                foreach (string script in scripts)
                {
                    ScriptsListBox.Items.Add(script.Split('\\')[script.Split('\\').Length - 1]);
                }
            }
            if (Directory.Exists(MainWindow.editPath + "\\structures"))
            {
                StatusTxt.Content = "Loading structures...";
                StructuresListBox.Items.Clear();
                string[] structures = Directory.GetFiles(MainWindow.editPath + "\\structures");
                foreach (string structure in structures)
                {
                    StructuresListBox.Items.Add(structure.Split('\\')[structure.Split('\\').Length - 1]);
                }
            }
        }

        private async void onLoad(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                load();
                await Task.Delay(1000);
            }
        }

        private void save()
        {
            
        }

        private void ModsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScriptsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StructuresDeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PropertiesBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
