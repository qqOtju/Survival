using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Gas", menuName = "MyAssets/Gas")]
    public class GasSO: ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _timeToDamage;
        
        public int Damage => _damage;
        public float TimeToDamage => _timeToDamage;
    }
}