using JavnaNabava.Models;
using System;
using System.Linq;

namespace JavnaNabava.Extensions.Selectors
{
  public static class PonudaSort
  {
    public static IQueryable<Ponudum> ApplySort(this IQueryable<Ponudum> query, int sort, bool ascending)
    {
      System.Linq.Expressions.Expression<Func<Ponudum, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = d => d.PonudaId;
          break;
        case 2:
          orderSelector = d => d.EvidBrojNatječaj;
          break;
        case 3:
          orderSelector = d => d.Text;
          break;
        case 4:
          orderSelector = d => d.Naslov;
          break;
        case 5:
          orderSelector = d => d.OibPonuditelj;
          break;
        
        
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
