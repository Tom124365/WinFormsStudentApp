using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class InstructorForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public InstructorForm()
        {
            InitializeComponent();
        }

        private void LoadInstructors()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT
                                i.InstructorID,
                                i.InstructorName,
                                i.DepartmentID,
                                d.DepartmentName
                             FROM tblInstructors i
                             INNER JOIN tblDepartments d
                                ON i.DepartmentID = d.DepartmentID";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvInstructors.DataSource = table;

                    dgvInstructors.Columns["DepartmentID"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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

                    cmbDepartment.DataSource = table;
                    cmbDepartment.DisplayMember = "DepartmentName";
                    cmbDepartment.ValueMember = "DepartmentID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void InstructorForm_Load(object sender, EventArgs e)
        {
            LoadInstructors();
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

                    string query = @"INSERT INTO tblInstructors
                            (InstructorName, DepartmentID)
                            VALUES
                            (@name, @department)";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@name",
                        txtInstructorName.Text);

                    cmd.Parameters.AddWithValue("@department",
                        cmbDepartment.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Instructor added successfully.");

                    LoadInstructors();
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
            if (string.IsNullOrWhiteSpace(txtInstructorID.Text))
            {
                MessageBox.Show("Please select an instructor first.");
                return;
            }

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE tblInstructors
                             SET InstructorName = @name,
                                 DepartmentID = @department
                             WHERE InstructorID = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id",
                        int.Parse(txtInstructorID.Text));

                    cmd.Parameters.AddWithValue("@name",
                        txtInstructorName.Text);

                    cmd.Parameters.AddWithValue("@department",
                        cmbDepartment.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Instructor updated successfully.");

                    LoadInstructors();
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
            if (string.IsNullOrWhiteSpace(txtInstructorID.Text))
            {
                MessageBox.Show("Please select an instructor first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this instructor?",
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
                            "DELETE FROM tblInstructors WHERE InstructorID = @id";

                        SqlCommand cmd =
                            new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue("@id",
                            int.Parse(txtInstructorID.Text));

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Instructor deleted successfully.");

                        LoadInstructors();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        private void ClearFields()
        {
            txtInstructorID.Clear();
            txtInstructorName.Clear();

            if (cmbDepartment.Items.Count > 0)
                cmbDepartment.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvInstructors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dgvInstructors.Rows[e.RowIndex];

                txtInstructorID.Text =
                    row.Cells["InstructorID"].Value.ToString();

                txtInstructorName.Text =
                    row.Cells["InstructorName"].Value.ToString();

                cmbDepartment.SelectedValue =
                    row.Cells["DepartmentID"].Value;
            }
        }
    }
}
