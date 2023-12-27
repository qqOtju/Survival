using System;
using System.Collections.Generic;
using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Zenject;

namespace Project_Assets.Scripts.UI.MainMenu
{
    public class ShopUI: MonoBehaviour
    {
        [Tab("Abilities")]
        [SerializeField] private TextMeshProUGUI[] _abilitiesNames;
        [SerializeField] private Image[] _abilityImages;
        [SerializeField] private TextMeshProUGUI[] _priceTexts;
        [SerializeField] private TextMeshProUGUI[] _countTexts;
        [SerializeField] private Button[] _buyButtons;
        [Tab("Skins")]
        [SerializeField] private TextMeshProUGUI[] _skinsNames;
        [SerializeField] private Image[] _skinsImages;
        [SerializeField] private TextMeshProUGUI[] _skinsPriceTexts;
        [SerializeField] private Button[] _skinsBuyButtons;
        [Tab("Other")]
        [Header("Panels")]
        [SerializeField] private GameObject _shopPanel;
        [Header("Values")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [Header("Buttons")]
        [SerializeField] private Button _backButton;

        private readonly Dictionary<int, AbilityData> _abilitiesData = new();
        private readonly Dictionary<int, SkinData> _skinsData = new();
        private SoundManager _soundManager;
        private GameData _data;

        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        {
            _data = data;
            _soundManager = soundManager;
            if(_data.Abilities.Length > 0 && _abilitiesNames != null)
                foreach (var abilityData in _data.Abilities)
                {
                    var id = abilityData.Ability.ID;
                    _abilitiesNames[id].text = abilityData.Ability.Name;
                    _abilityImages[id].sprite = abilityData.Ability.Icon;
                    _priceTexts[id].text = abilityData.Ability.Price.ToString();
                    _countTexts[id].text = abilityData.Count.ToString();
                    _abilitiesData.Add(id, abilityData);
                    _buyButtons[id].onClick.AddListener(() => OnAbilityButton(id));
                }
            if(_data.Skins.Length > 0 && _skinsNames != null)
                foreach (var skinData in _data.Skins)
                {
                    var id = skinData.Skin.ID;
                    _skinsNames[id].text = skinData.Skin.Name;
                    _skinsImages[id].sprite = skinData.Skin.Sprite;
                    _skinsPriceTexts[id].text = skinData.SkinState switch
                    {
                        SkinState.Locked => skinData.Skin.Price.ToString(),
                        SkinState.Unlocked => "Equip",
                        SkinState.Equipped => "Equipped",
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    _skinsData.Add(id, skinData);
                    _skinsBuyButtons[id].onClick.AddListener(() => OnSkinButton(id));
                }
            SetGoldCount();
        }
        
        private void Awake() =>
            _backButton.onClick.AddListener(OnBackButtonClicked);

        private void OnDestroy()
        {
            foreach (var button in _buyButtons)
                button.onClick.RemoveAllListeners();
            foreach (var button in _skinsBuyButtons)
                button.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
        }

        private void OnSkinButton(int index)
        {
            _soundManager.PlayButtonClick();
            var skin = _skinsData[index];
            if(skin.SkinState == SkinState.Equipped) return;
            if(skin.SkinState == SkinState.Unlocked)
            {
                foreach (var skinData in _skinsData)
                {
                    if (skinData.Value.SkinState != SkinState.Equipped) continue;
                    skinData.Value.SkinState = SkinState.Unlocked;
                    break;
                }
                skin.SkinState = SkinState.Equipped;
            }
            else
            {
                if (_data.Gold.GoldCount < skin.Skin.Price) return;
                _data.Gold.GoldCount -= skin.Skin.Price;
                skin.SkinState = SkinState.Unlocked;
                SetGoldCount();
            }
            UpdateSkins();
        }
        
        private void OnAbilityButton(int index)
        {
            _soundManager.PlayButtonClick();
            var ability = _abilitiesData[index];
            if(_data.Gold.GoldCount < ability.Ability.Price) return;
            _data.Gold.GoldCount -= ability.Ability.Price;
            ability.Count++;
            _countTexts[ability.Ability.ID].text = "x" + ability.Count;
            SetGoldCount();
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _shopPanel.SetActive(false);
        }

        private void SetGoldCount() =>
            _goldText.text = _data.Gold.GoldCount.ToString();

        private void UpdateSkins()
        {
            foreach (var skinData in _skinsData)
            {
                var skin = skinData.Value;
                var id = skin.Skin.ID;
                _skinsNames[id].text = skin.Skin.Name;
                _skinsImages[id].sprite = skin.Skin.Sprite;
                _skinsPriceTexts[id].text = skin.Skin.Price.ToString();
            }
        }
    }
}