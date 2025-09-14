using System.Linq;
using System.Text;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIProgrammerTest.View
{
    public class ExchangeCurrencyView : CurrencyView
    {
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text creditText;

        [Title("Exchange UI")] [SerializeField]
        private TMP_InputField coinInput;

        [SerializeField] private Button exchangeButton;
        [SerializeField] private TMP_Text resultText;

        [FormerlySerializedAs("OnExchangeButtonPressed")] public UnityEvent onExchangeButtonPressed;
        [FormerlySerializedAs("OnCoinsChanged")] public TMP_InputField.OnChangeEvent onCoinsChanged;

        public void SetResult(int rateResult)
        {
            if (resultText)
            {
                resultText.text = globalStyles.CreditColorHex + globalStyles.CreditsIcon +
                                  globalStyles.ColorEnd + rateResult;
            }
        }

        public void SetExchangeButtonInteractable(bool isInteractable)
        {
            if(exchangeButton)
            {
                exchangeButton.interactable = isInteractable;
            }
        }

        protected override void SetBalances(int coins, int credits)
        {
            if (!globalStyles)
            {
                Debug.LogWarning($"Set global styles for {name}");
                return;
            }
            if (coinText)
            {
                coinText.text = globalStyles.CoinColorHex + globalStyles.CoinIcon + coins + globalStyles.ColorEnd;
            }

            if (creditText)
            {
                creditText.text = globalStyles.CreditColorHex + globalStyles.CreditsIcon + credits +
                                  globalStyles.ColorEnd;
            }
        }

        private void OnEnable()
        {
            if (coinInput) coinInput.onValueChanged.AddListener(OnCoinChanged);
            if (exchangeButton) exchangeButton.onClick.AddListener(OnExchangeClick);
        }

        private void OnDisable()
        {
            if (coinInput)
            {
                coinInput.onValueChanged.RemoveListener(OnCoinChanged);
            }

            if (exchangeButton)
            {
                exchangeButton.onClick.RemoveListener(OnExchangeClick);
            }
        }

        private void OnExchangeClick() => onExchangeButtonPressed?.Invoke();

        private void OnCoinChanged(string arg0)
        {
            onCoinsChanged?.Invoke(arg0);
        }

        private int EnteredCoins(string value)
        {
            int.TryParse(FilterDigits(value), out var n);
            return n;
        }

        private static string FilterDigits(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";
            }

            var sb = new StringBuilder(s.Length);
            foreach (var c in s.Where(c => c is >= '0' and <= '9'))
            {
                sb.Append(c);
            }

            return sb.Length == 0 ? "0" : sb.ToString();
        }
    }
}
