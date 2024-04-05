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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public event EventHandler RefreshEvent;
        public event EventHandler AddPasswordEvent;
        public event EventHandler ShowPasswordEvent;
        public event EventHandler DeletePasswordEvent;
        public event EventHandler DeleteAccountEvent;
        public UserPage(string username)
        {
            InitializeComponent();

            //hello label
            HelloLabel.Content = $"Hello {username}!";
        }

        public void DisplaySources(List<string> sources)
        {
            PasswordsDataGrid.Items.Clear();
            foreach(string source in sources)
            {
                PasswordsDataGrid.Items.Add(new PasswordItem(source, ""));
            }
        }

        public void ShowPassword(object sender, EventArgs e)
        {
            Button showButton = (Button)sender;

            PasswordItem passwordItem = (PasswordItem)showButton.DataContext;

            if(ShowPasswordEvent != null)
            {
                PasswordItemEventArgs passwordItemEV = new PasswordItemEventArgs();
                passwordItemEV.PasswordItem = passwordItem;

                ShowPasswordEvent(sender, passwordItemEV);
            }
        }

        public void DeletePassword(object sender, EventArgs e)
        {
            Button showButton = (Button)sender;

            PasswordItem passwordItem = (PasswordItem)showButton.DataContext;

            if (ShowPasswordEvent != null)
            {
                PasswordItemEventArgs passwordItemEV = new PasswordItemEventArgs();
                passwordItemEV.PasswordItem = passwordItem;

                DeletePasswordEvent(sender, passwordItemEV);
            }
        }

        public void AddPassword(object sender, EventArgs e)
        {
            if(AddPasswordEvent != null)
            {
                //TBD special event args?
                AddPasswordEvent(sender, e);
            }
        }

        public void Refresh(object sender, EventArgs e)
        {
            if(RefreshEvent != null)
            {
                //TBD special event args?
                RefreshEvent(sender, e);
            }
        }

        public void DeleteAccount(object sender, EventArgs e)
        {
            if(DeleteAccountEvent != null)
            {
                //TBD special event args?
                DeleteAccountEvent(sender, e);
            }
        }
    }
}
