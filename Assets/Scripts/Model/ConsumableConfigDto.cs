namespace UIProgrammerTest.Model
{
    public readonly struct ConsumableConfigDto
    {
        public int CoinPrice { get; }
        public int CreditPrice { get; }

        public ConsumableConfigDto(int coinPrice, int creditPrice)
        {
            CoinPrice = coinPrice;
            CreditPrice = creditPrice;
        }
    }
}
