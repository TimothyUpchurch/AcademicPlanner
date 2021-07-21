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

            await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
