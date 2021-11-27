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
        public static int filesCopied = 0;

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
                filesCopied++;
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
            filesCopied = 0;
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
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\mods"))
            {
                if (!File.Exists(MainWindow.editPath + "\\mods"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\mods");
                }
                foreach (string file in Directory.GetFiles(path + "\\mods"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\mods\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\mods\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\resourcepacks"))
            {
                if (!File.Exists(MainWindow.editPath + "\\resourcepacks"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\resourcepacks");
                }
                foreach (string file in Directory.GetFiles(path + "\\resourcepacks"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\resourcepacks\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\resourcepacks\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\structures\\active"))
            {
                if (!File.Exists(MainWindow.editPath + "\\structures\\active"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\structures\\active");
                }
                foreach (string file in Directory.GetFiles(path + "\\structures\\active"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\structures\\active\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\structures\\active\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\structures\\downloads"))
            {
                if (!File.Exists(MainWindow.editPath + "\\structures\\downloads"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\structures\\downloads");
                }
                foreach (string file in Directory.GetFiles(path + "\\structures\\downloads"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\structures\\downloads\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\structures\\downloads\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\structures\\inactive"))
            {
                if (!File.Exists(MainWindow.editPath + "\\structures\\inactive"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\structures\\inactive");
                }
                foreach (string file in Directory.GetFiles(path + "\\structures\\inactive"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\structures\\inactive\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\structures\\inactive\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\structures\\schematics"))
            {
                if (!File.Exists(MainWindow.editPath + "\\structures\\schematics"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\structures\\schematics");
                }
                foreach (string file in Directory.GetFiles(path + "\\structures\\schematics"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\structures\\schematics\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\structures\\schematics\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
            }
            if (Directory.Exists(path + "\\config"))
            {
                if (!File.Exists(MainWindow.editPath + "\\config"))
                {
                    Directory.CreateDirectory(MainWindow.editPath + "\\config");
                }
                foreach (string file in Directory.GetFiles(path + "\\config"))
                {
                    if (!File.Exists(MainWindow.editPath + "\\config\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        File.Copy(file, MainWindow.editPath + "\\config\\" + file.Split('\\')[file.Split('\\').Length - 1]);
                        filesCopied++;
                    }
                }
                foreach (string file in Directory.GetDirectories(path + "\\config"))
                {
                    if (!Directory.Exists(MainWindow.editPath + "\\config\\" + file.Split('\\')[file.Split('\\').Length - 1]))
                    {
                        DirectoryCopy(file, MainWindow.editPath + "\\config\\" + file.Split('\\')[file.Split('\\').Length - 1], true);
                    }
                }
            }
            MessageBox.Show("Finished!" + Environment.NewLine + filesCopied.ToString() + " files copied!");
        }

        private void CopyEBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageResult = MessageBox.Show("You should use this if you have already tried importing and it not working, it might say Fatally missing registry entries. If that shows up or another error then use this feature.\nIt might create a mess in the server folder, are you sure you want to continue?", "Copy everything", MessageBoxButton.YesNo);
            if (messageResult == MessageBoxResult.Yes)
            {
                //gud
            }
            else if (messageResult == MessageBoxResult.No)
            {
                return;
            }
            filesCopied = 0;
            foreach (string folder in Directory.GetDirectories(DirBox.Text + "\\minecraft\\Instances\\" + PackListBox.SelectedItem))
            {
                DirectoryCopy(folder, MainWindow.editPath, true);
            }
            MessageBox.Show("Copied " + filesCopied + " files", "Copy completed");
        }
    }
}
