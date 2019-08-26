using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using BeltExam.Models;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [Route("reset")]
        [HttpGet]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            int CurrentLogNum = HttpContext.Session.GetInt32("LogNum") ?? 0;
            HttpContext.Session.SetInt32("LogNum", 0);
            if(CurrentLogNum == 1)
            {
                HttpContext.Session.SetInt32("LogNum", CurrentLogNum);
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                User LoggedInUser = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [Route("create/user")]
        [HttpPost]
        public IActionResult Create_User(Register newUser)
        {
            int CurrentLogNum = HttpContext.Session.GetInt32("LogNum") ?? 0;
            if(CurrentLogNum == 1)
            {
                HttpContext.Session.SetInt32("LogNum", CurrentLogNum);
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                User LoggedInUser = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);
                return RedirectToAction("Dashboard");
            }
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<Register> Hasher = new PasswordHasher<Register>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    User createUser = new User()
                    {
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Email = newUser.Email,
                        Password = newUser.Password
                    };
                    dbContext.Users.Add(createUser);
                    dbContext.SaveChanges();
                    User thisUser = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                    var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                    System.Console.WriteLine("Added new user,", thisUser.FirstName);
                    LoggedinUserId = LoggedinUserId + thisUser.UserId;
                    HttpContext.Session.SetInt32("UserId", LoggedinUserId);
                    HttpContext.Session.SetInt32("LogNum", 1);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                System.Console.WriteLine("Moving to else statement, model didn't validate");
                return View("Index");
            }
        }

        [Route("login/user")]
        [HttpPost]
        public IActionResult Login(LoginUser userSubmission)
        {
            int CurrentLogNum = HttpContext.Session.GetInt32("LogNum") ?? 0;
            if(CurrentLogNum == 1)
            {
                HttpContext.Session.SetInt32("LogNum", CurrentLogNum);
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                User LoggedInUser = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);
                return RedirectToAction("Dashboard");
            }
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.E_mail);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.P_assword);
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Password");
                    return View("Index");
                }
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                LoggedinUserId = LoggedinUserId + userInDb.UserId;
                HttpContext.Session.SetInt32("UserId", LoggedinUserId);
                HttpContext.Session.SetInt32("LogNum", 1);
                System.Console.WriteLine("Logged in", userInDb);
                return RedirectToAction("Dashboard");
            }
            else
            {
                System.Console.WriteLine("Moving to else statement, model didn't validate");
                return View("Index");
            }
        }

        [Route("dashboard")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            int CurrentLogNum = HttpContext.Session.GetInt32("LogNum") ?? 0;
            if(CurrentLogNum == 1)
            {
                HttpContext.Session.SetInt32("LogNum", CurrentLogNum);
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                // User LoggedInUser = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);

                User LoggedInUser = dbContext.Users
                .Include(u => u.CreatedEvents)
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.Event)
                .FirstOrDefault(u => u.UserId == LoggedinUserId);

                List<Event> AllActivity = dbContext.Events
                .Include(u => u.Creator)
                .Include(u => u.RSVPS)
                .ToList();

                ViewBag.User = LoggedInUser;
                ViewBag.Activity = AllActivity;
                ViewBag.Cats = 1;

                return View("Dashboard", AllActivity);
                // return RedirectToAction("User_Result", LoggedInUser);
            }
            return RedirectToAction("Index");
        }

        [Route("create/activity")]
        [HttpGet]
        public IActionResult CreateActivityPage()
        {
            int CurrentLogNum = HttpContext.Session.GetInt32("LogNum") ?? 0;
            if(CurrentLogNum == 1)
            {
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                User LoggedInUser = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);
                ViewBag.User = LoggedInUser;
                return View("NewActivity");
            }
            return View("Index");
        }
        [Route("create/new_event")]
        [HttpPost]
        public IActionResult Create_Event(ActivityForm InboundEvent)
        {   
            var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            User LoggedInUser = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);
            ViewBag.User = LoggedInUser;
            InboundEvent.UserId = LoggedInUser.UserId;
            if(ModelState.IsValid)
            {
                if(InboundEvent.DateAndTime < DateTime.Now)
                {
                    ModelState.AddModelError("DateAndTime", "Please select a date and time in the future.");
                    return View("NewActivity");
                }
                    Event createEvent = new Event()
                    {
                        Name = InboundEvent.Name,
                        Duration = InboundEvent.Duration + InboundEvent.Duration2,
                        DateAndTime = InboundEvent.DateAndTime,
                        Description = InboundEvent.Description,
                        UserId = InboundEvent.UserId,
                        Creator = InboundEvent.Creator,
                    };
                    dbContext.Events.Add(createEvent);
                    dbContext.SaveChanges();
                    System.Console.WriteLine("Added new Event,", InboundEvent.Name);
                    int EventId = createEvent.EventId;
                    return RedirectToAction("ViewEvent", new{EventId = EventId} );
                // }
            }
            else
            {
                // var LoggedinUserID = HttpContext.Session.GetInt32("UserId") ?? 0;
                // User LoggedInUseR = dbContext.Users.FirstOrDefault(u => LoggedinUserId == u.UserId);
                // ViewBag.User = LoggedInUseR.UserId;
                System.Console.WriteLine("Moving to else statement, model didn't validate");
                return View("NewActivity");
            }
        }
        [Route("view_event/{EventId:int}")]
        [HttpGet]
        public IActionResult ViewEvent(int EventId)
        {
            int CurrentLogNum = HttpContext.Session.GetInt32("LogNum") ?? 0;
            if(CurrentLogNum == 1)
            {
                var LoggedinUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                Event ThisEvent = dbContext.Events
                .Include(u => u.Creator)
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.User)
                .FirstOrDefault(u => u.EventId == EventId);
                // ViewBag.User = LoggedInUser;
                return View("ViewEvent", ThisEvent);
            }
            return View("Index");
        }
        [Route("delete_event/{EventId:int}")]
        [HttpGet]
        public IActionResult DeleteEvent(int EventId)
        {
            Event thisEventId = dbContext.Events.FirstOrDefault(u => u.EventId == EventId);
            // Then pass the object we queried for to .Remove() on Users
            dbContext.Events.Remove(thisEventId);
            // Finally, .SaveChanges() will remove the corresponding row representing this User from DB 
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("leave_event/{EventId:int}/{UserId:int}")]
        [HttpGet]
        public IActionResult LeaveEvent(int EventId, int UserId)
        {
            Event ThisEvent = dbContext.Events
                .Include(u => u.Creator)
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.User)
                .FirstOrDefault(u => u.EventId == EventId);

            User LoggedInUser = dbContext.Users
                .Include(u => u.CreatedEvents)
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.Event)
                .FirstOrDefault(u => u.UserId == UserId);
            
            List<Event> AllActivity = dbContext.Events
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.User)
                .ToList();

            foreach (var Event in dbContext.RSVPS
            .Where(x => x.EventId == ThisEvent.EventId))
                {
                dbContext.RSVPS.Remove(Event);
                }
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("join/{EventId:int}/{UserId:int}")]
        [HttpGet]
        public IActionResult JoinEvent(int EventId, int UserId)
        {
            Event ThisEvent = dbContext.Events
                .Include(u => u.Creator)
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.User)
                .FirstOrDefault(u => u.EventId == EventId);

            User LoggedInUser = dbContext.Users
                .Include(u => u.CreatedEvents)
                .Include(u => u.RSVPS)
                    .ThenInclude(sub => sub.Event)
                .FirstOrDefault(u => u.UserId == UserId);

            RSVP newRSVP = new RSVP()
            {
                UserId = LoggedInUser.UserId,
                EventId = ThisEvent.EventId,
                User = LoggedInUser,
                Event = ThisEvent,
            };
            dbContext.RSVPS.Add(newRSVP);
            dbContext.SaveChanges();;
            return RedirectToAction("Index");
        }
    }
}