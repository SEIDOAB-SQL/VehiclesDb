using System.ComponentModel.DataAnnotations;

namespace Models;

public class csVehicles : ISeed<csVehicles>{

    [Key]
    public Guid VehicleId { get; set; }
    public string RegNr { get; set; }
    public string Brand { get; set; }
    public int Year { get; set; }

    public override string ToString() => $"{VehicleId} is a {Year} {Brand} with reg nr {RegNr}";


    #region Seeder
    public bool Seeded { get; set; } = false;

    public csVehicles Seed(csSeedGenerator seedGenerator)
    {
        string s = seedGenerator.FromString("ABC, CCC, DEF, HTG, WER");
        s = $"{s} {seedGenerator.Next(100, 1000)}";
        
       return  new csVehicles() {
            Seeded = true,

            VehicleId = Guid.NewGuid(),

            RegNr = s,
            Brand = seedGenerator.FromString("Volvo, VW, Skoda, BWM, Audi"),
            Year = seedGenerator.Next (1970, 2020)
        };
    }
    #endregion
}