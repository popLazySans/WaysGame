using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;
using UnityEngine.UI;
public class PlayerRenderCharacter : NetworkBehaviour
{
    public int PlayerId;
    public int PlayerHP;
    public int PlayerDef;
    public int PlayerAtk;
    public string PlayerName;
    public SpriteRenderer charSprite;
    PlayerData playerData;
    LobbyController LobbyData;
    CharacterList characterData;
    public TMP_Text HPtext;
    public TMP_Text DEFtext;
    public TMP_Text ATKtext;
    public TMP_Text NameText;
    
    void Start()
    {
       playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
       LobbyData = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyController>();
       characterData = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
    }
    public void UpdatePlayerRender()
    {
        switch (OwnerClientId)
        {
            case 0:
                PlayerId = playerData.player1CharId.Value;
                PlayerHP = playerData.player1HP.Value;
                PlayerAtk = playerData.player1ATK.Value;
                PlayerDef = playerData.player1Def.Value;
                
                break;
            case 1:
                PlayerId = playerData.player2CharId.Value;
                PlayerHP = playerData.player2HP.Value;
                PlayerAtk = playerData.player2ATK.Value;
                PlayerDef = playerData.player2Def.Value;
                break;
            case 2:
                PlayerId = playerData.player3CharId.Value;
                PlayerHP = playerData.player3HP.Value;
                PlayerAtk = playerData.player3ATK.Value;
                PlayerDef = playerData.player3Def.Value;
                break;
            case 3:
                PlayerId = playerData.player4CharId.Value;
                PlayerHP = playerData.player3HP.Value;
                PlayerAtk = playerData.player3ATK.Value;
                PlayerDef = playerData.player3Def.Value;
                break;
        }
        PlayerName = LobbyData.getPlayerfromId(LobbyData.joinedLobby,Convert.ToInt32(OwnerClientId));
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerRender();
        charSprite.sprite = characterData.characters[PlayerId].characterSprite;
        NameText.text = PlayerName;
        HPtext.text = "HP " + PlayerHP;
        ATKtext.text = "ATK " + characterData.characters[PlayerId].characteStat.ATK + " + " + (PlayerAtk - characterData.characters[PlayerId].characteStat.ATK);
        DEFtext.text = "Def " + characterData.characters[PlayerId].characteStat.DEF + " + " + (PlayerDef - characterData.characters[PlayerId].characteStat.DEF);
    }
}
