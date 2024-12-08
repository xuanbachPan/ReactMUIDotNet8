using System.Linq;

namespace ReactMUIWebAPIApplication.Models
{
    public class FindTheUsedFuel
    {
        public FindTheUsedFuel() {
            FuelVal = 0;
            IslandsList = new List<Island>();
        }
        public double FuelVal { get; set; }

        public List<Island> IslandsList;

        public FindTheUsedFuel CloneObject()
        {
            List<Island> DestIslandsList = new List<Island>();
            foreach (Island s in IslandsList)
            {
                Island newS = new Island(s.X, s.Y,s.KeyNumber);
                DestIslandsList.Add(newS);
            }
            return new FindTheUsedFuel
            {
                FuelVal = this.FuelVal,
                IslandsList = DestIslandsList
            };
        }
    }

    public class Island { 

        public Island() { }

        public Island(int x, int y, int keyNumber) {
            this.X = x;
            this.Y = y;
            this.KeyNumber = keyNumber;
        }
        public int X {get; set; }
        public int Y {get; set; }
        public int KeyNumber { get; set; }
    }
}
