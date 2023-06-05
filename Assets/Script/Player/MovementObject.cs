using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class MovementObject : NetworkBehaviour
{
    public PlayerMove playerMove = new PlayerMove();
    public int[] moveToPosition = new int[2] { 0, 0 };
    internal TurnBaseStateMachine TurnBaseManager;
    public NetworkObject playerObject;
    public GameObject[] playerArray;
    public bool isEnemyPosition;
    // Start is called before the first frame update
    void Start()
    {
        TurnBaseManager = GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<TurnBaseStateMachine>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowPossibleWays();
        OnClickToMove();
    }
    private void FixedUpdate()
    {
        CheckEnemyPosition();
    }
    private void OnEnable()
    {
        StartCoroutine(waitForPosition());
        //Debug.Log("Enabled");
    }
    IEnumerator waitForPosition()
    {
        yield return new WaitForSeconds(0.75f);
        CheckEnemyPosition();
    }
    private void CheckEnemyPosition()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerArray)
        {
            PlayerMove othermove = player.GetComponent<PlayerMove>();
            if ((playerMove.playerPosition[0] + moveToPosition[0] >= othermove.retrunThisPosition()[0]-0.1
                && playerMove.playerPosition[0] + moveToPosition[0] < othermove.retrunThisPosition()[0]+0.1) 
                &&
                (playerMove.playerPosition[1] + moveToPosition[1] >= othermove.retrunThisPosition()[1]-0.1
                && playerMove.playerPosition[1] + moveToPosition[1] < othermove.retrunThisPosition()[1]+0.1) 
                && playerMove.gameObject != player)
            {
                isEnemyPosition = true;
                //Debug.Log("pos");
                break;
            }
            else
            {
                isEnemyPosition = false;
            }
        }
    }

    public void ShowPossibleWays()
    {
        if (TurnBaseManager.isRolled == false || isEnemyPosition == true||!playerObject.IsOwner||playerMove.playerPosition[0] + moveToPosition[0] < 0 ||playerMove.playerPosition[1] + moveToPosition[1] < 0 
            || playerMove.playerPosition[0]+moveToPosition[0]>TurnBaseManager.Position.GetLength(0)-1 || playerMove.playerPosition[1] + moveToPosition[1] > TurnBaseManager.Position.GetLength(1) - 1 
            ||TurnBaseStateMachine.isAnimated == true)
        {
            setEnableCollider(false);
        }
        else
        {
            setEnableCollider(true);
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    isEnemyPosition = (other.gameObject.tag == "Player") ? true : false;
    //}
    public void setEnableCollider(bool isEnabled)
    {
        gameObject.GetComponent<Collider>().enabled = isEnabled;
        gameObject.GetComponent<MeshRenderer>().enabled = isEnabled;
    }
    public void OnClickToMove()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == GetComponent<Collider>())
                {
                    OnMove();
                    StartCoroutine(waitForPosition());
                }
            }
        }
    }
    public void OnMove()
    {
        playerMove.playerPosition[0] += moveToPosition[0];
        playerMove.playerPosition[1] += moveToPosition[1];
        playerMove.GetNewPosition();
        TurnBaseManager.rollableDice -= 1;
        CheckEnemyPosition();
        if (TurnBaseManager.rollableDice == 0)
        {
            TurnBaseManager.isRolled = false;
            TurnBaseManager.skillObject.SetActive(false);
            TurnBaseManager.rollableText.enabled = false;
            TurnBaseManager.ChangeToAnotherPlayerTurnServerRpc();
        }
    }
}
