using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    interface IBookRepository
    {
        Book[] GetAllByTitle(string titlePart);
    }
}
