using AcademicPlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AcademicPlanner.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCoursePage : ContentPage
    {
        public EditCoursePage(Course course)
        {
            InitializeComponent();

            selectedCourseID.Text = course.CourseID.ToString();
            selectedTermID.Text = course.TermID.ToString();

            courseName.Text = course.CourseName;
            startDate.Date = course.StartDate;
            endDate.Date = course.EndDate;
            previousEndDate.Date = course.EndDate;
            status.SelectedItem = course.CourseStatus;
            courseNotes.Text = course.CourseNotes;
            instructorName.Text = course.InstructorName;
            instructorPhone.Text = course.InstructorPhone;
            instructorEmail.Text = course.InstructorEmail;
            setAlertsSwitch.IsToggled = course.SetAlerts;
            //setAlerts.IsToggled = course.SetAlerts;
        }
    }
}