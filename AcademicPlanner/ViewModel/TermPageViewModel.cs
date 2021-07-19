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
            _ = LoadCourses();

            MessagingCenter.Subscribe<Course>(this, "AddCourse", course =>
            {
                Courses.Add(course);
            });
        }

        async Task LoadCourses()
        {
            Courses.Clear();
            var courses = await CourseService.GetCourse();
            foreach (Course course in courses)
            {
                if (course.TermID == Int32.Parse(TermID))
                {
                    Courses.Add((Course)course);
                }
                
            }
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

        public ICommand Navigate => new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new AddCoursePage(Int32.Parse(TermID))));
    }
}
