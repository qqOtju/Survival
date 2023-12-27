using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Skin", menuName = "MyAssets/Skin")]
    public class SkinSO: ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [Header("Values")]
        [SerializeField] private int _price;

        public int ID => _id;
        public string Name => _name;
        public Sprite Sprite => _sprite;
        public int Price => _price;
    }
}