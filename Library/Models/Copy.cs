using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
    public class Copy
    {
        private int _numofCopies;
        private int _bookId;
        private int _id;

        public Copy(int numofCopies, int bookId, int id=0)
        {
            _numofCopies = numofCopies;
            _bookId = bookId;
            _id = id;
        }

    public override bool Equals(System.Object otherCopy)
    {
      if (!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool idEquality = (this.GetId() == newCopy.GetId());
        bool bookIdEquality = (this.GetBookId() == newCopy.GetBookId());
        bool numofCopiesEquality = (this.GetNumofCopies() == newCopy.GetNumofCopies());
        return (idEquality && bookIdEquality && numofCopiesEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
    public int GetBookId()
    {
      return _bookId;
    }
    public int GetNumofCopies()
    {
      return _numofCopies;
    }
    public int GetId()
    {
      return _id;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO copies (numof_copies, book_id) VALUES (@numofCopies, @bookId);";

      MySqlParameter numofCopies = new MySqlParameter();
      numofCopies.ParameterName = "@numofCopies";
      numofCopies.Value = this._numofCopies;
      cmd.Parameters.Add(numofCopies);

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = this._bookId;
      cmd.Parameters.Add(bookId);


      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
    public static List<Copy> GetAll()
    {
      List<Copy> allCopies = new List<Copy> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int CopyId = rdr.GetInt32(0);
        int CopyNumofcopies = rdr.GetInt32(1);
        int CopyBookId = rdr.GetInt32(2);
        Copy newCopy = new Copy(CopyNumofcopies, CopyBookId, CopyId);
        allCopies.Add(newCopy);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCopies;
    }

    public static Copy Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CopyId = 0;
      int CopyNumofcopies = 0;
      int CopyBookId = 0;

      while(rdr.Read())
      {
        CopyId = rdr.GetInt32(0);
        CopyNumofcopies = rdr.GetInt32(1);
        CopyBookId = rdr.GetInt32(2);
      }
      Copy newCopy = new Copy(CopyNumofcopies, CopyBookId, CopyId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCopy;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM copies;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteCopy()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM copies WHERE id = @thisId;";

      MySqlParameter thisIdParameter = new MySqlParameter();
      thisIdParameter.ParameterName = "@thisId";
      thisIdParameter.Value = _id;

      cmd.Parameters.Add(thisIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void UpdateCopy(int newCopyNumofCopies)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE copies SET numof_copies = @newnumofCopies WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter numofCopies = new MySqlParameter();
      numofCopies.ParameterName = "@newnumofCopies";
      numofCopies.Value = newCopyNumofCopies;
      cmd.Parameters.Add(numofCopies);


      cmd.ExecuteNonQuery();
      _numofCopies = newCopyNumofCopies;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public List<Patron> GetPatrons()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT patrons.* FROM copies JOIN checkouts ON (copies.id = checkouts.copy_id) JOIN patrons ON (checkouts.patron_id = patrons.id) WHERE copies.id = @CopyId;";

      MySqlParameter copyId = new MySqlParameter();
      copyId.ParameterName = "@CopyId";
      copyId.Value = _id;
      cmd.Parameters.Add(copyId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Patron> patrons = new List<Patron>{};
      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        string checkoutDate = rdr.GetString(2);
        Patron newPatron = new Patron(patronName, checkoutDate, patronId);
        patrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return patrons;
    }

    public void AddPatron(Patron newPatron)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO checkouts (copy_id, patron_id) VALUES (@CopyId, @PatronId);";

            MySqlParameter copy_id = new MySqlParameter();
            copy_id.ParameterName = "@CopyId";
            copy_id.Value = _id;
            cmd.Parameters.Add(copy_id);

            MySqlParameter patron_id = new MySqlParameter();
            patron_id.ParameterName = "@PatronId";
            patron_id.Value = newPatron.GetId();
            cmd.Parameters.Add(patron_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
  }

}
