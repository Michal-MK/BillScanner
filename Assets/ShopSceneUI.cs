using Igor.TCP;
using UnityEngine;
using UnityEngine.UI;

public class ShopSceneUI : MonoBehaviour {

	public static Text getMainText { get; private set; }

	public Button multiButton;
	private bool isOnline;
	private bool isConnected;

	private void Start() {
		getMainText = GameObject.Find("_MainText").GetComponent<Text>();
		ConnectionChecker.instance.OnConnectionChange += OnConnectionUpdate;
		Main.script.shopsScene.OnSuccessfullyConnected += OnConnected;
	}

	private void OnConnected(object sender, System.EventArgs e) {
		if (isOnline) {
			multiButton.GetComponentInChildren<Text>().text = "Send Data";
			multiButton.onClick.RemoveAllListeners();
			multiButton.onClick.AddListener(TriggerSendData);
		}
	}

	private void OnConnectionUpdate(object sender, bool e) {
		if (e) {
			if (!isConnected) {
				multiButton.GetComponentInChildren<Text>().text = "Connect";
				multiButton.onClick.RemoveAllListeners();
				multiButton.onClick.AddListener(TriggerGetConnection);
			}
			else {
				multiButton.GetComponentInChildren<Text>().text = "Send Data";
				multiButton.onClick.RemoveAllListeners();
				multiButton.onClick.AddListener(TriggerSendData);
			}
		}
		else {
			multiButton.GetComponentInChildren<Text>().text = "Stash Data";
			multiButton.onClick.RemoveAllListeners();
			multiButton.onClick.AddListener(TriggerStashData);
		}
		isOnline = e;
	}

	public void TriggerSendData() {
		Main.script.shopsScene.conn.SendData(Main.script.shopsScene.tcpManager.GetCurrentData());
	}

	public void TriggerStashData() {
		Main.script.shopsScene.Stash(Main.script.shopsScene.tcpManager.GetCurrentData());
	}

	public void TriggerGetConnection() {
		Main.script.shopsScene.GetConnectionData();
	}

	private void OnDestroy() {
		ConnectionChecker.instance.OnConnectionChange -= OnConnectionUpdate;
	}
}
