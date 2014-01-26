using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HMI.Entity;
using System.Threading;
using HMI.CommonForm;

namespace HMI.View
{
    public partial class OutForm : Form
    {
        //資料庫存取物件
        Dao dao = new Dao();
        DataTable dt;
        //預設狀態為查詢
        FormStatus status = FormStatus.Query;

        public OutForm()
        {
            InitializeComponent();
        }

        //更新畫面
        private void DataRefresh() {
            try
            {
                using(var context = new XINEntities()) {
                    var qdata = from q in context.Customer
                                select q;
                }
                dt = dao.Query("SELECT * FROM Delivery");

                var dvgdata = from q in dt.AsEnumerable()
                              select new
                              {
                                  DeliveryID = q.Field<string>("DeliveryID"),
                                  DeliveryDate = q.Field<DateTime>("DeliveryDate")
                              };

                this.dgvDelivery.DataSource = dvgdata.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //載入畫面
        private void OutForm_Load(object sender, EventArgs e)
        {
            DataRefresh();
        }

        //跳回主畫面
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //點選DataGridView
        private void dgvDelivery_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string Deliveryclick = dgvDelivery.CurrentRow.Cells[0].Value.ToString();

            var showdata = from q in dt.AsEnumerable()
                           where q.Field<string>("DeliveryID").Equals(Deliveryclick)
                           select new
                           {
                               deliveryid  = q.Field<string>("DeliveryID"),
                               deliverdate = q.Field<DateTime>("DeliveryDate").ToString("yyyy-MM-dd"),
                               customer    = q.Field<string>("CustomerID"),
                               joinman     = q.Field<string>("JoinMan"),
                               subtotal    = q.Field<int>("SubTotal"),
                               address     = q.Field<string>("ShipAddress"),
                               comment     = q.Field<string>("Comment"),
                               invoice     = q.Field<string>("InvoiceNo"),
                               valueaddtax = q.Field<int>("ValueAddTax"),
                               deliverytype= q.Field<string>("DeliveryType")
                           };
            
            this.txtDeliveryID.Text = showdata.First().deliveryid.ToString();
            this.txtDate.Text = showdata.First().deliverdate.ToString();
            this.txtCustomer.Text = showdata.First().customer.ToString();
            //因為資料允許null
            if (showdata.First().invoice!=null)
            {
                this.txtInvoiceNo.Text = showdata.First().invoice.ToString();
            }
            //因為資料允許null
            if (showdata.First().joinman!=null)
            {
                this.txtJoinMan.Text = showdata.First().joinman.ToString();
            }
            this.txtSubTotal.Text = showdata.First().subtotal.ToString();
            //因為資料允許null
            if (showdata.First().address!=null)
            {
                this.txtAddress.Text = showdata.First().address.ToString();
            }
            //因為資料允許null
            if (showdata.First().comment != null)
            {
                this.txtComment.Text = showdata.First().comment.ToString();
            }
            this.txtValueAddTax.Text = showdata.First().valueaddtax.ToString();
            this.txtDeliveryType.Text = showdata.First().deliverytype.ToString();

            showDeliveryDetail(Deliveryclick);
        }

        //顯示清單細項
        private void showDeliveryDetail(string deliveryID) {

            DataTable dtDetail = dao.Query("SELECT * FROM DeliveryDetails WHERE DeliveryID = '" + deliveryID + "'");

            this.dgvDeliveryDetail.DataSource = dtDetail;
        }

        //新增
        private void btnNew_Click(object sender, EventArgs e)
        {
            //改變按鈕狀態
            BtnControl(false);
            //設定為新增狀態
            status = FormStatus.Insert;
            //打開編輯
            TxtControl(true);

            txtDeliveryID.Text = "自動產生";

        }

        //刪除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            BtnControl(false);
            status = FormStatus.Delete;
        }

        //修改
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            BtnControl(false);
            TxtControl(true);
            status = FormStatus.Update;
        }

