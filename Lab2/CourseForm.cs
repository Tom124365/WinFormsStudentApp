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
    public partial class CourseForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public CourseForm()
        {
            InitializeComponent();
        }

        private void LoadCourses()
        {
            using (SqlConnection connection =
           new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT 
                                c.CourseID,
                                c.CourseName,
                                c.Credits,
                                c.DepartmentID,
                                d.DepartmentName,
                                c.InstructorID,
                                i.InstructorName
                             FROM tblCourses c
                             INNER JOIN tblDepartments d
                                ON c.DepartmentID = d.DepartmentID
                             INNER JOIN tblInstructors i
                                ON c.InstructorID = i.InstructorID";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvCourses.DataSource = table;

                    // Hide ID columns
                    dgvCourses.Columns["DepartmentID"].Visible = false;
                    dgvCourses.Columns["InstructorID"].Visible = false;
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
                        "SELECT DepartmentID, DepartmentName " +
                        "FROM tblDepartments";

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

        private void LoadInstructors()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query =
                        "SELECT InstructorID, InstructorName FROM tblInstructors";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    cmbInstructor.DataSource = table;
                    cmbInstructor.DisplayMember = "InstructorName";
                    cmbInstructor.ValueMember = "InstructorID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void CourseForm_Load(object sender, EventArgs e)
        {
            LoadCourses();
            LoadDepartments();
            LoadInstructors();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection =
           new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO tblCourses
                            (CourseName, Credits, DepartmentID, InstructorID)
                            VALUES
                            (@name, @credits, @department, @instructor)";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@name", txtCourseName.Text);
                    cmd.Parameters.AddWithValue("@credits",
                        int.Parse(txtCredits.Text));

                    cmd.Parameters.AddWithValue("@department",
                        cmbDepartment.SelectedValue);

                    cmd.Parameters.AddWithValue("@instructor",
                        cmbInstructor.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Course added successfully.");

                    LoadCourses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dgvCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCourses.Rows[e.RowIndex];

                txtCourseID.Text =
                    row.Cells["CourseID"].Value.ToString();

                txtCourseName.Text =
                    row.Cells["CourseName"].Value.ToString();

                txtCredits.Text =
                    row.Cells["Credits"].Value.ToString();

                cmbDepartment.SelectedValue =
                    row.Cells["DepartmentID"].Value;

                cmbInstructor.SelectedValue =
                    row.Cells["InstructorID"].Value;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseID.Text))
            {
                MessageBox.Show("Please select a course first.");
                return;
            }

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE tblCourses
                             SET CourseName = @name,
                                 Credits = @credits,
                                 DepartmentID = @department,
                                 InstructorID = @instructor
                             WHERE CourseID = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id",
                        int.Parse(txtCourseID.Text));

                    cmd.Parameters.AddWithValue("@name",
                        txtCourseName.Text);

                    cmd.Parameters.AddWithValue("@credits",
                        int.Parse(txtCredits.Text));

                    cmd.Parameters.AddWithValue("@department",
                        cmbDepartment.SelectedValue);

                    cmd.Parameters.AddWithValue("@instructor",
                        cmbInstructor.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Course updated successfully.");

                    LoadCourses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            txtCourseID.Clear();
            txtCourseName.Clear();
            txtCredits.Clear();

            if (cmbDepartment.Items.Count > 0)
                cmbDepartment.SelectedIndex = 0;

            if (cmbInstructor.Items.Count > 0)
                cmbInstructor.SelectedIndex = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseID.Text))
            {
                MessageBox.Show("Please select a course first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this course?",
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
                            "DELETE FROM tblCourses WHERE CourseID = @id";

                        SqlCommand cmd =
                            new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue(
                            "@id", int.Parse(txtCourseID.Text));

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Course deleted successfully.");

                        LoadCourses();

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
    }
}
