using Microsoft.Win32;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public event EventHandler LoginEvent;
        public event EventHandler CreateUserEvent;

        private string privateKeyFileName;
        private string publicKeyFileName;
        public LoginPage()
        {
            InitializeComponent();

            publicKeyChooseFileButton.Click += ChooseFileButtonClick;
            privateKeyChooseFileButton.Click += ChooseFileButtonClick;

            loginButton.Click += Login;
            newUserButton.Click += CreateUser;
        }

        private void Login(object sender, EventArgs e)
        {
            if(LoginEvent != null)
            {
                UserEventArgs userEventArgs = new UserEventArgs();
                userEventArgs.PublicKeyFileName = publicKeyFileName;
                userEventArgs.PrivateKeyFileName = privateKeyFileName;
                userEventArgs.UserName = usernameTextBox.Text;
                
                LoginEvent(sender, userEventArgs);
            }
        }

        private void CreateUser(object sender, EventArgs e)
        {
            if(CreateUserEvent != null)
            {
                UserEventArgs userEventArgs = new UserEventArgs();
                userEventArgs.PublicKeyFileName = publicKeyFileName;
                userEventArgs.PrivateKeyFileName = privateKeyFileName;
                userEventArgs.UserName = usernameTextBox.Text;

                CreateUserEvent(sender, userEventArgs);
            }
        }

        private void ChooseFileButtonClick(object sender, EventArgs e)
        {
            bool publicKey = sender == publicKeyChooseFileButton;
            string fileName = ChooseFile();

            publicKeyFileName = publicKey ? fileName : publicKeyFileName;
            privateKeyFileName = !publicKey ? fileName : privateKeyFileName;

            publicKeyFileLabel.Content = publicKeyFileName;
            publicKeyFileLabel.ToolTip = publicKeyFileName;

            privateKeyFileLabel.Content = privateKeyFileName;
            privateKeyFileLabel.ToolTip = privateKeyFileName;
        }

        private string ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}
