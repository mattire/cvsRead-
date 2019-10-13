using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace cvsRead
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [STAThread]
        static string RunOpenDialog(System.Windows.Forms.OpenFileDialog ofd) {
            ofd.ShowHelp = true;
            ofd.ShowDialog();
            return ofd.FileName;
            //System.Diagnostics.Debug.WriteLine(fn);
            //var lines = System.IO.File.ReadAllLines(fn);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowHelp = true;
            //var fn = RunOpenDialog(openFileDialog1);
            openFileDialog1.ShowDialog();

            try
            {
                var fn = openFileDialog1.FileName;
                var cc = new CvsContainer(fn);

                System.Diagnostics.Debug.WriteLine(fn);
                var sname = System.IO.Path.GetFileName(fn);

                //var sname = "Series1";
                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

                var ser = chart1.Series[sname];
                ser.XValueType = ChartValueType.Date;
                ser.YValueType = ChartValueType.Double;
                //chart1.Series.Add(sname);

                //foreach (var item in cc.mDTVals.Take(100))
                foreach (var item in cc.mDTVals)
                {
                    chart1.Series[sname].Points.AddXY(item.OADate, item.Val);
                }


                //chart1.Series[sname].Points.AddXY(0, 1);
                //chart1.Series[sname].Points.AddXY(1, 2);
                //chart1.Series[sname].Points.AddXY(3, 2);
                //chart1.Series[sname].Points.AddXY(4, 1);
            }
            catch (Exception)
            {

                //throw;
            }
        }

    }
}
