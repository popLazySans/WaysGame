using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    private CharacterList characterList = new CharacterList();
    public List<Image> chooseCharacter = new List<Image>();
    private Color normalCr = new Color(166f / 255f, 166f / 255f, 166f / 255f);
    private Color pressedCr = new Color(255f / 255f, 255f / 255f, 255f / 255f);

    private void Start()
    {
        characterList = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterList>();
        foreach (Image img in chooseCharacter) {
            img.GetComponent<Image>().color = new Color32(165, 165, 165, 255);
        }
    }
    private void Update()
    {
        if (characterList.selectedCharacter != 0) {
            for (int i = 0; i < chooseCharacter.Count; i++)
            {
                if (i == characterList.selectedCharacter - 1)
                {
                    chooseCharacter[i].GetComponent<Image>().color = new Color32(255, 255, 225, 255);
                    foreach (Image img1 in chooseCharacter)
                    {
                        if (img1 != chooseCharacter[characterList.selectedCharacter - 1])
                        {
                            img1.GetComponent<Image>().color = new Color32(165, 165, 165, 255);
                        }
                    }
                }
            }
        }
    }
}
