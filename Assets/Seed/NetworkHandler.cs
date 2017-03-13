﻿using UnityEngine;
using System.Collections;
using BestHTTP;
using BestHTTP.SocketIO;
using System;
using System.Text;


namespace SB.Seed
{

    public class NetworkHandler : MonoBehaviour
    {

        public string BASE_URI = "";
        public static string GRAPH = "/getPlanets";
        public static string CREATE = "/createPlanet";
        public static string ADDOBJECT = "/addObject";
        public static string GETOBJECTS = "/getObjects";


        SocketManager socketmanager;

        // Use this for initialization
        void Start()
        {

            SocketOptions options = new SocketOptions();
            options.AutoConnect = true;

            socketmanager = new SocketManager(new Uri(BASE_URI + "/socket.io/"));

            socketmanager.Socket.On("hello", onHello);
            socketmanager.Open();
        }

        public void SyncObject(ObjectData trans)
        {
            socketmanager.Socket.Emit("updateobject", JsonUtility.ToJson(trans)); 
        }

        public void onHello(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(args);
           // socketmanager.Socket.Emit ("dumb", "omg", "message");
        }

        public IEnumerator LoadStuff(HTTPMethods type, string endpoint, Action<string> callbacksuccess = null, Action<string> callbackerr = null, string data = null)
        {
            HTTPRequest request = new HTTPRequest(new Uri(BASE_URI + endpoint), type);
            Debug.Log(BASE_URI + endpoint);
            if(type == HTTPMethods.Post && data != null)
            {
				Debug.Log(data);
                request.SetHeader("Content-Type", "application/json; charset=UTF-8");
                request.RawData = Encoding.UTF8.GetBytes(data);

            }
            request.Send();
            yield return StartCoroutine(request);

            Debug.Log("Status code: " + request.Response.StatusCode);

            switch (request.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    Debug.Log("Request Finished!\n" + request.Response.StatusCode);

				if (request.Response.StatusCode == 200 && callbacksuccess != null)
                        callbacksuccess(request.Response.DataAsText);
				else if(request.Response.StatusCode == 500 & callbackerr != null)
					callbackerr(request.Response.DataAsText);
				 break;

                // The request finished with an unexpected error.
                // The request's Exception property may contain more information about the error.
                case HTTPRequestStates.Error:
                    Debug.LogError("Request Finished with Error! " +
                        (request.Exception != null ?
                            (request.Exception.Message + "\n" + request.Exception.StackTrace) :
                            "No Exception"));

                    if (callbackerr != null)
                        callbackerr(request.Response.DataAsText);
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
        void RequestBuilder(HTTPMethods type, string endpoint)
        {

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

        public HTTPMethods methodforCall(string method)
        {

            switch (method)
            {
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
}
