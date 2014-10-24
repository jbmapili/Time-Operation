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
        bool No2flag = false;
        bool No3flag = false;
        bool No4flag = false;


        DxpSimpleAPI.DxpSimpleClass opc = new DxpSimpleAPI.DxpSimpleClass();
        DateTime date = DateTime.Now;
        DateTime dt;
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
            if (opc.Connect("localhost", "Takebishi.dxp"))
            {
                txtYear1.Text = txtYear2.Text = txtYear3.Text 
                    = txtYear.Text = date.ToString("yyyy");
                txtMonth1.Text = txtMonth3.Text = txtMonth2.Text 
                    = txtMonth.Text = date.ToString("MM");
                txtDay1.Text = txtDay2.Text = txtDay3.Text
                    = txtDay.Text = date.ToString("dd");
                txtHour1.Text = txtHour2.Text = txtHour3.Text
                    = txtHour.Text = date.ToString("HH");
                txtMin1.Text = txtMin2.Text = txtMin3.Text
                    = txtMin.Text = date.ToString("mm");
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !(char.IsControl(e.KeyChar));
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            DateTime d1=new DateTime(Convert.ToInt32(txtYear1.Text), Convert.ToInt32(txtMonth1.Text), 
                                     Convert.ToInt32(txtDay1.Text), Convert.ToInt32(txtHour1.Text), 
                                     Convert.ToInt32(txtMin1.Text), 59);
            if (d1 > dt)
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

                    btnStart1.ForeColor = Color.Orange;
                    btnStop1.ForeColor = Color.Gray;

                    No1flag = false;
                }
            }
            else
            {
                MessageBox.Show("Date must be future");
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            dt = DateTime.Now;
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

                string[] targetRegs = new string[] { DeviceName + BitPrefix + No1SWAddr + "0", };
                object[] writeVals = new object[] { "1" };
                int[] errs;

                if (opc.Write(targetRegs, writeVals, out errs))
                {
                    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            opc.Disconnect();
        }

        private void btnStart3_Click(object sender, EventArgs e)
        {
            DateTime d1=new DateTime(Convert.ToInt32(txtYear3.Text), Convert.ToInt32(txtMonth3.Text), 
                                     Convert.ToInt32(txtDay3.Text), Convert.ToInt32(txtHour3.Text), 
                                     Convert.ToInt32(txtMin3.Text), 59);
            if (d1 > dt)
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

                btnStart3.ForeColor = Color.Orange;
                btnStop3.ForeColor = Color.Gray;

                No3flag = false;

                //string[] targetRegs = new string[] { DeviceName + BitPrefix + "100B", };
                //object[] writeVals = new object[] { "1" };
                //int[] errs;

                //if (opc.Write(targetRegs, writeVals, out errs))
                //{
                //    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                //}
            }
            }
            else
            {
                MessageBox.Show("Date must be future");
            }
        }

        private void btnStop3_Click(object sender, EventArgs e)
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

                btnStart3.ForeColor = Color.Gray;
                btnStop3.ForeColor = Color.Orange;

                //string[] targetRegs = new string[] { DeviceName + BitPrefix + "100B", };
                //object[] writeVals = new object[] { "0" };
                //int[] errs;

                //if (opc.Write(targetRegs, writeVals, out errs))
                //{
                //    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                //}
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (btnStart3.ForeColor == Color.Gray)
            {
                return;
            }

            DateTime date = DateTime.Now;
            if (txtYear3.Text == date.ToString("yyyy") &&
                txtMonth3.Text == date.ToString("MM") &&
                txtDay3.Text == date.ToString("dd") &&
                txtHour3.Text == date.ToString("HH") &&
                txtMin3.Text == date.ToString("mm") &&
                No3flag == false)
            {
                No3flag = true;

                string[] targetRegs = new string[] { DeviceName + BitPrefix + No1SWAddr + "1", };
                object[] writeVals = new object[] { "1" };
                int[] errs;

                if (opc.Write(targetRegs, writeVals, out errs))
                {
                    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                }
            }
        }

        private void btnStart2_Click(object sender, EventArgs e)
        {
            
            DateTime d1=new DateTime(Convert.ToInt32(txtYear2.Text), Convert.ToInt32(txtMonth2.Text), 
                                     Convert.ToInt32(txtDay2.Text), Convert.ToInt32(txtHour2.Text), 
                                     Convert.ToInt32(txtMin2.Text), 59);
            if (d1 > dt)
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

                btnStart2.ForeColor = Color.Orange;
                btnStop2.ForeColor = Color.Gray;

                No1flag = false;
            }
            }
            else
            {
                MessageBox.Show("Date must be future");
            }
        }

        private void btnStop2_Click(object sender, EventArgs e)
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

                btnStart2.ForeColor = Color.Gray;
                btnStop2.ForeColor = Color.Orange;

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (btnStart2.ForeColor == Color.Gray)
            {
                return;
            }

            DateTime date = DateTime.Now;
            if (txtYear2.Text == date.ToString("yyyy") &&
                txtMonth2.Text == date.ToString("MM") &&
                txtDay2.Text == date.ToString("dd") &&
                txtHour2.Text == date.ToString("HH") &&
                txtMin2.Text == date.ToString("mm") &&
                No2flag == false)
            {
                No2flag = true;

                string[] targetRegs = new string[] { DeviceName + BitPrefix + No1SWAddr + "2", };
                object[] writeVals = new object[] { "1" };
                int[] errs;

                if (opc.Write(targetRegs, writeVals, out errs))
                {
                    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            DateTime d1=new DateTime(Convert.ToInt32(txtYear.Text), Convert.ToInt32(txtMonth.Text), 
                                     Convert.ToInt32(txtDay.Text), Convert.ToInt32(txtHour.Text), 
                                     Convert.ToInt32(txtMin.Text), 59);
            if (d1 > dt)
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

                btnStart.ForeColor = Color.Orange;
                btnStop.ForeColor = Color.Gray;

                No4flag = false;
            }
            }
            else
            {
                MessageBox.Show("Date must be future");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
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

                btnStart.ForeColor = Color.Gray;
                btnStop.ForeColor = Color.Orange; 
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            
            if (btnStart.ForeColor == Color.Gray)
            {
                return;
            }

            DateTime date = DateTime.Now;
            if (txtYear.Text == date.ToString("yyyy") &&
                txtMonth.Text == date.ToString("MM") &&
                txtDay.Text == date.ToString("dd") &&
                txtHour.Text == date.ToString("HH") &&
                txtMin.Text == date.ToString("mm") &&
                No4flag == false)
            {
                No4flag = true;

                string[] targetRegs = new string[] { DeviceName + BitPrefix + No1SWAddr + "3", };
                object[] writeVals = new object[] { "1" };
                int[] errs;

                if (opc.Write(targetRegs, writeVals, out errs))
                {
                    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                }
            }
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {

        }
    }
}
