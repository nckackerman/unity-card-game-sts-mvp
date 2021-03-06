using UnityEngine;

public class StatusData
{
    public string name;
    public string description;
    public int statusCount = 1;
    public int statusDeltaPerTurn = -1;
    public Color32 color;

    public StatusData shallowCopy()
    {
        return (StatusData)this.MemberwiseClone();
    }
}