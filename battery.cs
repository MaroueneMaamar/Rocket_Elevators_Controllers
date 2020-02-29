using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket_Elevators_Controllers
{
    public class Battery
    {
        public int Id, NbColumns, NbFloors, NbBasements, NbElevators, UserPosition;
        public List<Column> ListColumns ;
        public List<int> ListFloors ;
        public Column BestColumn ;
        public Elevator BestElevator ;
        public List<Elevator> MovingElevators ,IdleElevators, GoodMovingElevators,GoodIdleElevators,PossibleElevators;

        // constucteur Battery
        public Battery(int id, int nbColumns, int nbFloors, int nbBasements, int nbElevators)
        {
            this.Id = id;
            this.NbColumns = nbColumns;
            this.NbFloors = nbFloors;
            this.NbBasements = nbBasements;
            this.NbElevators = nbElevators;
            FillListColums(nbColumns, nbFloors, nbBasements);
            FillListFloors();
        }
        // fonction qui remplit la liste des colonnes
        public void FillListColums(int nbColumns, int nbFloors, int nbBasements)
        {
            ListColumns = new List<Column>();
            for (char i = 'A'; i <= 'D'; i++)
            {
                Column c = new Column(i, this.NbElevators, this.NbColumns, this.NbFloors, this.NbBasements);
                ListColumns.Add(c);
            }
        }
        // fonction qui remplit la liste des etages qui vont etre desservis pour chaque colonne
        public void FillListFloors()
        {
            int AvgFloors = ((this.NbFloors - this.NbBasements) / (this.NbColumns - 1));
            int nbFilledColumns = 0;
            int minFloor = 2;
            int maxFloor = AvgFloors;
            foreach (Column c in ListColumns)
            {
                if (c.Id == 'A')
                {
                    c.ListFloors.Add(1);
                    for (int i = -1; i >= -c.NbBasements; i--)
                    {
                        c.ListFloors.Add(i);
                    }
                }
                else
                {
                    nbFilledColumns++;
                    c.ListFloors.Add(1);
                    for (int i = minFloor; i <= maxFloor; i++)
                    {
                        c.ListFloors.Add(i);
                    }
                    minFloor = maxFloor + 1;
                    maxFloor = maxFloor + AvgFloors;
                }
                Console.WriteLine("Column  " + c.Id + " -- ListFloors =  " + String.Join(" | ", c.ListFloors));
            }
        }
        // fonction qui cherche et retourne le meilleur ascenceur et le bouge vers lutilisateur puis vers le RC
        public void RequestElevator(int userPosition)
        {
            Console.WriteLine("--------- User at floor : " + userPosition + " and want go to groumdfloor RC ---------");

            this.BestColumn = this.ListColumns.Where(c => c.ListFloors.Contains(userPosition)).FirstOrDefault();
            GoodMovingElevators = new List<Elevator>();
            GoodIdleElevators = new List<Elevator>();
            PossibleElevators = new List<Elevator>();
            MovingElevators = new List<Elevator>();
            IdleElevators = new List<Elevator>();
            foreach (Elevator E in this.BestColumn.ListElevators)
            {
                if (userPosition > 1)
                {
                    if (E.Direction == "Down" && userPosition < E.ElevatorCurrentFloor)
                    {
                        this.GoodMovingElevators.Add(E);
                    }
                    else if (E.Direction == "Idle")
                    {
                        this.GoodIdleElevators.Add(E);
                    }
                    else
                    {
                        this.PossibleElevators.Add(E);
                    }
                }
                if (userPosition < 1)
                {
                    if (E.Direction == "Up" && userPosition > E.ElevatorCurrentFloor)
                    {
                        this.GoodMovingElevators.Add(E);
                    }
                    else if (E.Direction == "Idle")
                    {
                        this.GoodIdleElevators.Add(E);
                    }
                    else
                    {
                        this.PossibleElevators.Add(E);
                    }
                }
                Console.WriteLine("Elevator " + BestColumn.Id + E.Id + " Direction= " + E.Direction + "| CurrentFloor = " + E.ElevatorCurrentFloor + "| NextStop  = " + E.ElevatorNextStop);
            }
            // Selection du BestElevator : soit le plus proche dans les GoodElevators  sinon dans les PossibleElevators
            if (this.GoodMovingElevators.Count > 0)
            {
                this.BestElevator = this.GoodMovingElevators.OrderBy(E => Math.Abs(E.ElevatorNextStop - E.ElevatorCurrentFloor) + Math.Abs(E.ElevatorNextStop - userPosition)).First();
            }
            else if (this.GoodIdleElevators.Count > 0)
            {
                this.BestElevator = this.GoodIdleElevators.OrderBy(E => Math.Abs(E.ElevatorNextStop - E.ElevatorCurrentFloor) + Math.Abs(E.ElevatorNextStop - userPosition)).First();
            }
            else
                this.BestElevator = this.PossibleElevators.OrderBy(E => Math.Abs(E.ElevatorCurrentFloor - userPosition)).First();


            Console.WriteLine("**********  The Best Elevator is : " + this.BestColumn.Id + this.BestElevator.Id + " **********");

            MoveElevator(this.BestElevator,userPosition );
            MoveElevator(this.BestElevator,1);
        }
        // fonction qui cherche et retourne le meilleur ascenceur et le bouge vers le RC puis vers la destination de lutilisateur
        public void AssignElevator(int userDestination)
        {
            Console.WriteLine("---------- User at groundfloor  RC  and request the floor : " + userDestination + " ----------");

            this.BestColumn = this.ListColumns.Where(c => c.ListFloors.Contains(userDestination)).FirstOrDefault();

            MovingElevators = new List<Elevator>();
            IdleElevators = new List<Elevator>();
            foreach (Elevator E in this.BestColumn.ListElevators)
            {
                if (E.Direction == "Idle" || E.ElevatorCurrentFloor == 1)
                {
                    this.IdleElevators.Add(E);
                }
                else
                    this.MovingElevators.Add(E);
                Console.Write("Elevator " + BestColumn.Id + E.Id + " Direction= " + E.Direction + "| CurrentFloor = " + E.ElevatorCurrentFloor + "| NextStop  = " + E.ElevatorNextStop);
                //Console.WriteLine("Direction = "+E.Direction +" CurrentFloor = " + E.ElevatorCurrentFloor + " NextStop =" + E.ElevatorNextStop);
                Console.WriteLine("|  Gap = " + (Math.Abs(E.ElevatorCurrentFloor - E.ElevatorNextStop) + Math.Abs(E.ElevatorNextStop - 1)));
            }
            // Selection du BestElevator : soit le plus proche dans les IdleElevators  sinon dans les MovingElevators
            if (this.IdleElevators.Count > 0)
            {
                this.BestElevator = this.IdleElevators.OrderBy(E => Math.Abs(E.ElevatorCurrentFloor - 1)).First();
            }
            else
                this.BestElevator = this.MovingElevators.OrderBy(E => Math.Abs(E.ElevatorCurrentFloor - E.ElevatorNextStop) + Math.Abs(E.ElevatorNextStop - 1)).First();

            Console.WriteLine("**********  The Best Elevator is : " + this.BestColumn.Id + this.BestElevator.Id + " **********");

            MoveElevator(this.BestElevator,1 );
            MoveElevator(this.BestElevator,userDestination);
        }
        // fonction qui bouge lascenseur 'bestElevator' vers letage 'destination'
        public void MoveElevator(Elevator bestElevator, int destination)
        {
            Console.WriteLine("--------- Moving Elevator : "+ BestColumn.Id +bestElevator.Id +" from floor :"+ bestElevator.ElevatorCurrentFloor +" to floor : "+ destination+" ---------" );
            if (bestElevator.ElevatorCurrentFloor < destination)
            {
                Console.Write("Elevator "+ BestColumn.Id +bestElevator.Id + " is at floor : ");
                while (bestElevator.ElevatorCurrentFloor<destination)
                {
                        bestElevator.ElevatorCurrentFloor ++ ;
                        if (bestElevator.ElevatorCurrentFloor != 0)
                            Console.Write(bestElevator.ElevatorCurrentFloor + " : ");
                }
            }
            if (bestElevator.ElevatorCurrentFloor > destination)
            {
                Console.Write("Elevator "+ BestColumn.Id +bestElevator.Id + " is at floor : ");
                while (bestElevator.ElevatorCurrentFloor>destination)
                {
                    bestElevator.ElevatorCurrentFloor -- ;
                    Console.Write(bestElevator.ElevatorCurrentFloor + " : ");
                }
            }
            Console.WriteLine(" ");
        }

        // // -------------------SCENARIO1-------------------------
        public void scenario1 ()
        {
            ListColumns[1].ListElevators[0].Direction="Down";
            ListColumns[1].ListElevators[0].ElevatorCurrentFloor=20;
            ListColumns[1].ListElevators[0].ElevatorNextStop=5;

            ListColumns[1].ListElevators[1].Direction="Up";
            ListColumns[1].ListElevators[1].ElevatorCurrentFloor=3;
            ListColumns[1].ListElevators[1].ElevatorNextStop=15;

            ListColumns[1].ListElevators[2].Direction="Down";
            ListColumns[1].ListElevators[2].ElevatorCurrentFloor=13;
            ListColumns[1].ListElevators[2].ElevatorNextStop=1;

            ListColumns[1].ListElevators[3].Direction="Down";
            ListColumns[1].ListElevators[3].ElevatorCurrentFloor=15;
            ListColumns[1].ListElevators[3].ElevatorNextStop=2;

            ListColumns[1].ListElevators[4].Direction="Down";
            ListColumns[1].ListElevators[4].ElevatorCurrentFloor=6;
            ListColumns[1].ListElevators[4].ElevatorNextStop=1;

            AssignElevator(20);
        }
        // // -------------------SCENARIO 2-------------------------
        public void scenario2 ()
        {
            ListColumns[2].ListElevators[0].Direction="Up";
            ListColumns[2].ListElevators[0].ElevatorCurrentFloor=1;
            ListColumns[2].ListElevators[0].ElevatorNextStop=21;

            ListColumns[2].ListElevators[1].Direction="Up";
            ListColumns[2].ListElevators[1].ElevatorCurrentFloor=23;
            ListColumns[2].ListElevators[1].ElevatorNextStop=28;

            ListColumns[2].ListElevators[2].Direction="Down";
            ListColumns[2].ListElevators[2].ElevatorCurrentFloor=33;
            ListColumns[2].ListElevators[2].ElevatorNextStop=1;

            ListColumns[2].ListElevators[3].Direction="Down";
            ListColumns[2].ListElevators[3].ElevatorCurrentFloor=40;
            ListColumns[2].ListElevators[3].ElevatorNextStop=24;

            ListColumns[2].ListElevators[4].Direction="Down";
            ListColumns[2].ListElevators[4].ElevatorCurrentFloor=39;
            ListColumns[2].ListElevators[4].ElevatorNextStop=1;

            AssignElevator(36);
        }

        // // -------------------SCENARIO 3-------------------------
        public void scenario3 ()
        {
            ListColumns[3].ListElevators[0].Direction = "Down";
            ListColumns[3].ListElevators[0].ElevatorCurrentFloor = 58;
            ListColumns[3].ListElevators[0].ElevatorNextStop = 1;

            ListColumns[3].ListElevators[1].Direction = "Up";
            ListColumns[3].ListElevators[1].ElevatorCurrentFloor = 50;
            ListColumns[3].ListElevators[1].ElevatorNextStop = 60;

            ListColumns[3].ListElevators[2].Direction = "Up";
            ListColumns[3].ListElevators[2].ElevatorCurrentFloor = 46;
            ListColumns[3].ListElevators[2].ElevatorNextStop = 58;

            ListColumns[3].ListElevators[3].Direction = "Up";
            ListColumns[3].ListElevators[3].ElevatorCurrentFloor = 1;
            ListColumns[3].ListElevators[3].ElevatorNextStop = 54;

            ListColumns[3].ListElevators[4].Direction = "Down";
            ListColumns[3].ListElevators[4].ElevatorCurrentFloor = 60;
            ListColumns[3].ListElevators[4].ElevatorNextStop = 1;

            RequestElevator(54);
        }

        // // -------------------SCENARIO 4-------------------------
        public void scenario4 ()
        {
            ListColumns[0].ListElevators[0].Direction="Idle";
            ListColumns[0].ListElevators[0].ElevatorCurrentFloor=-4;
            ListColumns[0].ListElevators[0].ElevatorNextStop=0;

            ListColumns[0].ListElevators[1].Direction="Idle";
            ListColumns[0].ListElevators[1].ElevatorCurrentFloor=1;
            ListColumns[0].ListElevators[1].ElevatorNextStop=0;

            ListColumns[0].ListElevators[2].Direction="Down";
            ListColumns[0].ListElevators[2].ElevatorCurrentFloor=-3;
            ListColumns[0].ListElevators[2].ElevatorNextStop=-5;

            ListColumns[0].ListElevators[3].Direction="Up";
            ListColumns[0].ListElevators[3].ElevatorCurrentFloor=-6;
            ListColumns[0].ListElevators[3].ElevatorNextStop=1;

            ListColumns[0].ListElevators[4].Direction="Down";
            ListColumns[0].ListElevators[4].ElevatorCurrentFloor=-1;
            ListColumns[0].ListElevators[4].ElevatorNextStop=-6;

            RequestElevator(-3);
        }
    }
}
