using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlackOverload.Data;
using SlackOverload.Models;
using System.Diagnostics;
using PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;

namespace SlackOverload.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext db;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            db = dbContext;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewQuestion()
        {
            QuestionTagViewModel questionTag = new QuestionTagViewModel();
            return View(questionTag);
        }

        [HttpPost]
        public IActionResult NewQuestion(string title, string content, string? tags, string? tags2, string? tags3)
        {
            string username = User.Identity.Name;
            try
            {
                IdentityUser user = db.Users.First(u => u.Email == username);
                if (user != null)
                {
                    Question newQuestion = new Question { Title = title, Content = content, User = (ApplicationUser)user, Date = DateTime.Now, Tags = new List<Tag>() };
                    
                    if (tags != null)
                    {
                        bool tagExists = db.Tag.Any(t => t.Name == tags);
                        if (tagExists)
                        {
                            Tag tag = db.Tag.Include(t => t.Questions).First(t => t.Name == tags);
                            tag.Questions.Add(newQuestion);
                            newQuestion.Tags.Add(tag);
                        }
                        else
                        {
                            Tag newTag = new Tag { Name = tags, Questions = new List<Question>() };
                            db.Tag.Add(newTag);
                            newTag.Questions.Add(newQuestion);
                            newQuestion.Tags.Add(newTag);
                        }
                    }
                    if (tags2 != null)
                    {
                        bool tagExists = db.Tag.Any(t => t.Name == tags2);
                        if (tagExists)
                        {
                            Tag tag = db.Tag.Include(t => t.Questions).First(t => t.Name == tags2);
                            tag.Questions.Add(newQuestion);
                            newQuestion.Tags.Add(tag);
                        }
                        else
                        {
                            Tag newTag = new Tag { Name = tags2, Questions = new List<Question>() };
                            db.Tag.Add(newTag);
                            newTag.Questions.Add(newQuestion);
                            newQuestion.Tags.Add(newTag);
                        }
                    }
                    if (tags3 != null)
                    {
                        bool tagExists = db.Tag.Any(t => t.Name == tags3);
                        if (tagExists)
                        {
                            Tag tag = db.Tag.Include(t => t.Questions).First(t => t.Name == tags3);
                            tag.Questions.Add(newQuestion);
                            newQuestion.Tags.Add(tag);
                        }
                        else
                        {
                            Tag newTag = new Tag { Name = tags3, Questions = new List<Question>() };
                            db.Tag.Add(newTag);
                            newTag.Questions.Add(newQuestion);
                            newQuestion.Tags.Add(newTag);
                        }
                    }
                    db.Question.Add(newQuestion);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("MainPage");
        }

        public IActionResult AnswerQuestion(int questionId)
        {
            ViewBag.QuestionId = questionId;
            return View();
        }

        [HttpPost]
        public IActionResult AnswerQuestion(string content, int questionId)
        {
            string username = User.Identity.Name;
            try
            {
                Question questionAnswered = db.Question.First(q => q.Id == questionId);
                IdentityUser user = db.Users.First(u => u.Email == username);
                if (user != null && questionAnswered != null)
                {
                    Answer questionAnswer = new Answer { Content = content, User = (ApplicationUser)user, Question = questionAnswered };
                    db.Answer.Add(questionAnswer);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("MainPage");
        }

        public IActionResult AddComment(int id)
        {
            ViewBag.CommentId = id;
            return View();
        }

        [HttpPost]
        public IActionResult AddComment(int id, string content)
        {
            try
            {
                Question question = db.Question.First(q => q.Id == id);
                if (question != null)
                {
                    Comment comment = new Comment { Content = content, Question = question };
                    db.Comment.Add(comment);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("MainPage");
        }
        public IActionResult AddAnswerComment(int id)
        {
            ViewBag.CommentId = id;
            return View();
        }

        [HttpPost]
        public IActionResult AddAnswerComment(int id, string content)
        {
            try
            {
                Answer answer = db.Answer.First(q => q.Id == id);
                if (answer != null)
                {
                    AnswerComment comment = new AnswerComment { Content = content, Answer = answer };
                    db.AnswerComment.Add(comment);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("MainPage");
        }

        public IActionResult QuestionDetails(int id)
        {
            try
            {
                Question question = db.Question.Include(q => q.Answers).Include("Tags").Include("Answers.Comments").Include(q => q.Comments).First(q => q.Id == id);
                return View(question);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        public IActionResult Tags(int? id)
        {
            Tag tag = db.Tag.Include(t => t.Questions).Include("Questions.User").Include("Questions.Answers.Comments").First(q => q.Id == id);
            return View(tag);
        }

        public IActionResult Upvote(int id)
        {
            string username = User.Identity.Name;
            try
            {
                Question question = db.Question.Include(q => q.User).First(q => q.Id == id);
                IdentityUser currentUser = db.Users.First(u => u.Email == username);
                if (currentUser != question.User)
                {
                    question.Upvotes++;
                    question.User.Reputation += 5;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "You can't upvote your own question";
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("MainPage");
        }

        public IActionResult Downvote(int id)
        {
            string username = User.Identity.Name;
            try
            {
                Question question = db.Question.Include(q => q.User).First(q => q.Id == id);
                IdentityUser currentUser = db.Users.First(u => u.Email == username);
                if (currentUser != question.User)
                {
                    question.Downvotes++;
                    question.User.Reputation -= 5;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "You can't downvote your own question";
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("MainPage");
        }

        public IActionResult AnswerUpvote(int id)
        {
            string username = User.Identity.Name;
            try
            {
                Answer answer = db.Answer.Include(q => q.User).First(q => q.Id == id);
                IdentityUser currentUser = db.Users.First(u => u.Email == username);
                if (currentUser != answer.User)
                {
                    answer.Upvotes++;
                    answer.User.Reputation += 5;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "You can't upvote your own answer";
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("MainPage");
        }

        public IActionResult AnswerDownvote(int id)
        {
            string username = User.Identity.Name;
            try
            {
                Answer answer = db.Answer.Include(q => q.User).First(q => q.Id == id);
                IdentityUser currentUser = db.Users.First(u => u.Email == username);
                if (currentUser != answer.User)
                {
                    answer.Downvotes++;
                    answer.User.Reputation -= 5;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "You can't downvote your own answer";
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("MainPage");
        }

        [Authorize (Roles = "Admin")]
        public IActionResult SetUserAsAdmin()
        {
            SelectList users = new SelectList(db.Users, "Id", "Email");
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> SetUserAsAdmin(string? userId)
        {
            try
            {
                if(userId != null)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        bool userIsAlreadyInRole = await _userManager.IsInRoleAsync(user, "Admin");
                        if (!userIsAlreadyInRole)
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllQuestions()
        {
            List<Question> questionList = db.Question.ToList();
            return View(questionList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteQuestion(int id)
        {
            try
            {
                Question question = db.Question.Include(q => q.Comments).Include(q => q.Answers).Include("Answers.Comments").First(q => q.Id == id);
                if(question == null)
                {
                    return NotFound();
                }
                else
                {
                    db.Question.Remove(question);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("AllQuestions");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult MarkAsCorrect(int id)
        {
            Answer answer = db.Answer.Include("Question").Include("Question.User").First(a => a.Id == id);
            Question question = answer.Question;
            Question myQuestion = db.Question.Include(q => q.Answers).Include("Tags").Include("Answers.Comments").Include(q => q.Comments).First(q => q.Id == question.Id);
            ApplicationUser user = myQuestion.User;
            string username = User.Identity.Name;
            try
            {
               
                bool correctAnswerExists = myQuestion.Answers.Any(a => a.CorrectAnswer == 1);
                if (!correctAnswerExists && username == user.UserName)
                {
                    answer.CorrectAnswer += 1;
                    myQuestion.Answers.Remove(answer);
                    myQuestion.Answers.Insert(0, answer);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return View("QuestionDetails", myQuestion);
        }

        public async Task<IActionResult> MainPage(int? pageNumber)
        {
            try 
            { 
                var displayQuestion = db.Question.Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.Date);
                int pageSize = 10;
                return View(await PaginatedList<Question>.CreateAsync(displayQuestion.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public async Task<IActionResult> SortPage(int? pageNumber)
        {
            try 
            { 
                var displayQuestion = db.Question.Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.Answers.Count);
                int pageSize = 10;
                return View(await PaginatedList<Question>.CreateAsync(displayQuestion.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
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
    }
}