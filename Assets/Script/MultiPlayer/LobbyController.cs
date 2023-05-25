using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using QFSW.QC;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
public class LobbyController : Singleton<LobbyController>
{
    private string playerName;
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float lobbyUpdateTimer;
    public GameObject lobbyPanelGameObject;
    GameObject lobbyNameObject;
    public GameObject roomListPanel;
    public GameObject roomListItemPrefab;
    List<Lobby> lobbies = new List<Lobby>();
    public LoginManager loginManager;
    private void Start()
    {
        playerName = "name" + Random.Range(1, 999);
        Debug.Log("Player Name = "+ playerName);
        
    }
    private void Update()
    {
        HandleLobbyPollForUpdates();

    }
    private async void HandleLobbyPollForUpdates()
    {
        if(joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }
  
    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = 4;
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = new Player
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {
                            "PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName)
                        }
                    }
                },
                Data = new Dictionary<string, DataObject>
                {
                    {"JoinCodeKey",new DataObject(DataObject.VisibilityOptions.Public,joinCode) }
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer,options);
            hostLobby = lobby;
            joinedLobby = hostLobby;
            StartCoroutine(HeartBeatLobby(hostLobby.Id, 15));
            Debug.Log("Lobby is created : " + lobby.Name + " : " + lobby.MaxPlayers + " : " + lobby.Id + " : " + lobby.LobbyCode + " : " +lobby.Data["JoinCodeKey"].Value) ;
            PrintPlayer(hostLobby);
            lobbyPanelGameObject.SetActive(false);
            loginManager.Host(allocation);
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    

    public void PrintPlayer(Lobby lobby)
    {
        Debug.Log("Player in Lobby : " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log("Players = " + player.Id + " : " + player.Data["PlayerName"].Value);
        }
    }
    public void PrintPlayers(Lobby lobby)
    {
        PrintPlayer(joinedLobby);
    }
    private async void JoinLobbyByCode(string lobbyCode, string lobbyId)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
            loginManager.Client(lobbyCode);
            Debug.Log("Joined Lobby by code : " + lobbyCode + "Id : " +lobbyId);
            lobbyPanelGameObject.SetActive(false);
        }
        catch(LobbyServiceException e) { Debug.Log(e); }
    }
    private async void QuickJoinLobby() 
    {
        try
        {
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            Debug.Log("Joined Lobby : " + lobby.Name + " , " + lobby.AvailableSlots);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }
    private static IEnumerator HeartBeatLobby(string lobbyId,float waitTime)
    {
        var delay = new WaitForSecondsRealtime(waitTime);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
    [Command]
    private async void LobbiesList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found : " + queryResponse.Results.Count);
            int i = 0;
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log("Lobby : " + lobby.Name + " , " + lobby.MaxPlayers + " ,Code : "+ lobby.Data["JoinCodeKey"].Value);
                UpdateLobbyList(lobby,i);
                i += 1;
            }
            i = 0;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    void UpdateLobbyList(Lobby lobby ,int i)
    {
        //// Clear the room list panel
        //foreach (Transform child in roomListPanel.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        // Get the list of rooms


        // Populate the room list panel
        //foreach (Lobby lobby in lobbies)
        //{
            // Create a new room list item from the prefab
            GameObject roomListItem = Instantiate(roomListItemPrefab, roomListPanel.transform);
            RectTransform rect = roomListItem.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(-225,100 - i * 50,0);
            // Set the text of the room list item
            TMP_Text roomNameText = roomListItem.GetComponentInChildren<TMP_Text>();
            roomNameText.text = lobby.Name;
            
            // Add a button click listener to join the room
            Button joinButton = roomListItem.GetComponentInChildren<Button>();
            joinButton.onClick.AddListener(() => JoinLobbyByCode(lobby.Data["JoinCodeKey"].Value,lobby.Id));
        //}
    }
    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            Debug.Log("Joined Lobby : " + queryResponse.Results[0].Name + "," + queryResponse.Results[0].AvailableSlots);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }
    private async void LeaveLobby()
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void KickPlayer()
    {
        try
        {
            string playerId = joinedLobby.Players[1].Id;
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject> {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                }
            });
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}