using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSquadManager : MonoBehaviour {

    public List<GameObject> squad;
    static public Action alone;


    private void Start()
    {
        alone += isEmpty;
    }

    void isEmpty()
    {
        if(squad.Count == 0)
        {
            attackMinion.Thing(true);
        }
        else
        {
            attackMinion.Thing(false);
        }
    }



}
