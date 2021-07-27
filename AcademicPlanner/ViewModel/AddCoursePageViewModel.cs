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
    class AddCoursePageViewModel : BaseViewModel
    {
        // which term the courses are being saved to.
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

        private string _courseNotes;
        public string CourseNotes
        {
            get => _courseNotes;
            set
            {
                SetField(ref _courseNotes, value);
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

        private bool _setAlerts;
        public bool SetAlerts
        {
            get => _setAlerts;
            set
            {
                SetField(ref _setAlerts, value);
            }
        }

        private int courseCount = 0;

        public ICommand AddCourseCommand => new Command(AddCourse);
        async void AddCourse()
        {
            await CheckCourseCount();
            if (CourseName != null && StartDate != null && EndDate != null && CourseStatus != null && InstructorName != null && InstructorPhone != null && InstructorEmail != null)
            {
                if (Validations.EndDateAfterStart(StartDate, EndDate))
                {
                    if(courseCount != 6)
                    {
                        // create Course object from all user input data
                        Course course = new Course
                        {
                            // course name
                            CourseName = CourseName,
                            // term id
                            TermID = Int32.Parse(TermID),
                            // start date
                            StartDate = StartDate,
                            // end date
                            EndDate = EndDate,
                            // Course status
                            CourseStatus = CourseStatus,

                            // instructors name, phone, email
                            InstructorName = InstructorName,
                            InstructorPhone = InstructorPhone,
                            InstructorEmail = InstructorEmail,
                            // course notes
                            CourseNotes = CourseNotes,
                            // set alerts
                            SetAlerts = (bool)SetAlerts
                        };
                        await CourseService.AddCourse(course);

                        // when a course is added set notifications for the start and end date
                        SetNotifications(SetAlerts, CourseName, $"{CourseName} starts on {StartDate}", 1, DateTime.Now.AddSeconds(5));
                        SetNotifications(SetAlerts, CourseName, $"{CourseName} ends on {EndDate}", 2, DateTime.Now.AddSeconds(8));

                        // send message to termpageviewmodel to subscribe to the changes made here. // "AddCourse"
                        MessagingCenter.Send(course, "AddCourse");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Maximum Courses Reached", "Each Term Can Only Have Up To Six Courses.", "OK");
                    }
                }
                else
                {
                    // tell user end date needs to come after start date.
                    await Application.Current.MainPage.DisplayAlert("Invalid Date", "End Date Must Occur After The Start Date.", "OK");
                }
            }
            else
            {
                // all fields need to be occupied.
                await Application.Current.MainPage.DisplayAlert("Occupy All Fields", "All Fields Must Be Occupied.", "OK");
            }
        }

        async Task CheckCourseCount()
        {
            var courses = await CourseService.GetCourse();
            foreach (Course course in courses)
            {
                // only count courses with the same TermID as the selected term.
                if (course.TermID == Int32.Parse(TermID))
                {
                    courseCount++;
                }
            }
        }
    }
}
