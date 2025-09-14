namespace UIProgrammerTest.Currency
{
    public interface ICurrencyVm
    {
        int CoinBalance { get; }
        int CreditBalance { get; }
        int Rate { get; }
    }
}