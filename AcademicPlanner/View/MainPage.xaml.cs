using AcademicPlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AcademicPlanner.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            OnAppearing();
        }
        //async void TermSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    Term term = (Term)e.SelectedItem;
        //    if (term == null) return;
        //    await Navigation.PushAsync(new TermPage(term));
        //    termsListView.SelectedItem = null;
        //}
        protected override void OnAppearing()
        {
            termsListView.SelectedItem = null;
        }
    }
}