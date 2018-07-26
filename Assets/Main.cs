using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static void Main(string[] args) {
		Console.o
		Console.WriteLine("Client");
		Console.WriteLine("Enter host address");
		//IPAddress addr;
		//using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
		//	socket.Connect("8.8.8.8", 65530);
		//	IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
		//	addr = endPoint.Address;
		//}
		byte[] bytes = new byte[4086];
		TcpClient c = new TcpClient();
		c.Connect(IPAddress.Parse(Console.ReadLine()), 7890);
		Console.WriteLine("Connection established");
		NetworkStream ns = c.GetStream();
		IAsyncResult ress = ns.BeginRead(bytes, 0, bytes.Length, OnReceived, ":ok_hand:");
		evnt.Wait();
		int data_count = ns.EndRead(ress);
		evnt.Reset();

		Console.WriteLine(System.Text.Encoding.Default.GetString(SubArray(bytes, 0, data_count)));
		Console.ReadLine();

		return;
	}

	static ManualResetEventSlim evnt = new ManualResetEventSlim();

	private static void OnReceived(IAsyncResult ar) {
		Console.WriteLine("Got some data!");
		evnt.Set();

	}

	public static T[] SubArray<T>(T[] data, int index, int length) {
		T[] result = new T[length];
		Array.Copy(data, index, result, 0, length);
		return result;
	}
}
