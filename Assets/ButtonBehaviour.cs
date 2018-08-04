using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Igor.Enums;

public class ButtonBehaviour : MonoBehaviour {
	public RawImage image;
	public InputField amount;
	public Button button;
	public Button removeItem;
	public Text itemName;

	public MeassurementUnit unit;

	public ItemMeta itemMeta;

	private bool added = false;

	private void Start() {
		amount.onEndEdit.AddListener(OnInputFieldTextEntered);
		switch (unit) {
			case MeassurementUnit.PIECES:
			amount.contentType = InputField.ContentType.IntegerNumber;
			break;
			case MeassurementUnit.GRAMS:
			amount.contentType = InputField.ContentType.IntegerNumber;
			break;
			case MeassurementUnit.LITRES:
			amount.contentType = InputField.ContentType.IntegerNumber;
			break;
		}
	}

	#region ButtonPressEvents
	private void OnInputFieldTextEntered(string value) {
		button.image.color = new Color(0.5f, 1f, 0.6f, 0.8f);
		removeItem.gameObject.SetActive(true);
	}

	public void OnButtonPress() {
		if (!added) {
			button.image.color = new Color(0.5f, 1f, 0.6f, 0.8f);
			EventSystem.current.SetSelectedGameObject(amount.gameObject);
			removeItem.gameObject.SetActive(true);
			added = true;
			Main.script.shopsScene.tcpManager.Add(itemMeta.item);
		}
	}

	public void OnRemoveButton() {
		button.image.color = new Color32(255, 255, 255, 100);
		removeItem.gameObject.SetActive(false);
		amount.text = itemMeta.parsed.mostCommonAmount.ToString();
		Main.script.shopsScene.tcpManager.Remove(itemMeta.item);
		added = false;
	}
	#endregion

	public void SetItemMeta(ItemMeta meta) {
		itemMeta = meta;
		amount.text = meta.parsed.mostCommonAmount.ToString();
		itemName.text = meta.item.name;
	}
}

