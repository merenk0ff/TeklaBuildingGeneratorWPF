using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;
using WPFPlugin.Extensions;

namespace WPFPlugin
{
    public class PluginData
    {
        #region Fields
        //
        // Define the fields specified on the Form.
        //
        [StructuresField("name")]
        public string partName;
        
        [StructuresField("profile")]
        public string profile;

        [StructuresField("offset")]
        public double offset;

        [StructuresField("material")]
        public string material;

        [StructuresField("componentname")]
        public string componentname;

        [StructuresField("componentnumber")]
        public int componentnumber;

        [StructuresField("lengthfactor")]
        public int lengthfactor;

        #endregion
    }

    [Plugin("WPFPlugin")]
    [PluginUserInterface("WPFPlugin.MainWindow")]
    public class WPFPlugin : PluginBase
    {
        #region Fields
        private Model _Model;
        private PluginData _Data;
        //
        // Define variables for the field values.
        //
        private string _PartName = string.Empty;
        private string _Profile = string.Empty;
        private string _Material = string.Empty;
        private double _Offset = 0.0;
        private int _LengthFactor = 0;
        #endregion

        #region Properties
        private Model Model
        {
            get { return this._Model; }
            set { this._Model = value; }
        }

        private PluginData Data
        {
            get { return this._Data; }
            set { this._Data = value; }
        }
        #endregion

        #region Constructor
        public WPFPlugin(PluginData data)
        {
            Model = new Model();
            Data = data;
        }
        #endregion

        #region Overrides
        public override List<InputDefinition> DefineInput()
        {
            //
            // This is an example for selecting two points; change this to suit your needs.
            //
            var pointList = new List<InputDefinition>();
            var picker = new Picker();
            var pickedPoints = picker.PickPoints(Picker.PickPointEnum.PICK_TWO_POINTS);

            pointList.Add(new InputDefinition(pickedPoints));

            return pointList;
        }

        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                GetValuesFromDialog();

                var xSize = 6000;
                var ySize = 9000;
                var zSize = 10000;



                var column1 = InsertBeam(
                    new Point(0, 0, 0), 
                    new Point(0, 0, ySize)
                    );
                var column2 = InsertBeam(
                    column1.StartPoint.Move(new Vector(0,1,0), ySize), 
                    column1.EndPoint.Move(new Vector(0, 1, 0), ySize)
                    );
                //var column3 = InsertBeam(
                //    column1.StartPoint.Move(new Vector(1, 0, 0), xSize),
                //    column1.EndPoint.Move(new Vector(1, 0, 0), xSize)
                //);
                //var column4 = InsertBeam(
                //    column2.StartPoint.Move(new Vector(1, 0, 0), xSize),
                //    column2.EndPoint.Move(new Vector(1, 0, 0), xSize)
                //);
                var beam1 = new Beam(column1.EndPoint, column2.EndPoint);
                beam1.Position.PlaneOffset = _Offset;
                beam1.Position.Rotation = Position.RotationEnum.BELOW;
                beam1.Position.Depth = Position.DepthEnum.MIDDLE;
                beam1.Position.Plane = Position.PlaneEnum.MIDDLE;
                beam1.Name = _PartName;
                beam1.Profile.ProfileString = "I20B1_20_93";
                beam1.Material.MaterialString = _Material;
                beam1.Insert();

                var beam1Height = beam1.GetHeight();
                beam1.Select();
                beam1.StartPoint = beam1.StartPoint.Move(new Vector(0, 0, -1), beam1Height / 2);
                beam1.EndPoint = beam1.EndPoint.Move(new Vector(0, 0, -1), beam1Height / 2);
                beam1.Modify();





                //Operation.DisplayPrompt("Selected component " + _Data.componentname + " : " + _Data.componentnumber.ToString());

            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.ToString());
            }

            return true;
        }

        private Beam InsertBeam(Point startPoint, Point endPoint)
        {
            var beam = new Beam(startPoint, endPoint);
            beam.Position.PlaneOffset = _Offset;
            beam.Position.Depth = Position.DepthEnum.MIDDLE;
            beam.Position.Plane = Position.PlaneEnum.MIDDLE;
            beam.Position.Rotation = Position.RotationEnum.BACK;
            beam.Name = _PartName;
            beam.Profile.ProfileString = _Profile;
            beam.Material.MaterialString = _Material;
            beam.Insert();
            return beam;
        }
        private Beam InsertBeam(Point startPoint, Point endPoint, string profileName, string materialName)
        {
            var beam = new Beam(startPoint, endPoint);
            beam.Position.PlaneOffset = _Offset;
            beam.Position.Depth = Position.DepthEnum.MIDDLE;
            beam.Position.Plane = Position.PlaneEnum.MIDDLE;
            beam.Position.Rotation = Position.RotationEnum.BACK;
            beam.Name = _PartName;
            beam.Profile.ProfileString = profileName;
            beam.Material.MaterialString = materialName;
            beam.Insert();
            return beam;
        }
        private Beam InsertBeam(Point startPoint, Point endPoint, string profileName)
        {
            var beam = new Beam(startPoint, endPoint);
            beam.Position.PlaneOffset = _Offset;
            beam.Position.Rotation = Position.RotationEnum.BACK;
            beam.Name = _PartName;
            beam.Profile.ProfileString = profileName;
            beam.Material.MaterialString = _Material;
            beam.Insert();
            return beam;
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Gets the values from the dialog and sets the default values if needed
        /// </summary>
        private void GetValuesFromDialog()
        {
            _PartName = Data.partName;
            _Profile = Data.profile;
            _Material = Data.material;
            _Offset = Data.offset;
            _LengthFactor = Data.lengthfactor + 1;

            if (IsDefaultValue(_PartName))
                _PartName = "Part";
            if (IsDefaultValue(_Profile))
                _Profile = "I20K1_20_93";
            if (IsDefaultValue(_Material))
                _Material = "C255";
            if (IsDefaultValue(_Offset))
                _Offset = 0;
            if (IsDefaultValue(_LengthFactor) || _LengthFactor == 0)
                _Offset = 1;
        }

        // Write your private methods here.

        #endregion
    }
}
