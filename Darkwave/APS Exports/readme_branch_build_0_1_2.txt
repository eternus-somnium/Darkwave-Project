APS and HUD Branch Changelog - April 14, 2015

Architect.cs attribute setting system improved. Made changes to accomodate changes to AttributeHuntersMomemtum.cs. The most notable of changes is in AbilitySet(). The "format" for using Ability set is to modify non-repeating effects like adding buring to bullets and, if applicable, run the attribute trait's script's StartEffect() function if the script is enabled. If the script is disabled, remove non-repeating effects and run the StopEffect() function.

AttributeHuntersMomemtum.cs doesn't use a Coroutine anymore. Now uses InvokeRepeating and CancelInvoke to control the Effect() function.

Removed GUI functions and variables from Character.cs.

Added Regeneration and Degeneration effects to Entity.cs. They increase and decrease health by one respectively. Regeration currently does not heal past max health (we could have overhealing by setting the limit to max health times some float above 1, or have a difficulty setting to limit healing to a fraction of max health).

Entity.cs has significant changes to how cooldown works. It's now more like buffs and debuffs, and counts down using Time.deltaTime. Cooldowns set are now exactly by the seconds. For example, if cooldown1 is set to 2, you can fire the weapon every 2 seconds. cooldowns and currentCooldowns are also float, so we can set them to fractional numbers like 0.01 for the minigun.

Completely gutted the old HUD.cs. In it's current implementation, it takes public GUITexts set in the editor and updates information with OnGUI(). For now, it's completely text based until I have time to create image-based assets for them. No pausing for now, but I'll implement it really soon.