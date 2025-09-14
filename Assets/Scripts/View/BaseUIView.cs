using TMPUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace UIProgrammerTest.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUIView : MonoBehaviour
    {
        [FormerlySerializedAs("_globalStyles")] [SerializeField] protected GlobalStyles globalStyles;
        private CanvasGroup canvasGroup;

        public virtual void UpdateInteractability(bool value)
        {
            canvasGroup ??= GetComponent<CanvasGroup>();
            canvasGroup.interactable = value;
        }
    }
}
