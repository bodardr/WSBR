using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private Transform target;
    public string message;
	
    public void AssignTarget(Transform target)
    {
        this.target = target;
    }

	// Update is called once per frame
	void Update () {
        if(target != null)
            transform.position = Vector2.Lerp(transform.position, target.position, 0.05f);
	}



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == target)
        {
            target.GetComponent<Inventory>().SendMessage(message);
            Destroy(gameObject);
        }
    }
}
