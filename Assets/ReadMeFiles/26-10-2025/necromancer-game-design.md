# Necromancer's Rising - 2D Unity Game Design Document

## 🎮 Game Overview

**Genre:** Turn-based Monster Collection RPG with Crafting Elements  
**Platform:** PC (Initially), Mobile (Planned)  
**Engine:** Unity 2D  
**Art Style:** Dark Pixel Art with Gothic Fantasy Aesthetic  
**Target Audience:** Fans of Pokemon, Shin Megami Tensei, and Dark Fantasy  

### Core Concept
Play as an up-and-coming necromancer in a world where death is just the beginning. Instead of catching creatures, you harvest parts from defeated enemies to craft and customize your own undead army. Navigate the moral complexities of necromancy while building your power in a world that fears and hunts your kind.

### Unique Selling Points
- **Part-Harvesting System:** Defeat enemies to collect body parts and craft unique undead
- **Deep Type System:** 10 undead types with complex interactions
- **Moral Choices:** Your actions affect the world's perception and available content
- **Crafting Evolution:** Combine parts to create entirely new undead variants

---

## 🎯 Core Gameplay Loop

### Primary Loop
1. **Explore** environments to find enemies and resources
2. **Battle** using your undead minions in turn-based combat
3. **Harvest** parts from defeated enemies
4. **Craft** new undead or upgrade existing ones
5. **Progress** through increasingly challenging areas

### Secondary Systems
- Build reputation with different factions
- Manage undead decay and maintenance
- Unlock new necromancer abilities
- Trade parts with other necromancers

---

## ⚔️ Combat System

### Battle Mechanics
- **Turn-based combat** with up to 3 active undead
- **Stance System:**
  - Aggressive: +30% damage, -20% defense
  - Defensive: -20% damage, +30% defense, counterattack chance
  - Supportive: Enable healing/buffing abilities
  - Commanding: Direct control for precision tactics

### Type Chart System

#### Undead Types
| Type | Icon | Description | Strengths | Weaknesses |
|------|------|-------------|-----------|------------|
| **Bone** | 🦴 | Basic undead, physically durable | Resistant to physical | Weak to Blunt, Spirit |
| **Plague** | 🧟 | Disease carriers, spreading corruption | Inflicts infection debuff | Weak to Fire, Holy |
| **Feral** | 👅 | Fast, hunger-driven predators | High attack/agility | Weak to Fire, Bone |
| **Spirit** | 👻 | Intangible soul energy | Immune to physical | Weak to Dark, Light |
| **Dark** | 🌑 | Forbidden magic users | Life drain attacks | Weak to Light, Psychic |
| **Hex** | 💀 | Curse-bound undead | Powerful debuffs | Weak to Fire, Spirit |
| **Infernal** | 🔥 | Demonic fire undead | Fire damage, fear aura | Weak to Water, Spirit |
| **Frost** | 🌬️ | Ice-preserved undead | Slows enemies | Weak to Fire, Plague |
| **Blight** | 🌿 | Nature's decay incarnate | Poison, terrain control | Weak to Fire, Bone |
| **Soul** | 💠 | Psychic energy manipulators | Counters Dark/Ghost | Weak to Shadow, Hex |

#### Living Types (Enemies)
| Type | Icon | Description | Strong Against | Weak Against |
|------|------|-------------|----------------|--------------|
| **Knight** | 🛡️ | Heavily armored warriors | Feral, Bone | Hex, Spirit |
| **Soldier** | 🗡️ | Balanced fighters | - | Plague, Dark |
| **Mage** | 🔮 | Elemental magic users | Bone, Plague | Dark, Spirit |
| **Priest** | ✨ | Holy magic wielders | Dark, Spirit | Plague, Shadow |
| **Druid** | 🌿 | Nature magic users | Plague, Hex | Bone, Shadow |
| **Engineer** | ⚙️ | Technology users | Bone, Knight | Spirit, Plague |

---

## 🧬 Crafting & Evolution System

### Part Collection
Parts are categorized by:
- **Source Type:** Animal, Humanoid, Monster, Legendary
- **Quality:** Common, Rare, Epic, Legendary, Cursed
- **Body Part:** Head, Torso, Arms, Legs, Core (special)

### Crafting Examples

