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
using PasswordManagerClientDLL;
using System.Text.Json;

namespace PMApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private LoginPage loginPage;
        private UserPage userPage;
        private AddPasswordPage addPasswordPage;

        private string username;
        private string publicKeyFileName;
        private string privateKeyFileName;
        private string session;

        private System.Net.IPAddress serverIP;
        private int serverPort;
        private PasswordManagerClient pmClient;
        public MainWindow()
        {
            serverIP = System.Net.IPAddress.Parse("127.0.0.1");
            serverPort = 8080;

            pmClient = new PasswordManagerClient(serverIP, serverPort);

            loginPage = new LoginPage();
            loginPage.LoginEvent += Login;
            loginPage.CreateUserEvent += CreateUser;

            Navigate(loginPage);
        }

        private async void Login(object sender, EventArgs e)
        {
            UserEventArgs ue = (UserEventArgs)e;
            username = ue.UserName;
            publicKeyFileName = ue.PublicKeyFileName;
            privateKeyFileName = ue.PrivateKeyFileName;

            pmClient.ImportRSAKeys(publicKeyFileName, privateKeyFileName);

            string answerStr = await LoginToUser();

            if(answerStr != "Succeeded")
            {
                MessageBox.Show(answerStr);
                return;
            }

            userPage = new UserPage(username);
            userPage.RefreshEvent += RefreshUserPage;
            userPage.AddPasswordEvent += AddPassword;
            userPage.ShowPasswordEvent += ShowPassword;
            userPage.DeletePasswordEvent += DeletePasswordAction;
            Navigate(userPage);

            RefreshUserPage(null, null);
        }

        private void AddPassword(object sender, EventArgs e)
        {
            if(addPasswordPage == null)
            {
                addPasswordPage = new AddPasswordPage();
                addPasswordPage.SubmitEvent += SubmitPassword;
                addPasswordPage.CancelEvent += CancelSubmitPassword;
            }

            Navigate(addPasswordPage);
        }

        private void CancelSubmitPassword(object sender, EventArgs e)
        {
            Navigate(userPage);
        }

        private async void DeletePasswordAction(object sender, EventArgs e)
        {
            PasswordItemEventArgs passwordItemEV = (PasswordItemEventArgs)e;

            //get passwordItem and delete password with source
            PasswordItem passwordItem = passwordItemEV.PasswordItem;
            string result = await DeletePassword(passwordItem.Source);

            if(result != "Success")
            {
                MessageBox.Show($"Failed to delete password:\n{result}");
                return;
            }

            MessageBox.Show("Password was deleted successfully");
            userPage.PasswordsDataGrid.Items.Remove(passwordItem);
        }

        private async void ShowPassword(object sender, EventArgs e)
        {
            PasswordItemEventArgs passwordItemEV = (PasswordItemEventArgs)e;

            //place password inside password item
            PasswordItem passwordItem = passwordItemEV.PasswordItem;
            passwordItem.Password = await GetPassword(passwordItem.Source);

            //remove previous item and insert updated item to reflect change
            int idx = userPage.PasswordsDataGrid.Items.IndexOf(passwordItem);
            userPage.PasswordsDataGrid.Items.Remove(passwordItem);
            userPage.PasswordsDataGrid.Items.Insert(idx, passwordItem);
        }

        private async void SubmitPassword(object sender, EventArgs e)
        {
            SubmitPasswordEventArgs sp = (SubmitPasswordEventArgs)e;

            string result = await SetPassword(sp.Source, sp.Password);

            //prompt user on result
            if(result != "Success")
            {
                MessageBox.Show($"Failed to add password:\n{result}");
            }
            else
            {
                MessageBox.Show("Password was added successfully");
            }

            //return user to user page and refresh it
            Navigate(userPage);

            RefreshUserPage(null, null);
        }

        private async void RefreshUserPage(object sender, EventArgs e)
        {
            if(userPage == null)
            {
                return;
            }

            List<string> sources = await GetSources();

            if (sources != null)
            {
                userPage.DisplaySources(sources);
            }
        }

        private async void CreateUser(object sender, EventArgs e)
        {
            UserEventArgs ue = (UserEventArgs)e;
            username = ue.UserName;
            publicKeyFileName = username + "PublicKey";
            privateKeyFileName = username + "PrivateKey";

            pmClient.CreateNewRSAKeys(publicKeyFileName, privateKeyFileName);

            CommunicationProtocol answer = await Task.Run(() => pmClient.CreateUser(username));

            ue.PublicKeyFileName = publicKeyFileName;
            ue.PrivateKeyFileName = privateKeyFileName;

            Login(sender, ue);
        }

        private async Task<string> DeletePassword(string source)
        {
            CommunicationProtocol answer = await Task.Run(() => pmClient.DeletePassword(source, session));

            string answerStr = Encoding.ASCII.GetString(answer.Body);

            return answerStr;
        }

        private async Task<string> GetPassword(string source)
        {
            CommunicationProtocol answer = await Task.Run(() => pmClient.GetPassword(source, session));
            string password = pmClient.DecryptPassword(answer.Body);

            return password;
        }

        private async Task<string> SetPassword(string source, string password)
        {
            CommunicationProtocol answer = await Task.Run(() => pmClient.SetPassword(source, password, session));

            string answerStr = Encoding.ASCII.GetString(answer.Body);

            return answerStr;
        }

        private async Task<string> LoginToUser()
        {
            CommunicationProtocol answer = await Task.Run(() => pmClient.LoginRequest(username));

            if (answer.Body.Length == 0)
            {
                return "Failed to request login";
            }

            session = answer.GetHeaderValue("Session");
            answer = pmClient.LoginTest(answer.Body, session);
            string answerStr = Encoding.ASCII.GetString(answer.Body);

            return answerStr;
        }

        private async Task<List<string>> GetSources()
        {
            CommunicationProtocol answer = await Task.Run(() => pmClient.GetSources(session));

            if(answer.Body.Length == 0)
            {
                return null;
            }

            string sourcesJson = Encoding.ASCII.GetString(answer.Body);
            List<string> sources = JsonSerializer.Deserialize<List<string>>(sourcesJson);

            return sources;
        }
    }
}
