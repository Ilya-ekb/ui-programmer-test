using System.Collections;
using NUnit.Framework;
using UIProgrammerTest;
using UIProgrammerTest.Controller;
using UIProgrammerTest.Service;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ExchangeCurrencyControllerNegativeTests
    {
        private ExchangeCurrencyController controller;


        [UnityTest]
        public IEnumerator Exchange_Fail_When_NotEnoughCoins()
        {
            SceneManager.LoadScene("Scene");
            yield return null;
            var uiManager = Object.FindObjectOfType<UIManager>();
            uiManager.ShowExchangeWindow();
            yield return null;
            controller = Object.FindObjectOfType<ExchangeCurrencyController>();
            yield return null;

            var result = true;
            controller.OnResultExchangeCurrency += (_, r, _) => result = r;
            controller.SendMessage("OnCoinsChanged", "500", SendMessageOptions.DontRequireReceiver);
            controller.SendMessage("OnExchange", SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds(3.5f);
            Adapter.Model.Update();
            yield return new WaitForSeconds(1f);

            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator Exchange_Fail_When_InputZero()
        {
            SceneManager.LoadScene("Scene");
            yield return null;
            var uiManager = Object.FindObjectOfType<UIManager>();
            uiManager.ShowExchangeWindow();
            yield return null;
            controller = Object.FindObjectOfType<ExchangeCurrencyController>();
            yield return null;

            var result = true;
            controller.OnResultExchangeCurrency += (_, r, _) => result = r;
            controller.SendMessage("OnExchange", SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds(2f);

            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator Exchange_Many_Operation_PerFrame()
        {
            SceneManager.LoadScene("Scene");
            yield return null;
            var uiManager = Object.FindObjectOfType<UIManager>();
            uiManager.ShowExchangeWindow();
            yield return null;
            controller = Object.FindObjectOfType<ExchangeCurrencyController>();
            yield return null;

            var result = true;
            controller.OnResultExchangeCurrency += (_, r, _) => result = r;
            for (var i = 0; i < 181; i++)
            {
                controller.SendMessage("OnCoinsChanged", "1", SendMessageOptions.DontRequireReceiver);
                controller.SendMessage("OnExchange", SendMessageOptions.DontRequireReceiver);
            }

            yield return new WaitForSeconds(3.5f);
            Adapter.Model.Update();
            yield return new WaitForSeconds(1f);

            Assert.IsFalse(result);

        }
    }
}
