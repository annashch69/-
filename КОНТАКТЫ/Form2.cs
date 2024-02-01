using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КОНТАКТЫ
{

    public partial class Form2 : Form
    {
        private DataTable _dataTable;
        private BindingSource _bindingSource;

        public Form2(BindingSource bindingSource = null, DataTable dataTable = null)
        {
            InitializeComponent();
            this.Load += Form2_Load;

            if (bindingSource != null)
            {
                _bindingSource = bindingSource;
                dataGridView1.DataSource = _bindingSource;
            }
            else if (dataTable != null)
            {
                _dataTable = dataTable;
                _bindingSource = new BindingSource(_dataTable, null);
                dataGridView1.DataSource = _bindingSource;
            }
        }
        public Form2(DataTable dataTable) : this()
        {
            _dataTable = dataTable;

            // Установка источника данных для DataGridView
            dataGridView1.DataSource = _dataTable;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Statement 2
            var cs = "Host=localhost;Username=postgres;Password=1111;Database=number";

            //Statement 3
            NpgsqlConnection con = new NpgsqlConnection(cs);
            con.Open();

            //Statement 4
            var sql = "Select * from Контакты";

            //Statement 5
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);

            //Statement 6
            var dataReader = cmd.ExecuteReader();

            //Statement 7
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Load(dataReader);

            //Statement 8
            con.Close();
            dataGridView1.DataSource = dt;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1 ();
            f1.ShowDialog();
            this.Hide();
        }
    }
}
