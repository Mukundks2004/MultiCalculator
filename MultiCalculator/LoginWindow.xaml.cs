using Microsoft.Extensions.Hosting;
using MultiCalculator.Database;
using System.Windows;
using MultiCalculator.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MultiCalculator.Database.Services;
using System.CodeDom;
using MultiCalculator.Database.Models;

namespace MultiCalculator
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IDatabaseService databaseService;
        private string failureMessage = string.Empty;
        private readonly MainWindow mainWindow;

        public LoginWindow()
        {
            InitializeComponent();

            var builder = Host.CreateApplicationBuilder();
            builder.Services.AddDependencyGroup();

            using (var context = new CalculatorDbContext())
            {
                context.Database.EnsureCreated();
            }
            builder.Services.AddTransient<MainWindow>();

            var built = builder.Build();

            databaseService = built.Services.GetRequiredService<IDatabaseService>();
            mainWindow = built.Services.GetRequiredService<MainWindow>();
            //databaseService.ClearData();
            //databaseService.SeedData(); // (Uncomment if you need data seeded else please leave, so we build a larger database of questions).
        }

        void VerifyAndOpenWindow_Click(object sender, RoutedEventArgs e)
        {
            UserModel user;
            if (VerifyIdAndPassword(out user))
            {
                mainWindow.loggedUser = user;
                mainWindow.Show();
                this.Close();
            }
            else
            {
                failureMessageLabel.Content = "Incorrect ID or Password.\n" + failureMessage;
            }
        }

        bool VerifyIdAndPassword(out UserModel user)
        {
            var passedId = 0;
            try
            {
                passedId = Int32.Parse(idTextBlock.Text);
            }
            catch
            {
                failureMessage = "The ID must only be integers.";
                user = null;
                return false;
            }
            user = databaseService.LoadUserById(passedId);
            if (user != null)
            {
                return user.Password == passwordTextBlock.Password ? true : false;
            }
            return false;
        }

        void TempByPassButton_Click(object sender, RoutedEventArgs e) // Temp remove later so we dont waste time
        {
            var user = databaseService.LoadUserById(11111);
            mainWindow.loggedUser = user;
            mainWindow.Show();
            this.Close();
        }
    }
}
