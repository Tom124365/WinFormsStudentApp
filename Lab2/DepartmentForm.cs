using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Lab2
{
    public partial class DepartmentForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public DepartmentForm()
        {
            InitializeComponent();
        }

        private void LoadDepartments()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query =
                        "SELECT DepartmentID, DepartmentName FROM tblDepartments";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvDepartments.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            txtDepartmentID.Clear();
            txtDepartmentName.Clear();
        }

        private void DepartmentForm_Load(object sender, EventArgs e)
        {
            LoadDepartments();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection =
           new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO tblDepartments
                            (DepartmentName)
                            VALUES
                            (@name)";

                    SqlCommand cmd =
                        new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue(
                        "@name", txtDepartmentName.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Department added successfully.");

                    LoadDepartments();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDepartmentID.Text))
            {
                MessageBox.Show("Please select a department first.");
                return;
            }

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE tblDepartments
                             SET DepartmentName = @name
                             WHERE DepartmentID = @id";

                    SqlCommand cmd =
                        new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue(
                        "@id", int.Parse(txtDepartmentID.Text));

                    cmd.Parameters.AddWithValue(
                        "@name", txtDepartmentName.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Department updated successfully.");

                    LoadDepartments();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDepartmentID.Text))
            {
                MessageBox.Show("Please select a department first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this department?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection =
                       new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string query =
                            "DELETE FROM tblDepartments WHERE DepartmentID = @id";

                        SqlCommand cmd =
                            new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue(
                            "@id", int.Parse(txtDepartmentID.Text));

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Department deleted successfully.");

                        LoadDepartments();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dgvDepartments.Rows[e.RowIndex];

                txtDepartmentID.Text =
                    row.Cells["DepartmentID"].Value.ToString();

                txtDepartmentName.Text =
                    row.Cells["DepartmentName"].Value.ToString();
            }
        }
    }
}
