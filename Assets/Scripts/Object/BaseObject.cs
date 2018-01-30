using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{

    public ObjectState State
    {
        get; set;
    }

    public virtual void Init()
    {

    }

    public virtual void CustomUpdate()
    {

    }
}
