namespace LuccaDevises
{
    internal class CurrencyExchangeRate
    {
        private float _exchangeRate;
        private string _startingCurrency;
        private string _endingCurrency;

        public float ExchangeRate { get => _exchangeRate; set => _exchangeRate = value; }
        public string StartingCurrency { get => _startingCurrency; set => _startingCurrency = value; }
        public string EndingCurrency { get => _endingCurrency; set => _endingCurrency = value; }

        public bool hasSameCurrencies(CurrencyExchangeRate compare)
        {
            if ((compare.EndingCurrency == EndingCurrency && compare.StartingCurrency == StartingCurrency) || (compare.EndingCurrency == StartingCurrency && compare.StartingCurrency == EndingCurrency))
            {
                return true;
            }
            return false;
        }
        public bool ContainCurrency(string currency)
        {
            if (currency == StartingCurrency || currency == EndingCurrency)
            {
                return true;
            }
            return false;
        }

        public float exchangeCurrency(float amount)
        {
            return (float)Math.Round(amount * ExchangeRate, 4);
        }

        public void swapCurrenciesOrder()
        {
            string container = StartingCurrency;
            StartingCurrency = EndingCurrency;
            EndingCurrency = container;
            ExchangeRate = 1 / ExchangeRate;
        }

        public CurrencyExchangeRate(CurrencyExchangeRate toClone)
        {
            ExchangeRate = toClone.ExchangeRate;
            StartingCurrency = toClone.StartingCurrency;
            EndingCurrency = toClone.EndingCurrency;
        }

        public CurrencyExchangeRate(float exchangeRate, string startingCurrency, string endingCurrency)
        {
            ExchangeRate = exchangeRate;
            StartingCurrency = startingCurrency;
            EndingCurrency = endingCurrency;
        }

        public void display()
        {
            Console.Write("Rate:" + ExchangeRate);
            Console.Write("\tStarting:" + StartingCurrency);
            Console.WriteLine("\tEnding:" + EndingCurrency);
        }
    }
}
