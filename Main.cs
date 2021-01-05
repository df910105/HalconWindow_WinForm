using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalconWindow_WinForm
{
    public partial class Main : Form
    {
        private HTuple hv_WindowHandle;
        private int ImageWidth = 1000;
        private int ImageHeight = 800;
        private HTuple hv_chipWidth = new HTuple();
        private HTuple hv_chipLength = new HTuple();
        private HTuple hv_MouseRow = new HTuple();
        private HTuple hv_MouseCol = new HTuple();
        private HTuple hv_MouseVal = new HTuple();
        private HObject ho_Image = new HObject();

        public Main()
        {
            InitializeComponent();
            hv_WindowHandle = hWindowControl.HalconWindow;
        }

        private void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (!ho_Image.IsInitialized())
                {
                    return;
                }
                hv_MouseRow.Dispose();
                hv_MouseCol.Dispose();
                hv_MouseVal.Dispose();
                HOperatorSet.GetMposition(hv_WindowHandle, out hv_MouseRow, out hv_MouseCol, out _);
                if (hv_MouseRow > 0 && hv_MouseRow < ImageHeight && hv_MouseCol > 0 && hv_MouseCol < ImageWidth)
                {
                    HOperatorSet.GetGrayval(ho_Image, hv_MouseRow, hv_MouseCol, out HTuple pointGray);
                    StatusLabel.Text = $"X:{hv_MouseCol}  Y:{hv_MouseRow}  Value:{hv_MouseVal}";
                }
                else
                {
                    StatusLabel.Text = $"X:{hv_MouseCol}  Y:{hv_MouseRow}  Value:-";
                }
                hv_MouseRow.Dispose();
                hv_MouseCol.Dispose();
                hv_MouseVal.Dispose();
            }
            catch
            {

            }
        }

        private void hWindowControl_HInitWindow(object sender, EventArgs e)
        {
            HOperatorSet.SetWindowParam(hv_WindowHandle, "graphics_stack", "true");
            ResetWindow(ref hWindowControl, ImageHeight, ImageWidth);
        }

        private static void ResetWindow(ref HWindowControl h_HWindowControl, int ImageHeight, int ImageWidth)
        {
            try
            {
                int HW_h = h_HWindowControl.Height;
                int HW_w = h_HWindowControl.Width;
                if (HW_h == 0 || HW_w == 0)
                {
                    return;
                }
                HOperatorSet.TupleMax2(1.0 * ImageHeight / HW_h, 1.0 * ImageWidth / HW_w, out HTuple hv_rate);

                HOperatorSet.SetPart(h_HWindowControl.HalconWindow, 0, 0, HW_h * hv_rate - 1, HW_w * hv_rate - 1);
                hv_rate.Dispose();
            }
            catch
            {

            }
        }

        private void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            HTuple 
                Zoom = new HTuple(),
                Ht = new HTuple(), Wt = new HTuple(),
                r1 = new HTuple(), c1 = new HTuple(),
                r2 = new HTuple(), c2 = new HTuple(),
                Row = new HTuple(), Col = new HTuple(),
                Row0 = new HTuple(), Column0 = new HTuple(),
                Row00 = new HTuple(), Column00 = new HTuple();
            try
            {
                Zoom.Dispose();
                if (e.Delta > 0)
                {
                    Zoom = 2;
                }
                else
                {
                    Zoom = 0.5;
                }

                Row.Dispose(); Col.Dispose();
                HOperatorSet.GetMposition(hv_WindowHandle, out Row, out Col, out _);
                Row0.Dispose(); Column0.Dispose(); Row00.Dispose(); Column00.Dispose();
                HOperatorSet.GetPart(hv_WindowHandle, out Row0, out Column0, out Row00, out Column00);
                Ht = Row00 - Row0;
                Wt = Column00 - Column0;

                if (Ht * Wt < 32768 * 32768 || Zoom == 2.0)
                {
                    r1 = (Row0 + ((1 - (1.0 / Zoom)) * (Row - Row0)));
                    c1 = (Column0 + ((1 - (1.0 / Zoom)) * (Col - Column0)));
                    r2 = r1 + (Ht / Zoom);
                    c2 = c1 + (Wt / Zoom);
                    HOperatorSet.SetPart(hv_WindowHandle, r1, c1, r2, c2);
                }
            }
            catch 
            {

            }
            Zoom.Dispose();
            Ht.Dispose(); Wt.Dispose();
            r1.Dispose(); c1.Dispose();
            r2.Dispose(); c2.Dispose();
            Row.Dispose(); Col.Dispose();
            Row0.Dispose(); Column0.Dispose();
            Row00.Dispose(); Column00.Dispose();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {

        }
    }
}
