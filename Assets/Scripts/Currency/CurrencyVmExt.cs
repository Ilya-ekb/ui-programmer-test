namespace UIProgrammerTest.Currency
{
    public static class CurrencyVmExt
    {
        public static bool CanExchange(this ICurrencyVm vm, int amount)
        {
            return amount > 0 && amount <= vm.CoinBalance;
        }
    }
}