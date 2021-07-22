using AcademicPlanner.Model;
using AcademicPlanner.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcademicPlanner.ViewModel
{
    class AssessmentPageViewModel : BaseViewModel
    {
        public ICommand DeleteAssessmentCommand => new Command(DeleteAssessment);
        async void DeleteAssessment(Object assessment)
        {
            Assessment deleteAssessment = assessment as Assessment;
            await AssessmentService.RemoveAssessment(deleteAssessment);

            MessagingCenter.Send(deleteAssessment, "DeleteAssessment");

            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
