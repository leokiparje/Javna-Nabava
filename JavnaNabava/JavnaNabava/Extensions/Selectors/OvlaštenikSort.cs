using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    /// <summary>
    /// Klasa OvlaštenikSort ima jednu statičku metodu za sortiranje kolekcije ovlaštenika.
    /// Ovlaštenici se mogu sortirati po imenu, prezimenu, id-u povjerenstva i po oib-u naručitelja
    /// </summary>
    public static class OvlaštenikSort
    {
        public static IQueryable<Ovlaštenik> ApplySort(this IQueryable<Ovlaštenik> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Ovlaštenik, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = o => o.ImeOvlaštenik;
                    break;
                case 2:
                    orderSelector = o => o.PrezimeOvlaštenik;
                    break;
                case 3:
                    orderSelector = o => o.IdPovjerenstva;
                    break;
                case 4:
                    orderSelector = o => o.OibNaručitelja;
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
