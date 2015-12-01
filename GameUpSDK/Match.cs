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

    /// <summary> Match Filters </summary>
    public readonly String[] Filters ;

    /// <summary> Current turn number </summary>
    public readonly long TurnCount ;

    /// <summary> Name of gamer for the given turn  </summary>
    [Obsolete("Turn is deprecated, use TurnGamerId instead.")]
    public readonly String Turn ;

    /// <summary> ID of gamer for the given turn  </summary>
    public readonly String TurnGamerId ;

    /// <summary> Nickname of all the gamers in the current match </summary>
    [Obsolete("Gamers is deprecated, use ActiveGamers instead.")]
    public readonly String[] Gamers ;

    /// <summary> List of Map of Gamer Nickname to Gamer IDs </summary>
    public readonly List<IDictionary<string, string>> ActiveGamers ;

    /// <summary> When the match was created </summary>
    public readonly long CreatedAt ;

    /// <summary> When the match was last updated </summary>
    public readonly long UpdatedAt ;

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
        case "match_id":
          MatchId = valueString;
          break;
        case "filters":
          List<string> filterList = new List<string> ();
          JsonArray filterArray = (JsonArray)value;
          foreach (string entryObject in filterArray) {
            filterList.Add (entryObject);
          }
          Filters = filterList.ToArray ();
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        case "updated_at":
          UpdatedAt = (long)value;
          break;
        case "gamers":
          List<string> gamerList = new List<string> ();
          JsonArray gamerArray = (JsonArray)value;
          foreach (string entryObject in gamerArray) {
            gamerList.Add (entryObject);
          }
          Gamers = gamerList.ToArray ();
          break;
        case "active_gamers": 
          JsonArray activeGamersArray = (JsonArray) value;
          ActiveGamers = new List<IDictionary<string, string>>();
          foreach (JsonObject gamerPair in activeGamersArray) {
            IDictionary<string, string> nicknameIdPair = new Dictionary<string, string>();
            object nickname;
            gamerPair.TryGetValue("nickname", out nickname);
            object gamerId;
            gamerPair.TryGetValue("gamer_id", out gamerId);
            nicknameIdPair.Add("nickname", nickname.ToString());
            nicknameIdPair.Add("id", gamerId.ToString());

            ActiveGamers.Add(nicknameIdPair);
          }
          break;
        case "active":
          Active = (Boolean)value;
          break;
        case "turn":
          Turn = valueString;
          break;
        case "turn_gamer_id":
          TurnGamerId = valueString;
          break;
        case "turn_count":
          TurnCount = (long)value;
          break;
        case "whoami":
          Whoami = valueString;
          break;
        }
      }
    }
  }
}