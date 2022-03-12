# PATH OF RAGE

This mod is for Pathfinder: Wrath of the Righteous, it contains a number of changes for the Demon mythic path to make it a more interesting and better "feeling" mythic path, while still trying to keep it within the vision of the game. It contains a number of bug-fixes, new abilities, and a different way of handling the Demon's main mechanic: Rage. 

## How to Install

```
1. Download and install Unity Mod Manager, make sure it is at least version 0.23.0
2. Run Unity Mod Manger and set it up to find Wrath of the Righteous
3. Download the WOTR_PATH_OF_RAGE mod
4. Install the mod by dragging the zip file from step 3 into the Unity Mod Manager window under the Mods tab
```
## Changes
```
* Includes a number of changes to Demon Rage. This is now a toggleable ability that will consume a number of rounds instead of per-combat and a new icon is added.
* When you gain your Demon path, new special abilites called Demonologies are unlocked, allowing more rounds of rage per day.
* Brimorak Aspect uses a new component to fix its expected scaling.
* Succubus Aspect now properly gives a malus to Attack Bonus instead of AC.
* Kalvakus Aspect's bonus attack will only work while you have a melee weapon.
* Shadow Demon Aspect will no longer give double the wisdom bonus when toggled.
* Adds new icons to each aspect making it easier to tell them apart.
* Allows you to toggle Barbarian and Bloodrager rages while in a demon rage.
* Abyssal Storm no longer hurts the caster.
* Blood Haze now gives a +2 profane bonus to attack.
* At Mythic Rank 4 you gain an ability to use Demonic Charge that isn't a teleport. This is for instances where you can't use Demonic Charge
* As a Major demonic aspect you can select Lilithu, gaining a bonus to your Charisma as well as allowing all your spells and spell-like abilites to be considered using Selective Metamagic.
* As a Major demonic aspect you can select Quasit, gaining a bonus to your Dexterity and allowing all attacks to be rolled twice and take the highest result.
* Fixes Demonic Aura so that it will only apply its damage when an enemy starts a new round instead of every time you activate Bloodrage.
* Fixes autometamagic so that it will work on spells that are not originally part of a spellbook but added later. This is important for Arcane Bloodline for Bloodragers.
* Adds a new mythic ability called Mighty Demonrage. This allows you to cast a demon spell of fourth level or lower as a swift action while Demon Raging.
```
## Current New Abilities
![alt text](https://github.com/Balkoth-dev/WOTR_PATH_OF_RAGE/blob/master/PathOfRageDemonologies.png?raw=true)
## Change Log
```
03/11/2022
* Nocticula Aspect will now work with both Lilithu and Quasit major aspects.
* Nocticula Aspect has a better icon so you don't lose it with all the other abilities.
* Nocticula Aspect is restricted to once per combat.
* Legendary Proportions no longer changes your size to Huge. This is because it conflicts with Abyssal Bulk from Bloodragers, and it's not tabletop compliant.
03/06/2022
* Demon Lord aspects fix removed as it was fixed in 1.2
* Fixes autometamagic so that it will work on spells that are not originally part of a spellbook but added later. This is important for Arcane Bloodline for Bloodragers.
* Adds a new mythic ability called Mighty Demonrage. This allows you to cast a demon spell of fourth level or lower as a swift action while Demon Raging.
* Demon Rage changed to allow you to turn it off and on in the same turn, allowing you to rage-cycle using Demon Rage.
03/05/2022
* Bloodrage and Barbarian rage now will immediately end when you turn them off.
* Bloodrager's Demonic Aura will now only affect targets when they start their turn in the aura.
01/29/2022
* Consume Souls: Buff uptime has been increased from two minutes to twenty minutes, with an additional ten minutes every other level. This is because Enduring Spells no longer works on abilities.
* Aspect of Lilithu has been added. This gives you a Charisma bonus as well as allowing all your spells and spell-like abilites to be considered using Selective Metamagic.
* Aspect of Quasit has been added. This gives you a Dexterity bonus and allows all attacks to be rolled twice and take the highest result.
* BlueprintCore has been stripped down to the bare essentials to what this mod needs to hopefully make it easier to survive updates in the future.
01/09/2022
* Fixed text on Demon Rage to correctly state that it starts at 12 rounds instead of 18
* Demon Blast correctly is now given at rank 4 instead of rank 3
11/25/2021
* Fixed Demon Rage so that it no longer stays if triggered in a cutscene
* Updated Unleashed Demon so that you may polymorph into a Sin Guzzler at Rank 9 at will for 24 hours.

```

## Special Thanks
Thanks to @Vek17#1898, @bubbles#0538, @kadyn#6364, @WittleWolfie#8477, and everyone else on the [Discord](https://discord.com/invite/wotr). Without their assistance this project would not have been possible.


## License
[MIT](https://choosealicense.com/licenses/mit/)
