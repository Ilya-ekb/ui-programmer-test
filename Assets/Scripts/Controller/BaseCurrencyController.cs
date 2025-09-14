using UIProgrammerTest.Currency;
using UIProgrammerTest.Model;
using UIProgrammerTest.Service;
using UIProgrammerTest.View;
using UnityEngine;

namespace UIProgrammerTest.Controller
{
    public class BaseCurrencyController : MonoBehaviour
    {
        [SerializeField] protected CurrencyView view;

        protected IGameModel model;
        protected ICurrencyProvider currency;

        protected virtual void Awake()
        {
            if (!view)
            {
                view = GetComponent<CurrencyView>();
            }

            model = Adapter.Model;
            currency = new GameModelCurrencyProvider(model);
        }

        protected virtual void OnEnable()
        {
            currency.Changed += OnCurrencyChanged;
            Sync();
        }

        protected virtual void OnDisable()
        {
            currency.Changed -= OnCurrencyChanged;
        }

        protected virtual void OnCurrencyChanged() => Sync();

        protected virtual void UpdateInteractability()
        {
            if (!view)
            {
                return;
            }

            view.UpdateInteractability(!model.HasRunningOperations);
        }

        protected virtual void Sync()
        {
            if (!view)
            {
                return;
            }

            view.Bind(currency.Current, model.HasRunningOperations);
            UpdateInteractability();
        }
    }
}
