using Igor.TCP;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;


public class ShopsScene : MonoBehaviour {
	public TCPClient conn { get; private set; }
	public DatabaseParser parser;

	private readonly ManualResetEventSlim evnt = new ManualResetEventSlim();

	public MainUI shopUI;

	public TCPManager tcpManager;

	private void Start() {
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
		using (FileStream fs = new FileStream(Application.persistentDataPath + "/Stash/" + Guid.NewGuid() + ".data", FileMode.Create)) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, data);
		};
	}

	public void GetConnectionData() {
		GameObject prefab = Resources.Load<GameObject>("ConnInfo");
		GameObject instantiated = Instantiate(prefab, transform);
		instantiated.GetComponent<ConnectionData>().OnConnectionDataParsed += OnConnectionInfoGet;
	}

	private void OnConnectionInfoGet(object sender, ConnectionData e) {
		SetUpConnection(e);
		e.OnConnectionDataParsed -= OnConnectionInfoGet;
	}

	public void SetUpConnection(ConnectionData data) {
		conn = new TCPClient(data);
		new Thread(new ThreadStart(delegate () {
			conn.ListenForData();
			conn.OnTCPDataReceived += OnTCPDataReceived;
			conn.OnStringReceived += OnStringReceived;
			conn.OnInt64Received += OnInt64Received;
		})) {
			Name = "Data Reception"
		}.Start();
	}

	private void OnInt64Received(object sender, Int64 e) {
		Debug.Log(e);
	}

	private void OnStringReceived(object sender, string e) {
		Debug.Log(e);
	}

	private void OnEntriesParsed(object sender, ShopEntry e) {
		data = e;
		if (!evnt.IsSet) {
			evnt.Set();
		}
		else {
			Populate();
		}
	}

	private void OnTCPDataReceived(object sender, TCPData e) {
		foreach (Item item in e.items) {
			print(item.name);
		}
	}

	ShopEntry data;
	public void Populate() {
		GameObject prefab = Resources.Load<GameObject>("Entry");
		Transform panel = GameObject.Find("_ItemEntries").transform;
		foreach (ShopItem item in data.items) {
			GameObject entry = Instantiate(prefab, panel);
			ItemMeta meta = new ItemMeta(new Item(item.fullName, item.mostCommonAmount), item);
			entry.name = data.shopName + "-" + item.fullName;
			ButtonBehaviour behaviour = entry.GetComponent<ButtonBehaviour>();
			behaviour.SetItemMeta(meta);
		}
	}
}

