using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public int characterID;
    public string characterName;
    public string characterDetail;
    public CharacteStat characteStat;
    public CharacterSkill characterSkill;
    public CharacterStateMachine(string name,string detail,CharacteStat stat,CharacterSkill skill)
    {
        characterName = name;
        characterDetail = detail;
        characteStat = stat;
        characterSkill = skill;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
