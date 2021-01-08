using System;
using System.Diagnostics;
using Akka.Actor;
using Akka.Routing;

namespace PiApproximation
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int j = 1000; j <= 1e7; j *= 10)
            {
                int inside = 0;
                int n = j;

                // Normal approach --------------------------------------------
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int i = 0; i < n; i++)
                    if (generateAndCheck()) inside++;

                double pi = (double)inside / n * 4;
                stopwatch.Stop();
                Console.WriteLine($"normal approach - n = {n}     pi = {pi}   elapsed = {stopwatch.Elapsed}");
                // ------------------------------------------------------------

                // Actor model approach ---------------------------------------
                stopwatch = new Stopwatch();
                stopwatch.Start();

                using (var actorSystem = ActorSystem.Create("actor-system"))
                {
                    var distributor = actorSystem.ActorOf(Props.Create<Distributor>());

                    distributor.Tell(stopwatch);
                    distributor.Tell(n);
                    for (int i = 0; i < n; i++)
                        distributor.Tell("");

                    Console.ReadKey();
                }
                // ------------------------------------------------------------
            }
        }

        // generates a point x = -1 to 1, y = -1 to 1 and returns true if it is inside a circle with r = 1
        public static bool generateAndCheck()
        {
            Random rnd = new Random();
            double x = rnd.NextDouble() * 2 - 1;
            double y = rnd.NextDouble() * 2 - 1;
            double dist = x * x + y * y;
            return dist <= 1;
        }
    }

    class Distributor : UntypedActor
    {
        private IActorRef generator = Context.ActorOf(Props.Create<Generator>().WithRouter(new RoundRobinPool(5)));
        private int inside = 0;
        private int count = 0;
        private int n = int.MaxValue;
        private Stopwatch stopwatch;

        protected override void OnReceive(object message)
        {
            if (message is String)
            {
                generator.Tell("");
            }
            else if (message is bool)
            {
                if ((bool)message) inside++;
                count++;
                if (count == n)
                {
                    double pi = (double)inside / n * 4;
                    stopwatch.Stop();
                    Console.WriteLine($"actor model approach - n = {n}     pi = {pi}    elapsed = {stopwatch.Elapsed}");
                }
            }
            else if (message is int)
            {
                n = (int)message;
            }
            else if (message is Stopwatch)
            {
                stopwatch = (Stopwatch)message;
            }
        }
    }

    class Generator : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            bool b = Program.generateAndCheck();
            Sender.Tell(b);
        }
    }
}
