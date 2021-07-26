using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class AddTermPageViewModel : BaseViewModel
    {
        private string _termName;
        public string TermName {
            get => _termName;
            set => SetField(ref _termName, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetField(ref _startDate, value);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetField(ref _endDate, value);
        }

        public ICommand AddTermCommand => new Command(AddTerm);
        async void AddTerm()
        {
            // Check name value is entered in
            if (TermName != null)
            {
                if (Validations.EndDateAfterStart(StartDate, EndDate))
                {
                    // create new term from user selected values      
                    Term newTerm = new Term
                    {
                        TermName = TermName,
                        TermStart = StartDate,
                        TermEnd = EndDate
                    };
                    await TermService.AddTerm(newTerm);
                    // Send msg to add new term to the Terms observablecollection in the mainpageviewmodel
                    MessagingCenter.Send(newTerm, "AddTerm");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "End Date Should Occur After The Start Date.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Please Enter A Name.", "OK");
            }
        }
    }
}
