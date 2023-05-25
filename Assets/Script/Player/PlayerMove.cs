using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class PlayerMove : MonoBehaviour
{
    public GameObject possibleMoveObject;
    private TurnBaseStateMachine TurnBaseManager;
    public ulong playerNumber;
    internal int[] playerPosition = new int[2] {0,0};
    // Start is called before the first frame update
    void Start()
    {
        TurnBaseManager = GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<TurnBaseStateMachine>();
        GetPositionPlayer();
        playerNumber = NetworkManager.Singleton.LocalClientId; 
    }

    // Update is called once per frame
    void Update()
    {

        if (TurnBaseManager.playerTurn.Value == Convert.ToInt32(playerNumber))
        {
            TurnBaseManager.isChangeToYourTurn(true);
        }
        else 
        {
            TurnBaseManager.isChangeToYourTurn(false);
        }
        isShowPossibleMove(TurnBaseManager.isYourTurn);
    }
    public void GetPositionPlayer()
    {
        playerPosition[0] = Mathf.Abs((int)gameObject.transform.position.x/10);
        playerPosition[1] = Mathf.Abs((int)gameObject.transform.position.z/10);
        SetPositionPlayer();
        Debug.Log(playerPosition[0] + " " + playerPosition[1]);
    }
    public void GetNewPosition()
    {
        gameObject.transform.position = new Vector3(playerPosition[0]*10,transform.position.y,transform.position.z);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, playerPosition[1] * -10);
        SetPositionPlayer();
        Debug.Log(playerPosition[0] + " " + playerPosition[1]);
    }
    public void isShowPossibleMove(bool isShow)
    {
        possibleMoveObject.SetActive(isShow);
    }
    public void SetPositionPlayer()
    {
        for (int i = 0; i < TurnBaseManager.Position.GetLength(0); i++)
        {
            for (int j = 0; j < TurnBaseManager.Position.GetLength(1); j++)
            {
                if (TurnBaseManager.Position[i, j] == Convert.ToInt32(playerNumber))
                {
                    TurnBaseManager.Position[i, j] = 0;
                }
            }
        }
        TurnBaseManager.Position[playerPosition[0], playerPosition[1]] = Convert.ToInt32(playerNumber);
    }
}
