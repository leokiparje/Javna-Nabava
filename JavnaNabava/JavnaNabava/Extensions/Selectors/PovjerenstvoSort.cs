using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    /// <summary>
    /// Klasa PovjerenstvoSort ima jednu statičku metodu za sortiranje kolekcije povjerenstava.
    /// Povjerenstva se mogu sortirati po nazivu, id-u i po evidencijskom broju natječaja.
    /// </summary>
    public static class PovjerenstvoSort
    {
        public static IQueryable<Povjerenstvo> ApplySort(this IQueryable<Povjerenstvo> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Povjerenstvo, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = p => p.NazivPovjerenstva;
                    break;
                case 2:
                    orderSelector = p => p.IdPovjerenstva;
                    break;
                case 3:
                    orderSelector = p => p.EvidBrojNatječajNavigation.NazivNatječaja;
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
