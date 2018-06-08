using OfficeOpenXml;

namespace BillScanner.Shops {
	public class McDonalds : ShopsBase {

		public McDonalds() : base("Mc Donald's") {

		}

		public override void Parse(string[] bill) {

		}

		public override void WriteOut(ExcelPackage file, Meta info) {
			base.WriteOut(file, info);
		}
	}
}
