using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Store
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public Book[] GetAllByQuery(string query)
        {
            if (Book.IsIsbn(query))
                return bookRepository.GetAllByIsbn(query);
            else
            {
                return bookRepository.GetAllByTitleOrAuthor(query);
            }
        }

        private Book[] GetAllByIsbn(string isbn)
        {
            using( var connection = new SqlConnection())
            using(var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT" +
                    " Id, Title, Author, Isbn, Description, Price"+
                    "FROM Books"+
                    "WHERE Isbn = @Isbn";
                command.Parameters.AddWithValue("@Isbn", isbn);
                using (var reader = command.ExecuteReader())
                {
                    var result = new List<Book>();

                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var title = reader.GetString(1);
                        var author = reader.GetString(2);
                        var _isbn = reader.GetString(3);
                        var description = reader.GetString(4);
                        var price = reader.GetDecimal(5);

                        result.Add(new Book(id, title, author, _isbn, description, price));
                    }

                    return result.ToArray();
                }

            }
        }
    }
}
