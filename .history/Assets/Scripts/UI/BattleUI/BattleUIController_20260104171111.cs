using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace NecromancersRising.UI
{
    public class BattleUIController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _hpBarsContainer;
        
        // Pages
        private VisualElement _commandPage;
        private VisualElement _attackPage;
        private VisualElement _summonPage;
        private VisualElement _itemPage;
        private VisualElement _fleePage;
        
        private VisualElement _currentSelectedSkill;
        private List<MonsterHPBar> _activeHPBars = new List<MonsterHPBar>();

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            // Get HP bars container
            _hpBarsContainer = _root.Q<VisualElement>("HPBarsContainer");
            
            // Get page references
            _commandPage = _root.Q<VisualElement>("CommandPage");
            _attackPage = _root.Q<VisualElement>("AttackPage");
            _summonPage = _root.Q<VisualElement>("SummonPage");
            _itemPage = _root.Q<VisualElement>("ItemPage");
            _fleePage = _root.Q<VisualElement>("FleePage");
            
            SetupCommandClickHandlers();
            SetupBackButtons();
            SetupSkillClickHandlers();
            
            // Initialize HP bars for existing monsters
            InitializeMonsterHPBars();
        }

        private void InitializeMonsterHPBars()
        {
            // Find all MonsterHPBar components in the scene
            var hpBars = FindObjectsByType<MonsterHPBar>(FindObjectsSortMode.None);
            
            Debug.Log($"Found {hpBars.Length} HP bars to initialize");
            
            foreach (var hpBar in hpBars)
            {
                // The script is directly on the monster, so use its own transform
                Transform monsterTransform = hpBar.transform;
                
                Debug.Log($"Initializing HP bar for: {monsterTransform.name}");
                
                // Initialize the HP bar with the UI container
                hpBar.Initialize(_hpBarsContainer, monsterTransform);
                
                // Set initial HP
                hpBar.SetHP(100, 100);
                
                _activeHPBars.Add(hpBar);
            }
        }

        // Rest of your existing code...
        private void SetupCommandClickHandlers() { /* ... */ }
        private void SetupBackButtons() { /* ... */ }
        private void SetupSkillClickHandlers() { /* ... */ }
        private void ShowPage(VisualElement pageToShow) { /* ... */ }
        private void SelectSkill(VisualElement skillItem) { /* ... */ }
    }
}