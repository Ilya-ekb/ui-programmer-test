
using NUnit.Framework;
using Tests.Common;
using UIProgrammerTest.Currency;

namespace Tests.EditMode
{
    public class GameModelCurrencyProviderTests
    {
        [Test]
        public void Rebuild_UsesCoinAndCredit_FromModel()
        {
            var model = new FakeGameModel();
            var provider = new GameModelCurrencyProvider(model);
            var vm = provider.Current;
            Assert.AreEqual(model.CoinBalance, vm.CoinBalance, "CoinBalance should mirror model");
            // This assertion will fail if provider mistakenly copies CoinBalance into CreditBalance.
            Assert.AreEqual(model.CreditBalance, vm.CreditBalance, "CreditBalance should mirror model");
            Assert.AreEqual(model.ConversionRate, vm.Rate);
        }

        [Test]
        public void ChangeEvent_Fires_OnModelChange()
        {
            var model = new FakeGameModel();
            var provider = new GameModelCurrencyProvider(model);
            bool fired = false;
            provider.Changed += () => fired = true;
            model.ConvertCoinsToCredits(10);
            Assert.IsTrue(fired, "Changed should fire after model mutation");
        }
    }
}
