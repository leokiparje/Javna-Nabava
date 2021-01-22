using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    /// <summary>
    /// Klasa ZapisnikSort ima jednu statičku metodu za sortiranje kolekcije zapisnika.
    /// Zapisnici se mogu sortirati po id-u, nazivu, tekstu ponude i nazivu povjerenstva.
    /// Za sortiranje po tekstu ponude i nazivu povjerenstva koristi se navigacije preko id-a ponude ili povjerenstva.
    /// </summary>
    public static class ZapisnikSort
    {

        public static IQueryable<ViewZapisnikInfo> ApplySort(this IQueryable<ViewZapisnikInfo> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<ViewZapisnikInfo, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = z => z.ZapisnikId;
                    break;
                case 2:
                    orderSelector = z => z.NazivZapisnik;
                    break;
                case 3:
                    orderSelector = z => z.NazivPovjerenstva;
                    break;
                case 4:
                    orderSelector = z => z.TekstPonude;
                    break;
                default:
                    throw new Exception("Krivi Apply sort broj");
            }
            if (orderSelector != null)
            {
                query = ascending ?
                    query.OrderBy(orderSelector) :
                    query.OrderByDescending(orderSelector);
            }
            return query;
        }

    }
}
