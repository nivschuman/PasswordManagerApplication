using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PMApplication
{
    /// <summary>
    /// Interaction logic for AddPasswordPage.xaml
    /// </summary>
    public partial class AddPasswordPage : Page
    {
        public event EventHandler SubmitEvent;
        public event EventHandler CancelEvent;
        public AddPasswordPage()
        {
            InitializeComponent();
        }

        public void GeneratePassword(object sender, EventArgs e)
        {
            PasswordTextBox.Text = Password.Generate(15, 5);
        }

        public void Submit(object sender, EventArgs e)
        {
            if(SubmitEvent != null)
            {
                SubmitPasswordEventArgs sp = new SubmitPasswordEventArgs();
                sp.Source = SourceTextBox.Text;
                sp.Password = PasswordTextBox.Text;

                SubmitEvent(sender, sp);
            }
        }

        public void Cancel(object sender, EventArgs e)
        {
            if(CancelEvent != null)
            {
                CancelEvent(sender, e);
            }
        }
    }
}
