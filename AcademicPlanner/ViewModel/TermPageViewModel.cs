using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class TermPageViewModel : BaseViewModel
    {
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

        public ICommand DeleteTermCommand => new Command(DeleteTerm);
        async void DeleteTerm(Object term)
        {
            if(term != null)
            {
                Term deletedTerm = term as Term;
                await TermService.RemoveTerm(deletedTerm);
                MessagingCenter.Send(deletedTerm, "DeleteTerm");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
