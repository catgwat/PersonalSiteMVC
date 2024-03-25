using Microsoft.AspNetCore.Mvc;
using PersonalSiteMVC.Models;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using MimeKit; //Added for access to MimeMessage class
using MailKit.Net.Smtp; //Added for access to SmtpClient class

namespace PersonalSiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CredentialSettings _credentials;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IOptions<CredentialSettings> settings, IConfiguration config)
        {
            _logger = logger;
            _credentials = settings.Value;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Portfolio()
        {
            return View();
        }
        public IActionResult Classmates()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            //To handle sending the email, we'll need to install a NuGet Package
            //and add a few using statements. We can do this with the following steps:

            #region Email Setup Steps & Email Info

            //1. Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution
            //2. Go to the Browse tab and search for NETCore.MailKit
            //3. Click NETCore.MailKit
            //4. On the right, check the box next to the CORE1 project, then click "Install"
            //5. Once installed, return here
            //6. Add the following using statements & comments:
            //      - using MimeKit; //Added for access to MimeMessage class
            //      - using MailKit.Net.Smtp; //Added for access to SmtpClient class
            //7. Once added, return here to continue coding email functionality

            //MIME - Multipurpose Internet Mail Extensions - Allows email to contain
            //information other than ASCII, including audio, video, images, and HTML

            //SMTP - Simple Mail Transfer Protocol - An internet protocol (similar to HTTP)
            //that specializes in the collection & transfer of email data

            #endregion

            //Create the format for the message content we will recieve from the contact form
            string message = $"You have recieved a new email from your site's contact form!<br />" +
                $"Sender: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />" +
                $"Message: {cvm.Message}";

            //Create a MimeMessage object to assist with storing/transporting the
            //email information from the contact form.
            var mm = new MimeMessage();

            //Even though the user is the one attempting to send a message to us, the actual sender
            //of the email is the email user we set up with our hosting provider.

            //We can access the credentials for this email user from our appsettings.json file as shown below:
            mm.From.Add(new MailboxAddress("Sender", _credentials.Email.Username));

            //The recipient of this email will be our personal email address, also stored in appsettings.json
            mm.To.Add(new MailboxAddress("Personal", _credentials.Email.Recipient));

            //The subject will be the one provided by the user, which we stored in our cvm object
            mm.Subject = $"New contact form message: [{cvm.Subject}]";

            //The body of the message will be formatted with the string we created above
            mm.Body = new TextPart("HTML") { Text = message };

            //We can set the priority of the message as "urgent" so it will be flagged in our email client.
            mm.Priority = MessagePriority.Urgent;

            //We can also add the user's provided email address to the list of ReplyTo addresses
            //so our replies can be sent directly to them instead of the email user on our hosting provider.
            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            //The using directive will create the SmptClient object used to send the email.
            //Once all of the code inside of the using directive's scope has been executed,
            //it will close any open connections and dispose of the object for us. 
            using (var client = new SmtpClient())
            {
                //Connect to the mail server using credentials in our appsettings.json and port 8889
                client.Connect(_credentials.Email.Server, 8889);

                //Log in to the mail server using the credentials for our email user
                client.Authenticate(_credentials.Email.Username, _credentials.Email.Password);

                //It's possible the mail server may be down when the user attempts to contact us,
                //so we can encapsulate our code to send the message in a try/catch
                try
                {
                    //Try to send the email:
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    //If there is an issue, we can store an error message in a ViewBag variable
                    //to be displayed in the View.
                    ViewBag.ErrorMessage = $"There was an error in processing your request." +
                        $"Please try again later.<br />Error Message: {ex.StackTrace}";

                    //Return the user to the View with their form information intact.
                    return View(cvm);
                }
            }

            //If all goes well, return a View that displays a confirmation to the user
            //that their email was sent.
            return View("EmailConfirmation", cvm);
        }
        public IActionResult SurviveDetails()
        {
            return View();
        }
        public IActionResult StoreFrontDetails()
        {
            return View();
        }
        public IActionResult ToDoApiDetails()
        {
            return View();
        }
        public IActionResult ReactToDoDetails()
        {
            return View();
        }
        public IActionResult SATDetails()
        {
            return View();
        }
        
    }
}
