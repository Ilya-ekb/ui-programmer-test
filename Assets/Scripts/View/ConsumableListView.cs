using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UIProgrammerTest.Consumable;
using UnityEngine;

namespace UIProgrammerTest.View
{
    public class ConsumableListView : BaseUIView
    {
        //click proxy
        public event Action<IConsumableVm> OnBuyClicked;

        public IReadOnlyList<ConsumableItemView> ConsumableViews => items;

        [SerializeField] private Transform contentRoot;
        [SerializeField] private ConsumableItemView itemPrefab;

        private readonly List<ConsumableItemView> items = new();

        public void Render([NotNull] IReadOnlyList<IConsumableVm> vms, Func<string, string> loc, int coinBalance, int creditBalance)
        {

            if (!contentRoot || !itemPrefab)
            {
                return;
            }

            var vmCount = vms.Count;

            var i = 0;
            for (; i < items.Count; i++)
            {
                var cell = items[i];
                if (i >= vmCount)
                {
                    if (cell.gameObject.activeSelf)
                    {
                        cell.gameObject.SetActive(false);
                    }

                    continue;
                }

                var vm = vms[i];

                if (!cell.gameObject.activeSelf)
                {
                    cell.gameObject.SetActive(true);
                }


                cell.OnBuy -= HandleBuy;
                cell.OnBuy += HandleBuy;

                var canBuy = vm.CanBuy(coinBalance, creditBalance);
                cell.Bind(vm, loc, canBuy);
            }

            for (; i < vmCount; i++)
            {
                var cell = Instantiate(itemPrefab, contentRoot);
                var vm = vms[i];

                cell.OnBuy -= HandleBuy;
                cell.OnBuy += HandleBuy;

                var canBuy = vm.CanBuy(coinBalance, creditBalance);
                cell.Bind(vm, loc, canBuy);
                items.Add(cell);
            }
        }

        private void HandleBuy(IConsumableVm vm) => OnBuyClicked?.Invoke(vm);
    }
}
