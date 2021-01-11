using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelect : MonoBehaviour
{

    public Card card;

    public void onClick()
    {
        UiManager.destroyCardSelect();
        UiManager.cardSelect.SetActive(false);
        DeckState.addCardToDeck(card);
    }
}
