using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "TMP/Validators/Digits")]
public class DigitsValidator : TMP_InputValidator
{
    [SerializeField] private int maxDigitCount = 8;
    public override char Validate(ref string text, ref int pos, char ch)
    {
        if (ch is < '0' or > '9' || text.Length >= maxDigitCount)
        {
            return '\0';
        }

        text = text.Insert(pos, ch.ToString());
        pos++;
        return ch;
    }
}
