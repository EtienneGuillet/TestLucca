using System.Globalization;

namespace LuccaDevises
{
    internal class Parser
    {
        private string _initialCurrency;
        private float _amount;
        private string _finalCurrency;
        List<CurrencyExchangeRate> _currencyExchangeRates;

        public string InitialCurrency { get => _initialCurrency; set => _initialCurrency = value; }
        public float Amount { get => _amount; set => _amount = value; }
        public string FinalCurrency { get => _finalCurrency; set => _finalCurrency = value; }
        internal List<CurrencyExchangeRate> CurrencyExchangeRates { get => _currencyExchangeRates; set => _currencyExchangeRates = value; }

        private void Error(string errorText = "")
        {
            if (errorText != "")
            {
                Console.Error.WriteLine(errorText);
            }
            Environment.Exit(-1);
        }

        private void firstLineError(string firstLine)
        {
            string errorMessage = "The first line must contain:\n";
            errorMessage += "\t\tD1 the initial currency composed of 3 letters\n";
            errorMessage += "\t\tM the amount of the initial currency as a positive integer\n";
            errorMessage += "\t\tD2 the initial currency composed of 3 letters\n";
            errorMessage += "Such as:\n";
            errorMessage += "\t\tD1;M;D2\n";
            errorMessage += "Current first line:\n";
            errorMessage += "\t\t" + firstLine;
            Error(errorMessage);
        }

        private void currencyCodeNameError(string currencyCodeName)
        {
            string errorMessage = "The currency code name must be a string composed of 3 characters\n";
            errorMessage += "Example: 'USD'\n";
            errorMessage += "Here the currency code is: " + currencyCodeName;
            Error(errorMessage);
        }
        private void notPosIntError(string amount, string varName)
        {
            string errorMessage = "The " + varName + " must be a positive integer\n";
            errorMessage += "Amount here: " + amount;
            Error(errorMessage);
        }

        private void displayExchangeRateNumberNotMatchError(int exchangeRateNumber, int numberOfLines)
        {
            Console.WriteLine("The exchange rate number line 2 of the file must match the number of lines after it");
            Console.WriteLine("Exchange rate number: " + exchangeRateNumber + " number of lines: " + numberOfLines);
        }

        private bool checkCurrencyCodeName(string currencyCodeName)
        {
            if (currencyCodeName.Length != 3)
            {
                return false;
            }
            return true;
        }

        private bool isPosInt(string amount)
        {
            if (!int.TryParse(amount, out int x) || x <= 0)
            {
                return false;
            }
            return true;
        }

        private bool isPosFloatWith4Decimal(string number)
        {
            string[] values = number.Split('.');

            if (values.Length != 2)
            {
                return false;
            }
            if (!int.TryParse(values[0], out _) || !isPosInt(values[1]))
            {
                return false;
            }
            if (values[1].Length != 4)
            {
                return false;
            }
            return true;
        }

        private void getValuesOfFirstLine(string firstLine)
        {
            string[] values = firstLine.Split(';');

            if (values.Length < 3)
            {
                firstLineError(firstLine);
                Environment.Exit(-1);
            }

            if (!checkCurrencyCodeName(values[0]))
            {
                currencyCodeNameError(values[0]);
            }
            _initialCurrency = values[0];

            if (!isPosInt(values[1]))
            {
                notPosIntError(values[1], "amount");
            }
            _amount = int.Parse(values[1]);

            if (!checkCurrencyCodeName(values[2]))
            {
                currencyCodeNameError(values[2]);
            }
            _finalCurrency = values[2];
        }

        private int findExchangeRateNumber(string[] fileLines)
        {
            int exchangeRateNumber;

            if (!isPosInt(fileLines[1]))
            {

                Environment.Exit(-1);
            }

            exchangeRateNumber = int.Parse(fileLines[1]);
            if (exchangeRateNumber != fileLines.Length - 2)
            {
                displayExchangeRateNumberNotMatchError(exchangeRateNumber, fileLines.Length - 2);
                Environment.Exit(-1);
            }
            return exchangeRateNumber;
        }

        private void checkFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Error("Unable to open the file '" + filePath + "'");
            }
        }

        private string[] getFileContent(string filePath)
        {
            string[] fileLines = File.ReadAllText(filePath).Split(Environment.NewLine);

            if (fileLines.Length < 3)
            {
                Error("The file must at least have 3 lines but there is only " + fileLines.Length);
            }
            return fileLines;
        }

        private void checkExchangeRatesContainsInitialCurrency()
        {
            foreach (CurrencyExchangeRate currencyExchangeRate in _currencyExchangeRates)
            {
                if (currencyExchangeRate.StartingCurrency == _initialCurrency)
                {
                    return;
                }
            }
            Error("There is no exchange rate containing the starting currency: " + _initialCurrency);
        }

        private void checkExchangeRatesContainsFinalCurrency()
        {
            foreach (CurrencyExchangeRate currencyExchangeRate in _currencyExchangeRates)
            {
                if (currencyExchangeRate.EndingCurrency == _finalCurrency)
                {
                    return;
                }
            }
            Error("There is no exchange rate containing the ending currency: " + _finalCurrency);
        }

        private void parseExchangeRates(string[] fileLines)
        {

            _currencyExchangeRates = new List<CurrencyExchangeRate>();
            foreach (string line in fileLines.Skip(2))
            {
                string[] exchangeRate = line.Split(';');

                if (exchangeRate.Length != 3)
                {
                    Error("Exchange rates must be composed of starting currency CS, ending currency CE, and exchange rate ER such as CS;CE;ER\nHere: " + line);
                }
                else if (!checkCurrencyCodeName(exchangeRate[0]))
                {
                    currencyCodeNameError(exchangeRate[0]);
                }
                else if (!checkCurrencyCodeName(exchangeRate[1]))
                {
                    currencyCodeNameError(exchangeRate[0]);
                }
                else if (!isPosFloatWith4Decimal(exchangeRate[2]))
                {
                    Error("Exchange rate must be a float with 4 decimal\nHere: " + exchangeRate[2]);
                }
                float rate = float.Parse(exchangeRate[2], CultureInfo.InvariantCulture.NumberFormat);
                _currencyExchangeRates.Add(new CurrencyExchangeRate(rate, exchangeRate[0], exchangeRate[1]));
            }
        }
        public Parser(string filePath)
        {
            checkFile(filePath);
            string[] fileLines = getFileContent(filePath);
            getValuesOfFirstLine(fileLines[0]);
            findExchangeRateNumber(fileLines);
            parseExchangeRates(fileLines);
            checkExchangeRatesContainsInitialCurrency();
            checkExchangeRatesContainsFinalCurrency();
        }
    }
}
