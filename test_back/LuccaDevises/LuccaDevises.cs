namespace LuccaDevises
{
    internal class LuccaDevises
    {
        private int _finalAmount;

        public int FinalAmount { get => _finalAmount; set => _finalAmount = value; }

        private void displayUsage()
        {
            Console.WriteLine("USAGE:" + Environment.NewLine + "LuccaDevises [input file]");
        }



        public LuccaDevises(string[] args)
        {
            if (args.Length < 1 || args[0] == "-h" || args[0] == "--help")
            {
                displayUsage();
                return;
            }
            Parser parser = new Parser(args[0]);
            PathFinder pathFinder = new PathFinder(parser.CurrencyExchangeRates, parser.InitialCurrency, parser.FinalCurrency);
            Calculator calculator = new Calculator(pathFinder.ShortestPath, parser.Amount);
            _finalAmount = (int)Math.Ceiling(calculator.FinalAmount);
        }
    }
}
