using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HMI.View;

namespace HMI
{
    public partial class Form1 : Form
    {
        //查詢資料庫物件
        Dao dao = new Dao();

        MainForm main = new MainForm();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //輸入的帳號
            string account = txtAccount.Text;
            //輸入的密碼
            string passed = txtCode.Text;

            DataTable dt = null;
            try
            {
                dt = dao.Query("SELECT * FROM XINUsers WHERE EmployeeNo = '" + account + "' AND PasswordCode = '" + passed + "'");

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("帳號或密碼錯誤");
                }
                else
                {
                    MessageBox.Show("登入成功");
                    this.Hide();
                    
                    main.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();

            main.Show();
        }

    }
}
