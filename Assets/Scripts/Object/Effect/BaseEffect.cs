using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : BaseObject {

    public float playTime { get; set; }
    float currTime;
    public bool isEnd { get; set; }

    public void Start()
    {
        isEnd = false;
        currTime = 0;
    }
   
    public override void CustomUpdate()
    {
        currTime += Time.deltaTime;
        if (currTime > playTime)
        {
            isEnd = true;
        }
    }
}
