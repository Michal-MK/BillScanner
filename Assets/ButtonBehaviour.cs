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

	public int parsedArrayPosition;
	public MeassurementUnit unit;
	public ItemMeta itemMeta;

	private bool isAdded = false;
	private bool isSetForRemoval = false;

	private readonly Color defaultColor = new Color32(255, 255, 255, 100);
	private readonly Color selectedColor = new Color(0.5f, 1f, 0.6f, 0.8f);

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
		button.image.color = selectedColor;
		removeItem.gameObject.SetActive(true);
	}

	public void OnButtonPress() {
		if (!isAdded) {
			button.image.color = selectedColor;
			EventSystem.current.SetSelectedGameObject(amount.gameObject);
			removeItem.gameObject.SetActive(true);
			isAdded = true;
			Main.script.shopsScene.tcpManager.Add(itemMeta.item);
		}
	}

	public void OnRemoveButton() {
		button.image.color = defaultColor;
		removeItem.gameObject.SetActive(false);
		amount.text = itemMeta.parsed.mostCommonAmount.ToString();
		Main.script.shopsScene.tcpManager.Remove(itemMeta.item);
		isAdded = false;
	}
	#endregion

	public void SetItemMeta(ItemMeta meta, int arrayPosition) {
		parsedArrayPosition = arrayPosition;
		itemMeta = meta;
		amount.text = meta.parsed.mostCommonAmount.ToString();
		itemName.text = meta.item.name;
	}
}

