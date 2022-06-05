namespace LuccaDevises
{
    class Program
    {
        static int Main(string[] args)
        {
            int final = new LuccaDevises(args).FinalAmount;
            Console.WriteLine(final);
            return final;
        }
    }
}