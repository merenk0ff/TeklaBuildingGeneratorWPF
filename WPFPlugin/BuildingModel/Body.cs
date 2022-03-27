using System.Collections.Generic;

namespace WPFPlugin.BuildingModel
{
    public class Body
    {
        public List<double> xSteps { get; set; }
        public List<double> ySteps { get; set; }
        public double columnH_0 { get; set; }
        public double columnH_1 { get; set; }
        public string columnProfileString { get; set; }
        public string beamProfileString { get; set; }
        public string materialString { get; set; }


        public Body()
        {
            xSteps = new List<double>()
            {
                0,
                6000,
                6000,
                6000,
                6000,
                6000,
                6000
            };
            ySteps = new List<double>()
            {
                0,
                7000
            };
            columnH_0 = 9000;
            columnH_1 = 10000;
            columnProfileString = "I20K1_20_93";
            beamProfileString = "I20B1_20_93";
            materialString = "C255";
            
        }
    }
}