#### Starter Evolutions
| Base | Parts Added | Result |
|------|------------|---------|
| Skeleton | Warrior Parts | Bone Knight |
| Skeleton | Mage Parts | Bone Mage |
| Zombie | Multiple Disease Parts | Plaguebearer |
| Zombie | Mixed Random Parts | Abomination |
| Ghoul | Speed Focus | Ghast |
| Ghoul | Pack Hunter Parts | Wendigo |

#### Advanced Combinations
```
Skeleton + Mage Parts + Bat Wings = Bone Harpy (Bone/Spirit)
Zombie + Bear Parts + Warrior Armor = Undead Juggernaut (Plague/Bone)
Ghoul + Rogue Parts + Wolf Instinct = Shadow Pack Hunter (Feral/Dark)
```

### Part Abilities
| Animal | Parts | Abilities Granted |
|---------|-------|------------------|
| **Frog** | Toxic Glands, Leaping Legs | Poison attacks, +Agility |
| **Rat** | Disease Carrier, Swarm Mind | Infection chance, Summon swarm |
| **Lizard** | Regenerating Tail, Scales | Self-heal, +Evasion |
| **Bat** | Wings, Echo Location | Limited flight, +Accuracy |
| **Wolf** | Pack Instinct, Fierce Bite | Team buffs, +Critical |
| **Bear** | Brutal Strength, Thick Hide | +Attack, +Defense |

---

## 🗺️ World & Progression

### Starting Area
**The Bone Gardens** - Tutorial necropolis where you:
1. Choose your first undead (Skeleton/Zombie/Ghoul)
2. Learn basic combat
3. Harvest your first parts
4. Craft your first custom undead

### Region Progression
1. **Bone Gardens** (Levels 1-10) - Tutorial area
2. **Plague Marsh** (Levels 10-20) - Disease-themed enemies
3. **Haunted Woods** (Levels 20-30) - Spirit types introduced
4. **The Living Kingdoms** (Levels 30-40) - Human opposition
5. **Frozen Crypts** (Levels 40-50) - Ice dungeon
6. **Hell's Gate** (Levels 50+) - Endgame demonic area

### Necromancer Progression

#### Skill Tree
```
Tier 1 (Levels 1-10)
├── Corpse Preservation - Parts don't decay
├── Dark Vision - See in darkness
└── Soul Sense - Detect nearby spirits

Tier 2 (Levels 10-25)
├── Mass Animation - Control +1 undead
├── Bone Armor - Personal defense
└── Plague Immunity - Immune to disease

Tier 3 (Levels 25-40)
├── Soul Binding - Permanent stat boosts
├── Dark Bargains - Demon merchant access
└── Undead Mastery - All undead +20% stats

Tier 4 (Levels 40+)
├── Phylactery - Save undead permanently
├── Lich Transformation - Become undead
└── Army of Darkness - Control 5+ undead
```

---

## 🎨 Technical Specifications (Unity 2D)

### Core Systems to Implement

#### Phase 1: Foundation
- [ ] Turn-based combat system
- [ ] Type effectiveness calculator
- [ ] Basic movement and exploration
- [ ] Inventory system for parts

#### Phase 2: Core Loop
- [ ] Crafting system with part combinations
- [ ] Undead roster management
- [ ] Save/Load system
- [ ] Basic AI for enemies

#### Phase 3: Content
- [ ] 3 starter undead with evolution paths
- [ ] 20+ enemy types with harvestable parts
- [ ] 5 regions with unique encounters
- [ ] Type-based damage calculations

#### Phase 4: Polish
- [ ] Particle effects for abilities
- [ ] Day/night cycle system
- [ ] Weather effects
- [ ] Achievement system

### Technical Architecture

```
Unity 2D Project Structure:
├── Scripts/
│   ├── Combat/
│   │   ├── BattleManager.cs
│   │   ├── TypeChart.cs
│   │   └── TurnManager.cs
│   ├── Undead/
│   │   ├── UndeadBase.cs
│   │   ├── UndeadStats.cs
│   │   └── PartSystem.cs
│   ├── Crafting/
│   │   ├── CraftingManager.cs
│   │   ├── PartDatabase.cs
│   │   └── RecipeSystem.cs
│   └── Player/
│       ├── NecromancerController.cs
│       ├── Inventory.cs
│       └── SkillTree.cs
├── Prefabs/
│   ├── Undead/
│   ├── Enemies/
│   └── UI/
└── ScriptableObjects/
    ├── UndeadData/
    ├── PartData/
    └── RecipeData/
```

