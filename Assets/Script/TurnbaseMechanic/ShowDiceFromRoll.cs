using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowDiceFromRoll : MonoBehaviour
{
    public Image dice;
    private Animator diceAnimator;
    private DiceAnimation _diceAnimationScript;
    //[SerializeField] private SpriteRenderer _diceImage;
    [SerializeField] private List<Sprite> diceSpritesList;
    // Start is called before the first frame update
    void Start()
    {
        dice.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetValueToDice(int roll)
    {
        dice.enabled = true;
        for (int i =0;i<diceSpritesList.Count;i++)
        {
            if (i == roll)
            {
                StartCoroutine(ShowDice(roll));
            }
        }
    }
    IEnumerator ShowDice(int showDiceNumber)
    {
        dice.sprite = diceSpritesList[showDiceNumber];
        yield return new WaitForSeconds(2f);
        dice.sprite = null;
        dice.enabled = false;
    }
}
