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
    public partial class OutForm_1_1 : Form
    {
        Dao dao = new Dao();

        DataTable dt = new DataTable();

        OutForm_1 out1;

        public OutForm_1_1()
        {
            InitializeComponent();
        }

        public OutForm_1_1(OutForm_1 o)
        {
            out1 = o;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("hi");
        }
        
        public void DataRefresh() {
            try
            {
                string cmd = "SELECT ProductID, ProductName FROM Product ";
                dt = dao.Query(cmd);
                dgvmain.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OutForm_1_1_Load(object sender, EventArgs e)
        {
            DataRefresh();
            

        }

        private void dgvmain_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            out1.productID = dgvmain.CurrentRow.Cells[0].Value.ToString();
            out1.productName = dgvmain.CurrentRow.Cells[1].Value.ToString();
            this.Close();
        }
    }
}
