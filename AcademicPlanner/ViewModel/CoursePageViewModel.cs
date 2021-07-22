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
        async void DeleteCourse(Object course)
        {
            Course deleteCourse = course as Course;

            // delete associated Assessments
            DeleteAssociatedAssessments(deleteCourse);

            await CourseService.RemoveCourse(deleteCourse);

            MessagingCenter.Send(deleteCourse, "DeleteCourse");
            //await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            await Application.Current.MainPage.Navigation.PopAsync();

            // after deleting course navigate back to the previous page (termpage) set the previously selected term to null and refresh listview.
        }

        async void DeleteAssociatedAssessments(Course course)
        {
            var assessments = await AssessmentService.GetAssessment();
            foreach (Assessment assessment in assessments)
            {
                if (assessment.CourseID == course.CourseID)
                {
                    await AssessmentService.RemoveAssessment(assessment);
                }
            }
        }

        public ICommand UpdateCourseCommand => new Command(UpdateCourse);
        async void UpdateCourse(Object course)
        {
            Course updateCourse = course as Course;
            await Application.Current.MainPage.Navigation.PushAsync(new EditCoursePage(updateCourse));
        }

        public ICommand AddAssessmentCommand => new Command(AddAssessment);
        async void AddAssessment()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddAssessmentPage(Int32.Parse(CourseID)));
        }

        // Create an observablecollection to store assessments and set the binding to a listview in the ui
        public ObservableCollection<Assessment> AssessmentCollection { get; set; } = new ObservableCollection<Assessment>();

        public CoursePageViewModel()
        {
            _ = LoadAssessments();

            MessagingCenter.Subscribe<Assessment>(this, "AddAssessment", assessment =>
            {
                AssessmentCollection.Add(assessment);
            });

            MessagingCenter.Subscribe<Assessment>(this, "DeleteAssessment", assessment => 
            {
                AssessmentCollection.Remove(assessment);
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
        }
        async Task LoadAssessments()
        {
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
        //public static Assessment SelectedAssessment { get; set; } = new Assessment();
        // await Application.Current.MainPage.Navigation.PushAsync(new EditCoursePage(course))
    }
}
