using System;
using System.Windows;
using System.ComponentModel;
using System.IO;
using Tekla.Structures.Dialog;
using WPFPlugin.Extensions;

namespace WPFPlugin
{
    /// <summary>
    /// Interaction logic for MainPluginWindow.xaml
    /// </summary>
    public partial class MainWindow : PluginWindowBase
    {
        // define event
        public MainWindowViewModel dataModel;

        public MainWindow(MainWindowViewModel DataModel)
        {
            InitializeComponent();
            dataModel = DataModel;
        }

        private void WPFOkApplyModifyGetOnOffCancel_ApplyClicked(object sender, EventArgs e)
        {
            this.Apply();
        }

        private void WPFOkApplyModifyGetOnOffCancel_CancelClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_GetClicked(object sender, EventArgs e)
        {
            this.Get();
        }

        private void WPFOkApplyModifyGetOnOffCancel_ModifyClicked(object sender, EventArgs e)
        {
            this.Modify();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OkClicked(object sender, EventArgs e)
        {
            this.Apply();
            this.Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OnOffClicked(object sender, EventArgs e)
        {
            this.ToggleSelection();
        }

        private void WPFMaterialCatalog_SelectClicked(object sender, EventArgs e)
        {
            this.materialCatalog.SelectedMaterial = this.dataModel.Material;
        }

        private void WPFMaterialCatalog_SelectionDone(object sender, EventArgs e)
        {
            this.dataModel.Material = this.materialCatalog.SelectedMaterial;
        }

        private void profileCatalog_SelectClicked(object sender, EventArgs e)
        {
            this.profileCatalog.SelectedProfile = this.dataModel.Profilename;
        }

        private void profileCatalog_SelectionDone(object sender, EventArgs e)
        {
            this.dataModel.Profilename = this.profileCatalog.SelectedProfile;
        }
        private void componentCatalog_SelectClicked(object sender, EventArgs e)
        {
            this.componentCatalog.SelectedName = this.dataModel.ComponentName;
            this.componentCatalog.SelectedNumber = this.dataModel.ComponentNumber;
        }

        private void componentCatalog_SelectionDone(object sender, EventArgs e)
        {
            this.dataModel.ComponentName = this.componentCatalog.SelectedName;
            this.dataModel.ComponentNumber = this.componentCatalog.SelectedNumber;
        }

        
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var path = "C:\\Temp\\profiles.xml";

            var profiles = new DataSet.Profiles();
            profiles.ReadXml(path);

            /*
            var tempPath = "C:\\Temp\\Shape.txt";
            var file = File.ReadAllLines(tempPath);
            
            var arr = file[0].Replace("/><", "@").Split('@');
            foreach (var curStr in arr)
            {
                var curArr = curStr.Replace("<", "").Replace("row ", "").Replace("=", "").Split('"');
                var newStr = profiles.Shape.NewShapeRow();
                var i = 1;
                newStr.ID = curArr[i].Trim().ToInt();
                i += 2;
                newStr.NAME = curArr[i].Trim();
                i += 2;
                newStr.DIM1 = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.DIM2 = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.DIM3 = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SHAPE_TYPE = curArr[i].Trim().ToInt();
                i += 2;
                newStr.MASS = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SURF = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.H = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.B = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.EA = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.ES = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.RA = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.RS = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.GAP = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SX = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SY = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SZ = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.IX = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.IY = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.IZ = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.IOMEGA = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.VY = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.VPY = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.VZ = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.VPZ = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.MSY = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.MSZ = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.TORS = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SHEAR_QY = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SHEAR_QZ = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.GAMMA = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.SYMBOL = curArr[i].Trim();
                i += 2;
                i += 2;
                i += 2;
                i += 2;
                newStr.POINTS = curArr[i].Trim();
                i += 2;
                newStr.IS_THIN = curArr[i].Trim()=="true";
                i += 2;
                newStr.B_2 = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.ES_2 = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P1_L = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P1_T = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P2_L = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P2_T = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P3_L = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P3_T = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P4_L = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.P4_T = curArr[i].Trim().ToDouble();
                i += 2;
                newStr.NAME1 = curArr[i].Trim();
                
                
                

                profiles.Shape.AddShapeRow(newStr);

            }
            profiles.WriteXml(path);
            */


            ProfilesDataGrid.ItemsSource = profiles.Identification_number_RU.DefaultView;



        }
    }
}
