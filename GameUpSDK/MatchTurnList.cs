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
  /// Represents the whole response from the match turn endpoint, including meta
  /// information about the response.
  /// </summary>
  public class MatchTurnList : IEnumerable<MatchTurn>
  {
    /// <summary> The number of match turns returned as part of this response. </summary>
    public readonly long Count ;

    /// <summary> The match turns themselves. </summary>
    public readonly MatchTurn[] Turns ;

    internal MatchTurnList (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        
        switch (key) {
        case "count":
          Count = (long) value;
          break;
        case "turns":
          List<MatchTurn> tList = new List<MatchTurn> ();
          JsonArray turnsArray = (JsonArray)value;
          foreach (object turnObject in turnsArray) {
            MatchTurn turn = new MatchTurn ((IDictionary<string, object>)turnObject);
            tList.Add (turn);
          }
          Turns = tList.ToArray ();
          break;
        }
      }
    }

    public IEnumerator<MatchTurn> GetEnumerator ()
    {
      return (new List<MatchTurn> (Turns)).GetEnumerator ();
    }

    private IEnumerator GetEnumerator1 ()
    {
      return this.GetEnumerator ();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator1 ();
    }

  }
}