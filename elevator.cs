using System.Collections.Generic;
using System.Linq;

namespace Rocket_Elevators_Controllers
{
    public class Elevator
    {
        public int Id { get; set; }
        public int ElevatorCurrentFloor { get; set; }
        public string Direction { get; set; }
        public int ElevatorNextStop { get; set; }


        public Elevator(int id, int elevatorCurrentFloor, string direction, int elevatorNextStop)
        {
            this.Id = id;
            this.ElevatorCurrentFloor = elevatorCurrentFloor;
            this.Direction = direction;
            this.ElevatorNextStop = elevatorNextStop;
        }

    }
}