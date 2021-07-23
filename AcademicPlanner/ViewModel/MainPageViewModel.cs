﻿using AcademicPlanner.Model;
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
            MessagingCenter.Subscribe<string>(this, "DeleteTerm", term =>
            {
                for(int i = 0; i < Terms.Count; i++)
                {
                    if (Terms[i].TermID == Int32.Parse(term))
                    {
                        Terms.Remove(Terms[i]);
                    }
                }
            });

            MessagingCenter.Subscribe<Term>(this, "UpdateTerm", term =>
            {
                //_ = LoadTerms();
                for(int i = 0; i < Terms.Count; i++)
                {
                    if (Terms[i].TermID == term.TermID)
                    {
                        Terms[i] = term;
                    }
                }
                //SelectedTerm = term;
                //TermName = SelectedTerm.TermName;
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
