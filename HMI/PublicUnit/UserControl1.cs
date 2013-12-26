using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI.PublicUnit
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        //public delegate void myDelegate(string welkin);

        public event EventHandler welkEvent;

        private void button1_Click(object sender, EventArgs e)
        {
            EventHandler ball = this.welkEvent;
            ball(this, e);
        }
        //public void ff
        //private void b2click(EventArgs e) {
        //    EventHandler welkevent = this.welkEvent;
        //    if (welkevent != null)
        //    {
        //        welkEvent(this, e);
        //    }
        //}
    }
}
