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
	class Program {
		private static string folderPath { get; } = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
		private static string billPath { get; } = folderPath + "Bills" + Path.DirectorySeparatorChar;

		static void Main(string[] args) {

			if (!Directory.Exists(billPath)) {
				Directory.CreateDirectory(billPath);
			}

			UserCredential credential;

			using (FileStream stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
				string credPath = Path.Combine(folderPath, "drive-dotnet-quickstart.json");

				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new string[1] { DriveService.Scope.Drive },
					"user",
					CancellationToken.None,
					new FileDataStore(folderPath, true)).Result;
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Drive API service.
			DriveService service = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = credential, ApplicationName = "Bill Scanner" });

			FilesResource.ListRequest listRequest = service.Files.List();
			listRequest.Q = "mimeType != 'application/vnd.google-apps.folder' and name contains 'Dokument aplikace' and name contains 'Google Keep'";


			IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

			Console.WriteLine("Files:");
			if (files != null && files.Count > 0) {
				foreach (Google.Apis.Drive.v3.Data.File file in files) {
					Console.WriteLine("{0} ({1})", file.Name, file.Id);
					if (file.Name.Contains("Google") || file.Name.Contains("Keep")) {
						Stream fs;
						try {
							fs = new FileStream(billPath + file.Name + "-" + file.Id.Replace("-", "") + ".txt", FileMode.CreateNew);
						}
						catch (IOException e) {
							Console.WriteLine(e.Message);
							continue;
						}
						service.Files.Export(file.Id, "text/plain").DownloadWithStatus(fs);
						fs.Close();
						fs.Dispose();
						//TODO delete file from drive
					}
				}
			}
			else {
				Console.WriteLine("No files found.");
			}
			Console.WriteLine("All actions finished\n" +
							  "Press 'Enter' to exit...");
			Console.ReadLine();
		}
	}

}
