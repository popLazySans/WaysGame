using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class MovementObject : NetworkBehaviour
{
    public PlayerMove playerMove = new PlayerMove();
    public int[] moveToPosition = new int[2] { 0, 0 };
    private TurnBaseStateMachine TurnBaseManager;
    public NetworkObject playerObject;
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
    public void ShowPossibleWays()
    {
        if (!playerObject.IsOwner||playerMove.playerPosition[0] + moveToPosition[0] < 0 ||playerMove.playerPosition[1] + moveToPosition[1] < 0 
            || playerMove.playerPosition[0]+moveToPosition[0]>TurnBaseManager.Position.GetLength(0)-1 || playerMove.playerPosition[1] + moveToPosition[1] > TurnBaseManager.Position.GetLength(1) - 1)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Collider>().enabled =  true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
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
                }
            }
        }
    }
    public void OnMove()
    {
        playerMove.playerPosition[0] += moveToPosition[0];
        playerMove.playerPosition[1] += moveToPosition[1];
        playerMove.GetNewPosition();
        TurnBaseManager.ChangeToAnotherPlayerTurnServerRpc();
        
    }
}
