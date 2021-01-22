using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC.Models;

namespace MVC.ViewModels
{
    public class ZaposlenikFilter : IPageFilter
    {
        public string? OibZaposlenik { get; set; }
        public string ImeZaposlenik { get; set; }
        public string PrezimeZaposlenik { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DatumOd { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumDo { get; set; }
        public string MjestoPrebivališta { get; set; }


        public bool IsEmpty()
        {
            bool active = DatumOd.HasValue
                          || DatumDo.HasValue
      ;
            return !active;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}",
                OibZaposlenik,
                DatumOd?.ToString("dd.MM.yyyy"),
                DatumDo?.ToString("dd.MM.yyyy"));
        }

        public static ZaposlenikFilter FromString(string s)
        {
            var filter = new ZaposlenikFilter();
            if (!string.IsNullOrEmpty(s))
            {
                string[] arr = s.Split('-', StringSplitOptions.None);

                if (arr.Length == 5)
                {
                    filter.DatumOd = string.IsNullOrWhiteSpace(arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    filter.DatumDo = string.IsNullOrWhiteSpace(arr[2]) ? new DateTime?() : DateTime.ParseExact(arr[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                }
            }

            return filter;
        }
    }
}
