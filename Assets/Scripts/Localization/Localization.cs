using System.Collections.Generic;
using System.Globalization;

namespace Loc
{
    //TODO: implement service into DI Container 
    public sealed class Localization : ILocalization
    {
        private readonly Dictionary<string, string> map = new()
        {
            ["error.title"] = "Ошибка",
            ["exchange.title"] = "Обмен валют",
            ["Ok"] = "Ок",
            ["button.exchange"] = "Обменять",
            ["button.cancel"] = "Отмена",
            
            ["consumable.title"] = "Резервы",
            ["consumable.medpack.name"] = "Медпакет",
            ["consumable.medpack.description"] = "При использованиии в бою восстанавливает 30 очков здоровья",
            ["consumable.armorplate.name"] = "Бронепластина",
            ["consumable.armorplate.description"] = "При использовании в бою восстанавливает весь запас брони",
                
            ["placeholder.int"] = "Введите количество..."
        };
        private readonly CultureInfo ru = new("ru-RU");

        public string Localize(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            var fmt = map.GetValueOrDefault(key, key);
            if (args == null || args.Length == 0) return fmt;
            try { return string.Format(ru, fmt, args); }
            catch { return fmt; }
        }
    }
}