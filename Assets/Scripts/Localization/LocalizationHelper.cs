using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Loc
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizationHelper : MonoBehaviour
    {
        private enum TextStyle
        {
            Normal,
            ToLower,
            ToUpper,
        }

        [FormerlySerializedAs("_textStyle")] [SerializeField] private TextStyle textStyle = TextStyle.Normal;
        private ILocalization localization;
        private TMP_Text text;

        private void OnEnable()
        {
            //TODO: implement service into DI Container
            localization ??= new Localization();
            if(!text )
            {
                text = GetComponent<TMP_Text>();
            }

            var value = localization.Localize(name.ToLower());

            text.text = textStyle switch
            {
                TextStyle.Normal => value,
                TextStyle.ToLower => value.ToLower(),
                TextStyle.ToUpper => value.ToUpper(),
                _ => string.Empty
            };
        }
    }
}
