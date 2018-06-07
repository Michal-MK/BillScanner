using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillScanner {
	public class ShopsDefinition {
		public enum Shop {
			LIDL = 2,
			MC_DONALDS = 100,
			ŠMAK = 101,
			ALBERT = 1
		}

		public Shop GetShopType(string bill) {
			if(bill.Contains("Lidl") || bill.Contains("1359/11, 158 00, Praha 5") || bill.Contains("CZ26178541")){
				return Shop.LIDL;
			}

			if(bill.Contains("albert") || bill.Contains("AHOLD") || bill.Contains("520/117, 158 00") || bill.Contains("CZ44012373")) {
				return Shop.ALBERT;
			}

			if(bill.Contains("McDonald's") || bill.Contains("QTY ITEM") || bill.Contains("CZ16191129")) {
				return Shop.MC_DONALDS;
			}

			if(bill.Contains("BILBO") || bill.Contains("CZ25025228") || bill.Contains("EET rekapitulace")) {
				return Shop.ŠMAK;
			}

			throw new NotSupportedException(string.Format("We could not determine the shop this bill belongs to...\n" +
				"----------------------------------------------------------------------------------\n" +
				"{0}\n" +
				"----------------------------------------------------------------------------------", bill));
		}
	}
}
