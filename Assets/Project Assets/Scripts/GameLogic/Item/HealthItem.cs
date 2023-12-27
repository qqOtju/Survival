namespace Project_Assets.Scripts.GameLogic.Item
{
    public class HealthItem: ItemController
    {
        protected override void OnPlayerCollision()
        {
            GameData.Player.CurrentHealth += 1;
        }
    }
}