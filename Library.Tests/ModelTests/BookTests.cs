using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class BookTests : IDisposable
  {
        public BookTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=my_library_test;";
        }

       [TestMethod]
       public void GetAll_BooksEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Book.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

      [TestMethod]
      public void Equals_ReturnsTrueForSameTitleAndGenre_Book()
      {
        //Arrange, Act
        Book firstBook = new Book("Robinhood","Fiction");
        Book secondBook = new Book("Robinhood","Fiction");

        //Assert
        Assert.AreEqual(firstBook, secondBook);
      }

      [TestMethod]
      public void Save_SavesBookToDatabase_BookList()
      {
        //Arrange
        Book testBook = new Book("Robinhood","Fiction");
        testBook.Save();

        //Act
        List<Book> result = Book.GetAll();
        List<Book> testList = new List<Book>{testBook};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }


     [TestMethod]
     public void Save_DatabaseAssignsIdToBook_Id()
     {
       //Arrange
       Book testBook = new Book("Robinhood","Fiction");
       testBook.Save();

       //Act
       Book savedBook = Book.GetAll()[0];

       int result = savedBook.GetId();
       int testId = testBook.GetId();

       //Assert
       Assert.AreEqual(testId, result);
    }


    [TestMethod]
    public void Find_FindsBookInDatabase_Book()
    {
      //Arrange
      Book testBook = new Book("Robinhood","Fiction");
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      Assert.AreEqual(testBook, foundBook);
    }

    [TestMethod]
    public void Delete_DeletesBookAssociationsFromDatabase_BookList()
    {
      //Arrange
      Author testAuthor = new Author("Shakespeare");
      testAuthor.Save();

      Book testBook = new Book("Robinhood", "Fiction");
      testBook.Save();

      //Act
      testBook.AddAuthor(testAuthor);
      testBook.DeleteBook();

      List<Book> resultAuthorBooks = testAuthor.GetBooks();
      List<Book> testAuthorBooks = new List<Book> {};

      //Assert
      CollectionAssert.AreEqual(testAuthorBooks, resultAuthorBooks);
    }

    [TestMethod]
    public void Test_AddAuthor_AddsAuthorToBook()
    {
      //Arrange
      Book testBook = new Book("Robinhood", "Fiction");
      testBook.Save();

      Author testAuthor = new Author("Shakespeare");
      testAuthor.Save();

      Author testAuthor2 = new Author("Jane Austen");
      testAuthor2.Save();

      //Act
      testBook.AddAuthor(testAuthor);
      testBook.AddAuthor(testAuthor2);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{testAuthor, testAuthor2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAuthors_ReturnsAllBookAuthors_AuthorList()
    {
      //Arrange
      Book testBook = new Book("Robinhood", "Fiction");
      testBook.Save();

      Author testAuthor1 = new Author("Shakespeare");
      testAuthor1.Save();

      Author testAuthor2 = new Author("Jane Austen");
      testAuthor2.Save();

      //Act
      testBook.AddAuthor(testAuthor1);
      List<Author> savedAuthors = testBook.GetAuthors();
      List<Author> testList = new List<Author> {testAuthor1};

      //Assert
      CollectionAssert.AreEqual(testList, savedAuthors);
    }


    [TestMethod]
    public void Update_UpdatesBookInDatabase_String()
    {
      //Arrange

      Book testBook = new Book("Robinhood", "Fiction");
      testBook.Save();

      //Act
      testBook.UpdateBook("History", "Comedy");
      Book result = Book.Find(testBook.GetId());

      //Assert
      Assert.AreEqual(testBook, result);
    }

    [TestMethod]
    public void DeleteBook_DeleteBookInDatabase_Null()
    {
      //Arrange
      string title = "Robinhood";
      Book testBook = new Book(title, "Fiction");
      testBook.Save();
      // string deletedBook = "";

      //Act
      testBook.DeleteBook();
      int result = Book.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
  }


    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
