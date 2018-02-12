using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    Animation UIAnimation;
    string clipName;
    private void Awake()
    {
        UIAnimation = this.GetComponent<Animation>();
        clipName = "CoinItemChange";
    }

    public void PlayReverseAnimation()
    {
        UIAnimation[clipName].speed = -1;
        UIAnimation[clipName].time = UIAnimation[clipName].length;
        UIAnimation.Play(clipName);
    }

    public void PlayAnimation()
    {
        UIAnimation[clipName].speed = 1;
        UIAnimation[clipName].time = 0;
        UIAnimation.Play(clipName);
    }
}
