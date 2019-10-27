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
using static cvsRead.CvsContainer;

namespace cvsRead
{
    public partial class Form1 : Form, IMessageFilter
    {
        public Form1()
        {
            InitializeComponent();

            #region EnableZoomings
            var ca = chart1.ChartAreas[0];
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.MinorGrid.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            ca.AxisY.MinorGrid.Enabled = false;

            ca.AxisX.ScaleView.Zoomable = true;
            ca.CursorX.AutoScroll = true;
            ca.CursorX.IsUserSelectionEnabled = true;

            ca.AxisY.ScaleView.Zoomable = true;
            ca.CursorY.AutoScroll = true;
            ca.CursorY.IsUserSelectionEnabled = true;

            #endregion

            Application.AddMessageFilter(this);
        }

        private void Unzoom()
        {
            var xAxis = chart1.ChartAreas[0].AxisX;
            var yAxis = chart1.ChartAreas[0].AxisY;
            xAxis.ScaleView.ZoomReset();
            yAxis.ScaleView.ZoomReset();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
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
                DateTime start = DateTime.Now;
                var cc = new CvsContainer(fn);
                DateTime end = DateTime.Now;
                System.Diagnostics.Debug.WriteLine("-------------");
                System.Diagnostics.Debug.WriteLine((end - start).TotalMilliseconds);

                System.Diagnostics.Debug.WriteLine(fn);
                
                var sname = System.IO.Path.GetFileName(fn);
                AddToCombo(sname);

                AddSeries(sname, cc.mDTVals);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        internal void AddSeries(string name, List<DTVal> lst)
        {
            Series series1 = new Series();
            var newName = name;
            series1.Name = newName;
            series1.ChartType = SeriesChartType.Line;
            var oldS = chart1.Series.FirstOrDefault(s => s.Name == newName);
            if ( oldS!= null) { chart1.Series.Remove(oldS); }

            chart1.Series.Add(series1);

            //var ser = chart1.Series[sname];
            series1.XValueType = ChartValueType.Date;
            series1.YValueType = ChartValueType.Double;
            foreach (var item in lst)
            {
                chart1.Series[newName].Points.AddXY(item.OADate, item.Val);
            }
        }

        private void AddToCombo(string sname)
        {
            if (!comboBox1.Items.Contains(sname))
            {
                comboBox1.Items.Add(sname);
            }
        }

        public const int WM_KEYDOWN = 0x0100;
        public const int VK_ESCAPE = 0x1B;
        public event EventHandler EscapeKeyDown;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN && m.WParam == new IntPtr(VK_ESCAPE))
            {
                System.Diagnostics.Debug.WriteLine("esc");
                Unzoom();
            }
            //if (m.Msg == WM_KEYDOWN && Control.ModifierKeys == Keys.ControlKey)
            if (Control.ModifierKeys == Keys.Control)
            {
                if ((int)m.WParam == 83){
                    System.Diagnostics.Debug.WriteLine("##");
                    comboBox1.Focus();
                }
            }

            return false; //Do not Process anything
        }

        private void SelectedChanged(object sender, EventArgs e)
        {
            //string seriesName = (string) comboBox1.SelectedItem;
            //if (string.IsNullOrEmpty(seriesName)) { return; }
            //
            //label2.Text = trackBar1.Value.ToString();
            //var cc = Managers.DataMngr.Instance.GetCvs(seriesName);
            //var sa = new SlidingAverage(cc, trackBar1.Value);
            //AddSeries(seriesName.Substring(0, 3) + ".SA", sa.MDTVals);
        }

        private void SettingsClicked(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var si = (string)comboBox1.SelectedItem;
            var s = chart1.Series.FirstOrDefault(ser => ser.Name == si);
            chart1.Series.Remove(s);
            // rem sld if exist, rem from combo
            var sld = si.Substring(0, 3) + ".SA";
            comboBox1.Items.Remove(si);
            comboBox1.SelectedItem = "";
            var sldSer = chart1.Series.FirstOrDefault(ser => ser.Name == sld);
            if (sldSer != null) {
                chart1.Series.Remove(sldSer);
            }
        }

        private void SldAvgClick(object sender, EventArgs e)
        {
            string seriesName = (string)comboBox1.SelectedItem;
            if (string.IsNullOrEmpty(seriesName)) { return; }

            label2.Text = trackBar1.Value.ToString();
            var cc = Managers.DataMngr.Instance.GetCvs(seriesName);
            var sa = new SlidingAverage(cc, trackBar1.Value);
            AddSeries(seriesName.Substring(0, 3) + ".SA", sa.MDTVals);
        }

        private void DragBarValueChanged(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
        }

        private void SlideLblTextChanged(object sender, EventArgs e)
        {
            var si = (string)comboBox1.SelectedItem;
            if (!string.IsNullOrWhiteSpace(si))
            {
                var sld  = si.Substring(0, 3) + ".SA";
                var ser  = chart1.Series.FirstOrDefault(s => s.Name == sld);
                chart1.Series.Remove(ser);
                var mngr = Managers.DataMngr.Instance;
                var cvs  = mngr.GetCvs(si);
                var sa   = mngr.GetSliding(si, trackBar1.Value);
                AddSeries(sld, sa.MDTVals);
            }
        }
    }
}
