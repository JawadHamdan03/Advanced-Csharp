using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AdvancedCsharp;

internal class Program
{
    static void Main(string[] args)
    {
        // Race Condition
        // both the threads t1 and t2 race to execute Debit() 
        // we use lock to prevent race condition

        var wallet = new Wallet("jawad",50);

        Thread t1 = new Thread(()=>wallet.Debit(40));
        Thread t2 = new Thread(()=>wallet.Debit(40));

        t1.Start();
        t2.Start();


        t1.Join();
        t2.Join();

        Console.WriteLine(wallet);
        Console.ReadKey();
    }
}

class Wallet
{
    private readonly object _bitcoinsLock= new object();


    public string Name { get; private set; }
    public int Bitcoin { get; private set; }

    public Wallet(string name, int Bitcoin)
    {
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
        return $"Name :{Name} , Bitcoin : {Bitcoin}";
    }

}

