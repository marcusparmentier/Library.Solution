using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTest : IDisposable
  {

    public AuthorTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=my_library_test;";
        }
        public void Dispose()
        {
          Author.DeleteAll();
          Book.DeleteAll();
        }

        [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Author.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfNamesAreTheSame_Author()
    {
      // Arrange, Act
      Author firstAuthor = new Author("Jane Austen");
      Author secondAuthor = new Author("Jane Austen");

      // Assert
      Assert.AreEqual(firstAuthor, secondAuthor);
    }

    [TestMethod]
    public void Save_SavesToDatabase_AuthorList()
    {
      //Arrange
      Author testAuthor = new Author("Jane Austen");

      //Act
      testAuthor.Save();
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Author testAuthor = new Author("Jane Austen");

      //Act
      testAuthor.Save();
      Author savedAuthor = Author.GetAll()[0];

      int result = savedAuthor.GetId();
      int testId = testAuthor.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsAuthorInDatabase_Author()
    {
      //Arrange
      Author testAuthor = new Author("Jane Austen");
      testAuthor.Save();

      //Act
      Author foundAuthor = Author.Find(testAuthor.GetId());

      //Assert
      Assert.AreEqual(testAuthor, foundAuthor);
    }

    [TestMethod]
    public void AddBook_AddsBookToAuthor_BookList()
    {
      //Arrange
      Author testAuthor = new Author("Jane Austen");
      testAuthor.Save();

      Book testBook = new Book("Robinhood", "Fiction");
      testBook.Save();

      //Act
      testAuthor.AddBook(testBook);

      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetBooks_ReturnsAllAuthorBooks_BookList()
    {
      //Arrange
      Author testAuthor = new Author("Jane Austen");
      testAuthor.Save();

      Book testBook1 = new Book("Robinhood", "Fiction");
      testBook1.Save();

      Book testBook2 = new Book("History", "Literature");
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook1);
      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book> {testBook1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Update_UpdatesAuthorInDatabase_String()
    {
      //Arrange
      Author testAuthor = new Author("Jane Austen");
      testAuthor.Save();
      string newName = "Shakespeare";

      //Act
      testAuthor.UpdateAuthorName(newName);

      string result = Author.Find(testAuthor.GetId()).GetAuthorName();

      //Assert
      Assert.AreEqual(newName, result);
    }


    [TestMethod]
    public void Delete_DeletesAuthorAssociationsFromDatabase_AuthorList()
    {
      //Arrange
      Book testBook = new Book("History", "Literature");
      testBook.Save();

      string testName = "Shakespeare";
      Author testAuthor = new Author(testName);
      testAuthor.Save();

      //Act
      testAuthor.AddBook(testBook);
      testAuthor.Delete();

      List<Author> resultBookAuthors = testBook.GetAuthors();
      List<Author> testBookAuthors = new List<Author> {};

      //Assert
      CollectionAssert.AreEqual(testBookAuthors, resultBookAuthors);
    }
  }
}
