using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


/**
* This class is responsible for taking all Unity GameObjects for a scene and passing
* those GameObjects to other scripts. Basically a 
* DependencyInjection class that is also the layer between c# code and unity.
**/

public class MainSceneInjectionService : MonoBehaviour
{
    private GameObject playerObject;

    private GameObject enemyContainer;
    private GameObject playerHandObject;
    private Button startNewRunButton;
    private GameObject showDiscardObject;
    private GameObject showTrashObject;
    private Button nextFightButton;
    private Button runItBackButton;
    private GameObject endTurnObject;
    private Button closeCardListButton;
    private GameObject showDeckObject;
    private GameObject extraDrawObject;

    private GameObject cardListScene;
    private GameObject cardSelectUi;
    private GameObject upgradeSelect;
    private GameObject cardListGrid;
    private GameObject fightSceneObject;
    private GameObject victoryScene;
    private GameObject gameOverScene;
    private GameObject startScene;
    private GameObject upgradeList;

    void Start()
    {
        //Get references to GameObjects from scene
        takeObjectsFromScene();

        //prefabs. no dependencies.
        GameObject upgradePrefab = Resources.Load(FilePathUtils.prefabPath + "upgradeObject") as GameObject;
        GameObject cardPrefab = Resources.Load(FilePathUtils.prefabPath + "cardObject") as GameObject;
        GameObject healthBarPrefab = Resources.Load(FilePathUtils.prefabPath + "healthBarObject") as GameObject;
        GameObject enemyPrefab = Resources.Load(FilePathUtils.prefabPath + "enemyObject") as GameObject;

        //Data classes. no dependencies
        PlayerData playerData = new PlayerData();
        DeckData deckData = new DeckData();

        //Ui manager, only dependencies are GameObjects
        CardUiManager cardUiManager = new CardUiManager(
            cardPrefab,
            playerHandObject,
            cardListGrid,
            cardListScene,
            cardSelectUi
        );
        SceneUiManager sceneUiManager = new SceneUiManager(
            startScene,
            gameOverScene,
            victoryScene,
            cardListScene,
            fightSceneObject
        );
        UpgradeUiManager upgradeUiManager = new UpgradeUiManager(
            upgradeSelect,
            upgradePrefab,
            upgradeList
        );

        //Types
        StatusTypes statusTypes = new StatusTypes();
        EnemyTypes enemyTypes = new EnemyTypes(enemyPrefab);
        CardTypes cardTypes = new CardTypes();
        cardTypes.initialize(statusTypes);

        //Services
        UpgradeService upgradeService = new UpgradeService();
        EnemyTurnService enemyTurnService = new EnemyTurnService();
        StatusService statusService = new StatusService(statusTypes);

        EnemyService enemyService = new EnemyService(enemyTurnService, statusService);
        CardGenerator cardGenerator = new CardGenerator(cardTypes);

        PlayerService playerService = new PlayerService(playerData, sceneUiManager);
        DeckService deckService = new DeckService(deckData, cardUiManager, playerService);
        EnemyManagerService enemyManagerService = new EnemyManagerService(
            enemyPrefab,
            enemyContainer,
            playerService,
            enemyService,
            enemyTurnService,
            statusService,
            deckService,
            enemyTypes,
            cardUiManager,
            cardGenerator,
            sceneUiManager,
            upgradeUiManager,
            upgradeService
        );
        CardService cardService = new CardService(enemyManagerService, playerService, new AudioState(), deckService, enemyService);
        CardActionsService cardActionsService = new CardActionsService(deckService, playerService, cardService);
        EnemyManagerService.setInstance(enemyManagerService);

        FightSceneGameObject fightSceneGameObject = fightSceneObject.GetComponent<FightSceneGameObject>();
        fightSceneGameObject.initalize(fightSceneObject, deckData, playerData);


        PlayerGameObject playerGameObject = playerObject.GetComponent<PlayerGameObject>();
        playerGameObject.initalize(playerObject, playerData);

        UpgradeTypes upgradeTypes = new UpgradeTypes(playerService);

        FightManagerService fightManagerService = new FightManagerService(
            sceneUiManager,
            cardUiManager,
            playerService,
            upgradeUiManager,
            deckService,
            deckData,
            upgradeService,
            enemyManagerService
        );
        FightManagerService.setInstance(fightManagerService);
        cardUiManager.initialize(cardActionsService, playerData);
        upgradeUiManager.initialize(upgradeService);
        deckService.initialize(enemyManagerService);


        //init scene buttons + add click events
        startNewRunButton.onClick.AddListener(() => fightManagerService.startNewRun(upgradeTypes, cardTypes));
        runItBackButton.onClick.AddListener(() => fightManagerService.startNewRun(upgradeTypes, cardTypes));
        nextFightButton.onClick.AddListener(fightManagerService.startFight);
        closeCardListButton.onClick.AddListener(cardUiManager.hideCardPile);

        addEventTrigger(showDeckObject).callback.AddListener((data) => cardUiManager.showCardPile(deckData.deckCards));
        addEventTrigger(showDiscardObject).callback.AddListener((data) => cardUiManager.showCardPile(deckData.discardCards));
        addEventTrigger(showTrashObject).callback.AddListener((data) => cardUiManager.showCardPile(deckData.trash));
        addEventTrigger(endTurnObject).callback.AddListener((data) => fightManagerService.endTurn());
        addEventTrigger(extraDrawObject).callback.AddListener((data) => deckService.extraDraw());
    }

    private void takeObjectsFromScene()
    {
        playerHandObject = GameObject.Find("playerHand");
        upgradeList = GameObject.Find("UpgradeList");
        startNewRunButton = GameObject.Find("StartNewRunButton").GetComponent<Button>();
        nextFightButton = GameObject.Find("NextFightButton").GetComponent<Button>();
        runItBackButton = GameObject.Find("RunItBackButton").GetComponent<Button>();
        closeCardListButton = GameObject.Find("CloseCardListButton").GetComponent<Button>();
        showDeckObject = GameObject.Find("showDeckClickable");
        endTurnObject = GameObject.Find("EndTurnObject");
        showDiscardObject = GameObject.Find("discardClickable");
        showTrashObject = GameObject.Find("trashClickable");
        extraDrawObject = GameObject.Find("ExtraDrawObject");

        //sprites
        enemyContainer = GameObject.Find("enemyContainer");
        playerObject = GameObject.Find("playerObject");

        //views
        fightSceneObject = GameObject.Find("fightScene");
        startScene = GameObject.Find("startScene");
        startScene.SetActive(true);
        gameOverScene = GameObject.Find("gameOverScene");
        gameOverScene.SetActive(false);
        victoryScene = GameObject.Find("victoryScene");
        cardSelectUi = GameObject.Find("cardSelect");
        upgradeSelect = GameObject.Find("upgradeSelect");
        victoryScene.SetActive(false);
        cardListScene = GameObject.Find("cardListScene");
        cardListGrid = GameObject.Find("cardList");
        cardListScene.SetActive(false);
    }

    private EventTrigger.Entry addEventTrigger(GameObject gameObject)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        trigger.triggers.Add(entry);
        return entry;
    }
}