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

        [Header("Test Status Effect Icons")]
        [SerializeField] private Sprite[] testStatusIcons; // Just drag a bunch of icons here

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

        private void Update()
        {
            // Press T to test adding random status icons
            if (Input.GetKeyDown(KeyCode.T))
            {
                TestAddStatusIcons();
            }
            
            // Press R to remove all status icons
            if (Input.GetKeyDown(KeyCode.R))
            {
                TestRemoveStatusIcons();
            }
        }

        private void SetupCommandClickHandlers()
        {
            var attackCommand = _root.Q<VisualElement>("AttackCommand");
            var summonCommand = _root.Q<VisualElement>("SummonCommand");
            var itemCommand = _root.Q<VisualElement>("ItemCommand");
            var fleeCommand = _root.Q<VisualElement>("FleeCommand");

            attackCommand?.RegisterCallback<ClickEvent>(evt => ShowPage(_attackPage));
            summonCommand?.RegisterCallback<ClickEvent>(evt => ShowPage(_summonPage));
            itemCommand?.RegisterCallback<ClickEvent>(evt => ShowPage(_itemPage));
            fleeCommand?.RegisterCallback<ClickEvent>(evt => ShowPage(_fleePage));
        }

        private void SetupBackButtons()
        {
            // Find all back buttons - they're now VisualElements, not Labels
            var backButtons = _root.Query<VisualElement>(name: "BackButton").ToList();
            
            foreach (var backButton in backButtons)
            {
                // Make sure we're not in the CommandPage
                if (backButton.parent?.name != "CommandPage")
                {
                    backButton.RegisterCallback<ClickEvent>(evt => ShowPage(_commandPage));
                    Debug.Log($"Registered click handler for back button in {backButton.parent?.name}");
                }
            }
        }

        private void SetupSkillClickHandlers()
        {
            var allSkillItems = _root.Query<VisualElement>(className: "skill-item").ToList();
            
            foreach (var skillItem in allSkillItems)
            {
                skillItem.RegisterCallback<ClickEvent>(evt => 
                {
                    SelectSkill(skillItem);
                });
            }
        }

        private void ShowPage(VisualElement pageToShow)
        {
            // Hide all pages
            _commandPage.style.display = DisplayStyle.None;
            _attackPage.style.display = DisplayStyle.None;
            _summonPage.style.display = DisplayStyle.None;
            _itemPage.style.display = DisplayStyle.None;
            _fleePage.style.display = DisplayStyle.None;
            
            // Show requested page
            pageToShow.style.display = DisplayStyle.Flex;
            
            Debug.Log($"Showing page: {pageToShow.name}");
        }

        private void SelectSkill(VisualElement skillItem)
        {
            // Remove selection from previously selected skill
            if (_currentSelectedSkill != null)
            {
                _currentSelectedSkill.RemoveFromClassList("skill-item-selected");
            }
            
            // Add selection to new skill
            skillItem.AddToClassList("skill-item-selected");
            _currentSelectedSkill = skillItem;
            
            Debug.Log($"Selected skill: {skillItem.name}");
            
            // Here you would execute the skill
        }

        private void TestAddStatusIcons()
        {
            if (_activeHPBars.Count == 0 || testStatusIcons == null || testStatusIcons.Length == 0)
            {
                Debug.LogWarning("No HP bars or test icons available!");
                return;
            }

            // Add random icons to each enemy
            foreach (var hpBar in _activeHPBars)
            {
                // Add 1-3 random status effects
                int numEffects = Random.Range(1, 4);
                
                for (int i = 0; i < numEffects; i++)
                {
                    int randomIndex = Random.Range(0, testStatusIcons.Length);
                    string effectName = $"Effect{i}";
                    
                    hpBar.AddStatusEffectIcon(effectName, testStatusIcons[randomIndex]);
                }
            }
            
            Debug.Log("Added random status icons to enemies!");
        }

        private void TestRemoveStatusIcons()
        {
            foreach (var hpBar in _activeHPBars)
            {
                hpBar.ClearStatusEffects();
            }
            
            Debug.Log("Cleared all status icons!");
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
    }
}