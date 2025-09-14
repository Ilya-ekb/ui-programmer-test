using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

using UIProgrammerTest.Service;
using UIProgrammerTest.View;

using UnityEngine.UI;

namespace Tests.PlayMode
{
    public class ExchangeCurrencyPlayModeTests
    {
        [UnityTest]
        public IEnumerator InvalidInput_ShowsError_AndNotStartOperation()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene");
            yield return null;

            var ui = Object.FindObjectOfType<UIManager>();
            ui.ShowExchangeWindow();
            var view = Object.FindObjectOfType<ExchangeCurrencyView>(true);
            var errWindow = Object.FindObjectOfType<ErrorWindow>(true);
            Assert.IsNotNull(view);
            Assert.IsNotNull(errWindow);

            var input = GetPrivate<TMP_InputField>(view, "coinInput");
            var btn = GetPrivate<Button>(view, "exchangeButton");
            var err = GetPrivate<TMP_Text>(errWindow, "errorMessage");

            input.text = "abc";
            btn.onClick.Invoke();
            yield return new WaitForSeconds(3.5f);

            Assert.IsTrue(err.gameObject.activeSelf);
            StringAssert.Contains("coinCount", err.text);
        }

        [UnityTest]
        public IEnumerator NotEnoughCoins_ShowsError()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene");
            yield return null;

            var ui = Object.FindObjectOfType<UIManager>();
            ui.ShowExchangeWindow();
            var view = Object.FindObjectOfType<ExchangeCurrencyView>(true);
            var errWindow = Object.FindObjectOfType<ErrorWindow>(true);
            Assert.IsNotNull(view);
            Assert.IsNotNull(errWindow);

            var input = GetPrivate<TMP_InputField>(view, "coinInput");
            var btn = GetPrivate<Button>(view, "exchangeButton");
            var err = GetPrivate<TMP_Text>(errWindow, "errorMessage");

            input.text = "999";
            btn.onClick.Invoke();
            yield return new WaitForSeconds(3.5f);

            Assert.IsTrue(err.gameObject.activeSelf);
            StringAssert.Contains("Not enough", err.text);
        }

        private T GetPrivate<T>(object obj, string field) where T : class
        {
            var f = obj.GetType().GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return f?.GetValue(obj) as T;
        }
    }
}
