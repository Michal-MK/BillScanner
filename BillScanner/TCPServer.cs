using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

class TCPServer {

	TcpClient connected;
	NetworkStream stream;

	TCPData receivedData;

	public void Start() {
		Thread t = new Thread(NewThread);
		t.Start();

		TCPData data = new TCPData(new List<Item>());
		while (true) {
			Console.WriteLine("Send data?");
			Console.ReadLine();
			SendMerged();
		}
	}
	[Obsolete("This method is superceeded by SendData()")]
	public void SendData() {
		TCPData data = new TCPData(new List<Item>());
		using (MemoryStream ms = new MemoryStream()) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, data);
			byte[] bytes = ms.ToArray();

			SendLengthPacket(bytes.LongLength);
			Console.WriteLine("Sending data");
			stream.Write(bytes, 0, bytes.Length);
			Console.WriteLine(bytes.Length);

			ms.Seek(0, SeekOrigin.Begin);
			TCPData res = (TCPData)bf.Deserialize(ms);
			Console.WriteLine("Success");
		}
	}


	private void NewThread() {
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
		while (true) {
			TCPData receivedData = ReceiveDataMerged();
			Console.WriteLine("Got the data");
		}
	}
	[Obsolete("This method is now sureceeses by ReceiveDataMerged()")]
	public void ReceiveData() {
		while (true) {
			Int64 dataToReceive = ReceiveLengthPacket();
			Console.WriteLine("Waiting for Data 0/{0} bytes", dataToReceive);
			using (MemoryStream ms = new MemoryStream()) {
				byte[] buffer = new byte[dataToReceive];
				Int64 totalReceived = 0;
				while (totalReceived < dataToReceive) {
					totalReceived += stream.Read(buffer, (int)totalReceived, (int)(dataToReceive - totalReceived));
					Console.WriteLine("Waiting for Data {0}/{1} bytes", totalReceived, dataToReceive);
				}
				ms.Write(buffer, 0, buffer.Length);
				ms.Seek(0, SeekOrigin.Begin);
				BinaryFormatter bf = new BinaryFormatter();
				bf.Binder = new CustomBinder();
				receivedData = (TCPData)bf.Deserialize(ms);
				Console.WriteLine("Got the data");
			}
		}
	}


	public void SendMerged() {
		TCPData data = new TCPData(new List<Item>());
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
			Console.WriteLine(merged.Length);

			ms.Flush();

			ms.Seek(0, SeekOrigin.Begin);
			ms.Write(merged, 0, merged.Length);
			ms.Seek(8, SeekOrigin.Begin);

			TCPData dataa = (TCPData)bf.Deserialize(ms);
			Console.WriteLine("Success");
		}
	}

	public TCPData ReceiveDataMerged() {
		using (MemoryStream ms = new MemoryStream()) {
			Console.WriteLine("Waiting for PacketSize 0/8 bytes");
			byte[] packetSize = new byte[8];
			Int64 totalReceived = 0;
			while (totalReceived < 8) {
				totalReceived += stream.Read(packetSize, 0, 8);
				Console.WriteLine("Waiting for PacketSize " + totalReceived + "/" + 8 + " bytes");
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
			bf.Binder = new CustomBinder();
			TCPData receivedData = (TCPData)bf.Deserialize(ms);
			return receivedData;
		}
	}

	#region
	public void SendLengthPacket(Int64 length) {
		Console.WriteLine("Sending a length packet of {0} bytes", length);
		byte[] bytes = BitConverter.GetBytes(length);
		stream.Write(bytes, 0, bytes.Length);
	}

	public Int64 ReceiveLengthPacket() {
		byte[] longBytes = new byte[8];
		stream.Read(longBytes, 0, 8);
		return BitConverter.ToInt64(longBytes, 0);
	}
	#endregion
}

