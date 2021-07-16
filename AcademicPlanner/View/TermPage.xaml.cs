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
            deleteTermButton.CommandParameter = term;
            selectedTermName.Text = term.TermName;
            selectedTermID.Text = term.TermID.ToString();
        }
    }
}