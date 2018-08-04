using UnityEngine;
using System.IO;
using Igor.Enums;

public class Main : MonoBehaviour {

	public static Main script { get; set; }

	public ShopsScene shopsScene;
	public Shops shops;

	private void Awake() {
		if (script == null) {
			script = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void Start() {
		FileInfo file = new FileInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + "config.json");
		DirectoryInfo dirShops = new DirectoryInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + "Shops");
		DirectoryInfo dirStash = new DirectoryInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + "Stash");
		if (!file.Exists) {
			File.WriteAllText(file.FullName, Resources.Load<TextAsset>("config").text);
		}
		if (!dirShops.Exists) {
			Directory.CreateDirectory(dirShops.FullName);
		}

		if (!dirStash.Exists) {
			Directory.CreateDirectory(dirStash.FullName);
		}
		shops = new Shops(dirShops.FullName);
	}

	private void OnDestroy() {
		if (script == this) {
			script = null;
		}
	}
}
