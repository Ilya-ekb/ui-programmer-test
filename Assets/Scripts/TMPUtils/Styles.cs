using UnityEngine;

namespace TMPUtils
{
    [CreateAssetMenu(menuName = "TMP Utils/GlobalStyles")]
    public class GlobalStyles : ScriptableObject
    {
        public string CoinIcon = "<sprite name=coin>";
        public string ArrowIcon = "<sprite name=arrow>";
        public string CreditsIcon = "<sprite name=credit>";
        public string CreditColorHex;
        public Color CreditColor = new(64, 231, 255, 255);
        public Color CoinColor = new(64, 231, 255, 255);
        public string CoinColorHex;
        public string ColorEnd = "</color>";

        private void OnValidate()
        {
            CoinColorHex = "<color=#" + ColorUtility.ToHtmlStringRGBA(CoinColor) + ">";
            CreditColorHex = "<color=#" + ColorUtility.ToHtmlStringRGBA(CreditColor) + ">";
        }
    }
}