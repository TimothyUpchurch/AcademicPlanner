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
            _ = LoadTerms();

            // subscribe to the msg sent from the AddTermPageViewModel to add a term to the Terms collection
            MessagingCenter.Subscribe<Term>(this, "AddTerm", term => 
            {
                Terms.Add(term);
            });
            // subscribe to the msg sent from TermPageViewModel to delete a term from the Terms collection
            MessagingCenter.Subscribe<Term>(this, "DeleteTerm", term =>
            {
                Terms.Remove(term);
            });
        }
        // demo code
        async Task DemoTerms()
        {
            Terms.Clear();
            //await TermService.AddTerm("Term 1", DateTime.Now, DateTime.Today.AddDays(7));
            await LoadTerms();
        }
        async Task LoadTerms()
        {
            Terms.Clear();
            var terms = (await TermService.GetTerms());
            foreach (Term term in terms)
            {
                Terms.Add((Term)term);
            }
        }

        // Commands
        public ICommand Navigate => new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new AddTermPage()));
    }
}
