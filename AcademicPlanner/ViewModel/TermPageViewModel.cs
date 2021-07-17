using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class TermPageViewModel : BaseViewModel
    {
        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>(); // all courses in the term

        // selected term id. Will be used when creating new courses for the selected term.
        private string _termID;
        public string TermID
        {
            get => _termID;
            set
            {
                SetField(ref _termID, value);
            }
        }
        public TermPageViewModel()
        {
            Courses.Add(new Course()
            {
                CourseID = 0,
                CourseName = "C#",
                TermID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Today.AddDays(7),
                CourseStatus = "",
                InstructorName = "Timothy Upchurch",
                InstructorPhone = "618-210-6687",
                InstructorEmail = "Timmyupc@gmail.com",
                CourseNotes = "Creating an application in xamarin forms.",
                SetAlerts = true
            }) ;
        }

        public ICommand DeleteTermCommand => new Command(DeleteTerm);
        async void DeleteTerm(Object term)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are You Sure You Want To Delete This Term?", "Yes", "No");
            if (term != null && answer)
            {
                Term deletedTerm = term as Term;
                await TermService.RemoveTerm(deletedTerm);
                MessagingCenter.Send(deletedTerm, "DeleteTerm");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
