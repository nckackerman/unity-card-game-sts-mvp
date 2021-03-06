using System.Collections.Generic;
using System;

public class GameData
{
    // Global static game data class. Contains all gameobject data in some field or another. 
    // Use this class whenever you are modifiying any game state.
    //
    // A part of me feels dirty making a super large static object, but I've seen examples
    // of others doing so. And it does seem to make state mangement easier. Also, there
    // should only ever be EXACTLY 1 of these in a play session. And it should give us the
    // starting point for what needs to be saved/resumed inbetween sessions.

    public static GameData instance;

    public UpgradeGameData upgradeGameData = new UpgradeGameData();
    public List<EnemyGameObject> currEnemies = new List<EnemyGameObject>();
    public FightData fightData = new FightData();
    public PlayerGameObject playerGameObject;
    public DeckData deckData;
    public DeckService deckService;
    public PlayerService playerService;
    public UpgradeService upgradeService;
    public Card selectedCard;

    public static void setInstance(GameData gameData)
    {
        instance = gameData;
    }

    public static GameData getInstance()
    {
        if (instance == null)
        {
            throw new Exception("Error, attempted to retrieve a null gameData");
        }
        return instance;
    }
}