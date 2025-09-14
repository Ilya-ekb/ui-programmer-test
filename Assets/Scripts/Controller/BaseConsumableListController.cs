using System;
using Loc;
using UIProgrammerTest.Consumable;
using UIProgrammerTest.Model;
using UIProgrammerTest.Service;
using UIProgrammerTest.View;
using UnityEngine;

namespace UIProgrammerTest.Controller
{
    public class BaseConsumableListController : MonoBehaviour
    {
        [SerializeField] protected ConsumableListView view;
        [SerializeField] protected ConsumableDatabaseAsset database;

        protected IGameModel model;
        protected IConsumableProvider provider;
        private ILocalization localization;

        protected virtual Func<string, string> Loc => key => localization.Localize(key);

        protected virtual void Awake()
        {
            //TODO: implement service into DI Container
            localization = new Localization();
            model = Adapter.Model;
            provider = new GameModelConsumableProvider(model, database);
        }

        protected virtual void OnEnable()
        {
            provider.Changed += Refresh;
            Refresh();
        }

        protected virtual void OnDisable()
        {
            provider.Changed -= Refresh;
        }

        protected virtual void Refresh()
        {
            if (!view)
            {
                return;
            }

            var vms = provider.GetAll();
            view.Render(vms, Loc, model.CoinBalance, model.CreditBalance);
        }
    }
}
