using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "New Level", menuName = "MyAssets/Level")]
    public class LevelSO: ScriptableObject
    {
        [SerializeField] private int _id;
        
        public int ID => _id;
    }
}