using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Patron
  {
    private string _patronName;
    private string _checkoutDate;
    private int _id;

    public Patron(string patronName, string checkoutDate, int id = 0)
    {
      _patronName = patronName;
      _checkoutDate = checkoutDate;
      _id = id;
    }

    public override bool Equals(System.Object otherPatron)
    {
    if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = (this.GetId() == newPatron.GetId());
        bool patronNameEquality = (this.GetPatronName() == newPatron.GetPatronName());
        bool checkoutDateEquality = (this.GetCheckoutDate() == newPatron.GetCheckoutDate());
        return (idEquality && patronNameEquality && checkoutDateEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetPatronName().GetHashCode();
    }

    public string GetPatronName()
    {
      return _patronName;
    }

    public string GetCheckoutDate()
    {
      return _checkoutDate;
    }

    public int GetId()
    {
      return _id;
    }

    public void UpdatePatronName(string newPatronName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE patrons SET patron_name = @newPatronName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter patronName = new MySqlParameter();
      patronName.ParameterName = "@newPatronName";
      patronName.Value = newPatronName;
      cmd.Parameters.Add(patronName);

      cmd.ExecuteNonQuery();
      _patronName = newPatronName;

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
      cmd.CommandText = @"INSERT INTO patrons (patron_name, checkout_date) VALUES (@patronName, @checkoutDate);";

      MySqlParameter patronName = new MySqlParameter();
      patronName.ParameterName = "@patronName";
      patronName.Value = this._patronName;
      cmd.Parameters.Add(patronName);

      MySqlParameter checkoutDate = new MySqlParameter();
      checkoutDate.ParameterName = "@checkoutDate";
      checkoutDate.Value = this._checkoutDate;
      cmd.Parameters.Add(checkoutDate);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronPatronName = rdr.GetString(1);
        string patronCheckoutDate = rdr.GetString(2);
        Patron newPatron = new Patron(patronPatronName, patronCheckoutDate, patronId);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allPatrons;
    }

    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `patrons` WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int patronId = 0;
      string patronName = "";
      string patronCheckoutDate = "";

      while (rdr.Read())
      {
        patronId = rdr.GetInt32(0);
        patronName = rdr.GetString(1);
        patronCheckoutDate = rdr.GetString(2);
      }

      Patron newPatron= new Patron(patronName, patronCheckoutDate, patronId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newPatron;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddCopy(Copy newCopy)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts (copy_id, patron_id) VALUES (@CopyId, @PatronId);";

      MySqlParameter copy_id = new MySqlParameter();
      copy_id.ParameterName = "@CopyId";
      copy_id.Value = newCopy.GetId();
      cmd.Parameters.Add(copy_id);

      MySqlParameter patron_id = new MySqlParameter();
      patron_id.ParameterName = "@PatronId";
      patron_id.Value = _id;
      cmd.Parameters.Add(patron_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Copy> GetCopiesForPatrons()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT copies.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patron_id) JOIN copies ON (checkouts.copy_id = copies.id) WHERE patrons.id = @PatronId;";


      MySqlParameter patronIdParameter = new MySqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = _id;
      cmd.Parameters.Add(patronIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Copy> copies = new List<Copy>{};
      while(rdr.Read())
      {
          int copyId = rdr.GetInt32(0);
          int numofCopies = rdr.GetInt32(1);
          int bookId = rdr.GetInt32(2);
          Copy newCopy = new Copy(numofCopies, bookId, copyId);
          copies.Add(newCopy);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return copies;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @PatronId; DELETE FROM copies_patrons WHERE patron_id = @PatronId;";

      MySqlParameter patronIdParameter = new MySqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId();
      cmd.Parameters.Add(patronIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
