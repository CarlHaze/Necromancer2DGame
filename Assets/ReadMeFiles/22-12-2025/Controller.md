# NecromancerController Setup Guide

## Quick Setup Instructions

### 1. Create the Player GameObject
1. In Unity, create a new 2D GameObject: `Right-click Hierarchy → 2D Object → Sprites → Square` (or use your necromancer sprite)
2. Rename it to "Player" or "Necromancer"
3. Reset transform to origin (0, 0, 0)

### 2. Add Required Components
1. Select the Player GameObject
2. **Add Rigidbody2D**: `Add Component → Physics 2D → Rigidbody 2D`
   - Set Body Type: `Dynamic`
   - Gravity Scale: `0` (will be set by script automatically)
   - Freeze Rotation: `Z` (checked)
3. **Add Collider**: `Add Component → Physics 2D → Box Collider 2D` (or Circle Collider 2D)
   - Adjust size to match your sprite

### 3. Attach the Script
1. Drag `NecromancerController.cs` onto the Player GameObject
2. In the Inspector, set **Move Speed** to `5` (adjust to taste)

### 4. Configure Input (Unity's Input System)
The script uses Unity's default Input Manager. Verify these are set up:
1. `Edit → Project Settings → Input Manager`
2. Check that "Horizontal" and "Vertical" axes exist (they should by default)
   - **Horizontal**: A/D keys and Left/Right arrows
   - **Vertical**: W/S keys and Up/Down arrows

### 5. Set Up the Camera
1. Select your Main Camera
2. **Add Component → Cinemachine → Cinemachine Virtual Camera** (optional but recommended)
   - Set Follow to your Player GameObject
   - Or manually set Camera to follow player with a camera script

**OR Simple Camera Follow:**
Create this script and attach to Main Camera:
```csharp
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
```

### 6. Test It!
1. Press Play
2. Use **WASD** or **Arrow Keys** to move
3. Character should move up, down, left, right (no diagonals)

---

## How the 4-Directional Movement Works

The script **prioritizes vertical movement** over horizontal:
- Press **W** → Moves up
- Press **S** → Moves down  
- Press **A** → Moves left
- Press **D** → Moves right
- Press **W + A** → Moves **up only** (vertical takes priority)
- Press **S + D** → Moves **down only** (vertical takes priority)

This creates grid-like movement similar to classic RPGs.

---

## Optional: Animation Setup

If you want to add walking animations later:

### 1. Create an Animator Controller
1. `Right-click Project → Create → Animator Controller`
2. Name it "NecromancerAnimator"
3. Drag it onto the Player GameObject (creates Animator component)

### 2. Create Animation States
Create 4 directional animations:
- `Idle_Down` (default state)
- `Walk_Up`
- `Walk_Down`
- `Walk_Left`
- `Walk_Right`

### 3. Set Up Parameters
In the Animator window, add these parameters:
- `Horizontal` (Float)
- `Vertical` (Float)
- `Speed` (Float)

### 4. Create Transitions
Use Blend Trees or transitions based on the parameters above. The controller script already sends these values!

---

## Useful Public Methods

The controller exposes these methods for other systems:

```csharp
// Get which direction the player is facing
Vector2 facingDir = necromancerController.GetFacingDirection();

// Check if player is moving
bool moving = necromancerController.IsMoving();

// Disable movement (for cutscenes, battles, etc.)
necromancerController.SetMovementEnabled(false);

// Re-enable movement
necromancerController.SetMovementEnabled(true);
```

---

## Customization Options

### Adjustable Settings (in Inspector):
- **Move Speed**: How fast the character moves (default: 5)

### Code Modifications:

**Change movement priority** (make horizontal take priority):
```csharp
// In GetInput() method, swap the if statements:
if (horizontal != 0)
{
    movement = new Vector2(horizontal, 0);
}
else if (vertical != 0)
{
    movement = new Vector2(0, vertical);
}
```

**Add diagonal movement** (if you change your mind later):
```csharp
// In GetInput() method, replace the prioritization with:
movement = new Vector2(horizontal, vertical);
```

**Add smoothing/acceleration**:
```csharp
// Replace the Move() method with:
private void Move()
{
    Vector2 targetVelocity = movement.normalized * moveSpeed;
    rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.5f);
}
```

---

## Troubleshooting

**Character doesn't move:**
- Check that Rigidbody2D is attached
- Verify Gravity Scale is 0
- Check that the script is enabled (checkbox in Inspector)

**Character moves diagonally:**
- This shouldn't happen with the current code
- Check that you haven't modified the GetInput() method

**Character slides after stopping:**
- Add drag to Rigidbody2D (Linear Drag: 5-10)

**Character moves too fast/slow:**
- Adjust Move Speed in Inspector
- Note: Speed is in units per second

---

## Integration with Existing Systems

This controller will work seamlessly with your existing game:
- Place it in `Assets/Scripts/Player/` (per your design doc structure)
- Use `SetMovementEnabled(false)` when entering combat
- The facing direction can determine which enemies you encounter
- Movement can trigger random encounters when crossing tiles

---

Next steps for your game:
1. Set up basic environment/tilemap to walk around
2. Add encounter system (trigger battles when moving)
3. Create transition from exploration → battle
4. Add UI for inventory/undead roster