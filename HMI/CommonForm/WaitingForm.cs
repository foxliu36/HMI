using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HMI.CommonForm
{
    public partial class WaitingForm : Form
    {
        public static WaitingForm wformInstance = null;
        private static object m_LockObject = new object();


        public static WaitingForm Instance {
            get
            {
                lock (m_LockObject)
                {
                    if (wformInstance == null || wformInstance.IsDisposed)
                    {
                        wformInstance = new WaitingForm();
                    }
                    return wformInstance;
                }
            }
        }

        DateTime timeStart = DateTime.Now;

        DateTime timeReport;
        int firsttime = new Random().Next(20, 35);

        public WaitingForm()
        {
            InitializeComponent();
        }

        public static void Start()
        {
            if (!Instance.Created)
            {
                Instance.CreateControl();
            }
            Instance.Visible = true;
            Instance.Refresh();
            
        }

        public static void EndDisplay()
        {
            Instance.Visible = false;
        }

        public void WorkerReport(int p_ProcessPercent, string p_Speach) {

            timeReport = DateTime.Now;

            //作業時間太短
            if (p_ProcessPercent == 100 && progressBar1.Value < firsttime)
            {
                //backgroundWorker1.ReportProgress(p_ProcessPercent);
            }
            else if (p_ProcessPercent >= progressBar1.Value)
            {
                backgroundWorker1.ReportProgress(p_ProcessPercent);
            }

            label2.Text = p_Speach;
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(new Random().Next(100, 500));
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = string.Format("進行中... {0}% complete", e.ProgressPercentage);
            progressBar1.Value = e.ProgressPercentage;
            Instance.Refresh();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = string.Format("進行中... {0}% complete", 100);
            progressBar1.Value = 100;
            Thread.Sleep(1000);
            this.Close();
        }
    }
}
