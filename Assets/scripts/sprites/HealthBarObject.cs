using UnityEngine;
using TMPro;

public class HealthBarObject
{
    private GameObject healthBarInstance;
    private GameObject healthMissingUi;
    private RectTransform healthDamagedUi;
    private RectTransform healthUi;
    private TextMeshProUGUI healthText;
    private GameObject healthBorder;
    private GameObject blockUi;
    private TextMeshProUGUI blockText;

    private Vector2 initialHealthUiVector;
    private float maxHealthWidth = 0;

    public HealthBarObject(GameObject healthBarInstance)
    {
        this.healthBarInstance = healthBarInstance;

        this.healthMissingUi = healthBarInstance.transform.Find("healthMissingUi").gameObject;
        this.healthDamagedUi = healthBarInstance.transform.Find("healthDamagedUi").GetComponent<RectTransform>();
        this.healthUi = healthBarInstance.transform.Find("healthUi").GetComponent<RectTransform>();
        this.healthText = healthBarInstance.transform.Find("healthText").GetComponent<TextMeshProUGUI>();
        this.healthBorder = healthBarInstance.transform.Find("healthBorder").gameObject;
        this.blockUi = healthBarInstance.transform.Find("blockUi").gameObject;
        this.blockText = healthBarInstance.transform.Find("blockText").GetComponent<TextMeshProUGUI>();

        this.initialHealthUiVector = healthUi.offsetMax;
        this.maxHealthWidth = Mathf.Abs(healthUi.offsetMin.x) + Mathf.Abs(healthUi.offsetMax.x);
    }

    public void updateHealthBar(HealthBarData healthBarData)
    {
        {
            healthText.text = healthBarData.currHealth + "/" + healthBarData.maxHealth;
            if (healthBarData.currBlock > 0)
            {
                blockUi.SetActive(true);
                blockText.text = healthBarData.currBlock.ToString();
            }
            else
            {
                blockUi.SetActive(false);
                blockText.text = "";
            }

            float percentToReduceHealthBar = 1 - ((float)healthBarData.currHealth / healthBarData.maxHealth);
            healthUi.offsetMax = initialHealthUiVector - new Vector2(maxHealthWidth * percentToReduceHealthBar, 0);
        }
    }
}