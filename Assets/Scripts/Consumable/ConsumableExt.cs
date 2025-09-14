namespace UIProgrammerTest.Consumable
{
    public static class ConsumableExt
    {
        public static bool CanBuy(this IConsumableVm vm, int coinBalance, int creditBalance)
        {
            if (vm == null)
            {
                return false;
            }

            if (vm.CoinPrice > 0 && coinBalance >= vm.CoinPrice)
            {
                return true;
            }

            return vm.CreditPrice > 0 && creditBalance >= vm.CreditPrice;
        }
    }
}
