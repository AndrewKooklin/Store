using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Store
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;
        //private readonly DbConnectionFactory dbConnectionFactory;

        //private readonly Func<IDbConnection> dbConnectionFactory;

        //public BookService(Func<IDbConnection> dbConnectionFactory)
        //{
        //    this.dbConnectionFactory = dbConnectionFactory;
        //}

        //public BookService(DbConnectionFactory dbConnectionFactory)
        //{
        //    this.dbConnectionFactory = dbConnectionFactory;
        //}

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

        //private Book[] GetAllByIsbn(string isbn)
        //{
        //    //var connectionString = Environment.GetEnvironmentVariable("STORE_CONNECTION_STRING");
        //    //using( var connection = new SqlConnection(connectionString))

        //    using (var connection = dbConnectionFactory())//.Create())
        //    using (var command = connection.CreateCommand())
        //    {
        //        command.CommandType = CommandType.Text;
        //        command.CommandText = "SELECT" +
        //            " Id, Title, Author, Isbn, Description, Price" +
        //            "FROM Books" +
        //            "WHERE Isbn = @Isbn";
        //        command.Parameters.Add(isbn);

        //        using (var reader = command.ExecuteReader())
        //        {
        //            var result = new List<Book>();

        //            while (reader.Read())
        //            {
        //                var id = reader.GetInt32(0);
        //                var title = reader.GetString(1);
        //                var author = reader.GetString(2);
        //                var _isbn = reader.GetString(3);
        //                var description = reader.GetString(4);
        //                var price = reader.GetDecimal(5);

        //                result.Add(new Book(id, title, author, _isbn, description, price));
        //            }

        //            return result.ToArray();
        //        }

        //    }
        //}
    }

    //public class DbConnectionFactory
    //{
    //    private readonly IServiceProvider serviceProvider;

    //    public DbConnectionFactory(IServiceProvider serviceProvider)
    //    {
    //        this.serviceProvider = serviceProvider;
    //    }

    //    public IDbConnection Create()
    //    {
    //        return (IDbConnection)serviceProvider.GetService(typeof(IDbConnection));
    //    }
    //}
}
