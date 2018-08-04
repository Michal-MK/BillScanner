using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorSceneLoader : ScriptableObject {
	[MenuItem("Scenes/Swtich to MenuScene _F5")]
	static void Menu() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + "Menu" + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Swtich to GameScene _F6")]
	static void Game() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + "Shops" + ".unity", OpenSceneMode.Single);
	}
}