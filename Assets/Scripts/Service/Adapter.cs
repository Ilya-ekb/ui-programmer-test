using UIProgrammerTest.Model;
using UnityEngine;

namespace UIProgrammerTest.Service
{
    public class Adapter : MonoBehaviour
    {
        //TODO: Let’s imagine we take the model from the DI container
        public static IGameModel Model { get; } = new GameModelAdapter();

        private void Update() => Model.Update();
    }
}
