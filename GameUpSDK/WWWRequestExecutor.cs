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
using System.IO;
using Unity.IO.Compression;

namespace GameUp
{
  public class WWWRequestExecutor : SingletonMonoBehaviour<WWWRequestExecutor>
  {
    private static string UNITY_VERSION = Application.unityVersion;
    private static string BUILD_VERSION = Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
    private static string OPERATING_SYSTEM = SystemInfo.operatingSystem;
    private static string USER_AGENT =
      String.Format ("gameup-unity-sdk/{0} (Unity {1}; {2})", BUILD_VERSION, UNITY_VERSION, OPERATING_SYSTEM);

    internal static void Execute (WWWRequest req) {
      WWWRequestExecutor.Instance.InitExecute (req);
    }

    private void InitExecute (WWWRequest req) {
      StartCoroutine (ExecuteRequest(req));
    }

    private IEnumerator ExecuteRequest (WWWRequest req)
    {
      // Server hack for Unity's broken WWW module
      string query = "_status=200";
      if (req.Method.Equals ("PUT") || req.Method.Equals ("POST") || req.Method.Equals ("PATCH") || req.Method.Equals ("DELETE")) {
        query = query + "&_method=" + req.Method;
      }

      UriBuilder b = new UriBuilder (req.Uri);
      if (b.Query.Length > 0) {
        // because Uri.query will have two "?" characters...
        b.Query = b.Query.Remove(0, 1) + "&" + query;
      } else {
        b.Query = query;
      }

      // Add necessary request headers
#if (!UNITY_WEBPLAYER) 
      // One of the forbidden keys to override for WebPlayer
      req.AddHeader ("User-Agent", USER_AGENT);
#endif
      req.AddHeader ("Accept", "application/json");
      req.AddHeader ("Content-Type", "application/json");
      req.AddHeader ("Authorization", req.AuthHeader);

      byte[] reqBody = req.Body;
      if (Client.EnableGZip) {
        req.AddHeader ("Accept-Encoding", "gzip");

        if (reqBody != null && reqBody.Length > 300) {
          req.AddHeader ("Content-Encoding", "gzip");

          using (var msi = new MemoryStream (reqBody))
          using (var mso = new MemoryStream ()) {
            using (var gs = new GZipStream (mso, CompressionMode.Compress)) {
              CopyTo (msi, gs);
            }

            reqBody = mso.ToArray ();
          }
        }
      }

      WWW www = new WWW (b.Uri.AbsoluteUri, reqBody, req.GetHeaders());

      yield return www;

      if (!String.IsNullOrEmpty (www.error)) {
        if (www.error.Contains("504 GATEWAY_TIMEOUT") && req.shouldRetry ()) {
          float delay = UnityEngine.Random.Range (10F, 50F) / 100F;
          yield return new WaitForSeconds(delay);
          req.Execute ();
        } else {
          if (req.OnFailure != null) {
            req.OnFailure (500, www.error); 
          }
        }
      } else {
        if (www.text == null || www.text.Length == 0) {
          if (req.OnSuccess != null) {
            req.OnSuccess ("");
          }
        } else {
          var body = www.text;
          var headerValue = "";
          if (www.responseHeaders.TryGetValue("Content-Encoding", out headerValue)
              && headerValue.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
          {
            var bodyBytes = System.Text.Encoding.UTF8.GetBytes(www.text);
            using (var msi = new MemoryStream(bodyBytes))
            using (var mso = new MemoryStream()) {
              using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                CopyTo(gs, mso);
              }

              body = System.Text.Encoding.UTF8.GetString(mso.ToArray());
            }
          }

          Dictionary<string, object> json = SimpleJson.DeserializeObject<Dictionary<string, object>> (body);
          // HACK: make sure that the error is checking for GameUp error message combinations
          if (json.ContainsKey ("status") && json.ContainsKey ("message") && json.ContainsKey ("request")) {
            int statusCode = int.Parse (System.Convert.ToString (json ["status"]));
            if (req.OnFailure != null) {
              req.OnFailure (statusCode, System.Convert.ToString (json ["message"]));
            }
          } else {
            if (req.OnSuccess != null) {
              req.OnSuccess (www.text);
            }
          }
        }
      }
    }

    private static void CopyTo(Stream src, Stream dest)
    {
      byte[] bytes = new byte[4096];
      int cnt;
      while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
      {
        dest.Write(bytes, 0, cnt);
      }
    }
  }
}

