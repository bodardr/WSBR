using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCursor : MonoBehaviour {

    private void Start()
    {
       //Cursor.visible = false; 
    }
    
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
              
        pos.z = 0;

        transform.position = pos;
	}
}
