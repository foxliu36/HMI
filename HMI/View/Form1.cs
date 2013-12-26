using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI.View
{
    public partial class Form1 : Form
    {

        

        public Form1()
        {
            InitializeComponent();
            userControl11.welkEvent += this.fun2;
        }

        //public void fun1(Object o, EventArgs e) Handles

        public void fun2(Object sender, EventArgs e) { MessageBox.Show("hi"); }
    }
}
