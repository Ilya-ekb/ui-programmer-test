using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Tests.Common;
using UIProgrammerTest.Controller;
using UIProgrammerTest.Currency;
using UIProgrammerTest.View;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ExchangeCurrencyControllerPlayTests
    {
        GameObject go;
        ExchangeCurrencyController controller;
        ExchangeCurrencyView view;

        [SetUp]
        public void Setup()
        {
            go = new GameObject("exchange");
            view = go.AddComponent<ExchangeCurrencyView>();
            controller = go.AddComponent<ExchangeCurrencyController>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(go);
        }

        [UnityTest]
        public IEnumerator Exchange_Success_UpdatesBalances_AndEvents()
        {
            var model = Prepare(100, 10, 10);

            bool gotResult = false;
            controller.OnResultExchangeCurrency += (_, ok, _) => gotResult = ok;

            controller.SendMessage("OnCoinsChanged", "20", SendMessageOptions.DontRequireReceiver); // if method exists
            controller.SendMessage("OnExchange", SendMessageOptions.DontRequireReceiver);

            yield return null;
            yield return null;

            Assert.IsTrue(gotResult, "Controller should raise success result");
            Assert.Less(model.CoinBalance, 100, "Coins should decrease");
            Assert.GreaterOrEqual(model.CreditBalance, 10, "Credits should increase");
        }

        private FakeGameModel Prepare(int coinCount = 0, int creditCount = 0, int rate = 0)
        {
            var model = new FakeGameModel(coinCount, creditCount, rate);
            var provider = new GameModelCurrencyProvider(model);
            var exchanger = new FakeCoinExchanger(model, delayMs: 1);
            var type = typeof(ExchangeCurrencyController);


            type.GetField("model", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, model);
            type.GetField("currency", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, provider);
            type.GetField("view", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, view);
            type.GetField("exchangeService", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(controller, exchanger);
            return model;
        }
    }
}
