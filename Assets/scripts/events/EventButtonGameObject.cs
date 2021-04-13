using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EventButtonGameObject : MonoBehaviour, IPointerDownHandler
{
    private EventButtonData eventButton;
    public void initEventButton(EventButtonData eventButton, GameObject eventButtonInstance)
    {
        this.eventButton = eventButton;
        TextMeshProUGUI text = eventButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
        text.text = eventButton.text;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.eventButton.onClickAction();
    }
}