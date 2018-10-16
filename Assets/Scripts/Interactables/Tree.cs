using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    int treeHealth = 2;

    public GameObject woodYieldPrefab;

    float treeCuttingCooldown = 1;

    Interactable interactable;

    public void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.AddInteraction(Cut);
    }

    public void Cut()
    {
        GetComponent<Animator>().SetTrigger("Damaged");
        treeHealth--;        

        if(treeHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        int woodYieldAmount = Random.Range(3, 5);
        Transform target = interactable.GetInteractor();

        for(int i = 0; i < woodYieldAmount; i++)
        {
            GameObject woodYield = Instantiate(woodYieldPrefab, (Vector2)transform.position + Random.insideUnitCircle, Quaternion.identity);
            woodYield.GetComponent<Pickup>().AssignTarget(target);
        }

        interactable.DeSelect();
        Destroy(interactable);
        Destroy(this);
    }  
}