### Data Structure Examples

```csharp
// UndeadType Enum
public enum UndeadType {
    Bone, Plague, Feral, Spirit, Dark, 
    Hex, Infernal, Frost, Blight, Soul
}

// Part Class
[System.Serializable]
public class Part {
    public string name;
    public PartType type;
    public Quality quality;
    public List<StatModifier> modifiers;
    public List<Ability> grantedAbilities;
}

// Undead Creation Recipe
[CreateAssetMenu]
public class UndeadRecipe : ScriptableObject {
    public UndeadBase baseUndead;
    public List<Part> requiredParts;
    public UndeadData result;
}
```

---

## 🎮 Minimum Viable Product (MVP)

### Core Features for Initial Release
1. **3 Starter Undead** with basic evolution paths
2. **5 Undead Types** (Bone, Plague, Feral, Spirit, Dark)
3. **2 Regions** (Bone Gardens, Plague Marsh)
4. **20 Enemies** with harvestable parts
5. **Basic Crafting** with 15 recipes
6. **Turn-based Combat** with type effectiveness
7. **Save/Load System**

### Post-Launch Roadmap
- **Month 1-2:** Additional regions and enemy types
- **Month 3-4:** PvP necromancer battles
- **Month 5-6:** Faction system and moral choices
- **Month 7+:** Endless dungeon mode

---

## 💡 Unique Features

### Day/Night Cycle
- Undead gain +20% stats at night
- Different enemies appear
- Special night-only abilities unlock

### Decay System
- Undead lose 1% stats per battle
- Must use preservation items or rest
- Creates resource management tension

### Morality System
| Action | Karma Effect | Consequence |
|--------|-------------|------------|
| Spare defeated humans | +Karma | Access to town shops |
| Harvest human parts | -Karma | Better parts, town hostility |
| Help villages | +Karma | Quest rewards |
| Raid graveyards | -Karma | More undead materials |

### Rival Necromancers
- 5 rivals specializing in different types
- Can trade parts or battle
- Steal unique recipes upon defeat

---

## 🔊 Audio/Visual Style

### Art Direction
- **Color Palette:** Dark purples, sickly greens, bone whites, blood reds
- **Sprite Style:** 32x32 pixel art with smooth animations
- **UI Theme:** Gothic borders with bone/skull motifs
- **Battle Backgrounds:** Parallax scrolling environments

### Audio Design
- **Music:** Orchestral dark fantasy with combat variations
- **SFX:** Bone rattling, flesh tearing, ethereal whispers
- **Ambient:** Environmental sounds based on region

---

## 📱 Platform Considerations

### PC (Primary)
- Full keyboard/mouse controls
- Higher resolution sprites
- Extended UI for crafting

### Mobile (Future)
- Touch-optimized UI
- Simplified crafting interface
- Portrait orientation support
- Shorter session lengths

---

## 🎯 Success Metrics

### Player Engagement Goals
- Average session: 30-45 minutes
- Retention: 40% Day 7, 20% Day 30
- Completion rate: 15% finish main story
- Collection rate: 5% collect all undead types

### Monetization Strategy (F2P Mobile)
- **Cosmetic Skins** for undead
- **Storage Expansion** for more active undead
- **Experience Boosters** for faster progression
- **Part Packs** (never exclusive content)

---

## 📝 Development Notes

### Priority Tasks
1. Implement core combat loop with type system
2. Create part harvesting and inventory
3. Build crafting system with 5 basic recipes
4. Design first region with 10 enemy types
5. Polish combat animations and effects

### Technical Challenges
- Balancing complex type chart
- Managing many part combinations
- Preventing overpowered craft combinations
- Optimizing for mobile without losing depth

### Inspiration & References
- Pokemon (collection/battle mechanics)
- Shin Megami Tensei (dark themes, fusion)
- Darkest Dungeon (art style, decay mechanics)
- Monster Rancher (part combination system)

---

*This document is a living design guide and will evolve throughout development.*