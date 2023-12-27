using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Item
{
    public class EmptyItem: ItemController
    {
        protected override void OnPlayerCollision()
        {
            Debug.Log("EmptyItem");
        }
    }
}