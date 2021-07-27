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
    public partial class AssessmentPage : ContentPage
    {
        public AssessmentPage(Assessment assessment)
        {
            InitializeComponent();
            //deleteAssessmentButton.CommandParameter = assessment;

            assessmentIDLabel.Text = assessment.AssessmentID.ToString();
            courseIDLabel.Text = assessment.CourseID.ToString();
            previousEndDate.Text = assessment.EndDate.ToString();

            assessmentNameLabel.Text = assessment.AssessmentName;
            assessmentTypeLabel.SelectedItem = assessment.AssessmentType;
            startDate.Date = assessment.StartDate;
            endDate.Date = assessment.EndDate;
        }
    }
}