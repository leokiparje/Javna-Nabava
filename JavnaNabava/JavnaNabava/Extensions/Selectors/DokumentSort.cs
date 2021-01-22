using JavnaNabava.Models;
using System;
using System.Linq;

namespace JavnaNabava.Extensions.Selectors
{
  public static class DokumentSort
  {
    public static IQueryable<Dokument> ApplySort(this IQueryable<Dokument> query, int sort, bool ascending)
    {
      System.Linq.Expressions.Expression<Func<Dokument, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = d => d.DokumentId;
          break;
        case 2:
          orderSelector = d => d.Naslov;
          break;
        case 3:
          orderSelector = d => d.Vrsta;
          break;
        case 4:
          orderSelector = d => d.Blob;
          break;
        case 6:
          orderSelector = d => d.DatumPredaje;
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
