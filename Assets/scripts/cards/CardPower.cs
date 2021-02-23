using UnityEngine;
using System;

public class CardPower
{
    public Action<PlayerData> onTurnOverAction;

    public void onTurnOver(PlayerData playerData)
    {
        if (onTurnOverAction != null)
        {
            onTurnOverAction(playerData);
        }
    }
}