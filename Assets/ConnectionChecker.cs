using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionChecker : MonoBehaviour {

	public static ConnectionChecker instance;
	public ConnectionCheckerThread thread;

	private bool onlineStatus;

	public bool simulateMode;
	public bool simulatedState;

	public Image onlineImage;
	public Text onlineText;

	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
		thread = new ConnectionCheckerThread();
		thread.OnConnectedStatusUpdate += SetStatus;
		new Thread(new ThreadStart(thread.CheckOnline)) { Name = "OnlineCheck" }.Start();
	}

	public void Recheck() {

		new Thread(new ThreadStart(thread.Recheck)) { Name = "OnlineCheck" }.Start();
	}

	private void SetStatus(object obj, bool online) {
		onlineStatus = online;
	}

	private void Update() {
		if (onlineStatus) {
			onlineImage.color = Color.green;
			onlineText.text = "Online";
		}
		else {
			onlineImage.color = Color.red;
			onlineText.text = "Offline";
		}
	}
}

public class ConnectionCheckerThread {
	public event System.EventHandler<bool> OnConnectedStatusUpdate;

	private bool _isOnline = false;

	public void CheckOnline() {
		while (!_isOnline) {
			Thread.Sleep(2000);
			if (ConnectionChecker.instance.simulateMode) {
				_isOnline = ConnectionChecker.instance.simulatedState;
				OnConnectedStatusUpdate(this, _isOnline);
				return;
			}
			bool result = Helpers.CheckForOnline().Result;
			_isOnline = result;
			OnConnectedStatusUpdate(this, result);
		}
	}

	public void Recheck() {
		if (ConnectionChecker.instance.simulateMode) {
			_isOnline = ConnectionChecker.instance.simulatedState;
			OnConnectedStatusUpdate(this, _isOnline);
			return;
		}
		bool status = Helpers.CheckForOnline().Result;
		OnConnectedStatusUpdate(this, Helpers.CheckForOnline().Result);
	}
}
