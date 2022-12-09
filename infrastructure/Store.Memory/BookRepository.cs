using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1, "ISBN 12312-31231", "D.Knuth", "Art of Programming"),
            new Book(2, "ISBN 12312-31231", "M.Fowler",  "Refactoring"),
            new Book(3, "ISBN 12312-31231", "B.Kernighan, D.Ritchie", "C Programming Language")
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByTitle(string titlePart)
        {
            return books.Where(book => book.Title.Contains(titlePart))
                        .ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            throw new NotImplementedException();
        }
    }
}
