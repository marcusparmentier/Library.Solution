
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("/authors")]
        public ActionResult Authors()
        {
            List<Author> allAuthors = Author.GetAll();
            return View(allAuthors);
        }
        [HttpGet("/books")]
        public ActionResult Books()
        {
            List<Book> allBooks = Book.GetAll();
            return View(allBooks);
        }

        //NEW Author
        [HttpGet("/authors/new")]
        public ActionResult AuthorForm()
        {
            return View();
        }
        [HttpPost("/authors/new")]
        public ActionResult AuthorCreate()
        {
            Author newAuthor = new Author(Request.Form["author-name"]);
            newAuthor.Save();
            return View("Success");
        }

        //NEW Book
        [HttpGet("/books/new")]
        public ActionResult BookForm()
        {
            return View();
        }
        [HttpPost("/books/new")]
        public ActionResult BookCreate()
        {
            Book newBook = new Book(Request.Form["book-title"], Request.Form["book-genre"]);
            newBook.Save();
            return View("Success");
        }

        //ONE Author
        [HttpGet("/authors/{id}")]
        public ActionResult AuthorDetail(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Author selectedAuthor = Author.Find(id);
            List<Book> AuthorBooks = selectedAuthor.GetBooks();
            List<Book> AllBooks = Book.GetAll();
            model.Add("author", selectedAuthor);
            model.Add("authorBooks", AuthorBooks);
            model.Add("allBooks", AllBooks);
            return View("AuthorDetail",model);

        }

        //ONE Book
        [HttpGet("/books/{id}")]
        public ActionResult BookDetail(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Book SelectedBook = Book.Find(id);
            List<Author> BookAuthors = SelectedBook.GetAuthors();
            List<Author> AllAuthors = Author.GetAll();
            model.Add("book", SelectedBook);
            model.Add("bookAuthors", BookAuthors);
            model.Add("allAuthors", AllAuthors);
            return View(model);
        }

        //ADD Author TO CATEGORY
        [HttpPost("books/{bookId}/authors/new")]
        public ActionResult BookAddAuthor(int bookId)
        {
            Book book = Book.Find(bookId);
            Author author = Author.Find(Int32.Parse(Request.Form["author-id"]));
            book.AddAuthor(author);
            return View("Success");
        }
        //ADD Book TO Author
        [HttpPost("authors/{authorId}/books/new")]
        public ActionResult AuthorAddBook(int authorId)
        {
            Author author = Author.Find(authorId);
            Book book = Book.Find(Int32.Parse(Request.Form["book-id"]));
            author.AddBook(book);
            return View("Success");
        }

  //   [HttpGet("/")]
  //   public ActionResult Index()
  //   {
  //     return View();
  //   }
  //
  //   [HttpGet("/books/form")]
  //   public ActionResult BookForm()
  //   {
  //     return View();
  //   }
  //
  //   [HttpPost("/categories")]
  //   public ActionResult AddCategory()
  //   {
  //     Category newCategory = new Category(Request.Form["category-name"]);
  //     newCategory.Save();
  //     List<Category> allBooks = Category.GetAll();
  //     return View("Categories", allCategories);
  //   }
  //
  //   [HttpGet("/categories")]
  //   public ActionResult Categories()
  //   {
  //     List<Category> allCategories = Category.GetAll();
  //     return View(allCategories);
  //   }
  //
  //   [HttpGet("/categories/{id}")]
  //   public ActionResult CategoryDetail(int id)
  //   //id = 1
  //   {
  //     Dictionary<string, object> model = new Dictionary<string, object>();
  //     Category selectedCategory = Category.Find(id);
  //     List<Author> categoryAuthors = selectedCategory.GetAuthors();
  //     model.Add("category", selectedCategory);
  //     model.Add("authors", categoryTasks);
  //     return View(model);
  //   }
  //
  //   [HttpGet("/categories/{id}/authors/new")]
  //   public ActionResult CategoryTaskForm(int id)
  //   {
  //
  //     Category selectedCategory = Category.Find(id);
  //
  //      return View(selectedCategory);
  //   }
  //
  //   [HttpPost("/categories/{id}/authorlist")]
  //   public ActionResult AddedTask(int id)
  //   {
  //
  //     Task newTask = new Task(Request.Form["task-description"],(Request.Form["dueDate"]), id);
  //     newTask.Save();
  //     Dictionary<string, object> model = new Dictionary<string, object>();
  //     Category selectedCategory = Category.Find(id);
  //     List<Task> categoryTasks = selectedCategory.GetTasks();
  //     model.Add("tasks", categoryTasks);
  //     model.Add("category", selectedCategory);
  //     return View("CategoryDetail", model);
  //   }
  //
  //   [HttpGet("/categories/{id}/tasklist")]
  //   public ActionResult ViewTaskList(int id)
  //   {
  //     Dictionary<string, object> model = new Dictionary<string, object>();
  //     Category selectedCategory = Category.Find(id); //Category is selected as an object
  //     List<Task> categoryTasks = selectedCategory.GetTasks(); //Tasks are displayed in a list
  //
  //     model.Add("category", selectedCategory);
  //     model.Add("tasks", categoryTasks);
  //
  //     //return the task list for selected category
  //     return View("CategoryDetail", model);
  //   }
  //
  //   [HttpGet("/tasks/{id}/edit")]
  //   public ActionResult TaskEdit(int id)
  //   {
  //     Task thisTask = Task.Find(id);
  //     return View(thisTask);
  //   }
  //
  //   [HttpPost("/tasks/{id}/edit")]
  //   public ActionResult TaskEditConfirm(int id)
  //   {
  //     Task thisTask = Task.Find(id);
  //     thisTask.UpdateDescription(Request.Form["new-name"]);
  //     return RedirectToAction("Index");
  //   }
  //
  //   [HttpGet("/tasks/{id}/delete")]
  //   public ActionResult TaskDeleteConfirm(int id)
  //   {
  //     Task thisTask = Task.Find(id);
  //     thisTask.DeleteTask();
  //     return RedirectToAction("Index");
  //   }
  // }
}

}
