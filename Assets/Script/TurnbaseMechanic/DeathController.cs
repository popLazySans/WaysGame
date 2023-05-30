using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class DeathController : NetworkBehaviour
{
    public bool isDeath = false;
    public PlayerData playerData;
    public TurnBaseStateMachine turnBaseManager;
    public int deathCount;
    public bool isPlayer1 = false;
    public bool isPlayer2 = false;
    public bool isPlayer3 = false;
    public bool isPlayer4 = false;
    public PlayWhenReady play;
    public GameObject win;
    public GameObject lose;
    private bool gameIsPaused;
    // Start is called before the first frame update
    void Start()
    {
        gameIsPaused = false;
        win.SetActive(false);
        lose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsPaused) {
            Time.timeScale = 0f;
        }
        else{
            Time.timeScale = 1f;
            if (play.isStarted == true)
            {

                for (int i = 0; i < turnBaseManager.amountPlayer.Value; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (playerData.player1HP.Value <= 0 && isPlayer1 == false)
                            {
                                ClientRpcSendParams clientRpcSendParams = new ClientRpcSendParams { TargetClientIds = new List<ulong> { Convert.ToUInt64(i) } };
                                ClientRpcParams clientRpcParams = new ClientRpcParams { Send = clientRpcSendParams };
                                SetDeathClientRpc(clientRpcParams);
                                deathCount += 1;
                                isPlayer1 = true;
                            }
                            break;
                        case 1:
                            if (playerData.player2HP.Value <= 0 && isPlayer2 == false)
                            {
                                ClientRpcSendParams clientRpcSendParams = new ClientRpcSendParams { TargetClientIds = new List<ulong> { Convert.ToUInt64(i) } };
                                ClientRpcParams clientRpcParams = new ClientRpcParams { Send = clientRpcSendParams };
                                SetDeathClientRpc(clientRpcParams);
                                deathCount += 1;
                                isPlayer2 = true;
                            }
                            break;
                        case 2:
                            if (playerData.player3HP.Value <= 0 && isPlayer3 == false)
                            {
                                ClientRpcSendParams clientRpcSendParams = new ClientRpcSendParams { TargetClientIds = new List<ulong> { Convert.ToUInt64(i) } };
                                ClientRpcParams clientRpcParams = new ClientRpcParams { Send = clientRpcSendParams };
                                SetDeathClientRpc(clientRpcParams);
                                deathCount += 1;
                                isPlayer3 = true;
                            }
                            break;
                        case 3:
                            if (playerData.player4HP.Value <= 0 && isPlayer4 == false)
                            {
                                ClientRpcSendParams clientRpcSendParams = new ClientRpcSendParams { TargetClientIds = new List<ulong> { Convert.ToUInt64(i) } };
                                ClientRpcParams clientRpcParams = new ClientRpcParams { Send = clientRpcSendParams };
                                SetDeathClientRpc(clientRpcParams);
                                deathCount += 1;
                                isPlayer4 = true;
                            }
                            break;
                    }
                }
                if (deathCount + 1 == turnBaseManager.amountPlayer.Value && isDeath==false)
                {
                    gameIsPaused = true;
                    win.SetActive(true);
                    Debug.Log("Win");
                }
            }
        }
    }
    [ClientRpc]
    public void SetDeathClientRpc(ClientRpcParams clientRpcParams)
    {
        isDeath = true;
        gameIsPaused = true;
        lose.SetActive(true);
        Debug.Log("Lose");
    }
}
