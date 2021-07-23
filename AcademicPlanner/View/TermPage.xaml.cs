using AcademicPlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AcademicPlanner.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        public TermPage(Term term)
        {
            InitializeComponent();
            //deleteTermButton.CommandParameter = term;
            //editTerm.CommandParameter = term;
            selectedTermName.Text = term.TermName;
            selectedTermID.Text = term.TermID.ToString();

            selectedTermStart.Text = term.TermStart.ToString();
            selectedTermEnd.Text = term.TermEnd.ToString();
        }

        async void CourseSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Course course = (Course)e.SelectedItem;
            if (course == null) return;
            await Navigation.PushAsync(new CoursePage(course));
            coursesListView.SelectedItem = null;
        }
    }
}