using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Transform target;
    public float speed = 5.0f;
    
    //public GameObject impactEffect; //
    private Vector3 midPosition;
    public void Seek (Transform _target)
    {
        target = _target;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        midPosition = target.position;
        midPosition.y += 1.0f;
        midPosition.z -= 1.0f;
        Vector3 dir = midPosition - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }
    void HitTarget()
    {
        Debug.LogWarning("Hit!!! Hit!!! Hit!!!");
        Destroy(gameObject);
        //GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
       // Destroy(effectIns, 2f);
    }
}
