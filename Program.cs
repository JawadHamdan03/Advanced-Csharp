using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AdvancedCsharp;

internal class Program
{
    static void Main(string[] args)
    {


        // the default for a thread is to be foreground 
        Thread.CurrentThread.Name = "Main Thread";
        System.Console.WriteLine(Thread.CurrentThread.Name);
        Console.WriteLine($"BackGround Thread : {Thread.CurrentThread.IsBackground}");

        var wallet = new Wallet("Jawad",80);

        Thread t1 = new Thread(wallet.RunRandomTransactions);
        t1.Name = "T1";
        Console.WriteLine(t1.IsBackground);


        Console.ReadKey();
    }
}

    class Wallet
    {
        public string Name { get; private set; }
        public int Bitcoin { get; private set; }

        public Wallet(string name, int Bitcoin)
        {
            this.Name = name;
            this.Bitcoin = Bitcoin;
        }

        public void Debit(int amount)
        {
            Thread.Sleep(1000);
            Bitcoin -= amount;
            System.Console.WriteLine($"Thread Id :{Thread.CurrentThread.ManagedThreadId} , Processor Id :{Thread.GetCurrentProcessorId()}");
        }
        public void Credit(int amount)
        {
            Thread.Sleep(1000);
            Bitcoin += amount;
            System.Console.WriteLine($"Thread Id :{Thread.CurrentThread.ManagedThreadId} , Processor Id :{Thread.GetCurrentProcessorId()}");
        }

        public void RunRandomTransactions()
        {
            int[] amounts = { 10, 20, 30, -20, 10, -10, 30, -10, 40, -20 };

            foreach (var amount in amounts)
            {
                var absValue = Math.Abs(amount);
                if (amount < 0)
                    Debit(absValue);

                else
                    Credit(absValue);
            }
        }

        public override string ToString()
        {
            return $"Name :{Name} , Bitcoin : {Bitcoin}";
        }

    }

