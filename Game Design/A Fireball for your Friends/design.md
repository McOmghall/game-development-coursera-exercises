A Fireball for your Friends GDD
=====

A bullet hell game for PC/MAC/Linux, on 3D browser engines (WebGL), played with keyboard and mouse or a gamepad (with at least a crosspad and a joystick or 2 joysticks, and 2 action buttons).

Introduction and Gameworld
-----

A Fireball for your Friends (hereafter AFfyF) is a skill-focused bullet hell game where you play as a wizard who can only cast different types of fireballs. AFfyF is thought as a mixture of ideas from the bullet hell genre (Touhou, Gradius,...) where you have to detect bullet patterns to best avoid being hit with the controls of a birds-eye-view camera skill-based action RPG (The Legend of Zelda series, Diablo but with more direct controls,...). As the game title suggest this is a story of vengeance where the player needs to throw fireballs to who allegedly were her friends. As the main driver of the idea the main character, the Mage, is thrown in a magical labyrinth. To escape said labyrinth our main character has to destroy 3 powerful enemies, the Bosses, each with its unique attack patterns and skills.
 
The game drops you into the action immediately surrounding you with attacking enemies from the first second. Therefore you need to learn quickly to survive long enough to beat the game's Bosses, your Friends, that like the traditional bullet hell games are much bigger, much stronger and with a higher number of bullet patterns than the typical mob. A combo system for fireballs is included, meaning each successful hit fills both a random power-up generator for your fireballs and a explosive special fireball that can clear zones of enemies quickly.

Philosophy
-----

We want to make this game to explore the limits of a fast paced action game with simple mechanic elements that enable the generation of progressively more complex gameplay, so we can test how much of a game we can make on a reduced timeline, using procedural generation where possible. Also to test the inclusion of many simple elements (simple mobs, for example) as a procedural generator of complexity (complex bullet patterns).

Features
-----

- Fast paced combat system with evolving enemy setups
- Procedurally generated map and enemies, no 2 playthroughs are the same

How to play and Game Feel
-----

Aesthetically speaking the look we're going for is something like (http://www.diablo.phx.pl/Grafika/screeny/mody/hordes6.JPG), less grimdark and more of a lucid dream experience, with a lot more fireballs. Imagine Doom but with wizards, and you're in wizard hell. The music should support this with fast paced rithms with simple harmony, as it's typical of action/combat music in videogames.

For the Camera you see your mage from above all the time, and the camera moves a bit following the main crosshair which is controlled with the mouse or the main analog joystick on the gamepad, to allow you to control the direction your mage is facing and to be able to see more what's ahead of you and less what's behind you giving the opportunity to create tension attacking the main character from unexpected directions. 

WASD or the main cross-pad of the controller move the character in the traditional 8 directions (up, down, left, right and 2-combinations of them) relative to the camera, the M1 (mouse main button) or the main fire button allow to fire normal fireballs that can have power-ups, and M2 (mouse secondary button) allow you to fire an special explosive fireball that makes a lot of damage in an area.

The mage can resist 5 enemy attacks before diying.

###Combos and Power-ups###

Each attack modifies the Combo bar the following way:

- Every hit on an enemy adds 1 to the bar
- Every attack that doesn't hit anything subtracts 2 to the bar
- Every 10 successful attacks (fails irrelevant here) adds an explosive fireball to your ammo (starting the game with 0)

The Combo bar works as a point bar that assesses player performance, allowing her to measure her own skill. Risk is that the player might fail a lot of attacks early on, giving her a negative score and adding frustration (needs testing).

Every 20 points earned inside a 3-second window, and randomely with a given probabilty, the player receives one of the following power ups:

- Temporary homing normal fireballs
- Temporary piercing normal fireballs (can hit more than once)
- Add 1 more hit to your health
- 3x fireball for a long time (can stack with itself)
- Fireball shield

A hidden mechanic to be discovered by the player is the possibility to hit incoming fireballs with your own, to make space to move your character.

###Enemies and bosses###

The enemy generation algorithm has to be tweaked yet, but the general idea is that the game generates bullet patterns that are progressively more complex combining simple patterns that come from aesthetically distinct enemies:

- Basic mob, throws regularly a slow fireball at your current position
- More complex mob, throws slow fireballs in a circle
- More complex mob, throws fireballs in a spiral continuously
- More complex mob, throws formations of fireballs with geometric shapes (squares, triangles)
- Difficult mob, throws a quick fireball taking in account your current speed to fire where you're going to be.

As for the bosses a simple idea to create complexity is a boss that invokes different mobs and attacks you with a simple own pattern. Other bosses should have distinct animations for different patterns to allow the player to prepare, therefore they involve more modelling work.

###Level generation###

The magical labyrinth is generated as a plattform without walls, so you can fall to your death from it, that extends whatever the direction the player moves to. The idea of the level generator is for the player to feel they are trapped in a labirynth designed specially for them, therefore they won't find any closed corridors, with the path always extending before them. This can cause a sense of being lost (as is intended) so to give the player a sense of progress in this ever-sprawling world, experimentation with the Boss generation times or the level structure is needed. The following ideas are to be tested:

- A number of mobs is required for each Boss to appear, in order of increasing Boss difficulty
- The level expands as a closed plattform that has floors, generating upward stairs from time to time so the player can ascend. Bosses appear in given levels with an easily learnable pattern like (3, 6, 9) or (5, 10, 20). 
