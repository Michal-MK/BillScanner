using Igor.TCP;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;


public class ShopsScene : MonoBehaviour {
	public static ShopsScene script;

	public static ConnectionData connData;

	public TCPClient conn { get; private set; }
	public DatabaseParser parser;

	private readonly ManualResetEventSlim evnt = new ManualResetEventSlim();

	public ShopSceneUI shopUI;

	public TCPManager tcpManager;

	public event EventHandler OnSuccessfullyConnected;

	private void Start() {
		script = this;
		tcpManager = new TCPManager();
		ConnectionChecker.instance.Recheck();
		Main.script.shopsScene = this;
		parser = new DatabaseParser(TransitionData.instance.shopType);

		Thread t = new Thread(new ThreadStart(delegate () {
			parser.OnShopEntryParsed += OnEntriesParsed;
			parser.Parse();

		})) { Name = "ShopParser" };
		t.Start();
		evnt.Wait();
		Populate();
	}


	public void Stash(TCPData data) {
		using (FileStream fs = new FileStream(Application.persistentDataPath + "/Stash/" + Guid.NewGuid() + ".stashedData", FileMode.Create)) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, data);
		};
	}

	public void GetConnectionData() {
		GameObject prefab = Resources.Load<GameObject>("ConnInfo");
		GameObject instantiated = Instantiate(prefab, transform);
		instantiated.GetComponent<ConnectionDataComponent>().OnConnectionDataParsed += OnConnectionInfoGet;
	}

	private void OnConnectionInfoGet(object sender, ConnectionData e) {
		SetUpConnection(e);
	}

	public void SetUpConnection(ConnectionData data) {
		conn = new TCPClient(data);
		OnSuccessfullyConnected?.Invoke(this, EventArgs.Empty);
	}


	private void OnEntriesParsed(object sender, ShopEntry e) {
		data = e;
		if (!evnt.IsSet) {
			evnt.Set();
		}
		else {
			Repopulate();
		}
	}

	ShopEntry data;
	public void Populate() {
		GameObject prefab = Resources.Load<GameObject>("Entry");
		Transform panel = GameObject.Find("_ItemEntries").transform;
		int counter = 0;
		foreach (ShopItem item in data.items) {
			GameObject entry = Instantiate(prefab, panel);
			ItemMeta meta = new ItemMeta(new Item(item.fullName, item.mostCommonAmount), item);
			entry.name = data.shopName + "-" + item.fullName;
			ButtonBehaviour behaviour = entry.GetComponent<ButtonBehaviour>();
			behaviour.SetItemMeta(meta, counter);
			counter++;
		}
	}

	public void Repopulate() {
		Transform panel = GameObject.Find("_ItemEntries").transform;
		foreach (ButtonBehaviour b in panel.GetComponentsInChildren<ButtonBehaviour>()) {
			Destroy(b.gameObject);
		}
		Populate();
	}
}

