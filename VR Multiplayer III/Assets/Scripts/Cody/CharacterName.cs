using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterName : NetworkBehaviour
{

    [SyncVar]
    public string dinoName;

    void Start()
    {
        gameObject.name = dinoName;
    }
}
