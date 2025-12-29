# UndeadBattleTester.cs - Updated Documentation

## 🔄 What Changed?

### Old System (Stats Only):
```csharp
// Simple stat-based damage
damage = attacker.Attack - (defender.Defense / 2)
damage = Max(1, damage) // Minimum 1 damage
```

### New System (Moves + Type Effectiveness):
```csharp
// Move-based damage with type multipliers
baseDamage = (Attack - Defense/2) * (MovePower / 10)
finalDamage = baseDamage * typeMultiplier
damage = Max(1, Round(finalDamage))
```

---

## 🎮 Key Features

### 1. **Move System Integration**
- Selects moves from undead's move list
- Tracks and deducts AP per use
- Uses move's power, accuracy, and type

### 2. **Type Effectiveness**
- Calculates type multiplier via TypeChart
- Supports immunities (0x damage)
- Shows effectiveness messages ("It's super effective!")

### 3. **Simple AI**
- Uses first available move with AP
- No strategy (just tests balance)
- Consistent results for testing

### 4. **Battle Loop**
- Speed determines turn order
- Speed ties broken randomly (50/50)
- Max 100 turns prevents infinite loops
- Tracks AP depletion

---

## 📊 Damage Formula Breakdown

### Example 1: Skeleton vs Zombie (Both using Bone moves)

**Setup:**
- Skeleton: ATK 12, uses Slash (Bone, 10 power)
- Zombie: DEF 14, Plague type
- Type effectiveness: Bone vs Plague = 1.0x (neutral)

**Calculation:**
```
baseDamage = (12 - 14/2) * (10/10)
baseDamage = (12 - 7) * 1
baseDamage = 5

typeMultiplier = 1.0x (neutral)

finalDamage = 5 * 1.0 = 5
damage = Max(1, Round(5)) = 5
```

**Result:** 5 damage per hit

---

### Example 2: Zombie vs Skeleton (Zombie uses Plague Touch at Level 5)

**Setup:**
- Zombie: ATK 10, uses Plague Touch (Plague, 15 power)
- Skeleton: DEF 13, Bone type
- Type effectiveness: Plague vs Bone = 1.5x (super effective!)

**Calculation:**
```
baseDamage = (10 - 13/2) * (15/10)
baseDamage = (10 - 6.5) * 1.5
baseDamage = 3.5 * 1.5 = 5.25

typeMultiplier = 1.5x (super effective)

finalDamage = 5.25 * 1.5 = 7.875
damage = Max(1, Round(7.875)) = 8
```

**Result:** 8 damage per hit (60% more than neutral!)

---

### Example 3: Skeleton vs Spirit (Bone move can't hit)

**Setup:**
- Skeleton: ATK 12, uses Slash (Bone, 10 power)
- Spirit: Any defense, Spirit type
- Type effectiveness: Bone vs Spirit = 0x (IMMUNE)

**Calculation:**
```
typeMultiplier = 0x (immune)

damage = 0 (no calculation needed)
```

**Result:** "It has no effect!" - 0 damage

---

## 🧪 Testing Scenarios

### Scenario 1: Starter Balance (Level 1-4, Bone moves only)

**All use Bone-type moves → all 1.0x neutral**

Expected results:
```
Skeleton vs Zombie:  ~60-65% Skeleton (stat advantage)
Skeleton vs Ghoul:   ~70-75% Skeleton (bulk beats glass cannon)
Zombie vs Ghoul:     ~60-70% Zombie (tank outlasts burst)
```

**Why:** Type effectiveness = 1.0x for all, so stats determine winner (just like before!)

---

### Scenario 2: With Typed Moves (Level 5+)

**Skeleton:** Bone Strike (Bone, 15 power) → Still neutral vs everyone
**Zombie:** Plague Touch (Plague, 15 power) → 1.5x vs Skeleton!
**Ghoul:** Feral Bite (Feral, 17 power) → 1.5x vs Zombie!

Expected results:
```
Skeleton vs Zombie:  ~40-45% Skeleton (Zombie has type advantage)
Skeleton vs Ghoul:   ~60-65% Skeleton (neutral, stats win)
Zombie vs Ghoul:     ~30-35% Zombie (Ghoul has type advantage)
```

**Why:** Type advantages shift the balance! Rock-paper-scissors established.

---

## 🔧 How to Use

