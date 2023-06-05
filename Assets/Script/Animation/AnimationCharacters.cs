using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
public class AnimationCharacters : NetworkBehaviour
{
    public Animator playerAnimator;
    [SerializeField] private OwnerNetworkAnimator ownerNetworkAnimator;
    [SerializeField] public bool isServerAnimated = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAnimator.runtimeAnimatorController != null)
            ownerNetworkAnimator = playerAnimator.gameObject.GetComponent<OwnerNetworkAnimator>();
            ownerNetworkAnimator.Animator = playerAnimator;
        if(isServerAnimated == true)
        {
            StartCoroutine(OnDamagedtoIdleWait());
            isServerAnimated = false;
        }
    }
    public void OnIdle()
    {
        //playerAnimator.SetTrigger("Idle");
        ownerNetworkAnimator.SetTrigger("Idle");
    }
    public void OnWalk()
    {
        //playerAnimator.SetTrigger("Walk");
        ownerNetworkAnimator.SetTrigger("Walk");
    }
    public void OnHit()
    {
        //playerAnimator.SetTrigger("Hit");
        ownerNetworkAnimator.SetTrigger("Hit");
    }
    public void OnDamaged()
    {
        //playerAnimator.SetTrigger("Damaged");
        ownerNetworkAnimator.SetTrigger("Damaged");
        Debug.Log("Damaged");
    }
    public void OnHittoIdle()
    {
        StartCoroutine(OnHittoIdleWait());
    }
    public IEnumerator OnHittoIdleWait()
    {
        OnHit();
        TurnBaseStateMachine.isAnimated = true;
        yield return new WaitForSeconds(1f);
        TurnBaseStateMachine.isAnimated = false;
        OnIdle();
    }
    public void OnDamagedtoIdle()
    {
        StartCoroutine(OnDamagedtoIdleWait());
    }
    public IEnumerator OnDamagedtoIdleWait()
    {
        OnDamaged();
        TurnBaseStateMachine.isAnimated = true;
        yield return new WaitForSeconds(1f);
        TurnBaseStateMachine.isAnimated = false;
        OnIdle();
    }
}
