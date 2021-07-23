using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class AddAssessmentPageViewModel : BaseViewModel
    {
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

        public ICommand AddAssessmentCommand => new Command(AddAssessment);
        async void AddAssessment()
        {
            // check if two assessments are already added
        
            // create new assessment
            Assessment assessment = new Assessment()
            {
                CourseID = Int32.Parse(CourseID),
                AssessmentName = AssessmentName,
                AssessmentType = AssessmentType,
                StartDate = StartDate,
                EndDate = EndDate
            };
            await AssessmentService.AddAssessment(assessment);

            SetNotifications(true, AssessmentName, $"{AssessmentName} ends on {EndDate}", 3, DateTime.Now.AddSeconds(5));

            //send msg to update coursepageviewmodel assessment collection.
            MessagingCenter.Send(assessment, "AddAssessment");

            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
