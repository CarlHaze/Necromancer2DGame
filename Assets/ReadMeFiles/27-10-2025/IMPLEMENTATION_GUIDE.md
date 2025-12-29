# Implementation Guide - Type Chart Update

## 📦 What's Included

**C# Scripts:**
1. `TypeChart.cs` - Complete 6-type effectiveness system with immunities
2. `MoveData.cs` - Move data with Action Points (AP) system
3. `UndeadData.cs` - Updated to support move lists
4. `UndeadStats.cs` - Tracks AP usage during battles
5. `STARTER_MOVES.md` - Documentation for 3 starter moves
6. `TYPE_CHART_REFERENCE.md` - Complete type chart breakdown

---

## 🔧 Unity Implementation Steps

### Step 1: Replace Old Scripts
1. **Backup your current scripts** (just in case!)
2. Replace these in `Assets/Scripts/`:
   - `Combat/TypeChart.cs` → Use new version
   - `Combat/MoveData.cs` → NEW FILE
   - `Undead/UndeadData.cs` → Use new version  
   - `Undead/UndeadStats.cs` → Use new version

### Step 2: Create Starter Move Assets
Create 3 new ScriptableObject assets in `Assets/Data/Moves/`:

**Slash.asset** (Skeleton's move)
```
Move Name: Slash
Description: Slashes the enemy with sharp bones or a weapon.
Move Type: Bone
Power: 10
Accuracy: 100
Max Action Points: 35
```

**Tackle.asset** (Zombie's move)
```
Move Name: Tackle
Description: Charges and slams into the enemy with brute force.
Move Type: Bone
Power: 10
Accuracy: 100
Max Action Points: 35
```

**Claw.asset** (Ghoul's move)
```
Move Name: Claw
Description: Rakes the enemy with sharp claws.
Move Type: Bone
Power: 10
Accuracy: 100
Max Action Points: 35
```

### Step 3: Update Starter Assets
Open each starter asset and add their move:
- `Skeleton.asset` → Add "Slash" to Moves list
- `Zombie.asset` → Add "Tackle" to Moves list
- `Ghoul.asset` → Add "Claw" to Moves list

### Step 4: Update Battle Tester (if needed)
If your `UndeadBattleTester.cs` is using moves, you may need to update it to:
1. Get moves from the UndeadStats
2. Check AP availability
3. Use `stats.UseMove(move)` to deduct AP
4. Apply type effectiveness via `TypeChart.GetEffectiveness()`

---

## 🎯 Key Changes Summary

### Type System Changes:
✅ **Bone is now "Normal" type** - Neutral vs everything except Spirit
✅ **Immunities work** - Bone can't hit Spirit, Crushing can't hit Spirit
✅ **Multipliers reduced** - 1.5x super / 0.67x weak (down from 2x/0.5x)
✅ **6 core types** - Bone, Plague, Feral, Spirit, Dark, Fire
✅ **3 enemy types** - Living, Holy, Crushing

### Move System Changes:
✅ **Action Points (AP)** instead of PP
✅ **Move type ≠ creature type** - Plague zombie can use Bone-type Tackle
✅ **35 AP for basic moves** - Enough for multiple battles
✅ **Move effectiveness** matters, not creature type

### Balance Changes:
✅ **All starters use Bone moves** - Level 1-4 are stat-based battles
✅ **No type advantages yet** - Teaches basics before complexity
✅ **Expected winrates: ~60/70/65%** - Same as before when using only stats

---

## 🧪 Testing Checklist

### Manual Testing:
- [ ] Create all 3 move assets (Slash, Tackle, Claw)
- [ ] Assign moves to starter assets
- [ ] Run UndeadBattleTester.cs
- [ ] Verify ~60/70/65% winrates (should match old stat-only balance)
- [ ] Check that AP decreases when moves are used
- [ ] Verify type effectiveness messages appear

### Expected Results:
```
Skeleton vs Zombie: ~60-65% (Skeleton wins by stats)
Skeleton vs Ghoul: ~70-75% (Skeleton wins by stats)
Zombie vs Ghoul: ~60-70% (Zombie's bulk beats Ghoul's glass cannon)
```

### Type Effectiveness Tests:
- [ ] Bone move vs Spirit type = 0x (immune, no damage)
- [ ] Plague move vs Bone type = 1.5x (super effective)
- [ ] Fire move vs Plague type = 1.5x (super effective)
- [ ] Bone move vs Bone type = 1.0x (neutral)

---

## 🚀 Next Development Steps

After confirming balance:

### Phase 1: Level 5 Typed Moves
Create 3 new typed moves:
- **Bone Strike** (Skeleton, Bone-type, 15 power, 15 AP)
- **Plague Touch** (Zombie, Plague-type, 15 power, 15 AP)
- **Feral Bite** (Ghoul, Feral-type, 17 power, 12 AP)

Expected new matchups:
- Zombie beats Skeleton (Plague > Bone)
- Ghoul beats Zombie (Feral > Plague)
- Skeleton still competitive vs Ghoul (stats)

### Phase 2: Visual Battle UI
- Display move selection
- Show AP remaining
- Type effectiveness text ("It's super effective!")
- HP bars and animations

### Phase 3: Status Effects
- Poison (from Plague Touch)
- Burn (from Fire moves)
- Stat debuffs (future moves)

### Phase 4: Enemy AI
- Wild undead (random moves)
- Rival necromancers (smart move selection)
- Boss AI (setup moves, switches)

---

## 📖 Reference Documents

**TYPE_CHART_REFERENCE.md** - Complete breakdown of:
- All 9 types and their matchups
- Strategic loops (rock-paper-scissors)
- Design philosophy
- Future expansion plans

**STARTER_MOVES.md** - Details on:
- Why all moves are identical stats
- Thematic justification for each move
- Unity implementation steps

---

## 🐛 Common Issues & Solutions

**Issue:** "Type matchup not found" warning
**Solution:** Make sure TypeChart has all 9 types defined in InitializeTypeChart()

**Issue:** AP not decreasing
**Solution:** Call `stats.UseMove(move)` in your battle logic

**Issue:** Wrong type effectiveness
**Solution:** Double-check move type vs creature type (they're different!)

**Issue:** Starters still unbalanced
**Solution:** Verify all 3 moves have IDENTICAL stats (10 power, 100 acc, 35 AP, Bone type)

---

## ✅ Validation Checklist

Before moving to next phase:
- [ ] All 3 starters have moves assigned
- [ ] Battle simulator runs without errors
- [ ] Winrates are ~60/70/65% (±5%)
- [ ] Type effectiveness calculations work
- [ ] AP system tracks usage correctly
- [ ] Immunity (Bone vs Spirit) works
- [ ] No compilation errors

---

## 💡 Design Notes

**Why Bone as Normal?**
- Most early-game enemies will be Bone type (rats, skeletons)
- Players learn combat without type complexity
- Spirit immunity teaches advanced mechanics gradually
- Thematically fits (basic physical undead)

**Why identical starter moves?**
- Level 1-4 balance based purely on stats
- No "trap" starter that feels weak
- Type system introduced gradually at level 5+
- Teaches fundamentals before strategy

**Why 1.5x instead of 2x?**
- Pokemon uses 2x but has many balancing mechanics (abilities, held items, etc.)
- 1.5x still significant (~50% more damage) but not overwhelming
- Prevents 100% winrate scenarios
- Allows comebacks and strategy

---

Good luck with implementation! 🎮💀
