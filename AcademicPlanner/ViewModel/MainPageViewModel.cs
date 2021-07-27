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

        private Term _selectedTerm;
        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                SetField(ref _selectedTerm, value);
                if (SelectedTerm != null)
                { 
                    Application.Current.MainPage.Navigation.PushAsync(new TermPage(SelectedTerm));
                }
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

        private bool applicationLoaded = false;

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
            MessagingCenter.Subscribe<string>(this, "DeleteTerm", termID =>
            {
                for(int i = 0; i < Terms.Count; i++)
                {
                    if (Terms[i].TermID == Int32.Parse(termID))
                    {
                        Terms.Remove(Terms[i]);
                    }
                }
            });
            // subscribe to the message sent from EditTermPageVM.
            MessagingCenter.Subscribe<Term>(this, "UpdateTerm", term =>
            {
                for(int i = 0; i < Terms.Count; i++)
                {
                    // update the term with the matching termid
                    if (Terms[i].TermID == term.TermID)
                    {
                        Terms[i] = term;
                    }
                }
            });
        }

        // demo code
        async Task DemoData()
        {
            Term initialTerm = new Term()
            {
                TermName = "First Term",
                TermStart = DateTime.Now,
                TermEnd = DateTime.Today.AddDays(7)
            };

            if (applicationLoaded == false)
            {
                await TermService.AddTerm(initialTerm);
                Terms.Add(initialTerm);
                applicationLoaded = true;
            }
        }
        async Task LoadTerms()
        {
            bool nameMatch = false;
            // Init Terms collection
            Terms.Clear();
            var terms = await TermService.GetTerms();
            foreach (Term term in terms)
            {
                Terms.Add((Term)term);
                if (term.TermName == "First Term")
                {
                    nameMatch = true;
                }
            }
            if(nameMatch == false)
            {
                await DemoData();
            }
        }
        public ICommand Navigate => new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new AddTermPage()));
    }
}
