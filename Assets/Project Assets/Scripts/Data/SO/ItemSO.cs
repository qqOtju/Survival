using Project_Assets.Scripts.GameLogic.Item;
using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "MyAssets/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private ItemController _itemPrefab;
        [SerializeField] private string _name;
        [SerializeField] private int _price;

        public ItemController ItemPrefab => _itemPrefab;
        public string Name => _name;
        public int Price => _price;
    }
}