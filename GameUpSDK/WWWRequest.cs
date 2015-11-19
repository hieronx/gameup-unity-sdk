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
  public class WWWRequest
  {

    #if UNITY_4_4 || UNITY_4_3 || UNITY_4_2 || UNITY_4_2 || UNITY_4_0
    private Hashtable headers = new Hashtable();
    #else
    private Dictionary<string, string> headers = new Dictionary<string, string> ();
    #endif

    public Uri Uri { get; private set; }
    public string Method { get; private set; }
    public string AuthHeader { get; private set; }
    private byte[] _body;
    public byte[] Body { get { return _body; } }
    public SuccessCallback OnSuccess;
    public FailureCallback OnFailure;

    public static Boolean RETRY_REQUESTS = false;
    private int retryCount = 0;
    
    public WWWRequest (Uri uri, string method, string username, string password)
    {
      this.Uri = uri;
      this.Method = method.ToUpper ();
      byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes (username + ":" + password);
      this.AuthHeader = "Basic " + System.Convert.ToBase64String (buffer);
    }

    public delegate void SuccessCallback (String jsonResponse);
    
    public delegate void FailureCallback (int statusCode, string reason);
    
    public void SetBody (string body)
    {
      this._body = System.Text.Encoding.UTF8.GetBytes (body);
    }
    
    public void AddHeader (string name, string val)
    {
      if (headers.ContainsKey (name)) {
        headers.Remove (name);
      }
      headers.Add (name, val);
    }

    #if UNITY_4_4 || UNITY_4_3 || UNITY_4_2 || UNITY_4_2 || UNITY_4_0
    public Hashtable GetHeaders ()
    #else
    public Dictionary<string, string> GetHeaders ()
    #endif
    {
      return headers;
    }

    public void Execute ()
    {
      if (retryCount == 0 || shouldRetry ()) {
        retryCount++;
        WWWRequestExecutor.Execute (this);
      }
    }

    public bool shouldRetry () {
      return (RETRY_REQUESTS && (retryCount <= 2));
    }

  }
}
