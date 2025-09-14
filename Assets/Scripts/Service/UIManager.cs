using UIProgrammerTest.Controller;
using UIProgrammerTest.View;
using UnityEngine;

namespace UIProgrammerTest.Service
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject consumableWindow;
        [SerializeField] private GameObject exchangeWindow;
        [SerializeField] private GameObject modalOverlay;
        [SerializeField] private ErrorWindow errorWindow;
        private IMessageProvider[] messageProviders;

        private void Awake()
        {
            messageProviders = GetComponentsInChildren<IMessageProvider>(true);
        }

        private void OnEnable()
        {
            foreach (var provider in messageProviders)
            {
                provider.OnMessage += OnMessageProvide;
            }
        }

        private void OnDisable()
        {
            foreach (var provider in messageProviders)
            {
                provider.OnMessage -= OnMessageProvide;
            }
        }

        public void ShowConsumableWindow()
        {
            CloseAll();
            SetModal(true);
            if (consumableWindow)
            {
                consumableWindow.SetActive(true);
            }
        }

        public void ShowExchangeWindow()
        {
            CloseAll();
            SetModal(true);
            if (exchangeWindow)
            {
                exchangeWindow.SetActive(true);
            }
        }

        public void CloseAll()
        {
            if (consumableWindow)
            {
                consumableWindow.SetActive(false);
            }

            if (exchangeWindow)
            {
                exchangeWindow.SetActive(false);
            }

            if (errorWindow)
            {
                errorWindow.gameObject.SetActive(false);
            }

            SetModal(false);
        }

        private void SetModal(bool v)
        {
            if (modalOverlay)
            {
                modalOverlay.SetActive(v);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseAll();
            }
        }

        private void OnMessageProvide(bool isSuccess, string message)
        {
            if (!errorWindow || isSuccess)
            {
                return;
            }

            SetModal(true);
            errorWindow.SetMessageAndOpen(message);
        }
    }
}
