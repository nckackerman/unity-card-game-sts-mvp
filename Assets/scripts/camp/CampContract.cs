using System.Collections.Generic;

public class CampContract
{
    public List<Fight> fights;

    public CampContract(List<Fight> fights)
    {
        this.fights = fights;
    }
    public string getDescription()
    {
        return "Fight " + fights.Count + " battles before returning to camp.";
    }
}