using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
    List<BaseEffect> listEffect;

    public void Awake()
    {
        listEffect = new List<BaseEffect>();
    }

    public BaseEffect CreateEffect(Vector3 _position, float _playTime, string _path)
    {

        GameObject effect = Instantiate(Resources.Load(_path) as GameObject, _position, Quaternion.identity);
        effect.AddComponent<BaseEffect>();

        BaseEffect baseEffect = effect.GetComponent<BaseEffect>();
        baseEffect.playTime = _playTime;
        
        listEffect.Add(baseEffect);
        return baseEffect;
    }

    public BaseEffect CreateEffect(Vector3 _position, float _playTime, string _path,Quaternion quaternion)
    {

        GameObject effect = Instantiate(Resources.Load(_path) as GameObject, _position, quaternion);
        effect.AddComponent<BaseEffect>();

        BaseEffect baseEffect = effect.GetComponent<BaseEffect>();
        baseEffect.playTime = _playTime;
        
        listEffect.Add(baseEffect);
        return baseEffect;
    }

    public void DestroyEffect(BaseEffect effect)
    {
        effect.isEnd = true;
    }

    public void Update()
    {
        List<int> removeList = new List<int>();
        foreach (BaseEffect effect in listEffect)
        {
            effect.CustomUpdate();
            if(effect.isEnd)
            {
                removeList.Add(listEffect.IndexOf(effect));
                Destroy(effect.gameObject);
            }
        }
        for (int i = listEffect.Count; i >= 0; i--)
        {
            foreach (int j in removeList)
            {
                if (i == j)
                {
                    listEffect.RemoveAt(j);
                }
            }
        }       
    }
}
