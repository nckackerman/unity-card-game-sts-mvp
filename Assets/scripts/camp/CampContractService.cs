using System.Collections.Generic;
using UnityEngine;

public class CampContractService
{

    public CampContractUiManager campContractUiManager;

    public CampContractService(CampContractUiManager campContractUiManager)
    {
        this.campContractUiManager = campContractUiManager;
    }

    public void showContracts(List<CampContract> contracts)
    {

        campContractUiManager.destroyContractUi();
        foreach (CampContract campContract in contracts)
        {
            campContractUiManager.showCampContract(campContract);
        }
    }

    public List<CampContract> generateCampContracts()
    {
        int numEncounters = GameData.getInstance().fightData.totalEncounterCount;
        int totalContracts = GameData.getInstance().fightData.contractCount;
        List<CampContract> contracts = new List<CampContract>();
        for (int i = 0; i < totalContracts; i++)
        {
            List<CampEncounter> encounters = new List<CampEncounter>();
            for (int j = 0; j < numEncounters; j++)
            {
                int coinFlip = Random.Range(0, 4);
                if (coinFlip == 0)
                {
                    encounters.Add(CampEncounter.basic);
                }
                else if (coinFlip == 1)
                {
                    encounters.Add(CampEncounter.elite);
                }
                else if (coinFlip == 2)
                {
                    encounters.Add(CampEncounter.campEvent);
                }
                else
                {
                    encounters.Add(CampEncounter.campFire);
                }
            }
            contracts.Add(new CampContract(encounters));
        }
        return contracts;
    }
}