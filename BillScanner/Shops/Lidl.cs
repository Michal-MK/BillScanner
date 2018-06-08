using OfficeOpenXml;

namespace BillScanner.Shops {
	public class Lidl : ShopsBase {

		public Lidl() : base("Lidl") {

		}

		public override void Parse(string[] bill) {

		}

		public override void WriteOut(ExcelPackage file, Meta info) {
			base.WriteOut(file, info);
		}
	}
}
