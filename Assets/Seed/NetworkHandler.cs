using UnityEngine;
using System.Collections;


using BestHTTP;
using BestHTTP.SocketIO;

using System;



public class NetworkHandler : MonoBehaviour {

	string BASE_URI = "http://192.168.1.227:4000";
	public static string GRAPH = "/getPlanets";
	public static string CREATE = "/createPlanet";

	SocketManager socketmanager;

	// Use this for initialization
	void Start () {

		SocketOptions options = new SocketOptions();
		options.AutoConnect = false;

		socketmanager = new SocketManager(new Uri(BASE_URI + "/socket.io/"));

		socketmanager.Socket.On("boop", onHello);
		socketmanager.Open();

	
	}

	public void onHello(Socket socket, Packet packet, params object[] args) {
		Debug.Log (args);
		//socketmanager.Socket.Emit ("message", "omg", "message");
	}

	public IEnumerator LoadStuff(HTTPMethods type, string endpoint, Action<string> callbacksuccess = null,Action<string> callbackerr = null)
	{
		HTTPRequest request = new HTTPRequest(new Uri(BASE_URI + endpoint),type);
		request.Send();
		yield return StartCoroutine(request);

		Debug.Log("Status code: " + request.Response.StatusCode);


		switch (request.State)
		{
		// The request finished without any problem.
		case HTTPRequestStates.Finished:
			Debug.Log("Request Finished Successfully!\n" + request.Response.StatusCode);
			if (callbacksuccess != null)
				callbacksuccess (request.Response.DataAsText);
			
			break;

			// The request finished with an unexpected error.
			// The request's Exception property may contain more information about the error.
		case HTTPRequestStates.Error:
			Debug.LogError("Request Finished with Error! " +
				(request.Exception != null ?
					(request.Exception.Message + "\n" + request.Exception.StackTrace) :
					"No Exception"));

			if (callbackerr != null)
				callbackerr (request.Response.DataAsText);
			break;

			// The request aborted, initiated by the user.
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("Request Aborted!");
			break;

			// Ceonnecting to the server timed out.
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogError("Connection Timed Out!");
			break;

			// The request didn't finished in the given time.
		case HTTPRequestStates.TimedOut:
			Debug.LogError("Processing the request Timed Out!");
			break;
		}

	}

	/// <summary>
	/// Build a request for backend
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="endpoint">Endpoint.</param>
	void RequestBuilder(HTTPMethods type, string endpoint) {

		HTTPRequest request = new HTTPRequest(new Uri(BASE_URI + endpoint), type,
			OnRequestFinished);
		
		request.Send();


	}

	/// <summary>
	/// Raises the request finished event.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="response">Response.</param>
	void OnRequestFinished(HTTPRequest request, HTTPResponse response)
	{




	}



	public HTTPMethods methodforCall(string method) {

		switch (method) {
		case "GET":
			return HTTPMethods.Get;
			break;
		case "POST":
			return HTTPMethods.Post;
			break;
		default:
			return HTTPMethods.Get;
			break;
		}

	}

}
