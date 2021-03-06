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
    public partial class CoursePage : ContentPage
    {
        public CoursePage(Course course)
        {
            InitializeComponent();
            OnAppearing();

            courseID.Text = course.CourseID.ToString();
            termID.Text = course.TermID.ToString();
            courseNameLabel.Text = course.CourseName;
            courseStartDateLabel.Text = course.StartDate.ToString();
            courseEndDateLabel.Text = course.EndDate.ToString();

            courseStatusLabel.Text = course.CourseStatus;

            instructorNameLabel.Text = course.InstructorName;
            instructorPhoneLabel.Text = course.InstructorPhone;
            instructorEmailLabel.Text = course.InstructorEmail;

            courseNotesLabel.Text = course.CourseNotes;
            alertsEnabledLabel.Text = course.SetAlerts.ToString();
        }
        protected override void OnAppearing()
        {
            // resets the selected item
            if (assessmentListView.SelectedItem != null)
            {
                assessmentListView.SelectedItem = null;
            }
        }
    }
}