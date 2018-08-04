using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDefinition : MonoBehaviour {


	public InputField itemNameInput;
	public InputField priceInput;
	public InputField weightInput;

	public RawImage image;

	public void Create() {
		string name = itemNameInput.text;
		string price = priceInput.text.Replace(',', '.');
		int weight;
		if (int.TryParse(weightInput.text, out weight)) {
			ItemDefinitionStruct item = new ItemDefinitionStruct(name, price, "", weight);
			Main.script.shopsScene.parser.AddItem(item);
			gameObject.SetActive(false);
		}
	}


	public struct ItemDefinitionStruct {
		public ItemDefinitionStruct(string name, string price, string imgPath, int weight) {
			this.name = name;
			this.price = price;
			this.imgPath = imgPath;
			this.weight = weight;
		}

		public string name { get; }
		public string price { get; }
		public int weight { get; }

		public string imgPath { get; }
	}
}
