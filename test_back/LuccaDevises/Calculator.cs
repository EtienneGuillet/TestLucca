namespace LuccaDevises
{
    internal class Calculator
    {
        private float _finalAmount;
        public float FinalAmount { get => _finalAmount; set => _finalAmount = value; }

        private void calculateAfterExchangeAmount(List<CurrencyExchangeRate> shortestPath, float amount)
        {
            float currentAmount = shortestPath.First().exchangeCurrency(amount);

            for (int i = 1; i < shortestPath.Count; i++)
            {
                currentAmount = shortestPath[i].exchangeCurrency(currentAmount);
            }
            _finalAmount = currentAmount;
        }

        public Calculator(List<CurrencyExchangeRate> shortestPath, float amount)
        {
            calculateAfterExchangeAmount(shortestPath, amount);
        }
    }
}
