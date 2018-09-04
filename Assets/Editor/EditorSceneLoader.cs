using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorSceneLoader : ScriptableObject {
	[MenuItem("Scenes/Swtich to MenuScene _F5")]
	static void Menu() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + "Menu" + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Swtich to ShopsScene _F6")]
	static void Purchase() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + "Shops" + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Swtich to ShopManagement _F7")]
	static void Management() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + "ShopManagement" + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Swtich to Shoot _F8")]
	static void shoot() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + "ScreenShots" + ".unity", OpenSceneMode.Single);
	}
}