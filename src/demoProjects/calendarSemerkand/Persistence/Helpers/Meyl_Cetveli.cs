using System.Collections.Generic;

namespace Persistence.Helpers
{
  public class Meyl_Cetveli
  {
    public int Id { get; set; }
    public double Gunesin_Egimi { get; set; }
    public double Zaman_Denklemi { get; set; }
    public int Saat { get; set; }
    public string Tarih { get; set; }

    public List<Meyl_Cetveli> Rows = new List<Meyl_Cetveli>();
  }
}