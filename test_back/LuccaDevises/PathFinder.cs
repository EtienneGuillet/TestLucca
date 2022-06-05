namespace LuccaDevises
{
    internal class PathFinder
    {
        private List<CurrencyExchangeRate> _currencyExchangeRates;
        private string _initialCurrency;
        private string _finalCurrency;
        List<CurrencyExchangeRate> _shortestPath;

        internal List<CurrencyExchangeRate> ShortestPath { get => _shortestPath; set => _shortestPath = value; }

        private void Error(string errorText = "")
        {
            if (errorText != "")
            {
                Console.Error.WriteLine(errorText);
            }
            Environment.Exit(-1);
        }

        private bool pathContainCurrency(List<CurrencyExchangeRate> path, string currency)
        {
            CurrencyExchangeRate last = path.Last();
            foreach (CurrencyExchangeRate currencyExchangeRate in path)
            {
                if (!currencyExchangeRate.Equals(last) && (currencyExchangeRate.StartingCurrency == currency || currencyExchangeRate.EndingCurrency == currency))
                {
                    return true;
                }
            }
            return false;
        }

        private bool currencyInExchangeRate(List<CurrencyExchangeRate> path, CurrencyExchangeRate currencyExchangeRate)
        {
            if (!pathContainCurrency(path, currencyExchangeRate.EndingCurrency))
            {
                if (currencyExchangeRate.ContainCurrency(path.Last().EndingCurrency) || currencyExchangeRate.ContainCurrency(path.Last().StartingCurrency))
                {
                    return true;
                }
            }
            return false;
        }

        private void displayList(List<CurrencyExchangeRate> list)
        {
            foreach (var el in list)
            {
                el.display();
            }
            Console.WriteLine("\nEND LIST\n\n");
        }

        private List<CurrencyExchangeRate> findPath(List<CurrencyExchangeRate> path, List<CurrencyExchangeRate> currencyExchangeRates)
        {
            if (path.Last().ContainCurrency(_finalCurrency))
            {
                if (ShortestPath.Count == 0 || path.Count < _shortestPath.Count)
                {
                    _shortestPath = path;
                }
                return path;
            }
            foreach (CurrencyExchangeRate currencyExchangeRate in currencyExchangeRates)
            {
                if (currencyExchangeRate.ContainCurrency(_initialCurrency))
                {
                    continue;
                }
                if (!currencyExchangeRate.hasSameCurrencies(path.Last()))
                {
                    if (currencyInExchangeRate(path, currencyExchangeRate))
                    {
                        List<CurrencyExchangeRate> clone = currencyExchangeRates.ToList();
                        clone.Remove(currencyExchangeRate);
                        findPath(path.ToList(), clone);
                        path.Add(currencyExchangeRate);

                        return findPath(path, _currencyExchangeRates);
                    }
                }
            }
            return path;
        }

        private void checkCurrenciesToSwap()
        {
            if (_shortestPath[0].StartingCurrency != _initialCurrency)
            {
                _shortestPath[0].swapCurrenciesOrder();
            }
            for (int i = 1; i < _shortestPath.Count; i++)
            {
                if (_shortestPath[i - 1].EndingCurrency != _shortestPath[i].StartingCurrency)
                {
                    _shortestPath[i].swapCurrenciesOrder();
                }
            }
        }

        private void findShortestPath(List<CurrencyExchangeRate> initialCurrencyExchangeRates)
        {
            List<List<CurrencyExchangeRate>> paths = new List<List<CurrencyExchangeRate>>();
            foreach (CurrencyExchangeRate initialCurrencyExchangeRate in initialCurrencyExchangeRates)
            {
                List<CurrencyExchangeRate> path = new List<CurrencyExchangeRate>();
                path.Add(initialCurrencyExchangeRate);
                path = findPath(path, _currencyExchangeRates);
                if (path.Last().ContainCurrency(_finalCurrency))
                {
                    paths.Add(path);
                }
            }
            if (paths.Count < 1)
            {
                Error("There is no possible way to convert from " + _initialCurrency + " to " + _finalCurrency);
            }
        }

        private List<CurrencyExchangeRate> findInitialCurrencyExchangeRates()
        {
            List<CurrencyExchangeRate> initialCurrencyExchangeRates = new List<CurrencyExchangeRate>();

            foreach (CurrencyExchangeRate currencyExchangeRate in _currencyExchangeRates)
            {
                if (currencyExchangeRate.StartingCurrency == _initialCurrency || currencyExchangeRate.EndingCurrency == _initialCurrency)
                {
                    initialCurrencyExchangeRates.Add(currencyExchangeRate);
                }
            }
            return initialCurrencyExchangeRates;
        }
        public PathFinder(List<CurrencyExchangeRate> currencyExchangeRates, string initialCurrency, string finalCurrency)
        {
            _currencyExchangeRates = currencyExchangeRates;
            _initialCurrency = initialCurrency;
            _finalCurrency = finalCurrency;
            _shortestPath = new List<CurrencyExchangeRate>();
            List<CurrencyExchangeRate> initialCurrencyExchangeRates = findInitialCurrencyExchangeRates();
            findShortestPath(initialCurrencyExchangeRates);
            checkCurrenciesToSwap();
        }
    }
}
