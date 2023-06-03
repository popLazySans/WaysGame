using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using System.Threading.Tasks;
using ParrelSync;
public class RelayManager : Singleton<RelayManager>
{
    public UnityTransport Transport => NetworkManager.Singleton.GetComponent<UnityTransport>();
    public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
    private async void Start()
    {
        //Delete(Before build)
                InitializationOptions options = new InitializationOptions();
//#if UNITY_EDITOR
        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
//#endif
        await UnityServices.InitializeAsync(options);
        //EndDelete
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId); };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    public async Task CreateRelay(Allocation allocation)
    {
        try
        {
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            //NetworkManager.Singleton.StartHost();
            Debug.Log("Join code = "+ joinCode);
        }
        catch (RelayServiceException e) { Debug.Log(e); }
    }
    public async Task JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Join Relay with" + joinCode);
            JoinAllocation joinAllocation =  await RelayService.Instance.JoinAllocationAsync(joinCode);
       
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            //NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e) { Debug.Log(e); }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
