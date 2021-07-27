using AcademicPlanner.Model;
using AcademicPlanner.Services;
using AcademicPlanner.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class EditTermPageViewModel : BaseViewModel
    {
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

        public ICommand UpdateTermCommand => new Command(UpdateTerm);
        async void UpdateTerm()
        {
            if (TermName != null)
            {
                if (Validations.EndDateAfterStart(StartDate, EndDate))
                {
                    Term updateTerm = new Term
                    {
                        TermID = Int32.Parse(TermID),
                        TermName = TermName,
                        TermStart = StartDate,
                        TermEnd = EndDate
                    };
                    await TermService.UpdateTerm(updateTerm);

                    // send UpdateTerm msg to the 
                    MessagingCenter.Send(updateTerm, "UpdateTerm");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Date", "End Date Should Occur After The Start Date.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Please Enter A Name.", "OK");
            }
        }
    }
}