### Setup in Unity:
1. Attach `UndeadBattleTester.cs` to a GameObject in your scene
2. Drag your starter UndeadData assets into the inspector:
   - Skeleton.asset → Skeleton field
   - Zombie.asset → Zombie field
   - Ghoul.asset → Ghoul field
3. Set battles per matchup (default: 1000)
4. Enable "Show Detailed Logs" to see individual turns (optional, spammy)
5. Press Play!

### Reading Results:
```
--- Skeleton vs Zombie ---
Skeleton wins: 620/1000 (62.0%)
Zombie wins: 380/1000 (38.0%)
```

**Good balance:** No matchup should be >75% or <25%
**Poor balance:** 90%+ or <10% means one dominates

---

## 🐛 Troubleshooting

### Issue: "Type matchup not found" warning
**Cause:** TypeChart not initialized or missing matchup
**Fix:** Make sure TypeChart.cs is attached to a GameObject in your scene and Awake() has run

### Issue: "No moves available" warning
**Cause:** Undead has no moves, or all moves have 0 AP
**Fix:** Make sure each starter has at least one move assigned with 35 AP

### Issue: Battle exceeds 100 turns
**Cause:** Both undead are too tanky or deal too little damage
**Fix:** Adjust stats or increase max turns (unlikely with current balance)

### Issue: All battles result in one winner (100%)
**Cause:** Type effectiveness too strong, or stats too imbalanced
**Fix:** 
- Check move types (should all be Bone for Level 1-4)
- Verify TypeChart multipliers (should be 1.5x/0.67x, not 2x/0.5x)
- Check starter stats haven't changed

### Issue: NullReferenceException
**Cause:** Missing move assignment or TypeChart not initialized
**Fix:**
- Ensure TypeChart.cs is in scene and runs Awake() first
- Check that all starters have moves assigned
- Verify moves are not null in UndeadData

---

## 📈 Expected AP Depletion

With 35 AP per move and average battle length:

**Short battle (5 turns):**
- Each undead attacks ~3 times = 32 AP remaining
- Can fight ~11 more battles before rest

**Average battle (8 turns):**  
- Each undead attacks ~4 times = 31 AP remaining
- Can fight ~7 more battles before rest

**Long battle (15 turns):**
- Each undead attacks ~8 times = 27 AP remaining
- Can fight ~3 more battles before rest

**Why 35 AP?**
- Enough for 4-8 battles depending on difficulty
- Forces resource management without feeling restrictive
- Typed moves will have 12-20 AP (more powerful, less spam)

---

## 🎯 What This Tests

**Currently testing:**
✅ Stat balance between starters
✅ Damage formula accuracy
✅ Type effectiveness calculations
✅ Speed-based turn order
✅ Accuracy checks
✅ AP depletion over time

**Not yet testing:**
❌ Status effects (poison, burn, etc.)
❌ Stat stage changes (buffs/debuffs)
❌ Smart AI decisions
❌ Move selection strategy
❌ Multi-battle endurance (AP management)

---

## 🚀 Future Enhancements

**Phase 1 (Current):**
- [x] Move system with AP
- [x] Type effectiveness
- [x] Simple "use first move" AI

**Phase 2 (Next):**
- [ ] Smart AI (chooses best move based on type advantage)
- [ ] Status effect testing
- [ ] Stat stage modification testing

**Phase 3 (Later):**
- [ ] Multi-battle gauntlet (test AP management)
- [ ] Team battle simulation (3v3)
- [ ] Enemy AI simulation (wild vs trainer)

---

## 💡 Design Notes

**Why simple AI?**
- Tests raw balance without strategy
- Consistent, predictable results
- Easy to identify stat/type issues
- Real AI comes later (Phase 2)

**Why first available move?**
- All starters have 1 move at Level 1
- No decision-making needed
- Pure stat comparison
- When they learn 2+ moves, AI needs upgrading

**Why 1000 battles?**
- Statistical significance (large sample)
- Reduces variance/luck
- Confident in percentages (±1-2%)
- Fast enough to run frequently

---

## ✅ Success Criteria

**Balanced starters should have:**
- ✅ All matchups between 25-75% winrate
- ✅ No "useless" starter (all viable)
- ✅ Rock-paper-scissors dynamic (with typed moves)
- ✅ Winner not predetermined (stats + type + luck)

**Red flags:**
- ❌ 90%+ winrate (one dominates)
- ❌ <10% winrate (one is useless)
- ❌ All ties/draws (stats too similar)
- ❌ Battles exceed 100 turns (damage too low)

---

Good luck testing! 🎮💀
