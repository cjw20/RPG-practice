using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    public GameObject MemberA; //prefabs of members of party
    public GameObject MemberB;
    public GameObject MemberC;

   // public GameObject partyMemA; //instance of party member in battle
   // public GameObject partyMemB;
   // public GameObject partyMemC;

    Unit StatsA; //stat pages of corresponding party member
    Unit StatsB;
    Unit StatsC;

    public Unit[] partyStats; //array containing all party member stat pages
    int partySize;
    public GameObject[] party;

    void Start()
    {
        
        CountParty();
        CreateParty();
        GetStats();
        
    }
    void CountParty()  //Counts number of allies in current party
    {
        partySize = 0;
        if (MemberA != null)
        {
            partySize++;
        }
        if (MemberB != null)
        {
            partySize++;
        }
        if (MemberC != null)
        {
            partySize++;
        }
    }

    void CreateParty()
    {
        party = new GameObject[partySize];
        if(partySize >= 1)
        {
            
            party[0] = MemberA;
        }
        if (partySize >= 2)
        {
            party[1] = MemberB;
        }
        if (partySize >= 3)
        {
            party[2] = MemberC;
        }
        
    }

    void GetStats()
    {
        partyStats = new Unit[partySize];
        if (MemberA != null)
        {
            StatsA = MemberA.GetComponent<Unit>();
            partyStats[0] = StatsA;
        }
        if (MemberB != null)
        {
            StatsB = MemberB.GetComponent<Unit>();
            partyStats[1] = StatsB;
        }
        if (MemberC != null)
        {
            StatsC = MemberC.GetComponent<Unit>();
            partyStats[2] = StatsC;
        }
    }

    public void UpdateParty()
    {
        //change members of party, must  CountParty and Create Party Again!!!
    }
}
