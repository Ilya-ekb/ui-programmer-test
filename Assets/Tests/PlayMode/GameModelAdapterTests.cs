using System.Collections;
using NUnit.Framework;
using UIProgrammerTest.Service;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class GameModelAdapterTests
    {
        private GameModelAdapter adapter;

        [SetUp]
        public void SetUp()
        {
            adapter = new GameModelAdapter();
        }

        [UnityTest]
        public IEnumerator WaitAsync_Fail_When_Zero()
        {
            GameModel.ConvertCoinToCredit(0);
            var result = true;
            adapter.OperationComplete += _ =>  result = false;
            adapter.ConvertCoinsToCredits(5);
            yield return new WaitForSeconds(3.5f);
            adapter.Update();
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator Two_Parallel_Operations_DoNot_Interfere()
        {
            var a = GameModel.ConvertCoinToCredit(3);
            var ra = false;
            var rb = false;

            adapter.OperationComplete += or => ra = or.Guid == a || ra;

            var b = GameModel.ConvertCoinToCredit(7);
            adapter.OperationComplete += or => rb = or.Guid == b || rb;

            yield return new WaitForSeconds(3.5f);
            adapter.Update();
            Assert.IsTrue(ra, "Operation A result must be true");
            Assert.IsTrue(rb, "Operation B result must be true");
        }

        [UnityTest]
        public IEnumerator ConvertCoinsToCredits_TriggersOperationComplete()
        {
            bool gotEvent = false;
            adapter.OperationComplete += _ => gotEvent = true;
            adapter.ConvertCoinsToCredits(5);
            yield return new WaitForSeconds(3.5f);
            adapter.Update();
            Assert.IsTrue(gotEvent, "OperationComplete should be raised by adapter");
        }

        [Test]
        public void PriceCache_IsBuilt_AndHasEntries()
        {
            Assert.That(adapter.ConsumablesPrice.Count, Is.GreaterThan(0),
                "Consumable price cache should not be empty");
        }
    }
}
