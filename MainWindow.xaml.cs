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
using System.Security.Cryptography;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;

namespace PMApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            //server connection
            serverIP = System.Net.IPAddress.Parse("127.0.0.1");
            serverPort = 8080;

            pmClient = new PasswordManagerClient(serverIP, serverPort);

            //initialize logger
            InitializeLog();

            //login page
            loginPage = new LoginPage();
            loginPage.LoginEvent += Login;
            loginPage.CreateUserEvent += CreateUser;

            logger.Info("Started main window with new login page");

            Navigate(loginPage);
        }

        public static void InitializeLog()
        {
            //create logs directory
            string logsDirectory = "logs";

            if(!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            //get current date
            DateTime now = DateTime.Now;
            string nowDate = now.ToString("dd_MM_yyyy");
            string fileName = $"{nowDate}_logfile.log";

            //logger configuration - file target
            LoggingConfiguration config = new LoggingConfiguration();

            FileTarget fileTarget = new FileTarget("file")
            {
                Name = "LogFile",
                FileName = System.IO.Path.Combine(logsDirectory, fileName),
                Layout = "${longdate}|${level:uppercase=true}|${callsite}|${message}"
            };

            config.AddTarget(fileTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);

            //logger configuration - colored console target
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget()
            {
                Name = "ColoredConsole",
                Layout = "${date:format=HH\\:mm\\:ss} ${level:uppercase=true} ${message}"
            };

            config.AddTarget(consoleTarget);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);

            LogManager.Configuration = config;
        }

        private async void Login(object sender, EventArgs e)
        {
            UserEventArgs ue = (UserEventArgs)e;
            username = ue.UserName;
            publicKeyFileName = ue.PublicKeyFileName;
            privateKeyFileName = ue.PrivateKeyFileName;

            logger.Info("Entered login with username: {username}, public key: {publicKey}, private key: {privateKey}", username, publicKeyFileName, privateKeyFileName);

            pmClient.ImportRSAKeys(publicKeyFileName, privateKeyFileName);

            string answerStr = "";

            try
            {
                answerStr = await LoginToUser();
            }
            catch(PMClientException pme)
            {
                if(pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if(pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if(pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }
            catch(CryptographicException ce)
            {
                MessageBox.Show("Decryption failed during login, try using different keys", "Error");

                logger.Error("Decryption failed: {@CryptographicException}", ce);

                return;
            }
            

            if(answerStr != "Success")
            {
                MessageBox.Show(answerStr, "Error");

                logger.Warn("Failed to login to user {username}: {answerStr}", username, answerStr);

                return;
            }

            userPage = new UserPage(username);
            userPage.RefreshEvent += RefreshUserPage;
            userPage.AddPasswordEvent += AddPassword;
            userPage.ShowPasswordEvent += ShowPassword;
            userPage.DeletePasswordEvent += DeletePasswordAction;
            userPage.DeleteAccountEvent += DeleteAccountAction;
            Navigate(userPage);

            logger.Info("Created and navigated to new user page for {username}", username);

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

            logger.Info("Created and navigated to new add password page");
        }

        private void CancelSubmitPassword(object sender, EventArgs e)
        {
            Navigate(userPage);

            logger.Info("Canceled password submission, navigated to userPage");
        }

        private async void DeleteAccountAction(object sender, EventArgs e)
        {
            MessageBoxResult confirmBox = MessageBox.Show("Are you sure that you want to delete your account?", "Delete Confirmation", MessageBoxButton.YesNo);

            if(confirmBox == MessageBoxResult.No)
            {
                logger.Info("Canceled account delete action, returning");
                return;
            }

            string deleteResult = "";

            try
            {
                deleteResult = await DeleteUser();
            }
            catch (PMClientException pme)
            {
                if (pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }

            if (deleteResult != "Success")
            {
                MessageBox.Show($"Failed to delete user:\n{deleteResult}", "Error");

                logger.Warn("Failed to delete user {username}: {deleteResult}", username, deleteResult);

                return;
            }

            logger.Info("User {username} deleted successfully, returning to login page", username);

            //reset back to new login page
            addPasswordPage = null;
            userPage = null;
            username = "";
            publicKeyFileName = "";
            privateKeyFileName = "";
            session = "";

            loginPage = new LoginPage();
            loginPage.LoginEvent += Login;
            loginPage.CreateUserEvent += CreateUser;

            Navigate(loginPage);
        }

        private async void DeletePasswordAction(object sender, EventArgs e)
        {
            PasswordItemEventArgs passwordItemEV = (PasswordItemEventArgs)e;

            //get passwordItem and delete password with source
            PasswordItem passwordItem = passwordItemEV.PasswordItem;
            
            MessageBoxResult confirmBox = MessageBox.Show("Are you sure that you want to delete this password?", "Delete Confirmation", MessageBoxButton.YesNo);

            if (confirmBox == MessageBoxResult.No)
            {
                logger.Info("Canceled on deleting password with source={source}, returning", passwordItem.Source);

                return;
            }

            string result = "";

            try
            {
                result = await DeletePassword(passwordItem.Source);
            }
            catch (PMClientException pme)
            {
                if (pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }

            if (result != "Success")
            {
                MessageBox.Show($"Failed to delete password:\n{result}", "Error");

                logger.Warn("Failed to delete password with source={source}: {result}", passwordItem.Source, result);

                return;
            }

            logger.Info("Password with source={source} was deleted successfully", passwordItem.Source);

            MessageBox.Show("Password was deleted successfully", "Success");
            userPage.PasswordsDataGrid.Items.Remove(passwordItem);
        }

        private async void ShowPassword(object sender, EventArgs e)
        {
            PasswordItemEventArgs passwordItemEV = (PasswordItemEventArgs)e;

            //place password inside password item
            PasswordItem passwordItem = passwordItemEV.PasswordItem;

            try
            {
                passwordItem.Password = await GetPassword(passwordItem.Source);
            }
            catch (PMClientException pme)
            {
                if (pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }

            logger.Info("Got password for {source} from server", passwordItem.Source);

            //remove previous item and insert updated item to reflect change
            int idx = userPage.PasswordsDataGrid.Items.IndexOf(passwordItem);
            userPage.PasswordsDataGrid.Items.Remove(passwordItem);
            userPage.PasswordsDataGrid.Items.Insert(idx, passwordItem);
        }

        private async void SubmitPassword(object sender, EventArgs e)
        {
            SubmitPasswordEventArgs sp = (SubmitPasswordEventArgs)e;

            string result = "";

            try
            {
                result = await SetPassword(sp.Source, sp.Password);
            }
            catch (PMClientException pme)
            {
                if (pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }

            //prompt user on result
            if (result != "Success")
            {
                MessageBox.Show($"Failed to add password:\n{result}", "Error");

                logger.Warn("Failed to add password for source={source}: {result}", sp.Source, result);
            }
            else
            {
                MessageBox.Show("Password was added successfully", "Success");

                addPasswordPage.SourceTextBox.Text = "";
                addPasswordPage.PasswordTextBox.Text = "";

                logger.Info("Set password successfully for source={source}", sp.Source);
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

            List<string> sources = null;

            try
            {
                sources = await GetSources();
            }
            catch (PMClientException pme)
            {
                if (pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }

            if (sources != null)
            {
                logger.Info("Got sources for user {username}, displaying them", username);

                userPage.DisplaySources(sources);
            }
            else
            {
                MessageBox.Show("Failed to get sources from server", "Error");

                logger.Warn("Failed to get sources for user {username}", username);
            }
        }

        private async void CreateUser(object sender, EventArgs e)
        {
            UserEventArgs ue = (UserEventArgs)e;
            username = ue.UserName;
            publicKeyFileName = username + "PublicKey";
            privateKeyFileName = username + "PrivateKey";

            pmClient.CreateNewRSAKeys(publicKeyFileName, privateKeyFileName);

            CommunicationProtocol answer = null;
            try
            {
                answer = await Task.Run(() => pmClient.CreateUser(username));
            }
            catch (PMClientException pme)
            {
                if (pme.Reason == PMErrorReason.ConnectionRefused)
                {
                    MessageBox.Show("Failed to connect to server, it may be offline", "Error");

                    logger.Error("Connection to server refused: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.ConnectionTimeouted)
                {
                    MessageBox.Show("Server took too long to respond, it may be offline", "Error");

                    logger.Error("Connection to server timeouted: {@SocketException}", pme.SE);

                    return;
                }
                else if (pme.Reason == PMErrorReason.Unknown)
                {
                    MessageBox.Show("Server connection failed for unkown reason", "Error");

                    logger.Error("Server connection failed: {@SocketException}", pme.SE);

                    return;
                }
            }

            string result = Encoding.ASCII.GetString(answer.Body);

            if(result != "Success")
            {
                MessageBox.Show($"Failed to create user:\n{result}");

                logger.Warn("Failed to create user {username} with public key: {publicKey}, private key: {privateKey} - {result}", username, publicKeyFileName, privateKeyFileName, result);

                return;
            }

            logger.Info("successfully created user {username} with public key: {publicKey}, private key: {privateKey}", username, publicKeyFileName, privateKeyFileName);

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

            logger.Debug("Got password, decrypting it");
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

        private async Task<string> DeleteUser()
        {
            CommunicationProtocol answer = await Task.Run(() => pmClient.DeleteUser(session));

            string answerStr = Encoding.ASCII.GetString(answer.Body);

            return answerStr;
        }
    }
}
