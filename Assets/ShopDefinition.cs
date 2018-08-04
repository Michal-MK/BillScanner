using UnityEngine;
using UnityEngine.UI;

public class ShopDefinition : MonoBehaviour {

	public Button backBtn;
	public Button createBtn;
	public InputField shopNameInput;

	public void Create() {
		if(shopNameInput.text == "") {
			MenuUI.textLog.text = "Unable to create a shop with no name!";
			gameObject.SetActive(false);
			return;
		}
		Main.script.shops.Add(shopNameInput.text);
		MenuUI.textLog.text = "Shop Created!";
		gameObject.SetActive(false);
		FindObjectOfType<MenuUI>().Repopulate();
	}
}
