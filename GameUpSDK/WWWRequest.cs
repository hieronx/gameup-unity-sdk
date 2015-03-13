// Copyright 2015 GameUp.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameUp
{
  public class WWWRequest : SingletonMonoBehaviour<WWWRequest>
  {
    private static string UNITY_VERSION = Application.unityVersion;

    private static string BUILD_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();

    private static string OPERATING_SYSTEM = SystemInfo.operatingSystem;

    private static string USER_AGENT =
        String.Format ("gameup-unity-sdk/{0} (Unity {1}; {2})", BUILD_VERSION, UNITY_VERSION, OPERATING_SYSTEM);

    #if UNITY_4_4 || UNITY_4_3 || UNITY_4_2 || UNITY_4_2 || UNITY_4_0
    private Hashtable headers = new Hashtable();
    #else
    private Dictionary<string, string> headers = new Dictionary<string, string> ();
    #endif

    private MonoBehaviour monoBehaviour;

    private WWW www;

    private Uri uri;

    private string requestType;

    private string authString;

    private byte[] body;

    public SuccessCallback OnSuccess;

    public FailureCallback OnFailure;

    public WWWRequest (Uri uri, string requestType, string username, string password)
      : this (WWWRequest.Instance, uri, requestType, username, password)
    {
    }

    public WWWRequest (MonoBehaviour monoBehaviour, Uri uri, string requestType, string username, string password)
    {
      this.uri = uri;
      this.requestType = requestType.ToUpper ();
      this.authString = (username + ":" + password);
      this.monoBehaviour = monoBehaviour;
    }

    public delegate void SuccessCallback (String jsonResponse);

    public delegate void FailureCallback (int statusCode, string reason);

    public void SetBody (string body)
    {
      this.body = System.Text.Encoding.UTF8.GetBytes(body);
    }

    public void AddHeader (string name, string val)
    {
      headers.Add (name, val);
    }

    public void Execute ()
    {
      monoBehaviour.StartCoroutine (ExecuteRequest ());
    }

    private IEnumerator ExecuteRequest ()
    {
      // Server hack for Unity's broken WWW module
      string query = "_status=200";
      if (requestType.Equals ("PUT") || requestType.Equals ("POST") || requestType.Equals ("DELETE"))
      {
        query = query + "&_method=" + requestType;
      }

      UriBuilder b = new UriBuilder (uri);
      b.Query = query;

      // Add necessary request headers
      headers.Add ("User-Agent", USER_AGENT);
      headers.Add ("Accept", "application/json");
      headers.Add ("Content-Type", "application/json");
      
      byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes (authString);
      string authHeader = System.Convert.ToBase64String(buffer);
      headers.Add ("Authorization", authHeader);

      www = new WWW (b.Uri.ToString(), body, headers);

      yield return www;

      if (!String.IsNullOrEmpty (www.error))
      {
        Debug.LogError (www.error);
        if (OnFailure != null)
        {
          OnFailure (500, www.error);
        }
      }
      else
      {
    	if (www.text == null || www.text.Length == 0)
    	{
          Debug.Log (System.Convert.ToString (www));
          if (OnSuccess != null)
          {
    	    OnSuccess ("");
          }
    	}
        else
        {
          JsonObject json = SimpleJson.DeserializeObject<JsonObject> (www.text);
          
          // HACK make sure that the error is checking for GameUp error message combinations
          if (json.ContainsKey ("status") && json.ContainsKey("message") && json.ContainsKey("request"))
          {
            int statusCode = int.Parse (System.Convert.ToString (json["status"]));
            Debug.LogError (System.Convert.ToString (json["message"]));
            if (OnFailure != null)
            {
              OnFailure (statusCode, System.Convert.ToString (json["message"]));
            }
          }
          else
          {
            if (OnSuccess != null)
            {
              OnSuccess (www.text);
            }
          }
        }
      }
    }
  }
}
