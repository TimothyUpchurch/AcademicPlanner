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
        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>(); // all courses in the selected term

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

        private string _termName;
        public string TermName
        {
            get => _termName;
            set
            {
                SetField(ref _termName, value);
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

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                SetField(ref _selectedCourse, value);
            }
        }

        public TermPageViewModel()
        {
            // init the Courses collection
            _ = LoadCourses();

            // subscribe to messages sent from other viewmodels
            MessagingCenter.Subscribe<Course>(this, "AddCourse", course =>
            {
                Courses.Add(course);
            });
            MessagingCenter.Subscribe<Term>(this, "UpdateTerm", term =>
            {
                TermID = term.TermID.ToString();
                TermName = term.TermName;
                StartDate = term.TermStart;
                EndDate = term.TermEnd;
            });
            MessagingCenter.Subscribe<string>(this, "DeleteCourse", course =>
            {
                for(int i = 0; i < Courses.Count; i++)
                {
                    if (Courses[i].CourseID == Int32.Parse(course))
                    {
                        Courses.Remove(Courses[i]);
                    }
                }
            });
            MessagingCenter.Subscribe<Course>(this, "UpdateCourse", course =>
            {
                for (int i = 0; i < Courses.Count; i++)
                {
                    if (Courses[i].CourseID == course.CourseID)
                    {
                        Courses[i] = course;
                    }
                }
            });
        }

        async Task LoadCourses()
        {
            Courses.Clear();
            var courses = await CourseService.GetCourse();
            foreach (Course course in courses)
            {
                // only show courses related to the selected term
                if (course.TermID == Int32.Parse(TermID))
                {
                    Courses.Add((Course)course);
                }            
            }
        }

        public ICommand DeleteTermCommand => new Command(DeleteTerm);
        async void DeleteTerm()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are You Sure You Want To Delete This Term?", "Yes", "No");
            if (TermID != null && answer)
            {
                DeleteAssociatedCourses(Int32.Parse(TermID)); // delete courses before deleting the term they are associated with
                await TermService.RemoveTerm(Int32.Parse(TermID));
                // send a message notifying that a term has been deleted to the mainpage 
                MessagingCenter.Send(TermID, "DeleteTerm");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
        async void DeleteAssociatedCourses(int termID)
        {
            var courses = await CourseService.GetCourse();
            foreach (Course course in courses)
            {
                // Remove all courses that have the selected term id
                if (course.TermID == termID)
                {
                    await CourseService.RemoveCourse(course.CourseID);
                }
            }
        }

        public ICommand EditTermCommand => new Command(EditTerm);
        async void EditTerm()
        {
            Term editTerm = new Term()
            {
                TermID = Int32.Parse(TermID),
                TermName = TermName,
                TermStart = StartDate,
                TermEnd = EndDate
            };
            await Application.Current.MainPage.Navigation.PushAsync(new EditTermPage(editTerm));
        }

        public ICommand Navigate => new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new AddCoursePage(Int32.Parse(TermID))));

        public ICommand NavigateToCoursePageCommand => new Command(NavigateToCoursePage);
        async void NavigateToCoursePage(Object course)
        {
            // cast selectedItem to course
            Course courseToNavigate = course as Course;
            if (course == null) return;
            await Application.Current.MainPage.Navigation.PushAsync(new CoursePage(courseToNavigate));
        }
    }
}
