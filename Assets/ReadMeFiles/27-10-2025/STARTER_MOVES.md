# Starter Move Assets

These are the three Bone-type (normal) moves that the starters begin with.
All have identical stats but different flavor text.

## Move Data Assets to Create

### 1. Slash.asset (Skeleton's starter move)
```
Move Name: Slash
Description: Slashes the enemy with sharp bones or a weapon. A reliable physical attack.
Move Type: Bone
Power: 10
Accuracy: 100
Max Action Points: 35
Never Misses: false
Ignores Immunity: false
Status Effect Chance: 0
Is Stat Move: false
```

**Thematic Fit:** Skeletons use weapons or sharp bones as blades

---

### 2. Tackle.asset (Zombie's starter move)
```
Move Name: Tackle
Description: Charges and slams into the enemy with brute force. A reliable physical attack.
Move Type: Bone
Power: 10
Accuracy: 100
Max Action Points: 35
Never Misses: false
Ignores Immunity: false
Status Effect Chance: 0
Is Stat Move: false
```

**Thematic Fit:** Zombies use their body weight and momentum

---

### 3. Claw.asset (Ghoul's starter move)
```
Move Name: Claw
Description: Rakes the enemy with sharp claws. A reliable physical attack.
Move Type: Bone
Power: 10
Accuracy: 100
Max Action Points: 35
Never Misses: false
Ignores Immunity: false
Status Effect Chance: 0
Is Stat Move: false
```

**Thematic Fit:** Ghouls have elongated claws and nails

---

## Design Notes

**Why all identical stats?**
- Level 1-5 balance based purely on creature stats
- No unfair advantages from move selection
- Thematic flavor without mechanical differences
- Player learns combat basics before complexity

**Why Bone type?**
- Neutral damage (1.0x) against most types
- Can't hit Spirit types (introduces immunity concept)
- Teaches that moves have types, not just creatures

**Why 35 AP?**
- Enough for multiple battles (average battle = 5-8 turns)
- Requires management but not restrictive
- Can last 4-5 battles before needing to rest
- Typed moves will have 12-20 AP (more powerful, less spam)

**Why 100% accuracy?**
- Starter move should be reliable
- New players shouldn't miss basic attacks
- Teaches combat without frustration
- Typed moves will have 85-95% accuracy (tradeoff for power)

---

## Implementation in Unity

### Creating the Assets:
1. Right-click in Assets/Data/Moves/
2. Create > Necromancer's Rising > Move Data
3. Name it "Slash", "Tackle", or "Claw"
4. Fill in the values from above

### Assigning to Starters:
1. Open Skeleton.asset → Drag Slash into Moves list
2. Open Zombie.asset → Drag Tackle into Moves list
3. Open Ghoul.asset → Drag Claw into Moves list

### Testing:
- All three starters should now use their move in battle
- Damage should be identical (10 base power)
- Win rates should match previous stats-only testing (~60/70/65%)
