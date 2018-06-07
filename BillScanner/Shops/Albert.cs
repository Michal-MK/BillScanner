using OfficeOpenXml;

namespace BillScanner.Shops {
	public class Albert : ShopsBase {

		public Albert() : base("Albert") {

		}

		public override void Parse(string bill) {

		}

		public override void WriteOut(ExcelPackage file, Meta info) {
			base.WriteOut(file, info);
		}
	}
}
