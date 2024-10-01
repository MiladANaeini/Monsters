using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackAreaRayRight = default;
    private GameObject attackAreaRayLeft = default;


    private bool attacking = false;
    private float attackDuration = 0.5f;
    public float cooldownDuration = 3f;
    private Coroutine attackCoroutine = null; // Store reference to the coroutine
    private bool startTheAttack = false;
    void Start()
    {
        attackAreaRayRight = transform.GetChild(0).gameObject;
        attackAreaRayRight.SetActive(attacking);
        attackAreaRayLeft = transform.GetChild(1).gameObject;
        attackAreaRayLeft.SetActive(attacking);
        StartCoroutine(InitialDelay());
    }
    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(4f);
        startTheAttack = true;
    }
    void Update()
    {
           
        if (startTheAttack && attackCoroutine == null)
           {
                attackCoroutine = StartCoroutine(AttackSequence());
           }
     
    }
    private IEnumerator AttackSequence()
    {
     
        attacking = true;

        attackAreaRayRight.SetActive(attacking);
        attackAreaRayLeft.SetActive(attacking);

        yield return new WaitForSeconds(attackDuration);

        attacking = false;
        attackAreaRayRight.SetActive(attacking);
        attackAreaRayLeft.SetActive(attacking);

        yield return new WaitForSeconds(cooldownDuration);

      
        attackCoroutine = null;
    }
}