        //確認
        private void btnCommit_Click(object sender, EventArgs e)
        {
            try
            {
                if (status == FormStatus.Insert)
                {
                    //產生出貨編號
                    string ID = createDeliveryID();

                    string cmd = "INSERT INTO Delivery" +
                    "(DeliveryID,DeliveryDate,CustomerID,JoinMan,DeliveryType" +
                    ",InvoiceNo,SubTotal,ValueAddTax,ShipAddress,Comment)VALUES" +
                    "('" + ID + "','"
                        + txtDate.Text + "','"
                        + txtCustomer.Text + "','"
                        + txtJoinMan.Text + "','"
                        + txtDeliveryType.Text + "','"
                        + txtInvoiceNo.Text + "','"
                        + txtSubTotal.Text + "','"
                        + txtValueAddTax.Text + "','"
                        + txtAddress.Text + "','"
                        + txtComment.Text
                    + "')";

                    dao.ExcuteNonQuery(cmd);

                    TxtControl(false);
                }

                if (status == FormStatus.Delete)
                {
                    DataTable dtDetail = dao.Query("SELECT * FROM DeliveryDetails WHERE DeliveryID = '" + txtDeliveryID.Text + "'");

                    if (dtDetail.Rows.Count > 0)
                    {
                        MessageBox.Show("尚有細項無法刪除,請於畫面右上角編輯細項");
                    }
                    else
                    {
                        string cmd = "DELETE FROM Delivery WHERE DeliveryID = '" + txtDeliveryID.Text + "'";

                        dao.ExcuteNonQuery(cmd);
                    }
                }

                if (status == FormStatus.Update)
                {
                    string cmd = "UPDATE Delivery SET "+
                                 "DeliveryDate = '"     + txtDate.Text          + "',"+
                                 "CustomerID = '"       + txtCustomer.Text      + "',"+
                                 "JoinMan = '"          + txtJoinMan.Text       + "',"+
                                 "DeliveryType = '"     + txtDeliveryType.Text  + "',"+
                                 "InvoiceNo = '"        + txtInvoiceNo.Text     + "',"+
                                 "SubTotal = '"         + txtSubTotal.Text      + "',"+
                                 "ValueAddTax = '"      + txtValueAddTax.Text   + "',"+
                                 "ShipAddress = '"      + txtAddress.Text       + "',"+
                                 "Comment = '"          + txtComment.Text       + "'"+
                                 "WHERE DeliveryID = '" + txtDeliveryID.Text+"'";

                    dao.ExcuteNonQuery(cmd);
                    TxtControl(false);
                }

                DataRefresh();
                status = FormStatus.Query;
                BtnControl(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        //自動產生出貨編號
        private string createDeliveryID() {
            DataTable dt = dao.Query("SELECT DeliveryID FROM Delivery");

            string[] ID = txtDate.Text.Split(new char[]{'-'});

            string datesub = (ID[0] + ID[1] + ID[2]).Substring(2,6);

            var data = from q in dt.AsEnumerable()
                       where q.Field<string>("DeliveryID").Contains(datesub)
                       select new { deliveryid = q.Field<string>("DeliveryID") };

            if (data.ToList().Count>=1)
            {
                return datesub + (data.ToList().Count + 1).ToString("000");
            }
            else
            {
                return datesub + "001";
            }
        }

        //控制欄位
        private void TxtControl(bool openornot) {
            txtDate.Enabled = openornot;
            txtCustomer.Enabled = openornot;
            txtInvoiceNo.Enabled = openornot;
            txtJoinMan.Enabled = openornot;
            txtSubTotal.Enabled = openornot;
            txtAddress.Enabled = openornot;
            txtComment.Enabled = openornot;
            txtDeliveryType.Enabled = openornot;
            txtValueAddTax.Enabled = openornot;
        }

        //控制按鈕
        private void BtnControl(bool openornot)
        {
            btnNew.Enabled = openornot;
            btnUpdate.Enabled = openornot;
            btnDelete.Enabled = openornot;

            btnCommit.Enabled = !openornot;
            btnCancel.Enabled = !openornot;
        }

        //編輯細項
        private void btnEditDetail_Click(object sender, EventArgs e)
        {
            OutForm_1 outform1 = new OutForm_1();

            outform1.DeliveryID = txtDeliveryID.Text;

            outform1.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(500);
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label11.Text = string.Format("Processing... {0}% complete", e.ProgressPercentage);
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
            Thread.Sleep(1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

            
            //backgroundWorker1.RunWorkerAsync();
            //progressBar1.Value +=1;
            WaitingForm wform = new WaitingForm();
            Thread th = new Thread(wform.Start);
            th.Start();
            Thread.Sleep(10000);
            //wform.WorkerReport(100, "結束");
            

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        
    }
}
