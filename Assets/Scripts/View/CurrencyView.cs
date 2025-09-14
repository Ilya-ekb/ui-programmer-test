using TMPro;
using TriInspector;
using UnityEngine;
using UIProgrammerTest.Currency;

namespace UIProgrammerTest.View
{
    public class CurrencyView : BaseUIView
    {
        [Title("Bindings")]
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private TMP_Text rateText;

        public virtual void Bind(ICurrencyVm vm, bool blockAll)
        {
            if (vm == null) return;
            SetBalances(vm.CoinBalance, vm.CreditBalance);
            SetRate(vm.Rate);
            UpdateInteractability(blockAll);
        }

        protected virtual void SetBalances(int coins, int credits)
        {
            if (!currencyText)
            {
                return;
            }

            if (!globalStyles)
            {
                Debug.LogWarning($"Set global styles for {name}");
                return;
            }

            currencyText.text = globalStyles.CoinColorHex + globalStyles.CoinIcon + coins +
                                globalStyles.ColorEnd +
                                globalStyles.ArrowIcon +
                                globalStyles.CreditColorHex + globalStyles.CreditsIcon + credits +
                                globalStyles.ColorEnd;
        }

        private void SetRate(int coinToCreditRate)
        {
            if (!rateText )
            {
                return;
            }

            if (!globalStyles)
            {
                Debug.LogWarning($"Set global styles for {name}");
                return;
            }

            rateText.text = globalStyles.CoinColorHex + globalStyles.CoinIcon + globalStyles.ColorEnd + "1 = " +
                            globalStyles.CreditColorHex + globalStyles.CreditsIcon + globalStyles.ColorEnd +
                            coinToCreditRate;
        }
    }
}
