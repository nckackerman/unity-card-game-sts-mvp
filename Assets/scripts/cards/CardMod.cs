using System;

public class CardMod
{
    public Action onDiscardAction;

    public void onDiscard()
    {
        if (onDiscardAction != null)
        {
            onDiscardAction();
        };
    }
}