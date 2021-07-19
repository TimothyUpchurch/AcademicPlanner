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
    public partial class AddCoursePage : ContentPage
    {
        public AddCoursePage(int id)
        {
            InitializeComponent();
            selectedTermID.Text = id.ToString();
        }
    }
}