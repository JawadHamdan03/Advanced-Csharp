using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AdvancedCsharp;

internal class Program
{
    static void Main(string[] args)
    {
        // Deadlock Solution : using pre ordering 

        var wallet1 = new Wallet(1,"jawad",100);
        var wallet2 = new Wallet(2,"reem",50);

        Console.WriteLine("\n Before Transaction");
        Console.WriteLine("\n----------------------");
        Console.ReadKey(true);
        Console.WriteLine(wallet1+" , "+ wallet2);
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("\n After Transaction");
        Console.WriteLine("\n----------------------");
        var transferManager1 = new TransferManager(wallet1,wallet2,50);
        var transferManager2 = new TransferManager(wallet2,wallet1,30);

        Thread t1 = new Thread(transferManager1.Transfer);
        t1.Name = "T1";
        Thread t2 = new Thread(transferManager2.Transfer);
        t2.Name = "T2";

        t1.Start();
        t2.Start();


        t1.Join();
        t2.Join();




        Console.WriteLine(wallet1 + " , " + wallet2);
        Console.ReadKey(true);
    }
}

class Wallet
{
    private readonly object _bitcoinsLock= new object();

    public int Id { get; set; }
    public string Name { get; private set; }
    public int Bitcoin { get; private set; }

    public Wallet(int id,string name, int Bitcoin)
    {
        this.Id = id;
        this.Name = name;
        this.Bitcoin = Bitcoin;
    }

    public void Debit(int amount)
    {

        lock (_bitcoinsLock)
        {
         if (Bitcoin > amount)
           {
            Thread.Sleep(1000);
            Bitcoin -= amount;
           }
        }
    
   
    }
    public void Credit(int amount)
    {
        Thread.Sleep(1000);
        Bitcoin += amount;
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
        return $"[Name :{Name} , Bitcoin : {Bitcoin}]";
    }

}

class TransferManager
{
    private Wallet from;
    private Wallet to;
    private int amountToTransfer;

    public TransferManager(Wallet from, Wallet to, int amount)
    {
        this.from = from;
        this.to = to;
        this.amountToTransfer = amount;
    }

    public void Transfer()
    {
        // to implement ordering 
        var lock1 = from.Id < to.Id ? from : to;
        var lock2 = from.Id < to.Id ? to : from;

        Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock ... {from}");
        lock(lock1)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} lock aquired ... {from}");
            Thread.Sleep(1000);
            Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock ... {to}");

            // Critical Section
            lock(lock2)
            {
                from.Debit(amountToTransfer);
                to.Credit(amountToTransfer);
            }

           
        }
    }
}
