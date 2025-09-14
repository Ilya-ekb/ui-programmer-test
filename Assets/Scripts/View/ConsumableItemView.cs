using System;
using TMPro;
using UIProgrammerTest.Consumable;
using UnityEngine;
using UnityEngine.UI;

namespace UIProgrammerTest.View
{
    public class ConsumableItemView : BaseUIView
    {
        public Action<IConsumableVm> OnBuy { get; set; }

        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text desc;
        [SerializeField] private TMP_Text price;
        [SerializeField] private TMP_Text count;
        [SerializeField] private Button buyButton;

        public void Bind(IConsumableVm vm, Func<string, string> loc, bool canBuyNow)
        {
            if (!globalStyles)
            {
                Debug.LogWarning($"Set global styles for {name}");
                return;
            }

            if (icon) icon.sprite = vm.Icon;
            if (title) title.text = loc?.Invoke(vm.NameKey).ToUpper() ?? vm.NameKey;
            if (desc) desc.text = loc?.Invoke(vm.DescriptionKey) ?? vm.DescriptionKey;
            if (price)
                price.text = vm.CoinPrice > 0
                    ? globalStyles.CoinIcon + vm.CoinPrice
                    : globalStyles.CreditsIcon + vm.CreditPrice;
            if (count) count.text = vm.Count.ToString();

            if (!buyButton) return;

            buyButton.image.color = vm.CoinPrice > 0 ? globalStyles.CoinColor : globalStyles.CreditColor;
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => OnBuy?.Invoke(vm));
            buyButton.interactable = canBuyNow;
        }

        public override void UpdateInteractability(bool value)
        {
            if (buyButton) buyButton.interactable = value;
        }
    }
}
