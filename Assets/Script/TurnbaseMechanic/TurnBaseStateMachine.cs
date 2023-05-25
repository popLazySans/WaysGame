using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using QFSW.QC;
public class TurnBaseStateMachine : NetworkBehaviour
{
    public bool isYourTurn = false;
    public int[,] Position = new int[10,10];
    public NetworkVariable<int> playerTurn = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> amountPlayer  = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public void isChangeToYourTurn(bool yourTurn)
    {
        isYourTurn = yourTurn;
    }
    [Command][ServerRpc(RequireOwnership =false)]
    public void ChangeToAnotherPlayerTurnServerRpc()
    {
        if (playerTurn.Value == amountPlayer.Value)
        {
            playerTurn.Value = 0;
        }
        else
        {
            Debug.Log(playerTurn.Value);
            playerTurn.Value += 1;
        }
    }
    
}
