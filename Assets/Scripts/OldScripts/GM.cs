using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System;
public class GM : MonoBehaviour
{
    public static float vertVel = 0;
    public static int coinTotal = 0;
    public static float timeTotal = 0;
    public static float zVelAdj = 0.9f;
    public static string lvlCompStatus ="";
    public float waitToLoad = 0;
    public Transform bbNoPit;
    public Transform bbPitMid;
    public float zScenePos = 178;
    //public Random random;
    // Start is called before the first frame update
    void Start()
    {
        //random = new Random();
/*        Instantiate (bbNoPit,new Vector3(0, 2.87f, 154),bbNoPit.rotation);
        Instantiate (bbNoPit,new Vector3(0, 2.87f, 160),bbNoPit.rotation);
        Instantiate (bbPitMid,new Vector3(0, 2.87f, 166),bbPitMid.rotation);
        Instantiate (bbPitMid,new Vector3(0, 2.87f, 172),bbPitMid.rotation);
        
*/       
    }

    // Update is called once per frame
    void Update()
    {

 /*     timeTotal += Time.deltaTime;
        if (zScenePos<220){
            float num = Random.Range(0.0f, 1.0f);
            if(num<0.5)
                Instantiate (bbNoPit,new Vector3(0, 2.87f, zScenePos),bbNoPit.rotation);
            else
                Instantiate (bbPitMid,new Vector3(0, 2.87f, zScenePos),bbPitMid.rotation);
            zScenePos += 6;
        }

      if(lvlCompStatus == "fail"){
          waitToLoad += Time.deltaTime;
      }
      if(waitToLoad > 2)
      {
          SceneManager.LoadScene("levelComp");

      }
*/
    }
}
