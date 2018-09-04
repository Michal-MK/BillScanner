using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Igor.Enums;
using System.Linq;
using static ItemDefinition;

public class DatabaseParser {

	public KeyValuePair<string,int> shopType { get; }
	public event EventHandler<ShopEntry> OnShopEntryParsed;

	public void RemoveItem(int index) {
		JObject obj;
		using (JsonTextReader reader = new JsonTextReader((File.OpenText(_persistentDataPathValueMainThread + "/Shops/" + shopType.Key + ".json")))) {
			obj = JObject.Load(reader);
		}
		JArray array = (JArray)obj["items"];
		array.Remove(array[index]);
		using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(_persistentDataPathValueMainThread + "/Shops/" + shopType.Key + ".json"))) {
			obj["items"] = array;
			obj.WriteTo(writer);
		}
		Parse();
	}

	private string _persistentDataPathValueMainThread;

	public DatabaseParser(KeyValuePair<string,int> shopType) {
		this.shopType = shopType;
		_persistentDataPathValueMainThread = UnityEngine.Application.persistentDataPath;
	}

	public void Parse() {
		FileInfo file = new FileInfo(_persistentDataPathValueMainThread + "/Shops/" + shopType.Key + ".json");
		JsonSerializer ser = new JsonSerializer();
		using(JsonTextReader reader = new JsonTextReader(File.OpenText(file.FullName))){
			ShopEntry entry = ser.Deserialize<ShopEntry>(reader);
			OnShopEntryParsed?.Invoke(this, entry);
		}
	}


	public void AddItem(ItemDefinitionStruct item) {
		JObject obj;
		using (JsonTextReader reader = new JsonTextReader((File.OpenText(_persistentDataPathValueMainThread + "/Shops/" + shopType.Key + ".json")))) {
			obj = JObject.Load(reader);
		}
		string jsonItemString = GetJsonItemString(item);
		ShopItem ss = JsonConvert.DeserializeObject<ShopItem>(jsonItemString);
		((JArray)obj["items"]).Add(JToken.FromObject(ss));
		using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(_persistentDataPathValueMainThread + "/Shops/" + shopType.Key + ".json"))) {
			obj.WriteTo(writer);
		}
		Parse();
	}

	private string GetJsonItemString(ItemDefinitionStruct item) {
		JObject obj = JObject.Parse(Constants.itemEntry);
		((JArray)(obj["pricesInThePast"])).Add(item.price);
		obj["latestPrice"] = item.price;
		obj["fullName"] = item.name;
		obj["weight"] = item.weight;
		obj["metrics"] = "grams";
		return obj.ToString();
	}
}

public class ShopEntry {
	[JsonProperty("entries")]
	public int totalEntries { get; set; }
	[JsonProperty("shopFileVersion")]
	public string shopFileVersion { get; set; }
	[JsonProperty("shopName")]
	public string shopName { get; set; }
	[JsonProperty("items")]
	public List<ShopItem> items { get; set; } = new List<ShopItem>();
}

public class ShopItem {
	[JsonProperty("pricesInThePast")]
	public List<string> pricesInThePast { get; set; } = new List<string>();
	[JsonProperty("latestPrice")]
	public string latestPrice { get; set; }
	[JsonProperty("fullName")]
	public string fullName { get; set; }
	[JsonProperty("weight")]
	public int weight { get; set; }
	[JsonProperty("metrics")]
	private string _metrics { get; set; }
	public MeassurementUnit getMetrics { get {
			switch (_metrics) {
				case "grams": {
					return MeassurementUnit.GRAMS;
				}
				default: {
					return MeassurementUnit.UNKNOWN;
				}
			}
		}
	}
	[JsonProperty("mostCommonAmount")]
	public int mostCommonAmount { get; set; }
}


public class Constants {
	public const string jsonTemplate = "{\"entries\": 0,\"modified\": null,\"shopName\": \"My Shop\",\"items\": []}";
	public const string itemEntry = "{\"pricesInThePast\": [],\"latestPrice\": \"\",\"fullName\": \"\",\"weight\": \"\",\"metrics\": \"\"}";
}
