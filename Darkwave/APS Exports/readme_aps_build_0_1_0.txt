APS Build 0.1.0
April 7, 2015

The character prefab previously used the Character.cs as its main and only script component. In now uses the Architect.cs script and AttributeHuntersMomentum.cs script for a total of two script components. More script components, one for each attribute trait, will be added as more traits are created. Older scripts, including Character.cs and Entity.cs, were changed to accomodate the new functions.

The Architect.cs script, a child of Character.cs, features three public integer variables representing the three attribute lines: Marksmanship, Structures, and Perception. Currently, only Marksmanship has any effect, as it is the only one of the three used in determining whether or not the Architect's only trait so far is active. Other variables include GameObject player for referencing the Architect in the game world, and the private bool huntersMomentum. This bool is set to false, and is only true if the Hunter's Momemtum trait is unlocked by having at least a value of one in Marksmanship.

When void Start() runs, player is set to the GameObject tagged "Character", and AbilityUnlockMinor() and AbilitySet() are called in that order. AbilityUnlockMinor() checks the values of the attribute points (currently just Marksmanship) and sets trait bools to true if applicable. AbilitySet() activates the traits if they're true, and skips traits still marked as false. Traits that are true bewstow initial effects if applicable. If they have constant effects that must be periodically handled, the related script is set to a variable and used to manipulate its public variables.

AttributeHuntersMomentum.cs is the script for the Architect trait of the same name. It's public variables include int cooldown and bool run. Cooldown is the number of seconds the script has to wait before running the trait's function again, and run must be true to allow the trait to function at all. 

Private variables include GameObject player and Architect architect, which are used to reference the Architect's Game Object and script respectively.

When the script starts, private variables are set and the Coroutine Effect() runs. This public IEnumerator loops every ten seconds, a value taken from cooldown. During the loop, the Architect gains 10 focus if run is true. In either case, the script pauses for the length of cooldown and the while-loop starts again.

Character.cs is the parent of Architect.cs. The only changes made to the script is the addition of the void function UpdatePlayerGUI(). Currently, this prints out how much longer the character's focus will last. It uses a new gameobject called Focus Buff, a child under another new gameobject called GUI to keep the hierarchy clean.

Entity.cs has more significant changes. First, the weapon cooldown code was moved into a void function Cooldowns() and changed to utilize the focus buff. This change also called for minor changes to the Attack() function: the if condition is now <= instead of ==. Cooldowns() is called in the StatusUpdate(), so it's still fundamentally the same.

Another addition to Entity.cs is also in StatusUpdate(). Under the comment that states "Countdown status effects", a timer variabe set to zero is added by one times Time.deltaTime. If timer becomes at least one, EffectsTimer() runs, after which timer resets to zero. This effectively means EffectsTimer() runs every second. This function subtracts the value of current effects such as focus by the value of timer. This means effects go down by one every second, and situations such as a buff increasing by ten every ten seconds won't result in the buff going past ten.

Variables related to the functions in both Character.cs and Entity.cs were added.