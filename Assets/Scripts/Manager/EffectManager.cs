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

    public void CreateEffect(Vector3 _position, float _playTime, string _path)
    {   
        GameObject effect = Instantiate(Resources.Load(_path) as GameObject);

        effect.AddComponent<BaseEffect>();
        effect.transform.position = _position;
        BaseEffect baseEffect = effect.GetComponent<BaseEffect>();
        baseEffect.playTime = _playTime;
        
        listEffect.Add(baseEffect);
    }

    public void Update()
    {
        List<int> tmplist = new List<int>();
        foreach (BaseEffect effect in listEffect)
        {
            effect.CustomUpdate();
            if(effect.isEnd)
            {
                tmplist.Add(listEffect.IndexOf(effect));
                Destroy(effect.gameObject);
            }
        }
        for (int i = listEffect.Count; i >= 0; i--)
        {
            foreach (int j in tmplist)
            {
                if (i == j)
                {
                    listEffect.RemoveAt(j);
                }
            }
        }       
    }
}
