namespace UIProgrammerTest.Currency
{
    public sealed class CurrencyVm : ICurrencyVm
    {
        public int CoinBalance { get; }
        public int CreditBalance { get; }
        public int Rate { get; }

        public CurrencyVm(int coins, int credits, int rate)
        {
            CoinBalance = coins;
            CreditBalance = credits;
            Rate = rate;
        }
    }
}