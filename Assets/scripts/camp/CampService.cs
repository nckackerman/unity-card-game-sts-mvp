using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CampService
{

    public GameObject campScene;
    public GameObject campSelectionScene;
    public GameObject campItemList;
    public GameObject campBackgroundMask;
    public GameObject campItem;
    public CampDeckService campDeckService;
    public CardTypes cardTypes;
    private static GameObject campItemPrefab = Resources.Load(FilePathUtils.prefabPath + "campItemObject") as GameObject;

    public CampService(GameObject campScene, GameObject campSelectionScene, CampDeckService campDeckService, CardTypes cardTypes)
    {
        this.campScene = campScene;
        this.campSelectionScene = campSelectionScene;
        this.campItemList = GameObject.Find("campItemList");
        this.campItem = GameObject.Find("campItem");
        this.campBackgroundMask = GameObject.Find("CampBackgroundMask");
        this.campDeckService = campDeckService;
        this.cardTypes = cardTypes;

        hideCampFightList();
    }

    public void discardCampCards()
    {
        foreach (Transform child in campItemList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void enterCamp()
    {
        List<CampContract> contracts = generateCampContracts();
        campDeckService.showContracts(contracts);
    }

    private List<CampContract> generateCampContracts()
    {
        int numEncounters = GameData.getInstance().fightData.totalEncounterCount;
        int totalContracts = GameData.getInstance().fightData.contractCount;
        List<CampContract> contracts = new List<CampContract>();
        for (int i = 0; i < totalContracts; i++)
        {
            List<Card> encounters = new List<Card>();
            for (int j = 0; j < numEncounters; j++)
            {
                int coinFlip = Random.Range(0, 2);
                if (coinFlip == 0)
                {
                    encounters.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.enemy_basic));
                }
                else
                {
                    encounters.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.enemy_elite));
                }
            }
            contracts.Add(new CampContract(encounters));
        }
        return contracts;
    }

    public void showScene()
    {
        campBackgroundMask.SetActive(true);
        campScene.SetActive(true);
    }

    public void showCampFightList()
    {
        // List<CampContract> campContracts = GameData.getInstance().availableContracts;
        // if (campContracts == null || campContracts.Count == 0)
        // {
        //     campContracts = generateFightContracts();
        //     GameData.getInstance().availableContracts = campContracts;
        // }
        // foreach (CampContract campContract in campContracts)
        // {
        //     GameObject prefab = GameObject.Instantiate(campItemPrefab);
        //     ContractGameObject contractGameObject = prefab.GetComponentInChildren<ContractGameObject>();
        //     contractGameObject.initialize(prefab, campContract);
        //     contractGameObject.transform.SetParent(campItemList.transform, false);
        // }
        campSelectionScene.SetActive(true);
    }

    public void hideCampFightList()
    {
        foreach (Transform child in campItemList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        campSelectionScene.SetActive(false);
    }

    // public List<CampContract> generateFightContracts()
    // {
    //     List<CampContract> campContracts = new List<CampContract>();
    //     for (int i = 0; i < 3; i++)
    //     {
    //         List<Fight> fights = new List<Fight>();
    //         fights.Add(getFight(0));
    //         if (i > 0)
    //         {
    //             fights.Add(getFight(1));
    //         }

    //         if (i > 1)
    //         {
    //             fights.Add(getFight(2));
    //         }
    //         CampContract campContract = new CampContract(fights);
    //         campContracts.Add(campContract);
    //     }
    //     return campContracts;
    // }

    private Fight getFight(int count)
    {
        GameData gameData = GameData.getInstance();
        List<Enemy> enemies = new List<Enemy>();
        Fight fight = new Fight();
        fight.enemies = enemies;

        if (count == 0)
        {
            enemies.Add(gameData.enemyTypes.getFirstEnemy());
        }
        else if (count == 1)
        {
            enemies.Add(gameData.enemyTypes.getSecondEnemy());
        }
        else if (count == 2)
        {
            enemies.Add(gameData.enemyTypes.getBoss());
        }
        return fight;
    }
}