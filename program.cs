using System;

namespace Rocket_Elevators_Controllers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------Rocket_Elevators_Controllers!------------------");
            Battery battery = new Battery(1, 4, 66, 6, 5);
            battery.scenario1 ();
            // battery.scenario2 ();
            // battery.scenario3 ();
            // battery.scenario4 ();
        }
    }
}
