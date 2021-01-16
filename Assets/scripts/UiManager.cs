using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    private static GameObject cardInHandPrefab;
    private static GameObject selectableCardPrefab;
    public static Text deckText;
    public static Text playerHealthText;
    public static Text enemyHealthText;
    public static Text playerBlockText;
    public static Text playerEnergyText;
    public static Text discardText;

    public static Text enemyHealth;
    public static Text enemyBlockIntent;
    public static Text enemyAttackIntent;
    public static Text enemyBlock;
    public static GameObject playerHand;

    public static GameObject cardListScene;
    public static GameObject cardSelect;
    public static GameObject upgradeSelect;
    public static GameObject cardListGrid;
    public static GameObject fightScene;
    public static GameObject victoryScene;
    public static GameObject gameOverScene;
    public static GameObject startScene;
    public static GameObject upgradeList;
    public static GameObject upgradePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //player
        deckText = GameObject.Find("deckText").GetComponent<Text>();
        discardText = GameObject.Find("discardText").GetComponent<Text>();
        playerHealthText = GameObject.Find("playerHealth").GetComponent<Text>();
        playerBlockText = GameObject.Find("playerBlock").GetComponent<Text>();
        playerEnergyText = GameObject.Find("playerEnergy").GetComponent<Text>();
        playerHand = GameObject.Find("playerHand");

        //enemy
        enemyHealth = GameObject.Find("enemyHealth").GetComponent<Text>();
        enemyBlockIntent = GameObject.Find("blockIntent").GetComponent<Text>();
        enemyAttackIntent = GameObject.Find("attackIntent").GetComponent<Text>();
        enemyBlock = GameObject.Find("enemyBlock").GetComponent<Text>();

        //scenes
        startScene = GameObject.Find("startScene");
        startScene.SetActive(true);
        gameOverScene = GameObject.Find("gameOverScene");
        gameOverScene.SetActive(false);
        fightScene = GameObject.Find("fightScene");
        fightScene.SetActive(false);
        victoryScene = GameObject.Find("victoryScene");
        cardSelect = GameObject.Find("cardSelect");
        upgradeSelect = GameObject.Find("upgradeSelect");
        victoryScene.SetActive(false);
        cardListScene = GameObject.Find("cardListScene");
        cardListGrid = GameObject.Find("cardList");
        cardListScene.SetActive(false);

        upgradeList = GameObject.Find("UpgradeList");

        //cards
        string prefabPath = "prefabs/";
        cardInHandPrefab = Resources.Load(prefabPath + "cardInHand") as GameObject;
        selectableCardPrefab = Resources.Load(prefabPath + "selectableCard") as GameObject;
        upgradePrefab = Resources.Load(prefabPath + "upgradeObject") as GameObject;
    }

    public static void updatePlayerUiFields()
    {
        deckText.text = "Deck: " + DeckState.deckCards.Count.ToString();
        discardText.text = "Discard: " + DeckState.discardCards.Count.ToString();
        playerHealthText.text = PlayerState.currHealth.ToString() + "/" + PlayerState.maxHealth.ToString();
        playerBlockText.text = "Block: " + PlayerState.currBlock.ToString();
        playerEnergyText.text = "Energy: " + PlayerState.currEnergy.ToString();
    }

    public static void updateEnemyFields(Enemy currEnemy)
    {
        enemyHealth.text = currEnemy.currHealth.ToString() + "/" + currEnemy.maxHealth.ToString();
        enemyHealth.text = currEnemy.currHealth.ToString() + "/" + currEnemy.maxHealth.ToString();
        enemyBlock.text = "Block: " + currEnemy.currBlock.ToString();
    }

    public static void updateEnemyIntent(EnemyTurn enemyTurn)
    {
        enemyBlockIntent.text = "Block: " + enemyTurn.blockIntent.ToString();
        enemyAttackIntent.text = "Attack: " + enemyTurn.attackIntent.ToString();
        if (enemyTurn.attackMultiplier > 1)
        {
            enemyAttackIntent.text += " x " + enemyTurn.attackMultiplier.ToString();
        }
    }

    public static void showHand()
    {
        for (int i = 0; i < DeckState.hand.Count; i++)
        {
            Card currCard = DeckState.hand[i];
            showCardInHand(currCard);
        }
    }

    public static void showCardInHand(Card card)
    {
        GameObject cardInHandInstance = getCardObject(card, cardInHandPrefab);
        card.cardPrefab = cardInHandInstance;
        cardInHandInstance.transform.SetParent(playerHand.transform);
        cardInHandInstance.GetComponent<PlayerCardDragDrop>().card = card;
    }

    public static void destroyPlayerCardUi()
    {
        foreach (Transform child in playerHand.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void showCardPile(List<Card> cards)
    {
        cardListScene.SetActive(true);
        foreach (Card card in cards)
        {
            GameObject cardInHandInstance = getCardObject(card, cardInHandPrefab);
            cardInHandInstance.transform.SetParent(cardListGrid.transform);
        }
    }

    public static void showCardSelectUi(List<Card> cards)
    {
        destroyCardSelect();
        cardSelect.SetActive(true);
        foreach (Card card in cards)
        {
            GameObject selectableCard = getCardObject(card, selectableCardPrefab);
            card.cardPrefab = selectableCard;
            selectableCard.transform.SetParent(cardSelect.transform);
            selectableCard.GetComponent<CardSelect>().card = card;
        }
    }

    public static void showUpgradeSelectUi(List<Upgrade> upgrades)
    {
        destroyCardSelectUi();
        upgradeSelect.SetActive(true);
        foreach (Upgrade upgrade in upgrades)
        {
            GameObject upgradeInstance = Instantiate(upgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            UpgradeGameObject upgradeGameObject = upgradeInstance.GetComponentInChildren<UpgradeGameObject>();
            upgradeGameObject.initUpgradeData(upgrade);
            upgradeGameObject.onClickAction = () =>
            {
                UpgradeState.addUpgrade(upgrade);
                upgradeSelect.SetActive(false);
                destroyCardSelectUi();
            };
            upgradeInstance.transform.SetParent(upgradeSelect.transform);
        }
    }

    public static void destroyCardSelectUi()
    {
        foreach (Transform child in upgradeSelect.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private static GameObject getCardObject(Card card, GameObject prefab)
    {
        GameObject cardInHandInstance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Text cardText = cardInHandInstance.GetComponentInChildren<Text>();
        cardText.text = card.getCardText();
        return cardInHandInstance;
    }

    public static void destroyCardSelect()
    {
        foreach (Transform child in cardSelect.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void addUpgrade(Upgrade upgrade)
    {
        GameObject upgradeInstance = Instantiate(upgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        upgradeInstance.GetComponentInChildren<UpgradeGameObject>().initUpgradeData(upgrade);
        upgradeInstance.transform.SetParent(upgradeList.transform);
    }
}
