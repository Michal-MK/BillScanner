using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BillScanner.Shops {
	public class Albert : ShopsBase {

		private Regex itemRegex = new Regex(@"\w+\ ? (\w+)?\ ?\d+\ ?G(R)?");

		public Albert() : base("Albert") {
			dateRegEx = new Regex(@"\d+\.\d+\.\d+\ ?\d+:\d+");
		}

		public override void Parse(string[] bill) {
			bool foundStart = false;

			//Meta m;
			//List<Item> items = new List<Item>();
			//for (int i = 0; i < bill.Length; i++) {
			//	if(bill[i] == "Kč" || bill[i] == "Kc" || bill[i] == "KČ" && !foundStart) {
			//		foundStart = true;
			//		i++;
			//		List<string> _items = new List<string>();
			//		List<float> _values = new List<float>();
			//		while(WordSimilarity.Compute(bill[i], "Kód Sazba") >= 2) {
			//			if(float.TryParse(bill[i], out float f)){
			//				_values.Add(f);
			//			}
			//			if (itemRegex.IsMatch(bill[i])){
			//				_items.Add(bill[i]);
			//			}

			//			items.Add(new Item())
			//		}
			//	}
			//}
		}

		public override void WriteOut(ExcelPackage file, Meta info) {
			base.WriteOut(file, info);
		}
	}
}
