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
    public partial class StudentEnrollmentsForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public StudentEnrollmentsForm()
        {
            InitializeComponent();
        }

        private void LoadStudents()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query =
                        "SELECT StudentID, StudentName FROM tblStudents";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    cmbStudent.DataSource = table;
                    cmbStudent.DisplayMember = "StudentName";
                    cmbStudent.ValueMember = "StudentID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd =
                        new SqlCommand("GetCourses", connection);

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    cmbCourse.DataSource = table;
                    cmbCourse.DisplayMember = "CourseName";
                    cmbCourse.ValueMember = "CourseID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadEnrollments()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT
                                sc.StudentID,
                                s.StudentName,
                                sc.CourseID,
                                c.CourseName
                             FROM tblStudentCourses sc
                             INNER JOIN tblStudents s
                                ON sc.StudentID = s.StudentID
                             INNER JOIN tblCourses c
                                ON sc.CourseID = c.CourseID";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvEnrollments.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;

            if (cmbCourse.Items.Count > 0)
                cmbCourse.SelectedIndex = 0;

            dgvEnrollments.ClearSelection();
        }

        private void StudentEnrollmentsForm_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadCourses();
            LoadEnrollments();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection =
           new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO tblStudentCourses
                            (StudentID, CourseID)
                            VALUES
                            (@student, @course)";

                    SqlCommand cmd =
                        new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue(
                        "@student", cmbStudent.SelectedValue);

                    cmd.Parameters.AddWithValue(
                        "@course", cmbCourse.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Enrollment added successfully.");

                    LoadEnrollments();
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
            using (SqlConnection connection =
           new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE tblStudentCourses
                             SET CourseID = @newCourse
                             WHERE StudentID = @student
                             AND CourseID = @oldCourse";

                    // We need the original values from the selected row
                    DataGridViewRow row =
                        dgvEnrollments.CurrentRow;

                    int oldStudentID =
                        Convert.ToInt32(row.Cells["StudentID"].Value);

                    int oldCourseID =
                        Convert.ToInt32(row.Cells["CourseID"].Value);

                    SqlCommand cmd =
                        new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue(
                        "@student", oldStudentID);

                    cmd.Parameters.AddWithValue(
                        "@oldCourse", oldCourseID);

                    cmd.Parameters.AddWithValue(
                        "@newCourse", cmbCourse.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Enrollment updated successfully.");

                    LoadEnrollments();
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
            DataGridViewRow row = dgvEnrollments.CurrentRow;

            if (row == null)
            {
                MessageBox.Show("Please select an enrollment first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this enrollment?",
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

                        string query = @"DELETE FROM tblStudentCourses
                                 WHERE StudentID = @student
                                 AND CourseID = @course";

                        SqlCommand cmd =
                            new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue(
                            "@student",
                            row.Cells["StudentID"].Value);

                        cmd.Parameters.AddWithValue(
                            "@course",
                            row.Cells["CourseID"].Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Enrollment deleted successfully.");

                        LoadEnrollments();
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

        private void dgvEnrollments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dgvEnrollments.Rows[e.RowIndex];

                cmbStudent.SelectedValue =
                    row.Cells["StudentID"].Value;

                cmbCourse.SelectedValue =
                    row.Cells["CourseID"].Value;
            }
        }
    }
}
