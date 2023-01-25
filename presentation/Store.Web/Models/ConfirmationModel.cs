using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Models
{
    public class ConfirmationModel
    {
        public int OrderId { get; set; }

        public string CellPhone { get; set; }

        public Dictionary<string, string> Errors = new Dictionary<string, string>();
    }
}
