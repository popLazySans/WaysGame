using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class PlayerData : NetworkBehaviour
{
    public NetworkVariable<int> player1CharId = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2CharId = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3CharId = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4CharId = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player1HP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2HP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3HP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4HP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player1ATK = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2ATK = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3ATK = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4ATK = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player1Def = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2Def = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3Def = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4Def = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player1EvadeCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2EvadeCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3EvadeCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4EvadeCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player1GuardCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player2GuardCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player3GuardCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> player4GuardCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private CharacterList characterList = new CharacterList();
    public TurnBaseStateMachine turnBaseManager = new TurnBaseStateMachine();
    public PlayWhenReady playWhenReady = new PlayWhenReady();
    // Start is called before the first frame update
    void Start()
    {
        turnBaseManager = GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<TurnBaseStateMachine>();
        playWhenReady= GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<PlayWhenReady>();
    }
    // Update is called once per frame
    void Update()
    {
        //if (!IsOwner) return;
        characterList = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
        if (characterList.selectedCharacter != 0 && playWhenReady.isStarted == false)
        {
            UpdateCharServerRpc(characterList.selectedCharacter-1,new ServerRpcParams());
        }
    }
    [ServerRpc (RequireOwnership =false)]
    public void UpdateCharServerRpc(int selectedChar,ServerRpcParams serverRpcParams)
    {
        switch (serverRpcParams.Receive.SenderClientId)
        {
            case 0:
                player1CharId.Value = selectedChar;
                player1HP.Value = characterList.characters[selectedChar].characteStat.HP;
                player1ATK.Value = characterList.characters[selectedChar].characteStat.ATK;
                player1Def.Value = characterList.characters[selectedChar].characteStat.DEF;
                break;
            case 1:
                player2CharId.Value = selectedChar;
                player2HP.Value = characterList.characters[selectedChar].characteStat.HP;
                player2ATK.Value = characterList.characters[selectedChar].characteStat.ATK;
                player2Def.Value = characterList.characters[selectedChar].characteStat.DEF;
                break;
            case 2:
                player3CharId.Value = selectedChar;
                player3HP.Value = characterList.characters[selectedChar].characteStat.HP;
                player3ATK.Value = characterList.characters[selectedChar].characteStat.ATK;
                player3Def.Value = characterList.characters[selectedChar].characteStat.DEF;
                break;
            case 3:
                player4CharId.Value = selectedChar;
                player4HP.Value = characterList.characters[selectedChar].characteStat.HP;
                player4ATK.Value = characterList.characters[selectedChar].characteStat.ATK;
                player4Def.Value = characterList.characters[selectedChar].characteStat.DEF;
                break;
        }
    }
}
