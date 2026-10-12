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
    public partial class StudentForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public StudentForm()
        {
            InitializeComponent();
        }

        private void LoadStudents()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM tblStudents";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvStudents.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO tblStudents
                            (StudentName, DateOfBirth, City, Age, YearOfEnroll, Major, GPA)
                            VALUES
                            (@name, @dob, @city, @age, @year, @major, @gpa)";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                    cmd.Parameters.AddWithValue("@dob", DateTime.Parse(txtDateOfBirth.Text));
                    cmd.Parameters.AddWithValue("@city", txtCity.Text);
                    cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(txtYearOfEnroll.Text));
                    cmd.Parameters.AddWithValue("@major", txtMajor.Text);
                    cmd.Parameters.AddWithValue("@gpa", decimal.Parse(txtGPA.Text));

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student added successfully.");

                    LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentID.Clear();
            txtStudentName.Clear();
            txtDateOfBirth.Clear();
            txtCity.Clear();
            txtAge.Clear();
            txtYearOfEnroll.Clear();
            txtMajor.Clear();
            txtGPA.Clear();
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];

                txtStudentID.Text = row.Cells["StudentID"].Value.ToString();
                txtStudentName.Text = row.Cells["StudentName"].Value.ToString();
                txtDateOfBirth.Text = row.Cells["DateOfBirth"].Value.ToString();
                txtCity.Text = row.Cells["City"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                txtYearOfEnroll.Text = row.Cells["YearOfEnroll"].Value.ToString();
                txtMajor.Text = row.Cells["Major"].Value.ToString();
                txtGPA.Text = row.Cells["GPA"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE tblStudents
                             SET StudentName = @name,
                                 DateOfBirth = @dob,
                                 City = @city,
                                 Age = @age,
                                 YearOfEnroll = @year,
                                 Major = @major,
                                 GPA = @gpa
                             WHERE StudentID = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id", int.Parse(txtStudentID.Text));
                    cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                    cmd.Parameters.AddWithValue("@dob", DateTime.Parse(txtDateOfBirth.Text));
                    cmd.Parameters.AddWithValue("@city", txtCity.Text);
                    cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(txtYearOfEnroll.Text));
                    cmd.Parameters.AddWithValue("@major", txtMajor.Text);
                    cmd.Parameters.AddWithValue("@gpa", decimal.Parse(txtGPA.Text));

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student updated successfully.");

                    LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("Please select a student first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string query = "DELETE FROM tblStudents " +
                                       "WHERE StudentID = @id";

                        SqlCommand cmd = new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue(
                            "@id",
                            int.Parse(txtStudentID.Text));

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student deleted successfully.");

                        LoadStudents();

                        // Clear the textboxes
                        txtStudentID.Clear();
                        txtStudentName.Clear();
                        txtDateOfBirth.Clear();
                        txtCity.Clear();
                        txtAge.Clear();
                        txtYearOfEnroll.Clear();
                        txtMajor.Clear();
                        txtGPA.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
