﻿using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
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

        public ICommand AddCourseCommand => new Command(AddCourse);

        async void AddCourse()
        {
            // create Course object from all user input data
            Course course = new Course
            {
                // course name
                CourseName = _courseName,
                // term id
                TermID = Int32.Parse(_termID),
                // start date
                StartDate = _startDate,
                // end date
                EndDate = _endDate,
                // Course status
                CourseStatus = _courseStatus,

                // instructors name, phone, email
                InstructorName = _instructorName,
                InstructorPhone = _instructorPhone,
                InstructorEmail = _instructorEmail,
                // course notes
                CourseNotes = _courseNotes,
                // set alerts
                SetAlerts = _setAlerts
            };
            await CourseService.AddCourse(course);

            // send message to termpageviewmodel to subscribe to the changes made here. // "AddCourse"
            MessagingCenter.Send(course, "AddCourse");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}