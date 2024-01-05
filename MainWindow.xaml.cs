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

namespace PMApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private LoginPage loginPage;
        private UserPage userPage;

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
            Navigate(userPage);
        }

        private async void CreateUser(object sender, EventArgs e)
        {
            UserEventArgs ue = (UserEventArgs)e;
            username = ue.UserName;
            publicKeyFileName = username + "PublicKey";
            privateKeyFileName = username + "PrivateKey";

            pmClient.CreateNewRSAKeys(publicKeyFileName, privateKeyFileName);

            CommunicationProtocol answer = await Task.Run(() => pmClient.CreateUser(username));
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
    }
}
