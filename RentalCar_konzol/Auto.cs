using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar_konzol
{
    class Auto
    {
        private List<Kolcsonzes> kolcsonzesek = new List<Kolcsonzes>();
        int ar;
        int id;
        string marka;
        string model;
        string rendszam;
        

        public Auto(int ar, int id, string marka, string model, string rendszam)
        {
            this.Kolcsonzesek = new List<Kolcsonzes>();
            this.ar = ar;
            this.id = id;
            this.marka = marka;
            this.model = model;
            this.rendszam = rendszam;
        }

        public int Ar { get => ar;}
        public int Id { get => id; }
        public string Marka { get => marka; }
        public string Model { get => model; }
        public string Rendszam { get => rendszam; }
        internal List<Kolcsonzes> Kolcsonzesek { get => kolcsonzesek; set => kolcsonzesek = value; }
    }
}
