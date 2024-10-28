using Core_MVC.Models;
using Npgsql;
using System.Xml.Linq;

namespace Core_MVC.BAL
{
    public class BookHelper
    {
        private readonly NpgsqlConnection _con;
        public BookHelper(NpgsqlConnection con) 
        {
            _con = con;
        }

        public List<BookModel> getBook()
        {
            var books = new List<BookModel>();
            _con.Open();
            using NpgsqlCommand cmd = new NpgsqlCommand("SELECT *FROM t_book",_con);
            using NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                books.Add(new BookModel
                {
                    b_id = Convert.ToInt32(dr["b_id"]),
                    b_name = dr["b_name"].ToString(),
                    b_author = dr["b_author"].ToString(),
                    b_price = float.Parse(dr["b_price"].ToString())
                });
            }
            _con.Close();
            return books;
        }


        // public void insertBook(BookModel book)
        // {
        //     _con.Open();
        //     NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO t_book (b_name, b_author, b_price) VALUES(@name,@author,@price)", _con);
        //     cmd.Parameters.AddWithValue("@name",book.b_name);
        //     cmd.Parameters.AddWithValue("@author",book.b_author);
        //     cmd.Parameters.AddWithValue("@price",book.b_price);

        //     cmd.ExecuteNonQuery();
        //     _con.Close();
        // }


        public string insertBook(BookModel book)
        {
            if(string.IsNullOrEmpty(book.b_name) || string.IsNullOrEmpty(book.b_author) || book.b_price <= 0)
            {
                return "Enter All Fields";
            }
            try
            {
                _con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO t_book (b_name, b_author, b_price) VALUES(@name,@author,@price)", _con);
                cmd.Parameters.AddWithValue("@name",book.b_name);
                cmd.Parameters.AddWithValue("@author",book.b_author);
                cmd.Parameters.AddWithValue("@price",book.b_price);

                cmd.ExecuteNonQuery();
                return "Add Book Successfully...";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                _con.Close();
            }
        }


        public BookModel getSelectBook (int bookID)
        {
            BookModel book = new BookModel();
            _con.Open();
            using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM t_book WHERE b_id = @ID", _con);
            cmd.Parameters.AddWithValue("@ID",bookID);
            using NpgsqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                book.b_id = int.Parse(dr["b_id"].ToString());
                book.b_name = dr["b_name"].ToString();
                book.b_author = dr["b_author"].ToString();
                book.b_price = float.Parse(dr["b_price"].ToString());
            }

            _con.Close();
            return book;
        }


        public string UpdateBook(BookModel book)
        {
            try
            {
                 _con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE t_book SET b_name = @name, b_author = @author, b_price = @price WHERE b_id = @BookID", _con);
                cmd.Parameters.AddWithValue("@BookID",book.b_id);
                cmd.Parameters.AddWithValue("@name",book.b_name);
                cmd.Parameters.AddWithValue("@author",book.b_author);
                cmd.Parameters.AddWithValue("@price", book.b_price);
                cmd.ExecuteNonQuery();
                return "Updated Successfully...";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                _con.Close();
            }
        }


        public string deleteBook(int BookID)
        {
            _con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM t_book WHERE b_id = @BookID",_con);
            cmd.Parameters.AddWithValue("@BookID",BookID);
            cmd.ExecuteNonQuery();
            _con.Close();
            return "Book Delete Successfully...";
        }
    }
}
