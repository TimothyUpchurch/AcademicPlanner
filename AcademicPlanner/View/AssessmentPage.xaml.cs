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
            assessmentNameLabel.Text = assessment.AssessmentName;
            assessmentTypeLabel.Text = assessment.AssessmentType;
            startDate.Text = assessment.StartDate.ToString();
            endDate.Text = assessment.EndDate.ToString();
        }
    }
}