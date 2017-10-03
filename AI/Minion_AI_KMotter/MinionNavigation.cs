using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is designed to control Minion navigation. It requires an attached NavMesh Agent.
/// </summary>
public class MinionNavigation : MonoBehaviour {

    public Transform owningPlayer;

    private NavMeshAgent agent;
    private float updateDestinationDelay = .25f;

    Coroutine follow;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}

    /// <summary>
    /// This is a coroutine that keeps this minion following its player.
    /// </summary>
    /// <param name="_player"> The player </param>
    /// <returns></returns>
    IEnumerator Follow(Transform _player)
    {
        while (true)
        {
            agent.SetDestination(owningPlayer.position);
            yield return new WaitForSeconds(updateDestinationDelay);
        }
    }

    /// <summary>
    /// This function starts a Follow() coroutine and stores it in a variable for further control.
    /// </summary>
    public void Activate()
    {
        follow = StartCoroutine(Follow(owningPlayer));
    }
}
