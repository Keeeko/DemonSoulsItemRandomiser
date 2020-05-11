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
using System.IO;
using SoulsFormats;
using System.ComponentModel;
using System.Diagnostics;
using DemonSoulsItemRandomiser.Models;

namespace DemonSoulsItemRandomiser
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        readonly string pathToParamDataFile = Directory.GetCurrentDirectory() + @"\Data\gameparamna.parambnd.dcx";
        readonly string pathToParamDef = Directory.GetCurrentDirectory() + @"\Data\paramdef\paramdef.paramdefbnd.dcx";

        public event PropertyChangedEventHandler PropertyChanged;

        public static Dictionary<string, PARAMDEF> paramDefs;
        public static BND3 paramDefBnd;

        public static Dictionary<string, PARAM> parms;
        public static BND3 paramBnd;

        public static GameWorld GameWorld { get; set; }
        public static Randomiser Randomiser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            IDBanks.InitIDLists();
        }

        private void UnpackGameBNDFile()
        {
            // Reading an original paramdefbnd
            paramDefs = new Dictionary<string, PARAMDEF>();
            paramDefBnd = BND3.Read(pathToParamDef);

            foreach (BinderFile file in paramDefBnd.Files)
            {
                var paramdef = PARAMDEF.Read(file.Bytes);
                paramDefs[paramdef.ParamType] = paramdef;
            }

            parms = new Dictionary<string, PARAM>();
            paramBnd = BND3.Read(pathToParamDataFile);

            foreach (BinderFile file in paramBnd.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                var param = PARAM.Read(file.Bytes);
                param.ApplyParamdef(paramDefs[param.ParamType]);
                parms[name] = param;
            }
        }

        private void RepackGameBNDFile()
        {

            foreach (BinderFile file in paramBnd.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                file.Bytes = parms[name].Write();
            }

            paramBnd.Write(pathToParamDataFile);
        }

        private void RandomiseItems()
        {
            UnpackGameBNDFile();
            GameWorld = new GameWorld();
            Randomiser = new Randomiser();
            Randomiser.RandomiseItems();
            RepackGameBNDFile();
        }

        private void bttnRandomise_Click(object sender, RoutedEventArgs e)
        {
                RandomiseItems();
                MessageBox.Show("Randomisation Successful", "Items Randomised", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void bttnInstructions_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.CurrentDirectory + @"\Readme.txt");
        }
    }
}
