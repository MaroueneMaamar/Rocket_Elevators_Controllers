using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket_Elevators_Controllers
{
    public class Column
    {
        public char Id { get; set; }
        public List<Elevator> ListElevators { get; set; }
        public List<int> ListFloors { get; set; }
        public int NbColumns { get; set; }
        public int NbFloors { get; set; }
        public int NbBasements { get; set; }
        public int NbElevators { get; set; }

        public Column(char id, int nbElevators, int nbColumns, int nbFloors, int nbBasements)
        {
            this.Id = id;
            this.NbElevators = nbElevators;
            this.ListFloors = new List<int>();
            this.ListElevators = new List<Elevator>();
            this.NbColumns = nbColumns;
            this.NbFloors = nbFloors;
            this.NbBasements = nbBasements;
            this.NbElevators = nbElevators;
            fillListElevator(nbElevators);
        }

        //  Remplissage de la ListElevators par des elevators Idle au RC
        public void fillListElevator(int nbElevators)
        {
            for (int i = 1; i <= nbElevators; i++)
            {
                Elevator E = new Elevator(i, 1, "Idle", 1);
                this.ListElevators.Add(E);
            }
        }

    }
}
