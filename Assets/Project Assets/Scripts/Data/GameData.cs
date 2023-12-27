using Project_Assets.Scripts.Data.SO;

namespace Project_Assets.Scripts.Data
{
    public class GameData
    {
        public Gold Gold { get; }
        public Score Score { get; }
        public AbilityData[] Abilities { get; }
        public SkinData[] Skins { get; }
        public Player Player { get; }
        public Wave Wave { get; }

        public GameData(AbilitySO[] abilitiesSO, SkinSO[] skinsSO, PlayerSO playerSO, WaveSO waveSO)
        {
            Abilities = new AbilityData[abilitiesSO.Length];
            for (var i = 0; i < abilitiesSO.Length; i++)
                Abilities[i] = new AbilityData(abilitiesSO[i]);
            Skins = new SkinData[skinsSO.Length];
            for (var i = 0; i < skinsSO.Length; i++)
                Skins[i] = new SkinData(skinsSO[i]);
            Player = new Player(playerSO);
            Gold = new Gold();
            Score = new Score();
            Wave = new Wave(waveSO);
        }
    }
}