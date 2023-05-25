using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
public class LoginManager : MonoBehaviour
{
    private bool isApproveConnection = false;
    public int x_Range = 0;
    public int z_Range = 0;
    //public List<string> roomList = new List<string>();
    //public CharacterList characterList;
    UnityTransport transport;
    public TurnBaseStateMachine turnBaseStateMachine;
    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisConnected;
        //SetStartPanel(false);
    }
    private void SetStartPanel(bool isPlay)
    {
        //leaveButton.SetActive(isPlay);
    }
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return; }
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisConnected;
    }
    private void HandleClientDisConnected(ulong clientId)
    {
    }
    public void leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }
        SetStartPanel(false);
        DestroyAllGameObjectWithTag("PoolingObject");
    }

    private static void DestroyAllGameObjectWithTag(string tag)
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < allObjects.Length; i++)
        {
            Destroy(allObjects[i]);
        }
    }

    public void newGame()
    {
        NetworkManager.Singleton.Shutdown();
        NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        SetStartPanel(false);
    }
    private void HandleClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            SetStartPanel(true);
        }
    }
    private void OnConnectedToServer()
    {
        Debug.Log("ServerConnected");
    }
    private void HandleServerStarted()
    {
        Debug.Log("ServerConnected");
        //throw new System.NotImplementedException();
    }

    public async void Host(Allocation allocation)
    {
        if (RelayManager.Instance.IsRelayEnabled)
        {
            await RelayManager.Instance.CreateRelay(allocation);
        }
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }
    public async void Client(string joinCode)
    {
        //if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCode))
        //{
        await RelayManager.Instance.JoinRelay(joinCode);
        //}
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(joinCode);
        NetworkManager.Singleton.StartClient();
    }
    public void AddHostDataToList()
    {
        //roomList.Add(username + "_" + roomID);
    }
    public void stopServerForClient()
    {
        NetworkManager.Singleton.Shutdown();
    }
 
    public string getRoomIDFromUser()
    {
        string room = "";
        return room;
    }
    public string getUsernameFromUser()
    {
        string user = "";
        return user;
    }
    private bool approveNameConnection(string clientData, string serverData)
    {
        bool isApprove = System.String.Equals(clientData.Trim(), serverData.Trim()) ? false : true;
        return isApprove;
    }
    private bool approveRoomIDConnection(string clientData, string serverData)
    {
        bool isApprove = System.String.Equals(clientData.Trim(), serverData.Trim()) ? true : false;
        return isApprove;
    }
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;
        int byteLenght = connectionData.Length;
        //change it if use waiting room
        bool isApprove = true;
        string clientData = System.Text.Encoding.ASCII.GetString(connectionData, 0, byteLenght);
        if (byteLenght == 0)
        {

        }
        if (byteLenght > 0)
        {
            //string[] splitText = clientData.Split(char.Parse("_"));
            //string clientUsername = splitText[0];
            //string clientRoomID = splitText[1];
            //string usernameData = getUsernameFromUser();
            //string roomIDData = getRoomIDFromUser();
            //isApprove =  (approveNameConnection(clientUsername, usernameData))&(approveRoomIDConnection(clientRoomID,roomIDData));
            //if (isApprove == false) { clientId -= 1; }
            ////else
            ////{
            ////    foreach(string room in roomList){
            ////        if(room == clientData)
            ////        {
            ////            roomList.Remove(room);
            ////        }
            ////    }
            ////}
            //Debug.Log(clientUsername + " " + clientRoomID + " " + isApprove);
        }
 
        setSpawnLocation(clientId, response);
        NetworkLog.LogInfoServer("SpawnPos of " + clientId + "is" + response.Position.ToString());
        turnBaseStateMachine.amountPlayer.Value = (int)clientId;
        response.Approved = isApprove;
        response.CreatePlayerObject = isApprove;
        //startCount(isApprove);
        // Your approval logic determines the following values
        //if(connectionData = )

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        //response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        //response.Rotation = Quaternion.identity;

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        //response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }
    private void setSpawnLocation(ulong clientId,NetworkManager.ConnectionApprovalResponse response){
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            spawnPos = new Vector3(0, 4, 0);
           // spawnRot = Quaternion.Euler(0, 135, 0);
        }
        else
        {
            switch (NetworkManager.Singleton.ConnectedClients.Count)
            {
                case 1:

                    spawnPos = new Vector3(90, 4, -90);//spawnRot = Quaternion.Euler(0, 100, 0);
                    break;
                case 2:
                    spawnPos = new Vector3(0, 4, -90);//spawnRot = Quaternion.Euler(0, 80, 0);
                    break;
                case 3:
                    spawnPos = new Vector3(90, 4, -90);
                    break;
            }
        }
        response.Position = spawnPos;
        response.Rotation = spawnRot;
    }
    public int randomPosition(int range)
    {
        int randomNumber = Random.Range(1, range);
        return randomNumber;
    }

}
