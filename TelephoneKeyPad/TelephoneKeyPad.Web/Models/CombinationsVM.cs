using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TelephoneKeyPad.Web.Models
{
    public class CombinationsVM
    {
        [DisplayName("Phone Number")]
        public string OriginalNumber { get; set; }
        public int ItemCount { get; set; }
        public string[] PagedItems{ get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}
