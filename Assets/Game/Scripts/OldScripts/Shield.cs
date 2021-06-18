using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int shieldHealth = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "cannonball")
        {      
            shieldHealth--;
            Debug.Log("Shield Health :" + shieldHealth);
            Debug.LogWarning ("cannon ball connected" );
            if (shieldHealth<= 0)
            {
                Destroy (this.gameObject);
                Debug.LogWarning ("shield destroy success");
              
            }
           
            Destroy (other.gameObject);
        }
    }
}
