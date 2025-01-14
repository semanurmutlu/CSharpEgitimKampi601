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

namespace CSharpEgitimKampi601
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;port=5432;Database=CustomerDb;user Id=postgres;password=1";

        void EmployeeList()
        {
            var connection=new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From Employees";
            var command = new NpgsqlCommand(query, connection);
            var adapter=new NpgsqlDataAdapter(command);
            DataTable dataTable=new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource=dataTable;
            connection.Close();
        }

        void DepartmentList()
        {
            var connection=new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From Departments";
            var command=new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable=new DataTable();
            adapter.Fill(dataTable);
            cmbEmployeeDepartment.DisplayMember = "DepartmentName";
            cmbEmployeeDepartment.ValueMember = "DepartmentId";
            cmbEmployeeDepartment.DataSource = dataTable;
            connection.Close();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            EmployeeList();
        }

        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            EmployeeList();
            DepartmentList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string employeeName = txtEmployeeName.Text;
            string employeeSurname=txtEmployeeSurname.Text;
            decimal employeeSalary=decimal.Parse(txtEmployeeSalary.Text);
            int departmentId=int.Parse(cmbEmployeeDepartment.SelectedValue.ToString());
            
            var connection=new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "insert into Employees (EmployeeName,EmployeeSurname,EmployeeSalary,Departmentid) values (@employeeName,@employeeSurname,@employeeSalary,@departmentid)"; 
            var command=new NpgsqlCommand(query,connection);
            command.Parameters.AddWithValue("@employeeName",employeeName);
            command.Parameters.AddWithValue("@employeeSurname",employeeSurname);
            command.Parameters.AddWithValue("@employeeSalary",employeeSalary);
            command.Parameters.AddWithValue("@departmentid", departmentId);
            command.ExecuteNonQuery();
            MessageBox.Show("Ekleme işlemi başarılı");
            connection.Close();
            EmployeeList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id=int.Parse(txtEmployeeId.Text);
            var connection=new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Delete From Employees Where EmployeeId=@employeeid";
            var command=new NpgsqlCommand(query,connection);
            command.Parameters.AddWithValue("@employeeid", id);
            command.ExecuteNonQuery();
            MessageBox.Show("Silme işlemi Başarılı");
            connection.Close();
            EmployeeList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id=int.Parse(txtEmployeeId.Text);
            string employeeName=txtEmployeeName.Text;
            string employeeSurname=txtEmployeeSurname.Text;
            decimal employeeSalary=decimal.Parse(txtEmployeeSalary.Text);
            int departmentId=int.Parse(cmbEmployeeDepartment.SelectedValue.ToString());
            var connection=new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Update Employees Set EmployeeName=@employeeName,EmployeeSurname=@employeeSurname,EmployeeSalary=@employeeSalary,DepartmentId=@departmentid Where EmployeeId=@employeeid";
            var command= new NpgsqlCommand(query,connection);
            command.Parameters.AddWithValue("@employeeName", employeeName);
            command.Parameters.AddWithValue("@employeeSurname", employeeSurname);
            command.Parameters.AddWithValue("@employeeSalary", employeeSalary);
            command.Parameters.AddWithValue("@departmentid", departmentId);
            command.Parameters.AddWithValue("@employeeid", id);
            command.ExecuteNonQuery();
            MessageBox.Show("Güncelleme işlemi başarılı");
            connection.Close();
            EmployeeList();
        }

        private void btnGetById_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtEmployeeId.Text);
            var connection=new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From Employees Where EmployeeId=@employeeId";
            var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@employeeId", id);
            var adapter=new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource= dataTable;
            command.ExecuteNonQuery();
            MessageBox.Show("Id'ye göre getirme işlemi başarılı");
            connection.Close();

        }
    }
}
