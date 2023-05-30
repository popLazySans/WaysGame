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
    }
}
