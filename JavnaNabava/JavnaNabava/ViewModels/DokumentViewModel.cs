
using System;
namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Jednostavniji pogled na model za dohvat jednog dokumenta
    /// </summary>
  public class DokumentViewModel
  {
    public int DokumentId { get; set; }
    public string Naslov { get; set; }
    public string Vrsta { get; set; }
    public string Blob { get; set; }
    public string NazivPonude { get; set; }
    public int PonudaId { get; set; }
    public DateTime DatumPredaje {get ; set;}
  }
}
   