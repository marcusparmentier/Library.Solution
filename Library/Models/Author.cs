using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Author
  {
    private int _id;
    private string _authorName;


    public Author(string authorName, int Id = 0)
    {
      _id = Id;
      _authorName = authorName;
    }

    public override bool Equals(System.Object otherAuthor)
    {
    if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = (this.GetId() == newAuthor.GetId());
        bool authorNameEquality = (this.GetAuthorName() == newAuthor.GetAuthorName());
        return (idEquality && authorNameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetAuthorName().GetHashCode();
    }

    public string GetAuthorName()
    {
      return _authorName;
    }

    public int GetId()
    {
      return _id;
    }

    public void UpdateAuthorName(string newAuthorName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE authors SET author_name = @newAuthorName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter authorName = new MySqlParameter();
      authorName.ParameterName = "@newAuthorName";
      authorName.Value = newAuthorName;
      cmd.Parameters.Add(authorName);

      cmd.ExecuteNonQuery();
      _authorName = newAuthorName;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }



    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors (author_name) VALUES (@authorName);";

      MySqlParameter authorName = new MySqlParameter();
      authorName.ParameterName = "@authorName";
      authorName.Value = this._authorName;
      cmd.Parameters.Add(authorName);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorAuthorName = rdr.GetString(1);
        Author newAuthor = new Author(authorAuthorName, authorId);
        allAuthors.Add(newAuthor);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allAuthors;
    }

    public static Author Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `authors` WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int authorId = 0;
      string authorName = "";

      while (rdr.Read())
      {
        authorId = rdr.GetInt32(0);
        authorName = rdr.GetString(1);
      }

      Author newAuthor= new Author(authorName, authorId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newAuthor;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddBook(Book newBook)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books_authors (book_id, author_id) VALUES (@BookId, @AuthorId);";

      MySqlParameter book_id = new MySqlParameter();
      book_id.ParameterName = "@BookId";
      book_id.Value = newBook.GetId();
      cmd.Parameters.Add(book_id);

      MySqlParameter author_id = new MySqlParameter();
      author_id.ParameterName = "@AuthorId";
      author_id.Value = _id;
      cmd.Parameters.Add(author_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Book> GetBooks()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT book_id FROM books_authors WHERE author_id = @authorId;";

      MySqlParameter authorIdParameter = new MySqlParameter();
      authorIdParameter.ParameterName = "@authorId";
      authorIdParameter.Value = _id;
      cmd.Parameters.Add(authorIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> bookIds = new List<int> {};
      while(rdr.Read())
      {
          int bookId = rdr.GetInt32(0);
          bookIds.Add(bookId);
      }
      rdr.Dispose();

      List<Book> books = new List<Book> {};
      foreach (int bookId in bookIds)
      {
          var bookQuery = conn.CreateCommand() as MySqlCommand;
          bookQuery.CommandText = @"SELECT * FROM books WHERE id = @BookId;";

          MySqlParameter bookIdParameter = new MySqlParameter();
          bookIdParameter.ParameterName = "@BookId";
          bookIdParameter.Value = bookId;
          bookQuery.Parameters.Add(bookIdParameter);

          var bookQueryRdr = bookQuery.ExecuteReader() as MySqlDataReader;
          while(bookQueryRdr.Read())
          {
              int thisBookId = bookQueryRdr.GetInt32(0);
              string bookTitle = bookQueryRdr.GetString(1);
              string bookGenre = bookQueryRdr.GetString(2);
              Book foundBook = new Book(bookTitle, bookGenre, thisBookId);
              books.Add(foundBook);
          }
          bookQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return books;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors WHERE id = @AuthorId; DELETE FROM books_authors WHERE author_id = @AuthorId;";

      MySqlParameter authorIdParameter = new MySqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();
      cmd.Parameters.Add(authorIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
