using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainServerManager : MonoBehaviour
{
    // Start is called before the first frame update
  public void OnServerButtonClicked()
    {
        NetworkManager.Singleton.StartServer();
    }
  public void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
    }
  public void OnClientButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
    }
}
