using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target, aim, head;
    public float reloadTime = 1.0f , turnSpeed = 5.0f , firePauseTime =0.25f , range = 10;
    public Transform muzzlePos;
    public bool canSee = false;
    public GameObject muzzleFlash;
    public AudioClip fireSound;
    

    public float nextFireTime;
    public float nextMoveTime;
    private AudioSource audioS;

    public GameObject cannonBallPrefab;
    
        // Start is called before the first frame update
    void Start()
    {
        muzzleFlash.SetActive(false);
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AimFire();
        Tracking();
    }

    void AimFire()
    {
        if (target)
        {
            if(Time.time >= nextMoveTime)
            {
                aim.LookAt(target);
                aim.eulerAngles = new Vector3(0, aim.eulerAngles.y, 0);
                head.rotation = Quaternion.Lerp(head.rotation, aim.rotation,Time.deltaTime * turnSpeed);
            }

            if(Time.time >= nextFireTime && canSee == true) Fire();
            else
                {
                    muzzleFlash.SetActive(false);
                }
        }
        if(target==null)
        {
            muzzleFlash.SetActive(false);
        }
    }
    
    void Tracking()
    {
        Vector3 fwd = muzzlePos.TransformDirection(Vector3.back);
        RaycastHit hit;
        Debug.DrawRay(muzzlePos.position, fwd *range , Color.blue);
        Debug.LogWarning("In Tracking");
        if (Physics.Raycast(muzzlePos.position, fwd , out hit , range))
        {
            Debug.LogWarning("hit found");
            if(hit.collider.gameObject.name == "knight" )
            {
                Debug.LogWarning("Knight found");

                canSee =true;
            }
            else canSee = false;
        }
    }
    void Fire()
    {
        Debug.LogWarning("In Fire");
        nextFireTime = Time.time + reloadTime;
        nextMoveTime = Time.time + firePauseTime;
        audioS.PlayOneShot(fireSound);
        muzzleFlash.SetActive(true);
        GameObject cannonBallGO = (GameObject)Instantiate(cannonBallPrefab, muzzlePos.position, muzzlePos.rotation);
        CannonBall cannonBallScript = cannonBallGO.GetComponent<CannonBall>();
 //      GameObject tempcannonBallGO = (GameObject)Instantiate(cannonBallPrefab, target.position, target.rotation);
//        CannonBall tempBallScript = tempcannonBallGO.GetComponent<CannonBall>();

        if (cannonBallScript != null)
        {
            cannonBallScript.Seek(target);
  //          tempBallScript.Seek(target);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (!target)
        {
            Debug.LogWarning("target triggr working");
            if(col.gameObject.name == "knight")
            {
                Debug.LogWarning("stay triggr working");    
                nextFireTime = Time.time + (reloadTime * 0.5F);
                target = col.gameObject.transform;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.transform == target) target = null;
    }

}
