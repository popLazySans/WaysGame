using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class PlayWhenReady : NetworkBehaviour
{
    public NetworkVariable<bool> isPlayer2rdy  = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isPlayer3rdy = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isPlayer4rdy = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> numRdy = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public List<bool> rdyList = new List<bool>();
    //public List<GameObject> rdyGameObjectList = new List<GameObject>();
    public int clientID;
    public TurnBaseStateMachine turnBaseStateMachine;
    public GameObject LobbyObject;
    public GameObject startButton;
    public GameObject readyButton;
    public GameObject notreadyButton;
    public List<GameObject> markList = new List<GameObject>();
    public bool isHost;
    public bool isStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        addBooltoList();
       
    }

    // Update is called once per frame
    void Update()
    {
        clientID = Convert.ToInt32(NetworkManager.Singleton.LocalClientId);
        hostorclient();
        Allready();
        UpdateMark();
    }
    public void addBooltoList()
    {
        rdyList.Add(isPlayer2rdy.Value);
        rdyList.Add(isPlayer3rdy.Value);
        rdyList.Add(isPlayer4rdy.Value);
    }
    public void hostorclient()
    {
        if(clientID==0){
            isHost = true;
            Host();
        }
        else
        {
            Client();
        }
    }
    public void Host()
    {
        isHost = true;
        readyButton.SetActive(false);
    }
    public void Client()
    {
        isHost = false;
        readyButton.SetActive(true);
        startButton.SetActive(false);
    }
    public void Allready() {
        if(numRdy.Value == turnBaseStateMachine.amountPlayer.Value - 1 && isHost == true && numRdy.Value != 0)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }
    public void UpdateMark()
    {
        markList[0].SetActive(isPlayer2rdy.Value);
        markList[1].SetActive(isPlayer3rdy.Value);
        markList[2].SetActive(isPlayer4rdy.Value);
    }
    public void updateListtoBool()
    {
        isPlayer2rdy.Value = rdyList[0];
        isPlayer3rdy.Value = rdyList[1];
        isPlayer4rdy.Value = rdyList[2];
    }
    public void updateBooltoList()
    {
        rdyList[0] = isPlayer2rdy.Value;
        rdyList[1] = isPlayer3rdy.Value;
        rdyList[2] = isPlayer4rdy.Value;
    }
    //[ServerRpc (RequireOwnership =false)]
    public void ready()
    {
        switch(clientID){
            case 1:
                ready2ServerRpc();
                break;
            case 2:
                ready3ServerRpc();
                break;
            case 3:
                ready4ServerRpc();
                break;
        }
        readyButton.SetActive(false);
        notreadyButton.SetActive(true);
    }
    [ServerRpc(RequireOwnership = false)]
    public void ready2ServerRpc()
    {
        isPlayer2rdy.Value = true;
        numRdy.Value += 1;
    }
    [ServerRpc(RequireOwnership = false)]
    public void ready3ServerRpc()
    {
        isPlayer3rdy.Value = true;
        numRdy.Value += 1;
    }
    [ServerRpc(RequireOwnership = false)]
    public void ready4ServerRpc()
    {
        isPlayer4rdy.Value = true;
        numRdy.Value += 1;
    }
    public void notready()
    {
        switch (clientID)
        {
            case 1:
                unready2ServerRpc();
                break;
            case 2:
                unready3ServerRpc();
                break;
            case 3:
                unready4ServerRpc();
                break;
        }
        readyButton.SetActive(true);
        notreadyButton.SetActive(false);
    }
    [ServerRpc(RequireOwnership = false)]
    public void unready2ServerRpc()
    {
        isPlayer2rdy.Value = false;
        numRdy.Value -= 1;
    }
    [ServerRpc(RequireOwnership = false)]
    public void unready3ServerRpc()
    {
        isPlayer3rdy.Value = false;
        numRdy.Value -= 1;
    }
    [ServerRpc(RequireOwnership = false)]
    public void unready4ServerRpc()
    {
        isPlayer4rdy.Value = false;
        numRdy.Value -= 1;
    }
    [ClientRpc]
    public void readyClientRpc()
    {
        rdyList[Convert.ToInt32(NetworkManager.Singleton.LocalClientId - 1)] = true;
        numRdy.Value += 1;
        readyButton.SetActive(false);
        notreadyButton.SetActive(true);
    }
 

    [ClientRpc]
    public void startGameClientRpc()
    {
        LobbyObject.SetActive(false);
        isStarted = true;
    }
}
