using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A quick script to get the AI Minion stuff set up correctly for use with a networked game.
/// Created by Christian Clark
/// </summary>
public class MinionSquadLocalPlayerSetup : NetworkBehaviour {
    
    [SyncVar(hook = "SetTeamNum")]
    private int teamNum;
    //Static so that the server as it changes the value will in effect use it as a unique ID generator.
    private static int teamNumToAssign = int.MaxValue / 2;

    public override void OnStartServer()
    {
        //randomize the team number to some value that is unlikely to be used by anyone else and can't be used by other players.
        teamNum = teamNumToAssign++;
        GetComponent<MinionSquad>().teamNum = teamNum;
    }

    private void SetTeamNum(int teamNum)
    {
        this.teamNum = teamNum;
        GetComponent<MinionSquad>().teamNum = teamNum;
    }
}
