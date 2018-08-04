using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using Igor.TCP;

public class MenuUI : MonoBehaviour {

	public Text stashInfo;
	public Button stashResolveBtn;

	public static Text textLog { get; private set; }

	private DirectoryInfo stashInfoDir;

	public Transform shopList;

	private bool isOnline;


	 void Start () {
		ConnectionChecker.instance.thread.OnConnectedStatusUpdate += OnConnectionUpdate;
		textLog = GameObject.Find("_TextLog").GetComponent<Text>();
		stashInfoDir = new DirectoryInfo(Application.persistentDataPath + "/Stash/");
		FileInfo[] data = stashInfoDir.GetFiles("*.data");
		if(data.Length > 0) {
			stashInfo.text = string.Format("Found {0} items in stash!", data.Length);
		}
		else {
			stashInfo.text = "Stash is empty!";
		}

		PopulateShops();
		if (isOnline && data.Length > 0) {
			stashResolveBtn.interactable = true;
		}
	}

	private void OnConnectionUpdate(object sender, bool e) {
		isOnline = e;
	}

	public void SendStashed() {
		BinaryFormatter bf = new BinaryFormatter();
		foreach (FileInfo file in stashInfoDir.GetFiles("*.data")) {
			using(FileStream fs = File.OpenRead(file.FullName)) {
				TCPData data = (TCPData)bf.Deserialize(fs);
				Main.script.shopsScene.conn.SendData(data);
			}
		}
	}

	public void PopulateShops() {
		GameObject prefab = Resources.Load<GameObject>("_ShopTransition");
		shopList = GameObject.Find("ShopList").transform;
		foreach (KeyValuePair<string, int> item in Main.script.shops.shopEntries) {
			GameObject shopEntry = Instantiate(prefab, shopList);

			shopEntry.GetComponentInChildren<Text>().text = item.Key;
			shopEntry.name = item.Key;
		}
	}

	public void Repopulate() {
		foreach (Button button in shopList.GetComponentsInChildren<Button>()) {
			Destroy(button.gameObject);
		}
		PopulateShops();
	}
}
