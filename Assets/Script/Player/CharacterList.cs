using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    public List<CharacterStateMachine> characters = new List<CharacterStateMachine>();
    public int selectedCharacter;

    public void Start()
    {
    }
    public void SelectCharacter(int select)
    {
        selectedCharacter = select;
        Debug.Log("Selected character: " + characters[selectedCharacter-1].name);
        Debug.Log("Selected character: " + characters[selectedCharacter-1].characterSprite);
    }
}
