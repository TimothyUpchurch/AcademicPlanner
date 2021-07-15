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
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Term> Terms { get; set; } = new ObservableCollection<Term>();
        public MainPageViewModel()
        {
            _ = LoadTasks();           
        }
        public MainPageViewModel(INavigation navigation)
        {
            _ = LoadTasks();
            this.Navigation = navigation;
        }
        public INavigation Navigation { get; set; }
        async Task DemoTerms()
        {
            Terms.Clear();
            await TermService.AddTerm("Term 1", DateTime.Now, DateTime.Today.AddDays(7));
            await LoadTasks();
        }
        async Task LoadTasks()
        {
            Terms.Clear();
            var terms = (await TermService.GetTerms());
            foreach (Term term in terms)
            {
                Terms.Add((Term)term);
            }
        }

        public ICommand Navigate => new Command(NavigateAddTermPage);
        async void NavigateAddTermPage()
        {
            if (Navigation != null)
            await Navigation.PushAsync(new AddTermPage());
        }
    }
}
