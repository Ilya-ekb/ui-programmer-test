using System.Collections;
using NUnit.Framework;
using Tests.Common;
using UIProgrammerTest;
using UIProgrammerTest.Controller;
using UIProgrammerTest.Model;
using UIProgrammerTest.Service;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ConsumableControllerNegativeTests
    {
        private PurchasableConsumableListController controller;

        [UnityTest]
        public IEnumerator Buy_Fail_When_InvalidPrice()
        {
            SceneManager.LoadScene("Scene");
            yield return null;
            var uiManager = Object.FindObjectOfType<UIManager>();
            uiManager.ShowConsumableWindow();
            yield return null;
            controller = Object.FindObjectOfType<PurchasableConsumableListController>();
            yield return null;

            var result = true;
            controller.OnPurchaseResult += (_, r, _) => result = r;
            var vm = new FakeVm
            {
                Id = new ConsumableTypeId(1)
            };
            controller.SendMessage("OnBuy", vm, SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds(3.5f);
            Adapter.Model.Update();
            yield return new WaitForSeconds(1f);

            Assert.IsFalse(result);
        }
    }
}
