using OfficeOpenXml;

namespace BillScanner.Shops {
	public class BilboSmak : ShopsBase {

		public BilboSmak() : base("Šmak České kuchyně") {

		}

		public override void Parse(string[] bill) {

		}

		public override void WriteOut(ExcelPackage file, Meta info) {
			base.WriteOut(file, info);
		}
	}
}
