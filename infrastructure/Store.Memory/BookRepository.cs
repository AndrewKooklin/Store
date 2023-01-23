using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1, "ISBN 0201038013", "D. Knuth", "Art of Programming, Vol. 1",
                    "This volume begins with programming concept", 7.19m),
            new Book(2, "ISBN 0201485672", "M. Fowler",  "Refactoring",
                    "As the application of object technology--particularly the Java", 12.45m),
            new Book(3, "ISBN 0131101633", "B. W. Kernighan, D. M. Ritchie", "C Programming Language",
                    "Known as the bible of C, this classic bestseller introduces", 14.98m)
        };

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var foundBooks = from book in books
                             join bookId in bookIds on book.Id equals bookId
                             select book;

            return foundBooks.ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn)
                        .ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string query)
        {

            return books.Where(book => book.Author.Contains(query)
                                    || book.Title.Contains(query))
                        .ToArray();
        }

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
