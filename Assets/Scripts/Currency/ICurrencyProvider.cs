using System;

namespace UIProgrammerTest.Currency
{
    public interface ICurrencyProvider
    {
        ICurrencyVm Current { get; }

        event Action Changed;
    }
}