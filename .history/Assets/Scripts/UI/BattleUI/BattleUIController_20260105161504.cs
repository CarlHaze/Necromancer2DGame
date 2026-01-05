using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using NecromancersRising.Battle;

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
        [SerializeField] private Sprite[] testStatusIcons;

        [Header("Battle Manager")]
        [SerializeField] private BattleManager _battleManager;

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
            // Use new Input System instead of old Input class
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // Press T to test adding random status icons
            if (keyboard.tKey.wasPressedThisFrame)
            {
                TestAddStatusIcons();
            }
            
            // Press R to remove all status icons
            if (keyboard.rKey.wasPressedThisFrame)
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
            var backButtons = _root.Query<VisualElement>(name: "BackButton").ToList();
            
            foreach (var backButton in backButtons)
            {
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
            _commandPage.style.display = DisplayStyle.None;
            _attackPage.style.display = DisplayStyle.None;
            _summonPage.style.display = DisplayStyle.None;
            _itemPage.style.display = DisplayStyle.None;
            _fleePage.style.display = DisplayStyle.None;
            
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
            
            // Get the skill name from the label
            var skillNameLabel = skillItem.Q<Label>("SkillName");
            if (skillNameLabel != null && _battleManager != null)
            {
                string skillName = skillNameLabel.text;
                Debug.Log($"Selected skill: {skillName}");
                
                // Tell battle manager about the selection
                _battleManager.OnSkillSelected(skillName);
            }
        }

        private void TestAddStatusIcons()
        {
            if (_activeHPBars.Count == 0 || testStatusIcons == null || testStatusIcons.Length == 0)
            {
                Debug.LogWarning("No HP bars or test icons available!");
                return;
            }

            foreach (var hpBar in _activeHPBars)
            {
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
            var hpBars = FindObjectsByType<MonsterHPBar>(FindObjectsSortMode.None);
            
            Debug.Log($"Found {hpBars.Length} HP bars to initialize");
            
            foreach (var hpBar in hpBars)
            {
                Transform monsterTransform = hpBar.transform;
                
                // Get the BattleEntity to access the undead name
                var battleEntity = monsterTransform.GetComponent<BattleEntity>();
                string displayName = battleEntity != null ? battleEntity.Data.undeadName : monsterTransform.name;
                
                Debug.Log($"Initializing HP bar for: {monsterTransform.name} as {displayName}");
                
                hpBar.Initialize(_hpBarsContainer, monsterTransform, displayName);
                hpBar.SetHP(100, 100);
                
                _activeHPBars.Add(hpBar);
            }
        }

        public void PopulateAttackPage(List<IMove> moves)
        {
            // Get the attack skill list container
            var attackSkillList = _attackPage.Q<VisualElement>("AttackSkillList");
            
            if (attackSkillList == null)
            {
                Debug.LogWarning("AttackSkillList not found!");
                return;
            }
            
            // Clear existing skills
            attackSkillList.Clear();
            
            // Add each move dynamically
            for (int i = 0; i < moves.Count; i++)
            {
                var move = moves[i];
                
                // Create skill item
                var skillItem = new VisualElement();
                skillItem.AddToClassList("skill-item");
                skillItem.name = $"Skill{i + 1}";
                
                // Skill icon (placeholder for now)
                var skillIcon = new VisualElement();
                skillIcon.AddToClassList("skill-icon");
                skillIcon.name = "SkillIcon";
                skillIcon.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("PlaceHolderArt/Skulls/basicSkullGrey"));
                skillIcon.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
                skillItem.Add(skillIcon);
                
                // Skill name
                var skillName = new Label(move.MoveName);
                skillName.AddToClassList("font");
                skillName.AddToClassList("skill-name");
                skillName.name = "SkillName";
                skillItem.Add(skillName);
                
                // Skill type (you'll need to get this from the move data)
                var skillType = new Label("[Type]"); // TODO: Get actual type
                skillType.AddToClassList("font");
                skillType.AddToClassList("skill-type");
                skillType.name = "SkillType";
                skillItem.Add(skillType);
                
                // SP cost
                var skillCost = new Label($"{move.SPCost} SP");
                skillCost.AddToClassList("font");
                skillCost.AddToClassList("skill-cost");
                skillCost.name = "SkillCost";
                skillItem.Add(skillCost);
                
                // Add click handler
                skillItem.RegisterCallback<ClickEvent>(evt => SelectSkill(skillItem));
                
                // Add to list
                attackSkillList.Add(skillItem);
            }
            
            Debug.Log($"Populated attack page with {moves.Count} moves");
        }

        public void UpdatePartyDisplay(List<BattleEntity> partyMembers)
        {
            // Get the bottom UI container
            var bottomContainer = _root.Q<VisualElement>("BottomUIContainer");
            if (bottomContainer == null)
            {
                Debug.LogWarning("BottomUIContainer not found!");
                return;
            }
            
            // Clear existing party containers
            bottomContainer.Clear();
            
            // Create a party container for each member (max 3)
            for (int i = 0; i < Mathf.Min(3, partyMembers.Count); i++)
            {
                var member = partyMembers[i];
                var partyContainer = CreatePartyMemberUI(member, i);
                bottomContainer.Add(partyContainer);
            }
        }

        private VisualElement CreatePartyMemberUI(BattleEntity member, int index)
        {
            // Container
            var container = new VisualElement();
            container.AddToClassList("party-container");
            container.name = $"Party{index + 1}Container";
            
            // Set background color based on index
            Color[] colors = new Color[] 
            {
                new Color(0, 0.5f, 0.5f, 0.3f),  // Teal
                new Color(0.5f, 0, 0, 0.3f),      // Red
                new Color(0, 0, 0.5f, 0.3f)       // Blue
            };
            container.style.backgroundColor = colors[index];
            
            // Portrait
            var portrait = new VisualElement();
            portrait.AddToClassList("portrait");
            if (member.Data.sprite != null)
            {
                portrait.style.backgroundImage = new StyleBackground(member.Data.sprite);
                portrait.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
            }
            container.Add(portrait);
            
            // Name and Level
            var nameLabel = new Label(member.Data.undeadName);
            nameLabel.AddToClassList("font");
            nameLabel.AddToClassList("creature-name");
            container.Add(nameLabel);
            
            var levelLabel = new Label($"Level: {12}"); // TODO: Get actual level
            levelLabel.AddToClassList("font");
            levelLabel.AddToClassList("level-text");
            container.Add(levelLabel);
            
            // HP Bar
            var hpText = new Label($"HP: {member.CurrentHP}/{member.MaxHP}");
            hpText.name = "HPText";
            hpText.AddToClassList("font");
            hpText.AddToClassList("stat-text");
            container.Add(hpText);
            
            var hpBar = new VisualElement();
            hpBar.AddToClassList("stat-bar");
            hpBar.AddToClassList("hp-bar");
            var hpFill = new VisualElement();
            hpFill.AddToClassList("bar-fill");
            hpFill.AddToClassList("hp-fill");
            hpFill.name = "HPFill";
            hpFill.style.width = Length.Percent(100);
            hpBar.Add(hpFill);
            container.Add(hpBar);
            
            // SP Bar
            var spText = new Label($"SP: {member.CurrentSP}/{member.MaxSP}");
            spText.name = "SPText";
            spText.AddToClassList("font");
            spText.AddToClassList("stat-text");
            container.Add(spText);
            
            var spBar = new VisualElement();
            spBar.AddToClassList("stat-bar");
            spBar.AddToClassList("sp-bar");
            var spFill = new VisualElement();
            spFill.AddToClassList("bar-fill");
            spFill.AddToClassList("sp-fill");
            spFill.name = "SPFill";
            spFill.style.width = Length.Percent(100);
            spBar.Add(spFill);
            container.Add(spBar);
            
            return container;
        }




    }
}