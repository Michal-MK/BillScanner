using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager {

	public GameObject prefab;
	private GameObject instatntiated;

	private ManualResetEvent evnt = new ManualResetEvent(false);

	private TcpClient client;
	private byte[] bytes = new byte[2048];


	public void ConnectInfo() {
		instatntiated = UnityEngine.Object.Instantiate(prefab);
		instatntiated.GetComponentInChildren<InputField>().onEndEdit.AddListener(ParseIP);
	}

	private void ParseIP(string value) {
		IPAddress addr;
		if (IPAddress.TryParse(value, out addr)) {
			client = new TcpClient();
			client.Connect(addr, 7890);
			Main.getText.text = "Connection established";
		}
		else {
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(instatntiated);
		}
	}

	public void SendData() {
		NetworkStream ns = client.GetStream();
		IAsyncResult ress = ns.BeginRead(bytes, 0, bytes.Length, OnReceived, ":ok_hand:");
		evnt.WaitOne();
		int data_count = ns.EndRead(ress);
		evnt.Reset();
		Console.WriteLine(Encoding.Default.GetString(SubArray(bytes, 0, data_count)));
		Console.ReadLine();
	}

	private void OnReceived(IAsyncResult ar) {
		evnt.Set();
	}

	public T[] SubArray<T>(T[] data, int index, int length) {
		T[] result = new T[length];
		Array.Copy(data, index, result, 0, length);
		return result;
	}
}

