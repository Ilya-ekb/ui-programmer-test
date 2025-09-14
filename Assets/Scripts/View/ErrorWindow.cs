using TMPro;
using UnityEngine;

namespace UIProgrammerTest.View
{
    //TODO: It’s better to create an error display service and send error messages to it so that it decides how to present the errors.
    public class ErrorWindow : BaseUIView
    {
        [SerializeField] private TMP_Text errorMessage;

        public void SetMessageAndOpen(string message)
        {
            if(!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if(!errorMessage)
            {
                return;
            }

            errorMessage.text = message;
        }

        public void ClearAndClose()
        {
            gameObject.SetActive(false);
            if(!errorMessage)
            {
                return;
            }

            errorMessage.text = string.Empty;
        }
    }
}
