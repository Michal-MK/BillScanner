using Igor.TCP;
using System;
using System.Collections.Generic;


public class TCPManager {
	private readonly Dictionary<string, Item> items;

	public TCPManager() {
		items = new Dictionary<string, Item>();
	}


	public void Add(Item i) {
		items.Add(i.name, i);
	}

	public void Remove(Item i) {
		items.Remove(i.name);
	}

	public void Remove(string name) {
		items.Remove(name);
	}

	public TCPData GetCurrentData() {
		return new TCPData(new List<Item>(items.Values));
	}
}
