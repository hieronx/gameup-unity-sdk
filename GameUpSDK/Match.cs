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
  /// Represents a response containing GameUp Multiplayer Match with its metadata.
  /// </summary>
  public class Match
  {
    /// <summary> Nickname set for the current gamer in this given match
    /// If the current gamer's nickname is changed after the match is setup,
    /// the old nickname is still used hence the use of this 'whoami' field.
    /// </summary>
    public readonly String Whoami ;

    /// <summary> Match ID </summary>
    public readonly String MatchId ;

    /// <summary> Current turn number </summary>
    public readonly long TurnCount ;

    /// <summary> Name of gamer for the given turn  </summary>
    public readonly String Turn ;

    /// <summary> Nickname of all the gamers in the current match </summary>
    public readonly String[] Gamers ;

    /// <summary> When the match was created </summary>
    public readonly long CreatedAt ;

    /// <summary> Checks to see if the match is still ongoing or has ended. </summary>
    public readonly bool Active ;

    internal Match (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();
        
        switch (key) {
        case "whoami":
          Whoami = valueString;
          break;
        case "match_id":
          MatchId = valueString;
          break;
        case "turn_count":
          TurnCount = (long)value;
          break;
        case "turn":
          Turn = valueString;
          break;
        case "gamers":
          List<string> gamerList = new List<string> ();
          JsonArray gamerArray = (JsonArray)value;
          foreach (string entryObject in gamerArray) {
            gamerList.Add (entryObject);
          }
          Gamers = gamerList.ToArray ();
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        case "active":
          Active = (Boolean)value;
          break;
        }
      }
    }
  }
}