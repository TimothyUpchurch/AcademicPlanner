using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class AssessmentPageViewModel : BaseViewModel
    {

        private string _assessmentID;
        public string AssessmentID
        {
            get => _assessmentID;
            set
            {
                SetField(ref _assessmentID, value);
            }
        }

        private string _courseID;
        public string CourseID
        {
            get => _courseID;
            set
            {
                SetField(ref _courseID, value);
            }
        }

        private string _assessmentName;
        public string AssessmentName
        {
            get => _assessmentName;
            set
            {
                SetField(ref _assessmentName, value);
            }
        }

        private string _assessmentType;
        public string AssessmentType
        {
            get => _assessmentType;
            set
            {
                SetField(ref _assessmentType, value);
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetField(ref _startDate, value);
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetField(ref _endDate, value);
            }
        }


        public ICommand DeleteAssessmentCommand => new Command(DeleteAssessment);
        async void DeleteAssessment(Object assessment)
        {
            Assessment deleteAssessment = assessment as Assessment;
            await AssessmentService.RemoveAssessment(deleteAssessment);

            MessagingCenter.Send(deleteAssessment, "DeleteAssessment");

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public ICommand EditAssessmentCommand => new Command(EditAssessment);
        async void EditAssessment()
        {
            Assessment assessment = new Assessment()
            {
                AssessmentID = Int32.Parse(AssessmentID),
                CourseID = Int32.Parse(CourseID),
                AssessmentName = AssessmentName,
                AssessmentType = AssessmentType,
                StartDate = StartDate,
                EndDate = EndDate
            };

            await AssessmentService.UpdateAssessment(assessment);

            MessagingCenter.Send(assessment, "UpdateAssessment");

            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
