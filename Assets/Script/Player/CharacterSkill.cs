using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CharacterSkill : NetworkBehaviour
{
    public string skillName;
    public int skillID;

    public void UseSkill(int playerId)
    {
      
        switch (skillID)
        {
            case 0:
                SelectPlayerToUseSkill1(playerId);
                break;
            case 1:
                SelectPlayerToUseSkill2(playerId);
                break;
            case 2:
                SelectPlayerToUseSkill3(playerId);
                break;
            case 3:
                SelectPlayerToUseSkill4(playerId);
                break;
        }
    }
    public void SelectPlayerToUseSkill1(int playernum)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        CharacterList characterList = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
        switch (playernum)
        {
            case 0:
                playerData.player1Def.Value += characterList.characters[0].characteStat.DEF;
                playerData.player1GuardCount.Value += 1;
                break;
            case 1:
                playerData.player2Def.Value += characterList.characters[0].characteStat.DEF;
                playerData.player2GuardCount.Value += 1;
                break;
            case 2:
                playerData.player3Def.Value += characterList.characters[0].characteStat.DEF;
                playerData.player3GuardCount.Value += 1;
                break;
            case 3:
                playerData.player4Def.Value += characterList.characters[0].characteStat.DEF;
                playerData.player4GuardCount.Value += 1;
                break;
        }
    }
    public void SelectPlayerToUseSkill2(int playernum)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        CharacterList characterList = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
        switch (playernum)
        {
            case 0:
                playerData.player2HP.Value -= playerData.player1ATK.Value/3;
                playerData.player3HP.Value -= playerData.player1ATK.Value / 3;
                playerData.player4HP.Value -= playerData.player1ATK.Value / 3;
                break;
            case 1:
                playerData.player1HP.Value -= playerData.player2ATK.Value / 3;
                playerData.player3HP.Value -= playerData.player2ATK.Value / 3;
                playerData.player4HP.Value -= playerData.player2ATK.Value / 3;
                break;
            case 2:
                playerData.player1HP.Value -= playerData.player3ATK.Value / 3;
                playerData.player2HP.Value -= playerData.player3ATK.Value / 3;
                playerData.player4HP.Value -= playerData.player3ATK.Value / 3;
                break;
            case 3:
                playerData.player1HP.Value -= playerData.player4ATK.Value / 3;
                playerData.player2HP.Value -= playerData.player4ATK.Value / 3;
                playerData.player3HP.Value -= playerData.player4ATK.Value / 3;
                break;
        }
    }
    public void SelectPlayerToUseSkill3(int playernum)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        CharacterList characterList = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
        switch (playernum)
        {
            case 0:
                playerData.player1HP.Value += playerData.player1ATK.Value;
                if (playerData.player1HP.Value > characterList.characters[2].characteStat.HP)
                {
                    playerData.player1HP.Value = characterList.characters[2].characteStat.HP;
                }
                break;
            case 1:
                playerData.player2HP.Value += playerData.player2ATK.Value;
                if (playerData.player2HP.Value > characterList.characters[2].characteStat.HP)
                {
                    playerData.player2HP.Value = characterList.characters[2].characteStat.HP;
                }
                break;
            case 2:
                playerData.player3HP.Value += playerData.player3ATK.Value;
                if (playerData.player3HP.Value > characterList.characters[2].characteStat.HP)
                {
                    playerData.player3HP.Value = characterList.characters[2].characteStat.HP;
                }
                break;
            case 3:
                playerData.player4HP.Value += playerData.player4ATK.Value;
                if (playerData.player4HP.Value > characterList.characters[2].characteStat.HP)
                {
                    playerData.player4HP.Value = characterList.characters[2].characteStat.HP;
                }
                break;
        }
    }
    public void SelectPlayerToUseSkill4(int playernum)
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerData>();
        CharacterList characterList = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
        switch (playernum)
        {
            case 0:
                playerData.player1EvadeCount.Value += 1;
                break;
            case 1:
                playerData.player2EvadeCount.Value += 1;
                break;
            case 2:
                playerData.player3EvadeCount.Value += 1;
                break;
            case 3:
                playerData.player4EvadeCount.Value += 1;
                break;
        }
    }
}
