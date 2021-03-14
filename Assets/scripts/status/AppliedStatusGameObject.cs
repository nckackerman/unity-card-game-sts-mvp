using UnityEngine;
using UnityEngine.UI;

public class AppliedStatusGameObject : MonoBehaviour
{
    public GameObject appliedStatusInstance;
    public bool initalized = false;
    public Status status;
    public Image image;
    float step = 0;

    public void initialize(GameObject appliedStatusInstance, Status status)
    {
        this.appliedStatusInstance = appliedStatusInstance;
        this.image = gameObject.GetComponent<Image>();
        Color opaqueColor = status.data.color;
        opaqueColor.a = 0.5f;
        this.image.color = opaqueColor;
        this.status = status;

        this.initalized = true;
    }

    public void Update()
    {
        if (initalized)
        {
            growObject();
        }
    }

    public void growObject()
    {
        step += 2.5f * Time.deltaTime;
        float scale = Mathf.Lerp(1, 2, step);
        this.transform.localScale = new Vector2(scale, scale);

        if (scale == 2)
        {
            GameObject.Destroy(this.appliedStatusInstance);
            step = 0f;
        }
    }
}