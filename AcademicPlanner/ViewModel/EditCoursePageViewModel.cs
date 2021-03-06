using AcademicPlanner.Model;
using AcademicPlanner.Services;
using AcademicPlanner.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class EditCoursePageViewModel : BaseViewModel
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

        private DateTime _previousEndDate;
        public DateTime PreviousEndDate
        {
            get => _previousEndDate;
            set
            {
                SetField(ref _previousEndDate, value);
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

        public ICommand UpdateCourseCommand => new Command(UpdateCourse);
        async void UpdateCourse()
        {
            // ensure no empty fields
            if (CourseName != "" && StartDate != null && EndDate != null && CourseStatus != "" && InstructorName != "" && InstructorPhone != "" && InstructorEmail != "")
            {
                if (Validations.EndDateAfterStart(StartDate, EndDate))
                {
                    Course course = new Course
                    {
                        CourseID = Int32.Parse(CourseID),
                        TermID = Int32.Parse(TermID),
                        CourseName = CourseName,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        CourseStatus = CourseStatus,
                        CourseNotes = CourseNotes,
                        InstructorName = InstructorName,
                        InstructorPhone = InstructorPhone,
                        InstructorEmail = InstructorEmail,
                        SetAlerts = SetAlerts
                    };
                    await CourseService.UpdateCourse(course);

                    // Send message to the CoursePageVM to update course information
                    MessagingCenter.Send<Course>(course, "UpdateCourse");

                    // check if the EndDate is different. If so set a new reminder
                    if (PreviousEndDate != course.EndDate)
                    {
                        SetNotifications(SetAlerts, "Updated Course", $"End date for {CourseName} is now {EndDate}", 5, DateTime.Now.AddSeconds(5));
                    }

                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    // end date needs to come after the start date.
                    await Application.Current.MainPage.DisplayAlert("Invalid Date", "End Date Must Occur After The Start Date.", "OK");
                }
            }
            else
            {
                // all fields need to be occupied.
                await Application.Current.MainPage.DisplayAlert("Occupy All Fields", "All Fields Must Be Occupied.", "OK");
            }
        }
    }
}
