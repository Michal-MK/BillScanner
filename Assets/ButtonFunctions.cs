using Igor.TCP;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour {

	public void Toggle(GameObject obj) {
		obj.SetActive(!obj.activeInHierarchy);
	}

	public void SendString() {
		Main.script.shopsScene.conn.SendData("Hello World!");
	}

	public void SendData() {
		Main.script.shopsScene.conn.SendData(new TCPData(new Item[] {
			new Item("Testing stuff", 20),
			new Item("Whatever", 100)
		}));
	}

	public void SendInt64() {
		Main.script.shopsScene.conn.SendData(58478978313);
	}
}
