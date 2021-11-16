using System;
namespace TESTING
{
    public class ProductStock : EventArgs
    {
        public string ProductName;
        private int StockInHand;

        public delegate void OnStockLow(object sender, EventArgs e);

        public event OnStockLow StockLow;
        public ProductStock(string Name, int OpeningStock)
        {
            ProductName = Name;
            StockInHand = OpeningStock;
        }
        public void ReduceStock(int SalesDone)
        {
            StockInHand -= SalesDone;
            if (StockInHand < 5)
            {
                EventArgs arg = new();
                StockLow(this, arg);
            }
        }
    }
    public class Counter
    {
        private string CounterName;
        public Counter(string Name)
        {
            CounterName = Name;
        }
        public void Sales(ProductStock prod, int howmuch)
        {
            Console.WriteLine("{0} sold {1} times", prod.ProductName, howmuch);
            prod.ReduceStock(howmuch);
        }
        public void LowStockHandler(object Sender, EventArgs e)
        {
            Console.WriteLine("Anouncement " + "on {0}: Stock of Product {1}" + " gone Low", CounterName, ((ProductStock)Sender).ProductName);
        }
    }
    class ProgramEntry
    {
        [STAThread]
        static void Main()
        {
            Counter billing_counter1 = new("BOT1");
            Counter billing_counter2 = new("BOT2");
            ProductStock prod1 = new("Fridge", 7);
            ProductStock prod2 = new("Sony CD player", 6);
            ProductStock prod3 = new("Sony DVD player", 800);

            prod1.StockLow += new ProductStock.OnStockLow(billing_counter1.LowStockHandler);
            prod2.StockLow += new ProductStock.OnStockLow(billing_counter1.LowStockHandler);
            prod3.StockLow += new ProductStock.OnStockLow(billing_counter1.LowStockHandler);
            prod1.StockLow += new ProductStock.OnStockLow(billing_counter2.LowStockHandler);
            prod2.StockLow += new ProductStock.OnStockLow(billing_counter2.LowStockHandler);

            billing_counter1.Sales(prod1, 1);
            billing_counter2.Sales(prod1, 2);
            billing_counter1.Sales(prod3, 797);
            billing_counter2.Sales(prod2, 1);
            billing_counter1.Sales(prod2, 3);
            billing_counter1.Sales(prod3, 2);
        }
    }
}
