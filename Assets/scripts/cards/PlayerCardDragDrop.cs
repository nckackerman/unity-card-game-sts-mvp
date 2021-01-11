using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardDragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 startPosition;
    public Card card;

    private int yPositionForCardPlay = 250;

    public void onDragStart()
    {
        startPosition = transform.position;
        if (card.energyCost > PlayerState.currEnergy)
        {
            gameObject.GetComponent<Image>().color = ColorUtils.cardDefault;
            return;
        }
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

        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}
