using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Server {
	class Program {
		static void Main(string[] args) {
			Test();
		}


		ManualResetEventSlim evnt = new ManualResetEventSlim();

		public static void Test() {
			Thread t = new Thread(NewThread);
			t.Start();
		}

		private static void NewThread() {
			IPAddress addr;
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
				socket.Connect("8.8.8.8", 65530);
				IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
				addr = endPoint.Address;
			}
			Console.WriteLine(addr);
			TcpListener listener = new TcpListener(addr, 7890);
			listener.Start();
			TcpClient c = listener.AcceptTcpClient();
			Console.WriteLine("Client connected");
			NetworkStream stream = c.GetStream();
			stream.Write(Encoding.UTF8.GetBytes("Hello"), 0, Encoding.UTF8.GetByteCount("Hello"));
			Console.WriteLine("Data sent");
		}

		private void OnAccept(IAsyncResult ar) {
			Console.WriteLine("Connected");
			evnt.Set();
		}
	}
}
