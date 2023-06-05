using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;
public class AttackObject : NetworkBehaviour
{
    public MovementObject movementObject;
    public GameObject playerObject;
    public GameObject enemyTarget;
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
      if (!IsOwner) gameObject.SetActive(false);
      gameObject.GetComponent<Collider>().enabled = isEnabled();
      gameObject.GetComponent<MeshRenderer>().enabled = isEnabled();
      OnClickToAttack();
    }
    public bool isEnabled()
    {
        bool isEnabled = CheckEnemyTarget() != null && movementObject.TurnBaseManager.isRolled == true && movementObject.isEnemyPosition == true && TurnBaseStateMachine.isAnimated == false;
        return isEnabled;
    }
    public void OnClickToAttack()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == GetComponent<Collider>())
                {
                    OnAttack();
                }
            }
        }
    }

    public void OnAttack()
    {
       enemyTarget = CheckEnemyTarget();
       NetworkObject enemyNetworkObject = enemyTarget.GetComponent<NetworkObject>();
       Debug.Log("Attack : " + enemyNetworkObject.OwnerClientId);
       playerObject.GetComponent<AnimationCharacters>().OnHittoIdle();
       enemyTarget.GetComponent<AnimationCharacters>().OnDamagedtoIdle();
       if (enemyTarget.GetComponent<NetworkObject>().OwnerClientId == 0)
       {
            ShowDamagedAnimationServerRpc();
       }
       AttackServerRpc(Convert.ToInt32(enemyNetworkObject.OwnerClientId),Convert.ToInt32(movementObject.playerObject.OwnerClientId),new ServerRpcParams());
       movementObject.TurnBaseManager.rollableDice -= 1;
       if (movementObject.TurnBaseManager.rollableDice == 0)
       {
            movementObject.TurnBaseManager.isRolled = false;
            movementObject.TurnBaseManager.skillObject.SetActive(false);
            movementObject.TurnBaseManager.rollableText.enabled = false;
            movementObject.TurnBaseManager.ChangeToAnotherPlayerTurnServerRpc();
       }
    }
    [ServerRpc(RequireOwnership =false)]
    public void ShowDamagedAnimationServerRpc()
    {
        GameObject serverPlayerObject = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Damaged at me : "+serverPlayerObject.GetComponent<NetworkObject>().OwnerClientId);
        serverPlayerObject.GetComponent<AnimationCharacters>().isServerAnimated = true;
    }
   [ServerRpc (RequireOwnership =false)]
    public void AttackServerRpc(int playerNum,int AtkNum,ServerRpcParams serverRpcParams)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        int evade = 1;
        switch (playerNum)
        {
            case 0:
                if(playerData.player1EvadeCount.Value > 0)
                {
                     evade = RandomEvade();
                    playerData.player1EvadeCount.Value -= 1;
                }
                playerData.player1HP.Value -= DamageCalcule(playerData.player1Def.Value, AtkNum)*evade;
                if (playerData.player1GuardCount.Value != 0)
                {
                    playerData.player1Def.Value = playerData.player1Def.Value / (playerData.player1GuardCount.Value + 1);
                    playerData.player1GuardCount.Value = 0;
                }
                break;
            case 1:
                if (playerData.player2EvadeCount.Value > 0)
                {
                    evade = RandomEvade();
                    playerData.player2EvadeCount.Value -= 1;
                }
                playerData.player2HP.Value -= DamageCalcule(playerData.player2Def.Value,AtkNum) *evade;
                if (playerData.player2GuardCount.Value != 0)
                {
                    playerData.player2Def.Value = playerData.player2Def.Value / (playerData.player2GuardCount.Value + 1);
                    playerData.player2GuardCount.Value = 0;
                }
                break;
            case 2:
                if (playerData.player3EvadeCount.Value > 0)
                {
                    evade = RandomEvade();
                    playerData.player3EvadeCount.Value -= 1;
                }
                playerData.player3HP.Value -= DamageCalcule(playerData.player3Def.Value,AtkNum) * evade;
                if (playerData.player3GuardCount.Value != 0)
                {
                    playerData.player3Def.Value = playerData.player3Def.Value / (playerData.player3GuardCount.Value + 1);
                    playerData.player3GuardCount.Value = 0;
                }
                break;
            case 3:
                if (playerData.player4EvadeCount.Value > 0)
                {
                    evade = RandomEvade();
                    playerData.player4EvadeCount.Value -= 1;
                }
                playerData.player4HP.Value -= DamageCalcule(playerData.player4Def.Value,AtkNum) * evade;
                if (playerData.player1GuardCount.Value != 0)
                {
                    playerData.player4Def.Value = playerData.player4Def.Value / (playerData.player4GuardCount.Value + 1);
                    playerData.player4GuardCount.Value = 0;
                }
                break;

        }

    }
    public int DamageCalcule(int DEF,int AttackerId)
    {
        int Damage = 0;
        Damage = (selectAttacker(AttackerId) - DEF);
        if (DEF > selectAttacker(AttackerId))
        {
            Damage = 0;
        }
        Debug.Log("Damage = " + Damage);
        return Damage;
    }
    public int RandomEvade()
    {
        int evade = UnityEngine.Random.Range(0,2);
        Debug.Log("Luck : " + evade);
        return evade;
    }
    public int selectAttacker(int atkNumber)
    {
        int ATK = 0;
        switch (atkNumber)
        {
            case 0:
                ATK = playerData.player1ATK.Value;
                break;
            case 1:
                ATK = playerData.player2ATK.Value;
                break;
            case 2:
                ATK = playerData.player3ATK.Value;
                break;
            case 3:
                ATK = playerData.player4ATK.Value;
                break;
        }
        return ATK;
    }
    private GameObject CheckEnemyTarget()
    {
        GameObject target = null;
        foreach (GameObject player in movementObject.playerArray)
        {
            PlayerMove othermove = player.GetComponent<PlayerMove>();
            if (movementObject.playerMove.playerPosition[0] + movementObject.moveToPosition[0] == othermove.retrunThisPosition()[0] && movementObject.playerMove.playerPosition[1] + movementObject.moveToPosition[1] == othermove.retrunThisPosition()[1])
            {
                target = player;
            }
            else
            {
            }
        }
        return target;
    }
}
