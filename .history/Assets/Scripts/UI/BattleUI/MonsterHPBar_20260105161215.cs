using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace NecromancersRising.UI
{
    public class MonsterHPBar : MonoBehaviour
    {
        [SerializeField] private Transform _targetMonster;
        [SerializeField] private Vector3 _offset = new Vector3(0, 1.15f, 0);
        [SerializeField] private string _monsterName = "Enemy";
        
        private VisualElement _hpBarContainer;
        private VisualElement _hpBarFill;
        private VisualElement _statusIconsContainer;
        private Camera _mainCamera;
        
        private Dictionary<string, VisualElement> _activeStatusIcons = new Dictionary<string, VisualElement>();

        public void Initialize(VisualElement hpBarsParent, Transform target, string monsterName)
        {
            _targetMonster = target;
            _monsterName = monsterName; // Set the name first
            _mainCamera = Camera.main;
            
            // Create HP bar UI elements
            _hpBarContainer = new VisualElement();
            _hpBarContainer.AddToClassList("monster-hp-container");
            _hpBarContainer.style.position = Position.Absolute;
            
            // Status icons container
            _statusIconsContainer = new VisualElement();
            _statusIconsContainer.AddToClassList("status-icons-row");
            _hpBarContainer.Add(_statusIconsContainer);
            
            // HP bar background
            var hpBarBg = new VisualElement();
            hpBarBg.AddToClassList("monster-hp-bar-bg");
            
            // HP bar fill
            _hpBarFill = new VisualElement();
            _hpBarFill.AddToClassList("monster-hp-bar-fill");
            _hpBarFill.style.width = Length.Percent(100);
            hpBarBg.Add(_hpBarFill);
            
            _hpBarContainer.Add(hpBarBg);
            
            // Monster name label - use the passed in name
            var nameLabel = new Label(_monsterName);
            nameLabel.AddToClassList("font");
            nameLabel.AddToClassList("monster-name-label");
            _hpBarContainer.Add(nameLabel);
            
            // Add to parent
            hpBarsParent.Add(_hpBarContainer);
        }

        private void LateUpdate()
        {
            if (_targetMonster == null || _hpBarContainer == null || _mainCamera == null) return;
            
            // Convert world position to screen position
            Vector3 worldPos = _targetMonster.position + _offset;
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos);
            
            // Check if monster is in front of camera
            if (screenPos.z > 0)
            {
                _hpBarContainer.style.display = DisplayStyle.Flex;
                
                // UI Toolkit uses screen coordinates directly
                // Center the bar by offsetting by half its width (100px / 2 = 50px)
                float leftPos = screenPos.x - 50;
                float topPos = Screen.height - screenPos.y;
                
                _hpBarContainer.style.left = leftPos;
                _hpBarContainer.style.top = topPos;
            }
            else
            {
                _hpBarContainer.style.display = DisplayStyle.None;
            }
        }

        public void SetMonsterName(string name)
        {
            _monsterName = name;
            
            // Update the label if it exists
            if (_hpBarContainer != null)
            {
                var nameLabel = _hpBarContainer.Q<Label>();
                if (nameLabel != null)
                {
                    nameLabel.text = name;
                }
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

        // NEW METHOD - Use sprite icons (recommended)
        public void AddStatusEffectIcon(string effectName, Sprite icon)
        {
            if (_activeStatusIcons.ContainsKey(effectName)) return;
            
            var statusIcon = new VisualElement();
            statusIcon.AddToClassList("status-icon");
            statusIcon.name = effectName;
            
            if (icon != null)
            {
                statusIcon.style.backgroundImage = new StyleBackground(icon);
                statusIcon.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
            }
            
            _statusIconsContainer.Add(statusIcon);
            _activeStatusIcons[effectName] = statusIcon;
        }

        // OLD METHOD - Use text/unicode (fallback if you don't have sprites)
        public void AddStatusEffect(string effectName, string iconText, Color iconColor)
        {
            if (_activeStatusIcons.ContainsKey(effectName)) return;
            
            var statusIcon = new Label(iconText);
            statusIcon.AddToClassList("status-icon");
            statusIcon.style.fontSize = 14;
            statusIcon.style.unityTextAlign = TextAnchor.MiddleCenter;
            statusIcon.style.color = iconColor;
            statusIcon.name = effectName;
            
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

        public void Cleanup()
        {
            if (_hpBarContainer != null && _hpBarContainer.parent != null)
            {
                _hpBarContainer.parent.Remove(_hpBarContainer);
            }
        }

        private void OnDestroy()
        {
            Cleanup();
        }
    }
}