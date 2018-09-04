using System;
using UnityEngine;
using UnityEngine.UI;
using Igor.TCP;
using System.IO;
using System.Threading;



public class TakeScreenshot : MonoBehaviour {
	private bool camAvailable;
	private WebCamTexture tex;

	private Texture defaultBG;

	public RawImage display;
	public AspectRatioFitter fitter;

	private TCPClient client;

	private void Start() {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		client = new TCPClient("192.168.0.105", 7890);
		client.DefineResponseEntry<byte[]>(77, GetScreen);
		defaultBG = display.texture;
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0) {
			camAvailable = false;
			return;
		}

		for (int i = 0; i < devices.Length; i++) {
			if (!devices[i].isFrontFacing) {
				tex = new WebCamTexture(devices[i].name);
			}
		}

		if (tex == null) {
			return;
		}

		tex.Play();
		display.texture = tex;
		camAvailable = true;
	}

	private bool image = false;
	ManualResetEventSlim evnt = new ManualResetEventSlim();

	private byte[] GetScreen() {
		image = true;
		evnt.Wait();
		evnt.Reset();
		while(!new FileInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + "Image.png").Exists) {
			Thread.Sleep(10);
		}
		byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + Path.DirectorySeparatorChar + "Image.png");
		Debug.Log("Got bytes " + bytes.Length);
		return bytes;
	}

	private void Update() {
		if (image) {
			//ScreenCapture.CaptureScreenshot(Path.DirectorySeparatorChar + "Image.png");
			
			Debug.Log(tex.width + "x" + tex.height);
			//Debug.Log(File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Image.png"));
			evnt.Set();
			image = false;
		}

		if (camAvailable) {
			float ratio = tex.width / (float)tex.height;
			fitter.aspectRatio = ratio;

			int orientation = -tex.videoRotationAngle;

			display.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
		}
	}
}

