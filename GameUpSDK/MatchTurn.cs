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
  /// Represents a response containing GameUp global and/or server instance info.
  /// </summary>
  public class MatchTurn
  {
    /// <summary> Turn type </summary>
    public readonly String Type ;

    /// <summary> Current turn number </summary>
    public readonly long TurnNumber ;

    /// <summary> Name of gamer for this turn </summary>
    public readonly String Gamer ;

    /// <summary> Gamer ID for this turn </summary>
    public readonly String GamerId ;

    /// <summary> Data stored for this turn </summary>
    public readonly String Data ;

    /// <summary> When the match was created </summary>
    public readonly long CreatedAt ;

    internal MatchTurn (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();
        
        switch (key) {
        case "type":
          Type = valueString;
          break;
        case "turn_number":
          TurnNumber = (long)value;
          break;
        case "gamer":
          Gamer = valueString;
          break;
        case "gamer_id":
          GamerId = valueString;
          break;
        case "data":
          Data = valueString;
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        }
      }
    }
  }
}
