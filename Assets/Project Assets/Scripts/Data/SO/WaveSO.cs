using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Wave", menuName = "MyAssets/Wave")]
    public class WaveSO: ScriptableObject
    {
        [SerializeField] private float _duration;
        
        public float Duration => _duration;
    }
}