using UnityEngine;

public class SceneUiManager
{

    private GameObject startScene;
    private GameObject gameOverScene;
    private GameObject victoryScene;
    private GameObject cardListScene;
    private GameObject fightScene;
    private GameObject campScene;

    public SceneUiManager(
        GameObject startScene,
        GameObject gameOverScene,
        GameObject victoryScene,
        GameObject cardListScene,
        GameObject fightScene,
        GameObject campScene)
    {
        this.startScene = startScene;
        this.gameOverScene = gameOverScene;
        this.victoryScene = victoryScene;
        this.cardListScene = cardListScene;
        this.fightScene = fightScene;
        this.campScene = campScene;
    }

    public void startFight()
    {
        startScene.SetActive(false);
        gameOverScene.SetActive(false);
        victoryScene.SetActive(false);
        cardListScene.SetActive(false);
        campScene.SetActive(false);
        fightScene.SetActive(true);
    }

    public void showCampScene()
    {
        startScene.SetActive(false);
        gameOverScene.SetActive(false);
        victoryScene.SetActive(false);
        cardListScene.SetActive(false);
        fightScene.SetActive(false);
        campScene.SetActive(true);
    }

    public void showVictoryScene()
    {
        victoryScene.SetActive(true);
    }

    public void showGameOver()
    {
        fightScene.SetActive(false);
        gameOverScene.SetActive(true);
        //quick hack to show children of active/deactive scenes
        foreach (Transform child in gameOverScene.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}