
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

    [HttpGet("/librarian")]
    public ActionResult LibrarianDirectory()
    {
      return View();
    }

    [HttpGet("/patrons")]
    public ActionResult PatronHomepage()
    {
        return View();
    }

    [HttpGet("/books")]
    public ActionResult Books()
    {
        List<Book> allBooks = Book.GetAll();
        return View(allBooks);
    }

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

    [HttpGet("/books/search")]
    public ActionResult SearchBookForm()
    {
      List<Author> AllAuthors = Author.GetAll();
      return View(AllAuthors);
    }
//
    [HttpPost('/books/authors/list')]
    public ActionResult SearchBookFormSubmit()
    {


    }

    [HttpGet("/books/list")]
    public ActionResult SearchedBookList()
    {
      Author selectedAuthor = Author.Find(id);
      List<Book> AuthorBooks = selectedAuthor.GetBooks();
      return View(AuthorBooks);
    }
//


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

    //ADD Author TO Book
    [HttpPost("books/{bookId}/authors/new")]
    public ActionResult BookAddAuthor(int bookId)
    {
        Book book = Book.Find(bookId);
        Author author = Author.Find(Int32.Parse(Request.Form["author-id"]));

        book.AddAuthor(author);
        return View("Success");
    }

    [HttpGet("/authors")]
    public ActionResult Authors()
    {
        List<Author> allAuthors = Author.GetAll();
        return View(allAuthors);
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
        return View(model);
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













}

}
