using System.Reflection.Metadata.Ecma335;

namespace AdvancedCsharp;

internal class Program
{
    static void Main(string[] args)
    {
        Stock s = new Stock("Bateekh");
        s.Price = 100;
        s.OnPriceChanged += Stock_OnPriceChanged;

        s.ChangeStockPriceBy(0.05m);
        s.ChangeStockPriceBy(-0.02m);
        s.ChangeStockPriceBy(0.00m);



        Console.ReadKey();
    }
    private static void Stock_OnPriceChanged(Stock stock, decimal oldPrice)
    {
        if (stock.Price > oldPrice)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else if (stock.Price < oldPrice)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        System.Console.WriteLine($"{stock.Name} , {stock.Price}");
    }
}

delegate void StockPriceChangeHandler(Stock stock, decimal oldPrice);


class Stock
{
    private decimal _price;
    private string _name;

    public event StockPriceChangeHandler OnPriceChanged;

    public string Name => _name;
    public decimal Price { get => this._price; set => this._price = value; }

    public Stock(string name)
    {
        this._name = name;
    }


    public void ChangeStockPriceBy(decimal percent)
    {
        decimal oldPrice = Price;
        Price += Price * (percent / 100);
        if (OnPriceChanged != null)
        {
            OnPriceChanged(this, oldPrice);
        }
    }
}

