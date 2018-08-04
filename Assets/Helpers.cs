using System;
using System.Threading.Tasks;

public static class Helpers {
	public static async Task<bool> CheckForOnline() {
		return await Task.Run(delegate () {
			try {
				using (System.Net.WebClient client = new System.Net.WebClient()) {
					using (client.OpenRead("http://clients3.google.com/generate_204")) {
						return true;
					}
				}
			}
			catch {
				return false;
			}
		});
	}
}

