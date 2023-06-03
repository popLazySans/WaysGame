using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _diceSprite;
    [SerializeField] private List<Sprite> _diceSpritesList;
    private Animator animator;
    private TurnBaseStateMachine turnBaseStateScript;
    private void Start(){
        turnBaseStateScript = GetComponent<TurnBaseStateMachine>();
        _diceSprite.sprite = null;
    }
    private void Update(){
        Debug.Log("Dice");
    }
}
