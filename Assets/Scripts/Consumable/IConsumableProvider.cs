using System;
using System.Collections.Generic;

namespace UIProgrammerTest.Consumable
{
    public interface IConsumableProvider
    {
        event Action Changed;
        IReadOnlyList<IConsumableVm> GetAll();
    }
}
