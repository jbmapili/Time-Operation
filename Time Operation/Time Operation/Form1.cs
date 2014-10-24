using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpcRcw.Da;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Time_Operation
{
    public partial class Form1 : Form
    {
        string DeviceName = Properties.Settings.Default.DeviceName;
        string BitPrefix  = Properties.Settings.Default.BitPrefix;
        string WordPrefix = Properties.Settings.Default.WordPrefix;
        string No1SWAddr = Properties.Settings.Default.No1SWAddr;
        string RemoteFlag = Properties.Settings.Default.RemoteFlag;
        bool No1flag = false;


        DxpSimpleAPI.DxpSimpleClass opc = new DxpSimpleAPI.DxpSimpleClass();
        DateTime date = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
        }

        private void txtYear2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
        //    foreach (GroupBox gb in this.Controls.OfType<GroupBox>())
        //    {
        //        foreach (TextBox tb in gb.Controls.OfType<TextBox>())
        //        {
        //            if (tb.Text.ToString() == "Year") { tb.Text = date.ToString("yyyy"); }
        //        }
        //    }
            if (opc.Connect("localhost", "Takebishi.dxp"))
            {
                txtYear1.Text = txtYear2.Text = txtYear3.Text 
                    = txtYear.Text = date.ToString("yyyy");
                txtMonth1.Text = txtMonth3.Text = txtMonth2.Text 
                    = txtMonth.Text = date.ToString("MM");
                txtDay1.Text = date.ToString("dd");
                txtDay2.Text = date.ToString("dd");
                txtDay3.Text = date.ToString("dd");
                txtDay.Text = date.ToString("dd");
                txtHour1.Text = date.ToString("HH");
                txtHour2.Text = date.ToString("HH");
                txtHour3.Text = date.ToString("HH");
                txtHour.Text = date.ToString("HH");
                txtMin1.Text = date.ToString("mm");
                txtMin2.Text = date.ToString("mm");
                txtMin3.Text = date.ToString("mm");
                txtMin.Text = date.ToString("mm");
//                btnStop1.Enabled = false;
            }
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            string[] targets = new string[] { DeviceName + BitPrefix + RemoteFlag, };
            object[] values;
            short[] qualities;
            FILETIME[] fileTimes;
            int[] errors;

            if (opc.Read(targets, out values, out qualities, out fileTimes, out errors))
            {
                if (Convert.ToInt32(values[0]) == 0 )
                {
                    return;
                }

                btnStart1.ForeColor = Color.Orange;
                btnStop1.ForeColor = Color.Gray;

                No1flag = false;
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblDate.Text = dt.ToString("MM/dd/yyyy");
            lblTime.Text = dt.ToString("HH:mm:ss");
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            string[] targets = new string[] { DeviceName + BitPrefix + RemoteFlag, };
            object[] values;
            short[] qualities;
            FILETIME[] fileTimes;
            int[] errors;

            if (opc.Read(targets, out values, out qualities, out fileTimes, out errors))
            {
                if (Convert.ToInt32(values[0]) == 0)
                {
                    return;
                }

                btnStart1.ForeColor = Color.Gray;
                btnStop1.ForeColor = Color.Orange; 
            }
            //List<string> targetRegs = new List<string>();
            //List<object> writeVals = new List<object>();
            //int[] errs;
            //writeVals.Add("0");

            //if (opc.Write(targetRegs.ToArray(), writeVals.ToArray(), out errs))
            //{
            //    timer1.Enabled = false;
            //    btnStart1.Enabled = true;
            //    btnStop1.Enabled = false;
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (btnStart1.ForeColor == Color.Gray)
            {
                return;
            }

            DateTime date = DateTime.Now;
            if (txtYear1.Text == date.ToString("yyyy") &&
                txtMonth1.Text == date.ToString("MM") &&
                txtDay1.Text == date.ToString("dd") &&
                txtHour1.Text == date.ToString("HH") &&
                txtMin1.Text == date.ToString("mm") &&
                No1flag == false)
            {
                No1flag = true;

                string[] targetRegs = new string[] { DeviceName + BitPrefix + No1SWAddr, };
                object[] writeVals = new object[] { "1" };
                int[] errs;

                if (opc.Write(targetRegs, writeVals, out errs))
                {
                    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                    //timer1.Enabled = true;
                    //btnStart1.Enabled = false;
                    //btnStop1.Enabled = true;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            opc.Disconnect();
        }
    }
}
