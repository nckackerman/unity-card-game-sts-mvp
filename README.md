## Latest Web Build Hosted on AWS [Here](http://deck-builder-demo-nckackerman.s3-website-us-east-1.amazonaws.com/)
Build is free to play [Here](http://deck-builder-demo-nckackerman.s3-website-us-east-1.amazonaws.com/)

## Purpose
This is a for-fun project to build a POC/MVP for a deckbuilder game in [Unity-2d](https://unity.com/solutions/2d) game engine. [Slay the Spire](https://store.steampowered.com/app/646570/Slay_the_Spire/) was taken as starting inspiration. The end goal is to get a playable demo up that doesnt look like it was made by someone brand new to game development.

## Can I use this code?
Absolutely! Please reach out to nckackerman@gmail.com for any question about this code. Happy to help however I can :)

## Updates

### 2/14/2021
Added better placeholder assets

Whats next?
- I want to explore refactoring teh code. As is I've sort of rolled my own event system for updating, and I ahve to assume there is a Unity provided way that is better than my implmentation
- start building interesting cards, enemies, amd upgrades to interact with

### 1/30/2021
Revisiting the goal for this project, I wanted to play around with unity and release a deck building game end to end. Slay the Spire (StS) was taken as inspiration for a handful of reasons. I spent a lot of the last week thinking of novel game mechanics that I could incorporate to this deck builder. I wanted something "new" this game could explore in the genre. I learned a few things this week. 

1) A lot of ideas sound good until you start writing them down and realize they're much messier on paper than in your head.  
2) The deck building genre has been around a while and most good ideas are already implemented
3) StS is really well-designed. Every mechanic is fun and nothing feels like bloat to me.

After enough time and internalizing the mantra "Dont let perfect be the enemy of good" I've decided a single deck that shares both player card/action and enemy actions is a good enough novel approach for this game. The goal of this mechanic is to move away from the traditional deckbuilding best practice that a thin deck with card draw is almost always the strongest option. What this means for gameplay is players will need to think more critically about when drawing is the "right" move. Since drawing will mean the player may draw an enemy action which makes the enemy more powerful.

New game mechanics this release:
player can draw up to 5 cards per turn, but at a cost
Enemies can now add their own cards to the player's deck. When these cards are drawn, they'll be added to the enemies current attack. For example: Attack player for x damage OR repeat current attack twice

Whats next?
- Play around with assets. Need to make the game more attractive to look at.

### 1/19/2021

Did a lot of code refactoring. Mostly moving away from every object being static and callable at any point. While the code is more sensible now, I'm sure we'll have more code reorganization as I get more familiar with Unity.

Added a quick sound affect for when a card is played. Thanks to Dustyroom for providing the assets on Unities Free section [here](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116)

Whats next?
- Take the MVP/POC bits and turn it into something fun to interact with. More interesting card + enemies for example.

### 1/17/2021

Added POC/MVP for upgrades/relics. Functionality includes:
- adding block at the start of combat
- doing damage at the start of combat
- increasing energy
- increasing health

Whats next?
- After spending this much time in Unity, I'm finding ways I prefer to organize code. I'll be doing a larger refactoring next to make future code changes easier.
- MVP/POC for sound bites.

### 1/14/2021

Added POC/MVP sprites+animations to game. Thanks to PixelFrog for providing the asstes on Unities Free section [here](https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360)

Whats Next?
- Incorporating some type of "relics" into gameplay. Example: the second attack played each turn hits twice.
- Add some POC/MVP sound bites. Example: some sort of "hit" sound when a card is played.

### 1/11/2021 

The very basic gameloop is working. Functionality includes:
- Deck of cards that can be played.
- Cards for Attacking/Defending/Drawing
- Enemies that will attack/defend each turn
- Fight rewards where players can add cards to their deck

Whats next?
- Look into incorporating assets into the game
- Continue defining what the game should be. Need more of a design than "POC/MVP" deck builder
