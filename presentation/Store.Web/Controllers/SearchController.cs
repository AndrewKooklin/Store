using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Memory;

namespace Store.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly BookService bookService;
        public SearchController(BookService bookService)
        {
            this.bookService = bookService;
        }
        // /search/index?query=title
        public IActionResult Index(string query)
        {
            var books = bookService.GetAllByQuery(query);

            return View("Index", books);
        }
    }
}
