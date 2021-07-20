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
    public partial class EditTermPage : ContentPage
    {
        public EditTermPage(Term term)
        {
            InitializeComponent();
            // set fields.
            termID.Text = term.TermID.ToString();
            termName.Text = term.TermName;
            startDate.Date = term.TermStart;
            endDate.Date = term.TermEnd;
        }
    }
}