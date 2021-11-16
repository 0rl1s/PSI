using System;

namespace SEvent
{
    public delegate void Print(int value);
    public class MaximumReachedEventArgs : EventArgs
    {
        public int Maximum { get; set; }
        public DateTime TimeReached { get; set; }
    }
    class Counter
    {
        private int maximum;
        private int total;

        public Counter(int passedMaximum)
        {
            maximum = passedMaximum;
        }

        public void Add(int x)
        {
            total += x;
            if (total >= maximum)
            {
                MaximumReachedEventArgs args = new();
                args.Maximum = maximum;
                args.TimeReached = DateTime.Now;
                OnMaximumReached(args);
            }
        }

        protected virtual void OnMaximumReached(MaximumReachedEventArgs e)
        {
            EventHandler<MaximumReachedEventArgs> handler = MaximumReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //Generics in events?
        public event EventHandler<MaximumReachedEventArgs> MaximumReached;
    }
    class Program
    {
        static void Main()
        {
            int i = new Random().Next(10);

            //Anonymous Method
            Print print = delegate (int val)
            {
                val = i + 1;
                Console.WriteLine("Maximum is: {0}", val);
            };
            print(i);

            Counter c = new(i);

            //Anonymous Method as Event Handler
            c.MaximumReached += delegate (object sender, MaximumReachedEventArgs e)
            {
                Console.WriteLine("Got you, the real maximum of {0} was reached at {1}.", e.Maximum, e.TimeReached);
                Environment.Exit(0);
            };

            Console.WriteLine("press '+' key to increase total");
            while (Console.ReadKey(true).KeyChar == '+')
            {
                Console.WriteLine("adding one");
                c.Add(1);
            }
        }
    }
}
