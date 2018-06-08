using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

namespace BillScanner {
	static class BillParserMain {
		private static string folderPath { get; } = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
		private static string billPath { get; } = folderPath + "Bills" + Path.DirectorySeparatorChar;

		static void Main(string[] args) {

			if (!Directory.Exists(billPath)) {
				Directory.CreateDirectory(billPath);
			}

			UserCredential credential;

			using (FileStream stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new string[1] { DriveService.Scope.Drive },
					"user",
					CancellationToken.None,
					new FileDataStore(folderPath, true)).Result;
			}

			// Create Drive API service.
			DriveService service = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = credential, ApplicationName = "Bill Scanner" });

			FilesResource.ListRequest listRequest = service.Files.List();

			//TODO: if other ways are found, this has to be edited
			listRequest.Q = "mimeType != 'application/vnd.google-apps.folder' and name contains 'Dokument aplikace' and name contains 'Google Keep'";


			IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

			Console.WriteLine("Files to download ({0}):", files.Count);

			if (files.Count == 0) {
				Console.WriteLine("No Files found in the drive!");
				End();
				return;
			}

			ShopsDefinition def = new ShopsDefinition();

			foreach (Google.Apis.Drive.v3.Data.File file in files) {
				Console.WriteLine("{0} ({1})", file.Name, file.Id);
				if (file.Name.Contains("Google") || file.Name.Contains("Keep")) {
					Stream fs;
					string fileName = billPath + file.Name + "-" + file.Id.Replace("-", "") + ".txt";
					try {
						fs = new FileStream(fileName, FileMode.CreateNew);
					}
					catch (IOException e) {
						Console.WriteLine(e.Message);
						continue;
					}
					service.Files.Export(file.Id, "text/plain").DownloadWithStatus(fs);
					fs.Close();
					fs.Dispose();
					//TODO delete file from drive
					string content = File.ReadAllText(fileName);
					ShopsDefinition.Shop shop = def.GetShopType(content);
					switch (shop) {
						case ShopsDefinition.Shop.LIDL: {
							Shops.Lidl l = new Shops.Lidl();
							l.Parse(content);
							break;
						}
						case ShopsDefinition.Shop.MC_DONALDS: {
							Shops.McDonalds m = new Shops.McDonalds();
							m.Parse(content);
							break;
						}
						case ShopsDefinition.Shop.ŠMAK: {
							Shops.BilboSmak bs = new Shops.BilboSmak();
							bs.Parse(content);
							break;
						}
						case ShopsDefinition.Shop.ALBERT: {
							Shops.Albert a = new Shops.Albert();
							a.Parse(content);
							break;
						}
					}
				}
			}
			End();
		}

		private static void End() {
			Console.WriteLine("All actions finished\n" +
								  "Press 'Enter' to exit...");
			Console.ReadLine();
			Environment.Exit(0);
		}
	}
}
