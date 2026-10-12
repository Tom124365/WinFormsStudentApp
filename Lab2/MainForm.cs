using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentForm studentForm = new StudentForm();
            studentForm.ShowDialog();
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CourseForm courseForm = new CourseForm();
            courseForm.ShowDialog();

        }

        private void instructorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstructorForm instructorForm = new InstructorForm();
            instructorForm.ShowDialog();
        }

        private void departmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepartmentForm departmentForm = new DepartmentForm();
            departmentForm.ShowDialog();
        }

        private void studentEnrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentEnrollmentsForm form = new StudentEnrollmentsForm();
            form.ShowDialog();
        }
    }
}
