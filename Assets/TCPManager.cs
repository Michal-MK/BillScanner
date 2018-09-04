using Igor.TCP;
using System.Collections.Generic;
using UnityEngine;

public class TCPManager {
	private readonly Dictionary<string, Item> items;
	public static PurchaseMeta? currentMeta;

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
		if (!currentMeta.HasValue) {
			Debug.Log("Purchase information are not filled in!");
			return default(TCPData);
		}
		return new TCPData(new List<Item>(items.Values),currentMeta.Value);
	}

	public int getItemAmount { get { return items.Count; } }
}
