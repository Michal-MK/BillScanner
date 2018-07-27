using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class ButtonBehaviour : MonoBehaviour {
	public RawImage image;
	public InputField amount;
	public Button button;
	public Button removeItem;

	public Main.MeassurementUnit unit;

	private void Start() {
		amount.onEndEdit.AddListener(OnInputFieldTextEntered);
		switch (unit) {
			case Main.MeassurementUnit.PIECES:
			amount.contentType = InputField.ContentType.IntegerNumber;
			break;
			case Main.MeassurementUnit.WEIGHT:
			amount.contentType = InputField.ContentType.DecimalNumber;
			break;
			case Main.MeassurementUnit.LITRES:
			amount.contentType = InputField.ContentType.IntegerNumber;
			break;
		}
	}

	private void OnInputFieldTextEntered(string value) {
		button.image.color = new Color(0.5f, 1f, 0.6f, 0.8f);
		removeItem.gameObject.SetActive(true);
	}

	public void OnButtonPress() {
		button.image.color = new Color(0.5f, 1f, 0.6f, 0.8f);
		EventSystem.current.SetSelectedGameObject(amount.gameObject);
		removeItem.gameObject.SetActive(true);
		amount.text = "1";
	}



	public void OnRemoveButton() {
		button.image.color = new Color32(255, 255, 255, 100);
		removeItem.gameObject.SetActive(false);
		amount.text = "";
	}
}

