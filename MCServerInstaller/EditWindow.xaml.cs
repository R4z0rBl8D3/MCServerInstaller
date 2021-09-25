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
        }

        private void onLoad(object sender, RoutedEventArgs e)
        {
            load();
            
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
