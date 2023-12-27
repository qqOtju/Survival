using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "GameDataSO", menuName = "MyAssets/GameDataSO")]
    public class GameDataSO: ScriptableObject
    {
        [SerializeField] private PlayerSO _playerSO;
        [SerializeField] private AbilitySO[] _abilitiesSO;
        [SerializeField] private SkinSO[] _skinsSO;
        [SerializeField] private WaveSO _waveSO;
        
        public PlayerSO PlayerSO => _playerSO;
        public AbilitySO[] AbilitiesSO => _abilitiesSO;
        public SkinSO[] SkinsSO => _skinsSO;
        public WaveSO WaveSO => _waveSO;
    }
}