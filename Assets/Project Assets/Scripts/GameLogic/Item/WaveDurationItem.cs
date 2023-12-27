namespace Project_Assets.Scripts.GameLogic.Item
{
    public class WaveDurationItem: ItemController
    {
        protected override void OnPlayerCollision()
        {
            GameData.Wave.AdditionalWaveDuration = -3;
        }
    }
}