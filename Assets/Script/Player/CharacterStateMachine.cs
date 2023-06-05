using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public int characterID;
    public string characterName;
    public SpriteRenderer characterSprite;
    public Animator characterAnimator;
    public string characterDetail;
    public CharacteStat characteStat;
    public CharacterSkill characterSkill;
    void Start(){
        // characterSprite = gameObject.GetComponent<SpriteRenderer>();
    }
    public CharacterStateMachine(string name,Sprite charImg, string detail,CharacteStat stat,CharacterSkill skill)
    {
        characterName = name;
        characterSprite.sprite = charImg;
        characterDetail = detail;
        characteStat = stat;
        characterSkill = skill;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
