using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BillScanner.Shops {
	public class ShopsBase {

		protected string shop_name;

		public Regex dateRegEx { get; set; } = new Regex(@"\d+\.\d+\.\ ?\d+\ ?\,?\ ?\,?\d+:\d+:?(\d+)?");

		protected ShopsBase(string shop_name) {
			this.shop_name = shop_name;
		}

		public virtual void WriteOut(ExcelPackage file, Meta info) {
			Console.WriteLine("Writing out to " + shop_name);
		}

		public virtual void Parse(string[] bill) {
			Console.WriteLine("Parsing as " + shop_name);
		}
	}

	public struct Item {
		public Item(string name, float cost, int quantity) {
			this.name = name;
			this.cost = cost;
			this.quantity = quantity;
			shop = 0;
		}

		public string name { get; }
		public float cost { get; }
		public int quantity { get; }

		public ShopsDefinition.Shop shop { get; set; }
	}

	public struct Meta {
		public Meta(DateTime time) {
			items = new List<Item>();
			this.time = time;
		}

		public List<Item> items { get; }
		public DateTime time { get; }
		
	}
}
