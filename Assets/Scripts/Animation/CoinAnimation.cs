using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    GameObject CoinObject; //애니매이션이 들어갈 코인 오브젝트 프리팹
    Transform spawnTarget; //애니메이션을 실행할 캐릭터의 트랜스폼 
    Transform attackTarget; //공격을 당할 대상의 트랜스폼
    Coin coinInfo; //전투중에 선택한 코인의 정보
    List<GameObject> coinObjectList = new List<GameObject>(); //생성한 프리팹의 리스트
    CoinAnimType animType; //코인의 애니메이션 타입
    GameObject parent; //정리용 부모 오브젝트
    float speed = 40.0f; //공격시의 스피드

    bool _startAnimation = false;
    public bool StartAnimation //애니메이션을 실행할지의 여부를 판단하는 변수
    {
        get { return _startAnimation; }
        set { _startAnimation = value; }
    }

    public void CoinAnimInit(Coin coin, GameObject instObj, Transform sTarget, Transform aTarget, CoinAnimType type) //애니메이션 정보 초기화
    {
        parent = new GameObject("CoinRoot");
        CoinObject = instObj;
        spawnTarget = sTarget;
        attackTarget = aTarget;
        coinInfo = coin;
        animType = type;
    }

    public IEnumerator SpawnCoin() //코인 프리팹 생성 메서드
    {
        int amount = ConstValue.ThirdRound;
        Vector3 setPos = spawnTarget.position;
        switch (animType)
        {
            case CoinAnimType.TYPE_BASE_ATTACK_ANIM:
                {                    
                    coinObjectList.Add(Instantiate(CoinObject,setPos,Quaternion.identity));
                    yield break;
                }
                break;
            case CoinAnimType.TYPE_BASE_DEFFENCE_ANIM:
                {

                }
                break;
            case CoinAnimType.TYPE_GATE_BABYLON_ANIM:
                {
                    int[] index;

                    coinObjectList.Add(Instantiate(CoinObject, setPos, Quaternion.identity));
                    int secondRound = amount - ConstValue.FirstRound;

                    index = Util.GetRandomIndex(1, ConstValue.FirstRound);
                    for (int j = 1; j < ConstValue.FirstRound; j++)
                    {
                        SetGOBCoinBullet(ConstValue.FirstRound - 1, index[j - 1], 1.0f, setPos);
                        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                    }

                    index = Util.GetRandomIndex(ConstValue.FirstRound, ConstValue.SecondRound);
                    for (int j = ConstValue.FirstRound; j < ConstValue.SecondRound; j++)
                    {
                        SetGOBCoinBullet(ConstValue.SecondRound - ConstValue.FirstRound, index[j - ConstValue.FirstRound], 2.0f, setPos);
                        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                    }

                    index = Util.GetRandomIndex(ConstValue.SecondRound, amount);
                    for (int j = ConstValue.SecondRound; j < amount; j++)
                    {
                        SetGOBCoinBullet(amount - ConstValue.SecondRound, index[j - ConstValue.SecondRound], 3.0f, setPos);
                        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                    }

                    for (int i = 0; i < coinObjectList.Count; ++i)
                        coinObjectList[i].transform.SetParent(parent.transform);

                    yield break;
                }
        }
    }

    void SetGOBCoinBullet(int degreeCount, int index, float range, Vector3 orgPos) //애니메이션 타입이 GOB의 경우 생성위치를 결정하는 메서드
    {
        float degree = (360 / degreeCount) * index;
        float x = Mathf.Cos(Mathf.Deg2Rad * degree);
        float y = Mathf.Sin(Mathf.Deg2Rad * degree);
        float len = Random.Range(range, range + 0.5f);
        Vector3 dir = new Vector3(x, y, 0);

        Vector3 pos = orgPos + (dir * len);
        Vector3 effectPos = pos;
        Vector3 dirTarget = (attackTarget.position - spawnTarget.position).normalized;
        if (dir.z > 0)
        {
            effectPos.z += 0.8f;
        }
        else if (dir.z < 0 )
        {
            effectPos.z -= 0.8f;
        }
        Quaternion effectQuarernion = Quaternion.identity;
        effectQuarernion.eulerAngles = new Vector3(90, 0,0);
  
        EffectManager.Instance.CreateEffect(effectPos, 0.8f, "Prefab/Effect/GOBEffect", effectQuarernion);

        coinObjectList.Add(Instantiate(CoinObject, effectPos, Quaternion.identity));
    }

    void CoinAttackAnimation() //공격시의 애니메이션 메서드 
    {
        if (coinObjectList.Count == 0)
        {
            Debug.LogError("CoinObject is empty");
            return;
        }

        switch (animType)
        {
            case CoinAnimType.TYPE_BASE_ATTACK_ANIM:
                for (int i = 0; i < coinObjectList.Count; ++i)
                {
                    Vector3 dir = (attackTarget.position + new Vector3(0,2)) - coinObjectList[i].transform.position;                   

                    if (dir.sqrMagnitude < Random.Range(0.5f, 1))
                    {
                        Vector3 position = coinObjectList[i].transform.position;
                        Destroy(coinObjectList[i]);
                        coinObjectList.RemoveAt(i);
                        EffectManager.Instance.CreateEffect(position, 0.8f, "Prefab/Effect/GOBHIT");

                        if (coinObjectList.Count == 0)
                            StartAnimation = false;
                        break;
                    }
                    else
                        dir = dir.normalized;

                    coinObjectList[i].transform.position += dir * speed/2 * Time.deltaTime;
                }
                break;
            case CoinAnimType.TYPE_BASE_DEFFENCE_ANIM:
                break;
            case CoinAnimType.TYPE_GATE_BABYLON_ANIM:
                for (int i = 0; i < coinObjectList.Count; ++i)
                {
                    Vector3 dir = attackTarget.position - coinObjectList[i].transform.position;

                    if (dir.sqrMagnitude < Random.Range(0.5f,1))
                    {
                        Vector3 position = coinObjectList[i].transform.position;
                        Destroy(coinObjectList[i]);
                        coinObjectList.RemoveAt(i);
                        EffectManager.Instance.CreateEffect(position, 0.8f, "Prefab/Effect/GOBHIT");

                        if (coinObjectList.Count == 0)
                            StartAnimation = false;
                        break;
                    }                   
                    else
                        dir = dir.normalized;

                    coinObjectList[i].transform.position += dir * speed * Time.deltaTime;
                }
                break;
        }
    }

    void GOBCoinMoveFoward() //애니메이션 타입이 GOB인 경우 초기 움직임
    {
        if (coinObjectList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < coinObjectList.Count; ++i)
        {
            Transform coinTrans = coinObjectList[i].transform;
            Vector3 dir = (attackTarget.position - spawnTarget.position).normalized;
            if (dir.z > 0 && coinTrans.position.z < spawnTarget.transform.position.z + 1)
            {
                coinTrans.Translate(coinTrans.forward * Time.deltaTime*0.4f);
            }
            else if (dir.z < 0 && coinTrans.position.z > spawnTarget.transform.position.z - 1)
            {
                coinTrans.Translate(coinTrans.forward * -Time.deltaTime);
            }
        }
    }

    private void Update()
    {
        if (animType == CoinAnimType.TYPE_GATE_BABYLON_ANIM)
        {
            GOBCoinMoveFoward();
        }

        if (StartAnimation == true)
            CoinAttackAnimation();
    }
}
