using Igor.TCP;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionDataComponent : MonoBehaviour {
	public EventHandler<ConnectionData> OnConnectionDataParsed;
	public void Connect() {
		string ipAddress = transform.GetChild(0).GetComponent<InputField>().text;
		ushort port = ushort.Parse(transform.GetChild(1).GetComponent<InputField>().text);
	 	
		ShopsScene.connData = new ConnectionData(ipAddress, port);
		OnConnectionDataParsed(this, ShopsScene.connData);
	}
}