using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

	public static Text getMainText { get; private set; }

	public Button multiButton;
	private bool isConnected;

	private void Start() {
		getMainText = GameObject.Find("_MainText").GetComponent<Text>();
		ConnectionChecker.instance.thread.OnConnectedStatusUpdate += OnConnectionUpdate;
	}


	private bool connectionStatus;
	private bool change;

	private void OnConnectionUpdate(object sender, bool e) {
		connectionStatus = e;
		change = true;
	}


	private void Update() {
		if (change) {
			if (connectionStatus) {
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
			change = false;
		}
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
		ConnectionChecker.instance.thread.OnConnectedStatusUpdate -= OnConnectionUpdate;
	}
}
