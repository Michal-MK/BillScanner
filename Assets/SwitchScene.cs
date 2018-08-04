using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {
	public void SwitchToScene(string sceneName) {
		TransitionData.instance = new TransitionData() {
			shopType = new System.Collections.Generic.KeyValuePair<string, int>(name, Main.script.shops.shopEntries[name]),
		};
		SceneManager.LoadScene(sceneName);
	}

	public void SwitchToSimple(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}
