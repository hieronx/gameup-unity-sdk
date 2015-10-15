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
using System.Collections.Generic;
using System.Collections;

namespace GameUp
{
  /// <summary>
  /// Represents a response from a purchase verification.
  /// </summary>
  public class PurchaseVerification
  {
    /// <summary>
    /// High level flag indicating verification success or failure.
    /// </summary>
    public readonly bool Success ;

    /// <summary>
    /// Whether this is a new transaction, or GameUp had an existing record of it.
    /// </summary>
    public readonly bool SeenBefore ;

    /// <summary>
    /// Whether or not the remote (Apple/Google) purchase provider responded to the
    /// verification request. If this is false the client should retry the request
    /// later.
    /// </summary>
    public readonly bool PurchaseProviderReachable ;

    /// <summary>
    /// A message indicating the verification error reason, if applicable.
    /// </summary>
    public readonly String Message ;

    /// <summary>
    /// The complete response from the remote (Apple/Google) purchase provider.
    /// </summary>
    public readonly IDictionary<string, object> Data ;

    internal PurchaseVerification (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();
        
        switch (key) {
        case "success":
          Success = (Boolean)value;
          break;
        case "seen_before":
          SeenBefore = (Boolean)value;
          break;
        case "purchase_provider_reachable":
          PurchaseProviderReachable = (Boolean)value;
          break;
        case "message":
          Message = valueString;
          break;
        case "data":
          Data = (IDictionary<string,object>)value;
          break;
        }
      }
    }
  }
}
