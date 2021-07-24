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

        public TermPageViewModel()
        {
            _ = LoadCourses();

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
                //Term deletedTerm = term as Term;
                DeleteAssociatedCourses(Int32.Parse(TermID)); // delete courses before deleting the term they are associated with
                await TermService.RemoveTerm(Int32.Parse(TermID));
                MessagingCenter.Send(TermID, "DeleteTerm");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        async void DeleteAssociatedCourses(int termID)
        {
            var courses = await CourseService.GetCourse();
            foreach (Course course in courses)
            {
                if (course.TermID == termID)
                {
                    // Remove Course
                    await CourseService.RemoveCourse(course.CourseID);
                }
            }
        }

        public ICommand EditTermCommand => new Command(EditTerm);

        //Object term
        async void EditTerm()
        {
            //Term editTerm = term as Term;

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
    }
}
