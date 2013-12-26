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
    public partial class OutForm_1 : Form
    {

        Dao dao = new Dao();

        DataTable dt = new DataTable();

        OutForm_1_1 out_1_1;

        public string productID;
        public string productName;

        public OutForm_1()
        {
            InitializeComponent();
        }

        public void DataRefresh() {
            try
            {
                string cmd = "SELECT D.*,P.ProductName FROM [DeliveryDetails]AS D JOIN [Product]AS P " +
                             "ON D.ProductID = P.ProductID WHERE D.DeliveryID = '" + DeliveryID + "'";
                dt = dao.Query(cmd);
                dgvmain.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OutForm_1_Load(object sender, EventArgs e)
        {
            txtDeliveryID.Text = DeliveryID;
            DataRefresh();
        }

        //送貨編號
        public string DeliveryID
        {
            get;
            set;
        }

        private void btnProduck_Click(object sender, EventArgs e)
        {
            out_1_1 = new OutForm_1_1(this);
            out_1_1.FormClosed += new FormClosedEventHandler(FindProduck);
            out_1_1.ShowDialog();
        }

        public void FindProduck(object sender, FormClosedEventArgs e)
        {
            this.txtProductID.Text = this.productID;
            this.txtProductName.Text = this.productName;
        }

        //增加
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSeq.Text != "" | txtProductID.Text != "" | txtQuantity.Text != "" | txtUnitPrice.Text != "" | txtAmount.Text != "")
                {
                    string cmd = "INSERT INTO DeliveryDetails VALUES ( " +
                             "'" + DeliveryID           + "'," +
                             "'" + txtSeq.Text          + "'," +
                             "'" + txtProductID.Text    + "'," +
                             "'" + txtQuantity.Text     + "'," +
                             "'" + txtUnitPrice.Text    + "'," +
                             "'" + txtAmount.Text       + "')";
                    dao.ExcuteNonQuery(cmd);
                }
                else {
                    MessageBox.Show("不能有欄位是空值");
                }
                DataRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //刪除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string seq = dgvmain.CurrentRow.Cells[1].Value.ToString();
                string pID = dgvmain.CurrentRow.Cells[2].Value.ToString();
                string cmd = "DELETE FROM DeliveryDetails " +
                             "WHERE DeliveryID = '"     + DeliveryID + "' AND " +
                                   "DeliverySeq = '"    + seq        + "' AND " +
                                   "ProductID = '"      + pID        + "'";
                dao.ExcuteNonQuery(cmd);
                
                DataRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvmain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
