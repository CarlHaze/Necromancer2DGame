using UnityEngine;
using UnityEngine.UIElements;

namespace NecromancersRising.UI
{
    public class BattleUIController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _root;
        
        // Pages
        private VisualElement _commandPage;
        private VisualElement _attackPage;
        private VisualElement _summonPage;
        private VisualElement _itemPage;
        private VisualElement _fleePage;
        
        private VisualElement _currentSelectedSkill;

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            // Get page references
            _commandPage = _root.Q<VisualElement>("CommandPage");
            _attackPage = _root.Q<VisualElement>("AttackPage");
            _summonPage = _root.Q<VisualElement>("SummonPage");
            _itemPage = _root.Q<VisualElement>("ItemPage");
            _fleePage = _root.Q<VisualElement>("FleePage");
            
            SetupCommandClickHandlers();
            SetupBackButtons();
            SetupSkillClickHandlers();
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
    }
}