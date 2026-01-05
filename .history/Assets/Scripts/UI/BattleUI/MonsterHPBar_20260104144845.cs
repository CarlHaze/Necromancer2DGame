using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace NecromancersRising.UI
{
    public class MonsterHPBar : MonoBehaviour
    {
        [SerializeField] private UIDocument _hpBarDocument;
        [SerializeField] private Transform _targetMonster;
        [SerializeField] private Vector3 _offset = new Vector3(0, 1.5f, 0);
        
        private VisualElement _root;
        private VisualElement _hpBarFill;
        private VisualElement _statusIconsContainer;
        private Label _monsterNameLabel;
        
        private Camera _mainCamera;
        private Dictionary<string, VisualElement> _activeStatusIcons = new Dictionary<string, VisualElement>();

        private void Awake()
        {
            _mainCamera = Camera.main;
            
            if (_hpBarDocument != null)
            {
                _root = _hpBarDocument.rootVisualElement.Q<VisualElement>("MonsterHPBarContainer");
                _hpBarFill = _root.Q<VisualElement>("HPBarFill");
                _statusIconsContainer = _root.Q<VisualElement>("StatusIconsContainer");
                _monsterNameLabel = _root.Q<Label>("MonsterName");
            }
        }

        private void LateUpdate()
        {
            if (_targetMonster == null || _root == null) return;
            
            // Convert world position to screen position
            Vector3 worldPos = _targetMonster.position + _offset;
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos);
            
            // Check if monster is in front of camera
            if (screenPos.z > 0)
            {
                _root.style.display = DisplayStyle.Flex;
                
                // Convert to UI Toolkit coordinates (origin is top-left)
                float screenHeight = Screen.height;
                _root.style.left = screenPos.x - 60; // Center the 120px wide bar
                _root.style.top = screenHeight - screenPos.y - 50; // Flip Y axis
                _root.style.position = Position.Absolute;
            }
            else
            {
                _root.style.display = DisplayStyle.None;
            }
        }

        public void SetHP(float currentHP, float maxHP)
        {
            if (_hpBarFill == null) return;
            
            float hpPercent = Mathf.Clamp01(currentHP / maxHP);
            _hpBarFill.style.width = Length.Percent(hpPercent * 100);
            
            // Change color based on HP percentage
            if (hpPercent > 0.5f)
                _hpBarFill.style.backgroundColor = new Color(0.6f, 0.2f, 0.2f);
            else if (hpPercent > 0.25f)
                _hpBarFill.style.backgroundColor = new Color(0.8f, 0.4f, 0.1f);
            else
                _hpBarFill.style.backgroundColor = new Color(0.9f, 0.2f, 0.1f);
        }

        public void SetMonsterName(string name)
        {
            if (_monsterNameLabel != null)
                _monsterNameLabel.text = name;
        }

        public void AddStatusEffect(string effectName, Sprite icon)
        {
            if (_activeStatusIcons.ContainsKey(effectName)) return;
            
            var statusIcon = new VisualElement();
            statusIcon.AddToClassList("status-icon");
            statusIcon.name = effectName;
            
            if (icon != null)
            {
                statusIcon.style.backgroundImage = new StyleBackground(icon);
                // Instead of unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                statusIcon.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);           
            }
            
            _statusIconsContainer.Add(statusIcon);
            _activeStatusIcons[effectName] = statusIcon;
        }

        public void RemoveStatusEffect(string effectName)
        {
            if (!_activeStatusIcons.ContainsKey(effectName)) return;
            
            var icon = _activeStatusIcons[effectName];
            _statusIconsContainer.Remove(icon);
            _activeStatusIcons.Remove(effectName);
        }

        public void ClearStatusEffects()
        {
            _statusIconsContainer.Clear();
            _activeStatusIcons.Clear();
        }
    }
}