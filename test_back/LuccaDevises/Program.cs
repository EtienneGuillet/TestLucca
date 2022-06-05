namespace LuccaDevises
{
    class Program
    {
        static int Main(string[] args)
        {
            int finalAmount = new LuccaDevises(args).FinalAmount;
            Console.WriteLine("The final amount of money is: " + finalAmount);
            return finalAmount;
        }
    }
}