using AcademicPlanner.Model;
using AcademicPlanner.Services;
using AcademicPlanner.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace AcademicPlanner.ViewModel
{
    class CoursePageViewModel : BaseViewModel
    {
        // add course information properties and bind to the UI
        private string _courseID;
        public string CourseID
        {
            get => _courseID;
            set
            {
                SetField(ref _courseID, value);
            }
        }

        private string _termID;
        public string TermID
        {
            get => _termID;
            set
            {
                SetField(ref _termID, value);
            }
        }

        private string _courseName;
        public string CourseName
        {
            get => _courseName;
            set
            {
                SetField(ref _courseName, value);
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

        private string _courseStatus;
        public string CourseStatus
        {
            get => _courseStatus;
            set
            {
                SetField(ref _courseStatus, value);
            }
        }

        private string _instructorName;
        public string InstructorName
        {
            get => _instructorName;
            set
            {
                SetField(ref _instructorName, value);
            }
        }

        private string _instructorPhone;
        public string InstructorPhone
        {
            get => _instructorPhone;
            set
            {
                SetField(ref _instructorPhone, value);
            }
        }

        private string _instructorEmail;
        public string InstructorEmail
        {
            get => _instructorEmail;
            set
            {
                SetField(ref _instructorEmail, value);
            }
        }

        private string _courseNotes;
        public string CourseNotes
        {
            get => _courseNotes;
            set
            {
                SetField(ref _courseNotes, value);
            }
        }

        private bool _setAlerts;
        public bool SetAlerts
        {
            get => _setAlerts;
            set
            {
                SetField(ref _setAlerts, value);
            }
        }

        private Assessment _selectedAssessment = new Assessment();
        public Assessment SelectedAssessment
        {
            get => _selectedAssessment;
            set
            {
                SetField(ref _selectedAssessment, value);

                // Navigate to AssessmentPage and pass the selected assessment
                if (SelectedAssessment != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new AssessmentPage(SelectedAssessment));
                }
            }
        }

        public ICommand DeleteCourseCommand => new Command(DeleteCourse);
        async void DeleteCourse()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are You Sure You Want To Delete This Course?", "Yes", "No");
            if (answer)
            {
                // delete associated Assessments before deleting course
                DeleteAssociatedAssessments();

                await CourseService.RemoveCourse(Int32.Parse(CourseID));
                // send a message to the termpageVM notifying that a course has been deleted
                MessagingCenter.Send(CourseID, "DeleteCourse");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        async void DeleteAssociatedAssessments()
        {
            var assessments = await AssessmentService.GetAssessment();
            foreach (Assessment assessment in assessments)
            {
                // delete all assessments related to the selected course
                if (assessment.CourseID == Int32.Parse(CourseID))
                {
                    await AssessmentService.RemoveAssessment(assessment.AssessmentID);
                }
            }
        }

        public ICommand UpdateCourseCommand => new Command(UpdateCourse);
        async void UpdateCourse()
        {
            Course updateCourse = new Course()
            {
                CourseID = Int32.Parse(CourseID),
                CourseName = CourseName,
                TermID = Int32.Parse(TermID),
                StartDate = StartDate,
                EndDate = EndDate,
                CourseStatus = CourseStatus,
                InstructorName = InstructorName,
                InstructorPhone = InstructorPhone,
                InstructorEmail = InstructorEmail,
                CourseNotes = CourseNotes,
                SetAlerts = SetAlerts
            };

            await Application.Current.MainPage.Navigation.PushAsync(new EditCoursePage(updateCourse));
        }

        public ICommand AddAssessmentCommand => new Command(AddAssessment);
        async void AddAssessment()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddAssessmentPage(Int32.Parse(CourseID)));
        }

        public ICommand ShareCourseNotesCommand => new Command(ShareCourseNotes);
        async void ShareCourseNotes()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = $"Course Notes For {CourseName}",
                    Body = CourseNotes,
                    To = { "Tupchu5@wgu.edu" }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                _ = Application.Current.MainPage.DisplayAlert("Feature Not Supported", "Emailing not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        // Create an observablecollection to store assessments associated with selected course
        public ObservableCollection<Assessment> AssessmentCollection { get; set; } = new ObservableCollection<Assessment>();

        public CoursePageViewModel()
        {
            _ = LoadAssessments();

            // subscribe to messages and update information accordingly
            MessagingCenter.Subscribe<Assessment>(this, "AddAssessment", assessment =>
            {
                AssessmentCollection.Add(assessment);
            });
            MessagingCenter.Subscribe<String>(this, "DeleteAssessment", assessment => 
            {
                for (int i = 0; i < AssessmentCollection.Count; i++)
                {
                    if (AssessmentCollection[i].AssessmentID == Int32.Parse(assessment))
                    {
                        AssessmentCollection.Remove(AssessmentCollection[i]);
                    }
                }
            });
            MessagingCenter.Subscribe<Assessment>(this, "UpdateAssessment", assessment =>
            {
                for(int i = 0; i < AssessmentCollection.Count; i++)
                {
                    if (AssessmentCollection[i].AssessmentID == assessment.AssessmentID)
                    {
                        AssessmentCollection[i] = assessment;
                    }
                }
            });
            MessagingCenter.Subscribe<Course>(this, "UpdateCourse", course =>
            {
                CourseID = course.CourseID.ToString();
                TermID = course.TermID.ToString();
                CourseName = course.CourseName;
                StartDate = course.StartDate;
                EndDate = course.EndDate;
                CourseStatus = course.CourseStatus;
                InstructorName = course.InstructorName;
                InstructorPhone = course.InstructorPhone;
                InstructorEmail = course.InstructorEmail;
                CourseNotes = course.CourseNotes;
                SetAlerts = course.SetAlerts;
            });
        }
        async Task LoadAssessments()
        {
            // Init the AssessmentCollection with data from database
            AssessmentCollection.Clear();
            var assessments = await AssessmentService.GetAssessment();
            foreach (Assessment assessment in assessments)
            {
                if (assessment.CourseID == Int32.Parse(CourseID))
                {
                    AssessmentCollection.Add(assessment);
                }
            }
        }
    }
}
