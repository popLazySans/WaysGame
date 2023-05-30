using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class PlayerMove : NetworkBehaviour
{
    public GameObject possibleMoveObject;
    public GameObject possibleAttackObject;
    private TurnBaseStateMachine TurnBaseManager;
    public NetworkVariable<int> playerNumber = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public int[] playerPosition = new int[2] {0,0};
    public NetworkVariable<int> player1PosX = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player1PosY = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2PosX = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2PosY = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3PosX = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3PosY = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4PosX = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4PosY = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    void Start()
    {
        TurnBaseManager = GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<TurnBaseStateMachine>();
        GetPositionPlayer();
        if (IsOwner)
        {
            playerNumber.Value = Convert.ToInt32(NetworkManager.Singleton.LocalClientId + 1);
        }
        TurnBaseManager.playerTurn.Value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (TurnBaseManager.playerTurn.Value == playerNumber.Value)
        {
            TurnBaseManager.isChangeToYourTurn(true);
        }
        else 
        {
            TurnBaseManager.isChangeToYourTurn(false);
        }
        isShowPossible(TurnBaseManager.isYourTurn);
        SetPosPlayerNetwork(playerNumber.Value);
       
    }
    public int[] retrunThisPosition()
    {
        int[] array = new int[2];
        switch (playerNumber.Value)
        {
            case 1:
                array[0] = player1PosX.Value;
                array[1] = player1PosY.Value;
                break;
            case 2:
                array[0] = player2PosX.Value;
                array[1] = player2PosY.Value;
                break;
            case 3:
                array[0] = player3PosX.Value;
                array[1] = player3PosY.Value;
                break;
            case 4:
                array[0] = player4PosX.Value;
                array[1] = player4PosY.Value;
                break;
        }
        return array;
    }
    public void SetPosPlayerNetwork(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                player1PosX.Value = playerPosition[0];
                player1PosY.Value = playerPosition[1];
                break;
            case 2:
                player2PosX.Value = playerPosition[0];
                player2PosY.Value = playerPosition[1];
                break;
            case 3:
                player3PosX.Value = playerPosition[0];
                player3PosY.Value = playerPosition[1];
                break;
            case 4:
                player4PosX.Value = playerPosition[0];
                player4PosY.Value = playerPosition[1];
                break;
        }
    }
    public void GetPositionPlayer()
    {
        playerPosition[0] = Mathf.Abs((int)gameObject.transform.position.x/10);
        playerPosition[1] = Mathf.Abs((int)gameObject.transform.position.z/10);
        //SetPositionPlayer();
        //Debug.Log(playerPosition[0] + " " + playerPosition[1]);
    }
    public void GetNewPosition()
    {
        gameObject.transform.position = new Vector3(playerPosition[1]*10,transform.position.y,transform.position.z);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, playerPosition[0] * -10);
        //SetPositionPlayer();
        //Debug.Log(playerPosition[0] + " " + playerPosition[1]);
    }
    public void isShowPossible(bool isShow)
    {
        possibleMoveObject.SetActive(isShow);
        possibleAttackObject.SetActive(isShow);
    }
    //[ServerRpc(RequireOwnership =false)]
    public void SetPositionPlayer()
    {
        //if (!IsOwner) return;
        for (int i = 0; i < TurnBaseManager.NetPosition.Value.Array.GetLength(0); i++)
        {
            for (int j = 0; j < TurnBaseManager.NetPosition.Value.Array.GetLength(1); j++)
            {
                if (TurnBaseManager.NetPosition.Value.Array[i, j] == playerNumber.Value)
                {
                    TurnBaseManager.NetPosition.Value.Array[i, j] = 0;
                }
            }
        }
        TurnBaseManager.NetPosition.Value.Array[playerPosition[0], playerPosition[1]] = playerNumber.Value;
        TurnBaseManager.ShowPosition();
    }
}
