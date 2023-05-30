using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class DisappearObject : NetworkBehaviour
{
    public int selectedObj;
    public RandomSpawnItem randomSpawnItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            NetworkObject networkObject = other.gameObject.GetComponent<NetworkObject>();
            DestroyServerRpc(Convert.ToInt32(networkObject.OwnerClientId),new ServerRpcParams());
            //ulong networkObjectId = GetComponent<NetworkObject>().NetworkObjectId;
            //Debug.Log(networkObjectId.ToString());
            //spawner.DestroyNetworkObjectServerRpc(networkObjectId);
            //gameObject.SetActive(false);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc(int targetId,ServerRpcParams serverRpcParams)
    {
        AddEffect(selectedObj,targetId);
        randomSpawnItem.DestroyObjectServerRpc(gameObject.GetComponent<NetworkObject>().NetworkObjectId);
    }
    public void AddEffect(int selected,int targetId)
    {
        switch (selected)
        {
            case 0:
                Sword(targetId);
                break;
            case 1:
                Sheild(targetId);
                break;
            case 2:
                ClientRpcSendParams clientRpcSendParams = new ClientRpcSendParams { TargetClientIds = new List<ulong> {Convert.ToUInt64(targetId)} };
                ClientRpcParams clientRpcParams = new ClientRpcParams { Send = clientRpcSendParams };
                BoostClientRpc(clientRpcParams);
                break;
        }
    }
    //[ServerRpc(RequireOwnership = false)]
    public void Sword(int targetId)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        switch (targetId)
        {
            case 0:
                playerData.player1ATK.Value += 5;
                break;
            case 1:
                playerData.player2ATK.Value += 5;
                break;
            case 2:
                playerData.player3ATK.Value += 5;
                break;
            case 3:
                playerData.player4ATK.Value += 5;
                break;
        }
    }
    //[ServerRpc(RequireOwnership = false)]
    public void Sheild(int targetId)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        switch (targetId)
        {
            case 0:
                playerData.player1Def.Value += 5;
                break;
            case 1:
                playerData.player2Def.Value += 5;
                break;
            case 2:
                playerData.player3Def.Value += 5;
                break;
            case 3:
                playerData.player4Def.Value += 5;
                break;
        }
    }
    //[ServerRpc(RequireOwnership = false)]
    [ClientRpc]
    public void BoostClientRpc(ClientRpcParams clientRpcParams)
    {
        TurnBaseStateMachine turnBaseManager = GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<TurnBaseStateMachine>();
        turnBaseManager.rollableDice += 2;
    }
}
