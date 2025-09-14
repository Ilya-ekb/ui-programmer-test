using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Tests.Common;
using UIProgrammerTest.Consumable;
using UIProgrammerTest.Controller;
using UIProgrammerTest.Model;
using UIProgrammerTest.View;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class PurchasableConsumableListControllerPlayTests
    {
        private FakeGameModel model;
        private ConsumableDatabaseAsset db;
        private ConsumableTypeId id;
        private ConsumableDatabaseAsset.Entry entry;
        private GameObject go;
        private ConsumableListView view;
        private PurchasableConsumableListController controller;

        [SetUp]
        public void Setup()
        {
            model = new FakeGameModel();
            go = new GameObject("consumables");
            view = go.AddComponent<ConsumableListView>();
            db = ScriptableObject.CreateInstance<ConsumableDatabaseAsset>();
            id = new ConsumableTypeId(1);
            entry = new ConsumableDatabaseAsset.Entry
            {
                id = id,
                meta = new ConsumableMeta
                {
                    nameKey = null,
                    descriptionKey = null,
                    icon = null
                }
            };

            var entries = (List<ConsumableDatabaseAsset.Entry>)typeof(ConsumableDatabaseAsset)
                .GetField("entries", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(db);
            entries?.Add(entry);
            controller = go.AddComponent<PurchasableConsumableListController>();
            var type = typeof(PurchasableConsumableListController);

            type.GetField("model", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, model);
            type.GetField("view", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, view);
            type.GetField("database", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, db);
            type.GetField("provider", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, new GameModelConsumableProvider(model, db));
            type.GetField("purchaseActor", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(controller, new FakePurchaseActor());
        }

        [TearDown]
        public void Teardown()
        {
            model = null;
            id = default;
            view = null;
        }

        [UnityTest]
        public IEnumerator Purchase_Flow_Raises_Callback_And_Updates_Model()
        {
            bool raised = false;
            controller.OnPurchaseResult += (_, ok, __) => raised = ok;
            var vm = new FakeVm
            {
                Id = entry.id,
                CoinPrice = 5
            };
            controller.SendMessage("OnBuy", vm);
            yield return null;
            yield return null;
            Assert.IsTrue(raised, "Purchase callback should be raised");
            Object.DestroyImmediate(go);
        }
    }
}
