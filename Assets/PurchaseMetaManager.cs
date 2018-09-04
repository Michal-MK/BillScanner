using Igor.TCP;
using System;
using UnityEngine;

public class PurchaseMetaManager : MonoBehaviour {
	public void GetMeta() {
		TCPManager.currentMeta = new PurchaseMeta(DateTime.Now, TransitionData.instance.shopType.Key, ShopsScene.script.tcpManager.getItemAmount);
	}
}

