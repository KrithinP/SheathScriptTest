using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      /*  if (gameObject.name == "Capsule")
        {
            transform.Rotate (3, 0, 0);
        }*/
        if (gameObject.name == "SilverCoin")
        {
            transform.Rotate (0, 0, 3);
            
        }
        if (gameObject.name == "GoldCoin")
        {
            transform.Rotate (0, 0, 3);
        }
        if (gameObject.name == "CopperCoin")
        {
            transform.Rotate (0, 0, 3);
        }
    }
   // void OnTriggerEnter(Collider other)
      void OnTriggerEnter(Collider other)
       {
           
         //print("Effects Trigger");
            if (gameObject.name == "SilverCoin")
            {
                AudioSource source = GetComponent<AudioSource>();
                source.Play();
                GetComponent<Renderer>().enabled = false;
                Destroy(gameObject,0.1f);
                
                //print("Destoy on Trigger");
                GM.coinTotal += 5 ;
            }
            if (gameObject.name == "GoldCoin")
            {
                AudioSource source = GetComponent<AudioSource>();
                source.Play();
                GetComponent<Renderer>().enabled = false;
                Destroy(gameObject,0.1f);
                
                //print("Destoy on Trigger");
                GM.coinTotal += 20 ;
            }
            if (gameObject.name == "CopperCoin")
            {
                AudioSource source = GetComponent<AudioSource>();
                source.Play();
                GetComponent<Renderer>().enabled = false;
                Destroy(gameObject,0.2f);
                
                //print("Destoy on Trigger");
                GM.coinTotal += 1 ;
            }

        }
 
}

