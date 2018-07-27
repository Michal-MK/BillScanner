using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Main : MonoBehaviour {

	public enum MeassurementUnit {
		PIECES,
		WEIGHT,
		LITRES
	}

	public static Text getText { get; private set; }
	public ConnectionManager conn;

	// Use this for initialization
	void Start () {
		getText = transform.Find("Text").GetComponent<Text>();
		conn = new ConnectionManager();
		conn.prefab = Resources.Load<GameObject>("ConnInfo");
		conn.ConnectInfo();
	}

	public void Send() {
		conn.SendData();
	}
}
