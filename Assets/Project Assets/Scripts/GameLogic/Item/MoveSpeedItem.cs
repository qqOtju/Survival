namespace Project_Assets.Scripts.GameLogic.Item
{
    public class MoveSpeedItem: ItemController
    {
        protected override void OnPlayerCollision()
        {
            GameData.Player.AdditionalMoveSpeed += 1;
        }
    }
}