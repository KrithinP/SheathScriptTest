using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    // Start is called before the first frame update
    public string triggerName;
    public float orbVel;
    
    public Segment(string tName, float oVel){
        triggerName = tName;
        orbVel = oVel;
    }
    
}
