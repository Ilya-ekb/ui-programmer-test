using System;

namespace UIProgrammerTest.Controller
{
    public interface IMessageProvider
    {
        event Action<bool, string> OnMessage;
    }
}