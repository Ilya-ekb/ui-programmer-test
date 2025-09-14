namespace Loc
{
    public interface ILocalization
    {
        string Localize(string key, params object[] args);
    }
}

