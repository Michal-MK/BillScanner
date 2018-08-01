using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

class TCPServer<TData> {

	TcpClient connected;
	NetworkStream stream;
	private bool listeningForData;

	public event EventHandler<TData> OnDataReceived;

	public void Start() {
		Thread t = new Thread(StartServer);
		t.Start();
	}

	public void StopListening() {
		listeningForData = false;
	}

	private void StartServer() {
		IPAddress addr;
		using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
			socket.Connect("8.8.8.8", 65530);
			IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
			addr = endPoint.Address;
		}
		Console.WriteLine(addr);
		TcpListener listener = new TcpListener(addr, 7890);
		listener.Start();
		connected = listener.AcceptTcpClient();
		stream = connected.GetStream();
		Console.WriteLine("Client connected");
		listeningForData = true;
		DataReception();
	}


	public void SendMerged(TData data) {
		using (MemoryStream ms = new MemoryStream()) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, data);
			byte[] bytes = ms.ToArray();
			byte[] packetSize = BitConverter.GetBytes(bytes.LongLength);
			byte[] merged = new byte[bytes.Length + packetSize.Length];

			packetSize.CopyTo(merged, 0);
			bytes.CopyTo(merged, packetSize.Length);

			Console.WriteLine("Sending data");
			stream.Write(merged, 0, merged.Length);
		}
	}

	private void DataReception() {
		while (listeningForData) {
			TData data = ReceiveDataMerged();
			OnDataReceived(this, data);
		}
	}

	public TData ReceiveDataMerged() {
		using (MemoryStream ms = new MemoryStream()) {
			Console.WriteLine("Waiting for PacketSize bytes");
			byte[] packetSize = new byte[8];
			Int64 totalReceived = 0;
			while (totalReceived < 8) {
				totalReceived += stream.Read(packetSize, 0, 8);
			}
			totalReceived = 0;
			Int64 toReceive = BitConverter.ToInt64(packetSize, 0);
			Console.WriteLine("Waiting for Data 0/" + toReceive + " bytes");
			byte[] data = new byte[toReceive];
			while (totalReceived < toReceive) {
				if ((int)(toReceive - totalReceived) == 0) {
					break;
				}
				totalReceived += stream.Read(data, (int)totalReceived, (int)(toReceive - totalReceived));
				Console.WriteLine("Waiting for Data " + totalReceived + "/" + toReceive + " bytes");
			}
			ms.Flush();
			ms.Write(data, 0, data.Length);
			ms.Seek(0, SeekOrigin.Begin);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Binder = new MyBinder();
			TData receivedData = (TData)bf.Deserialize(ms);
			return receivedData;
		}
	}
}

