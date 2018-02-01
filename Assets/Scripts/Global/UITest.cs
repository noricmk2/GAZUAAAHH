using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MarketManager.Instance.RegistLineGraph(CoinName.NEETCOIN, gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
