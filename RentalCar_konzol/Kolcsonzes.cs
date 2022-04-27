using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar_konzol
{
    class Kolcsonzes
    {
        DateTime tol;
        DateTime ig;

        public Kolcsonzes(DateTime tol, DateTime ig)
        {
            this.tol = tol;
            this.ig = ig;
        }

        public DateTime Tol { get => tol; set => tol = value; }
        public DateTime Ig { get => ig; set => ig = value; }
    }
}
