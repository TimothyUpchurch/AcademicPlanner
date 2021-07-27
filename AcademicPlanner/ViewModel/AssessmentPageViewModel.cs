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

        private DateTime _previousEndDate;
        public DateTime PreviousEndDate
        {
            get => _previousEndDate;
            set
            {
                SetField(ref _previousEndDate, value);
            }
        }

        private bool conflictingAssessmentType;


        public ICommand DeleteAssessmentCommand => new Command(DeleteAssessment);
        async void DeleteAssessment()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are You Sure You Want To Delete This Assessment?", "Yes", "No");
            if (answer)
            {
                await AssessmentService.RemoveAssessment(Int32.Parse(AssessmentID));

                // notify coursepageVM that an assessment has been deleted
                MessagingCenter.Send(AssessmentID, "DeleteAssessment");

                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public ICommand EditAssessmentCommand => new Command(EditAssessment);
        async void EditAssessment()
        {
            conflictingAssessmentType = false;

            // check for invalid dates
            if (Validations.EndDateAfterStart(StartDate, EndDate))
            {
                // check for empty fields
                if (AssessmentName != "" && AssessmentType != "")
                {
                    await CheckConflictingAssessmentTypes();
                    // check for existing assessment types and avoid duplicates
                    if (conflictingAssessmentType == false)
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

                        if (PreviousEndDate != assessment.EndDate)
                        {
                            // if the end date was updated send out a new notification.
                            SetNotifications(true, AssessmentName, $"{AssessmentName} ends on {EndDate}", 3, DateTime.Now.AddSeconds(5));
                        }

                        await AssessmentService.UpdateAssessment(assessment);
                        MessagingCenter.Send(assessment, "UpdateAssessment");

                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Assessment Type Already Added", "Please Try Adding A Different Assessment Type.", "OK");
                    }
                }
                else
                {
                    // all fields need to be occupied.
                    await Application.Current.MainPage.DisplayAlert("Occupy All Fields", "All Fields Must Be Occupied.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Date", "End Date Must Occur After The Start Date.", "OK");
            }
        }
        async Task CheckConflictingAssessmentTypes()
        {
            var assessments = await AssessmentService.GetAssessment();
            
            foreach (Assessment assessment in assessments)
            {
                // check the assessments relevant to this course and ignore the selected assessment when comparing Types.
                if (assessment.CourseID == Int32.Parse(CourseID) && assessment.AssessmentID != Int32.Parse(AssessmentID))
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
