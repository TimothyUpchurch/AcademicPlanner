using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        private int count = 0;
        private bool conflictingAssessmentType;

        public ICommand AddAssessmentCommand => new Command(AddAssessment);
        async void AddAssessment()
        {
            count = 0;
            conflictingAssessmentType = false;
            // check if two assessments are already added
            await NumberOfAssessments();
            if (count != 2)
            {
                if (Validations.EndDateAfterStart(StartDate, EndDate))
                {
                    if (AssessmentName != null && AssessmentType != null && StartDate != null && EndDate != null)
                    {
                        await CheckConflictingAssessmentTypes();
                        if (conflictingAssessmentType == false)
                        {
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
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Assessment Type Already Added", "Please Try Adding A Different Assessment Type.", "OK");
                            AssessmentType = "";
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Empty Fields", "All Fields Should Be Occupied.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Date", "End Date Should Occur After Start Date.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Maximum Assessments", "Two Assessments Have Already Been Planned For This Course.", "OK");
            }
        }

        async Task NumberOfAssessments()
        {
            var assessments = await AssessmentService.GetAssessment();
            foreach(Assessment assessment in assessments)
            {
                if (assessment.CourseID == Int32.Parse(CourseID))
                {
                    count++;
                }
            }
        }

        async Task CheckConflictingAssessmentTypes()
        {
            var assessments = await AssessmentService.GetAssessment();
            foreach (Assessment assessment in assessments)
            {
                // check the assessments relevant to this course
                if (assessment.CourseID == Int32.Parse(CourseID))
                {
                    // if the assessment type already exists
                    if (assessment.AssessmentType == AssessmentType)
                    {
                        conflictingAssessmentType = true;
                    }
                }
            }
        }
    }
}
