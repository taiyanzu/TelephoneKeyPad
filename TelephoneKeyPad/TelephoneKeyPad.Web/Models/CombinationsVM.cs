using MvcPaging;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TelephoneKeyPad.Web.Models
{
    public class CombinationsVM : IPagedList<string>
    {
        const string INVALID_PHONE_NUMBER = "Please enter a 7 or 10-digit phone number.";

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = INVALID_PHONE_NUMBER)]
        [RegularExpression(@"^(\d{7}|\d{10})$", ErrorMessage = INVALID_PHONE_NUMBER)]
        public string OriginalNumber { get; set; }

        public int PageSize { get; set; }
        public string[] PagedItems { get; set; }

        #region IPagedList interface
        public int TotalItemCount { get; set; }
        
        public int PageIndex { get; set; }

        public int PageCount => PageSize == 0 ? 0 : TotalItemCount / PageSize;

        public int PageNumber => PageIndex + 1;

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < PageCount;

        public bool IsFirstPage => PageIndex == 1;

        public bool IsLastPage => PageIndex == PageCount;

        public int ItemStart => PageIndex * PageSize + 1;

        public int ItemEnd => ItemStart + PagedItems?.Length ?? 0;

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var s in PagedItems)
            {
                yield return s;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => PagedItems.GetEnumerator();
        #endregion
    }
}
