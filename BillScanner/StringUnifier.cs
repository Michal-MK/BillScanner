namespace BillScanner {
	public static class StringUnifier {

		private static (char confusing, char unified)[] confusingLetteres = new(char, char)[6] {
			('0', 'O'),
			('1', 'I'),
			('5', 'S'),
			('l', 'I'),
			('U', 'O'),
			('u', 'o'),
		};

		public static string Unify(string s) {
			foreach ((char confusing, char unified) in confusingLetteres) {
				s = s.Replace(confusing, unified);
			}
			return s;
		}
	}
}
