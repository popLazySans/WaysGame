using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using QFSW.QC;
using TMPro;
public class TurnBaseStateMachine : NetworkBehaviour
{
    public bool isYourTurn = false;
    public NetworkVariable<NetworkIntArray2D> NetPosition = new NetworkVariable<NetworkIntArray2D>(new NetworkIntArray2D(new int[10, 10]),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public int[,] Position = new int[10, 10];
    public NetworkVariable<int> playerTurn = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> totalTurn = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> amountPlayer = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public GameObject rollObject;
    public int rollableDice;
    public TMP_Text rollableText;
    public GameObject skillObject;
    public RandomSpawnItem randomSpawnItem;
    public bool isRolled;
    public DeathController deathController;
    public struct NetworkIntArray2D : INetworkSerializable
    {
        public int[,] Array;

        public NetworkIntArray2D(int[,] array)
        {
            Array = array;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            // Serialize the array dimensions
            int length0 = Array.GetLength(0);
            int length1 = Array.GetLength(1);
            serializer.SerializeValue(ref length0);
            serializer.SerializeValue(ref length1);

            // Serialize the array elements
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    serializer.SerializeValue(ref Array[i, j]);
                }
            }
        }
    }
    public void Start()
    {
        rollableText.enabled = false;
        rollObject.SetActive(false);
        skillObject.SetActive(false);
        isRolled = false;
    }
    public void isChangeToYourTurn(bool yourTurn)
    {
        if (deathController.isDeath == false)
        {
            isYourTurn = yourTurn;
            if (isRolled == false)
            {
                rollObject.SetActive(yourTurn);
            }
        }
        else
        {
            ChangeToAnotherPlayerTurnServerRpc();
        }
    }
    [Command] [ServerRpc(RequireOwnership = false)]
    public void ChangeToAnotherPlayerTurnServerRpc()
    {
        if (deathController.deathCount + 1 == amountPlayer.Value) return;
        randomSpawnItem.RandomSpawn();
        if (playerTurn.Value == amountPlayer.Value)
        {
            playerTurn.Value = 1;
            
        }
        else
        {
            //Debug.Log(playerTurn.Value);
            totalTurn.Value += 1;
            playerTurn.Value += 1;
        }
    }
    private void Update()
    {
        //for (int i = 0; i < Position.GetLength(0); i++)
        //{
        //    for (int j = 0; j < Position.GetLength(1); j++)
        //    {
        //        if (NetPosition.Value.Array[i,j] == 0 && Position[i,j]!= 0)
        //        {
        //            NetPosition.Value.Array[i, j] = Position[i, j];
        //        }
        //    }
        //}
        rollableText.text = "Roll : "+rollableDice;
       
    }

    [Command]
    public void ShowPosition()
    {
        string positionText = "";
        for (int i =0;i<Position.GetLength(0);i++)
        {
            for (int j=0;j<Position.GetLength(1);j++)
            {
                positionText += NetPosition.Value.Array[i,j].ToString() + " ";
            }
            positionText += "\n";
        }
        //Debug.Log(positionText);
    }
    public void Roll()
    {
        rollableDice = Random.Range(0,7);
        //Debug.Log(rollableDice);
        isRolled = true;
        rollObject.SetActive(false);
        skillObject.SetActive(true);
        rollableText.enabled = true;
        if (rollableDice == 0)
        {
            skillObject.SetActive(false);
            isRolled = false;
            ChangeToAnotherPlayerTurnServerRpc();
            StartCoroutine(SadZero());
        }
    }
    IEnumerator SadZero()
    {
        yield return new WaitForSeconds(2);
        rollableText.enabled = false;
    }
}
