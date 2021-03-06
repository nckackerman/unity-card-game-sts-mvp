using System.Collections.Generic;

public class PlayerCardData
{
    public int energyCost = 0;
    public int memoryCount = 0;
    public bool hitsAll = false;
    public bool needsTarget = false;
    public bool firstCardPlayed = false;
    public List<Status> statuses = new List<Status>();
}