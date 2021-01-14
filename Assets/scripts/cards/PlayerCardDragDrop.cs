using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardDragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 startPosition;
    public Card card;

    private int yPositionForCardPlay = -250;
    private float startYposDrag = 0;
    private float startXposDrag = 0;


    public void onDragStart()
    {
        startPosition = transform.position;
        if (card.energyCost > PlayerState.currEnergy)
        {
            gameObject.GetComponent<Image>().color = ColorUtils.cardDefault;
            return;
        }
        startYposDrag = Input.mousePosition.y;
        startXposDrag = Input.mousePosition.x;
        isDragging = true;
    }

    public void onDragEnd()
    {
        isDragging = false;
        if (transform.position.y >= yPositionForCardPlay)
        {
            FightManagerService.onCardPlayed(card);
            Destroy(gameObject);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    void Update()
    {
        if (!isDragging)
        {
            return;
        }
        if (transform.position.y >= yPositionForCardPlay)
        {
            gameObject.GetComponent<Image>().color = ColorUtils.cardPlayedGreen;
        }
        else
        {
            gameObject.GetComponent<Image>().color = ColorUtils.white;
        }
        RectTransform playerHandTransform = gameObject.transform.parent.GetComponent<RectTransform>();
        transform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - playerHandTransform.rect.height / 2);
    }
}
