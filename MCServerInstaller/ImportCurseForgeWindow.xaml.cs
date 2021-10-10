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
    /// Interaction logic for ImportCurseForgeWindow.xaml
    /// </summary>
    public partial class ImportCurseForgeWindow : Window
    {
        public ImportCurseForgeWindow()
        {
            InitializeComponent();
            Loaded += onLoad;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private void reload()
        {
            PackListBox.Items.Clear();
            if (!Directory.Exists(DirBox.Text + "\\minecraft\\Instances"))
            {
                return;
            }
            foreach (string pack in Directory.GetDirectories(DirBox.Text + "\\minecraft\\Instances"))
            {
                PackListBox.Items.Add(pack.Split('\\')[pack.Split('\\').Length - 1]);
            }
        }

        private async void onLoad(object sender, RoutedEventArgs e)
        {
            string CFDir = new DirectoryInfo(Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)).ToString() + "\\curseforge";
            if (!Directory.Exists(CFDir))
            {
                MessageBox.Show("Could not find Curseforge Minecraft directory!");
            }
            else
            {
                DirBox.Text = CFDir;
            }
            while (true)
            {
                await Task.Delay(1000);
                int selectedPack = PackListBox.SelectedIndex;
                reload();
                PackListBox.SelectedIndex = selectedPack;
            }
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PackListBox.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select something!");
                return;
            }
            string path = DirBox.Text + "\\minecraft\\Instances\\" + PackListBox.SelectedItem;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Error!");
                return;
            }
            if (Directory.Exists(path + "\\resources"))
            {
                if (!File.Exists(MainWindow.editPath + "\\resources"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\resources");
                }
                foreach (string file in Directory.GetDirectories(path + "\\resources"))
                {
                    if (!Directory.Exists(MainWindow.editPath + "\\resources\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        DirectoryCopy(file, MainWindow.editPath + "\\resources\\" + file.Split('\\')[file.Split('\\').Length - 1], true);
                    }
                }
            }
            if (Directory.Exists(path + "\\scripts"))
            {
                if (!Directory.Exists(MainWindow.editPath + "\\scripts"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\scripts");
                }
                foreach (string file in Directory.GetFiles(path + "\\scripts"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\scripts\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\scripts\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                    }
                }
            }
            if (Directory.Exists(path + "\\mods"))
            {
                if (!File.Exists(MainWindow.editPath + "\\mods"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\resources");
                }
                foreach (string file in Directory.GetFiles(path + "\\mods"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\mods\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\mods\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                    }
                }
            }
            MessageBox.Show("Finished!");
        }
    }
}
