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
                txtYear1.Text = date.ToString("yyyy");
                txtYear2.Text = date.ToString("yyyy");
                txtYear3.Text = date.ToString("yyyy");
                txtYear.Text = date.ToString("yyyy");
                txtMonth1.Text = date.ToString("MM");
                txtMonth2.Text = date.ToString("MM");
                txtMonth3.Text = date.ToString("MM");
                txtMonth.Text = date.ToString("MM");
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
                btnStop1.Enabled = false;
            }
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            if(txtYear1.Text!=date.ToString("yyyy")||
                txtMonth1.Text!=date.ToString("MM")||
                txtDay1.Text!=date.ToString("dd")||
                txtHour1.Text!=date.ToString("HH")||
                txtMin1.Text!=date.ToString("mm")||
                txtTemp1.Text == "")
            {
                return;
            }
            else
            {                
                List<string> targetRegs = new List<string>();
                List<object> writeVals = new List<object>();
                int[] errs;
                targetRegs.Add("DEV1.B10B2");
                writeVals.Add("1");

                if (opc.Write(targetRegs.ToArray(), writeVals.ToArray(), out errs))
                {
                    Debug.WriteLine("Set Writing Succeed in WriteTimeValues()");
                    timer1.Enabled = true;
                    btnStart1.Enabled = false;
                    btnStop1.Enabled = true;
                }
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
            List<string> targetRegs = new List<string>();
            List<object> writeVals = new List<object>();
            int[] errs;
            targetRegs.Add("DEV1.B10B2");
            writeVals.Add("0");

            if (opc.Write(targetRegs.ToArray(), writeVals.ToArray(), out errs))
            {
                timer1.Enabled = false;
                btnStart1.Enabled = true;
                btnStop1.Enabled = false;
            }
        }
    }
}
