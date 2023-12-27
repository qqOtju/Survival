namespace Project_Assets.Scripts.GameLogic.Item
{
    public class DoubleScoreItem: ItemController
    {
        protected override void OnPlayerCollision()
        {
            GameData.Score.DoubleScore = true;
        }
    }
}