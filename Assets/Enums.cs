using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Igor.Enums {

	public class Shops {
		public Dictionary<string, int> shopEntries { get; } = new Dictionary<string, int>();

		private DirectoryInfo shopsDir;

		public Shops(string shopsPath) {
			shopsDir = new DirectoryInfo(shopsPath);
			FileInfo[] files = shopsDir.GetFiles("*.json");
			for (int i = 0; i < files.Length; i++) {
				
				Add(files[i].Name.Replace(files[i].Extension, ""));
			}
		}

		public FileInfo SelectShop(string name) {
			try {
				return shopsDir.GetFiles(name + ".json")[0];
			}
			catch {
				return null;
			}
		}

		public void Add(string name) {
			shopEntries.Add(name, shopEntries.Count);
			//JObject obj = JObject.Parse(Constants.jsonTemplate);
			//obj["shopName"] = name;
			//File.WriteAllText(shopsDir + "/" + name + ".json", obj.ToString());
		}

		public void Remove(string name) {
			shopEntries.Remove(name);
		}
	}

	public enum MeassurementUnit {
		UNKNOWN,
		PIECES,
		GRAMS,
		LITRES
	}
}