using JavnaNabava.Models;
using System;
using System.Linq;

namespace JavnaNabava.Extensions.Selectors
{
  public static class PonudaStavkeSort
  {
    public static IQueryable<PonudaStavke> ApplySort(this IQueryable<PonudaStavke> query, int sort, bool ascending)
    {
      System.Linq.Expressions.Expression<Func<PonudaStavke, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = d => d.PonudaId;
          break;
        case 2:
          orderSelector = d => d.IdStavke;
          break;
        case 3:
          orderSelector = d => d.CijenaStavke;
          break;
        case 4:
          orderSelector = d => d.KoličinaStavke;
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
