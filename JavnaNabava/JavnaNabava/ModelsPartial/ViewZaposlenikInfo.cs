using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
  public class ViewZaposlenikInfo
  {
    public string OibZaposlenik { get; set; }
    public string ImePartnera { get; set; }
    public string PrezimePartnera { get; set; }
    public DateTime DatumRođenja { get; set; }
    public string MjestoPrebivališta { get; set; }


    public int IdKompetencije { get; set; }
    public int IdStručneSpreme { get; set; }
    public string OibPonuditelj { get; set; }

        [NotMapped]    
    public int Position { get; set; } //Position in the result
  }
}
