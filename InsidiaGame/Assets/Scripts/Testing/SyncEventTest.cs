using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncEventTest : NetworkBehaviour {

    [System.Serializable]
    public struct NetTestStruct
    {
        public GameObject sender;
        public int randoNumba;

        public NetTestStruct(GameObject sender, int randoNumba)
        {
            this.sender = sender;
            this.randoNumba = randoNumba;
        }
    }

    public delegate void EventTestDelegate(NetTestStruct testy);

    [SyncEvent]
    public event EventTestDelegate EventTest;

	// Use this for initialization
	void Start () {
        EventTest += OnTestEvent;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.T) && isLocalPlayer && isServer)
        {
            RpcTest();
            EventTest(new NetTestStruct(gameObject, Random.Range(0, 100)));
        }
	}

    [ClientRpc]
    private void RpcTest()
    {
        print("rpc get");
        //GetComponent<CharacterController>().Move(Vector3.up * 5f);
    }
    
    private void OnTestEvent(NetTestStruct testy)
    {
        print("event get: " + (testy.sender != gameObject) + " " + testy.randoNumba);
    }
}
