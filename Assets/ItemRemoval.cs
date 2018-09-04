using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRemoval : MonoBehaviour {
	public Transform entriesPanel;
	public static List<ButtonBehaviour> buttonsToRemove;

	// Use this for initialization
	void Start () {
		GetComponent<Button>().onClick.AddListener(Removal);
		buttonsToRemove = new List<ButtonBehaviour>();
	}

	public static bool isToggled = false;

	private void Removal() {
		isToggled = !isToggled;
		if (!isToggled) {
			bool itemsRemoved = false;
			foreach (ButtonBehaviour b in buttonsToRemove) {
				b.itemMeta.RemoveSelf(b.parsedArrayPosition);
				itemsRemoved = true;
			}
			if (itemsRemoved) {
				ShopsScene.script.Repopulate();
				buttonsToRemove.Clear();
			}
		}
	}
}
