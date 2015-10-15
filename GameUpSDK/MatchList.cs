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
  /// Represents the whole response from the matches endpoint, including meta
  /// information about the response.
  /// </summary>
  public class MatchList : IEnumerable<Match>
  {
    /// <summary> The number of matches returned as part of this response. </summary>
    public readonly long Count ;

    /// <summary> The matches themselves. </summary>
    public readonly Match[] Matches ;

    internal MatchList (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        
        switch (key) {
        case "count":
          Count = (long)value;
          break;
        case "matches":
          List<Match> mList = new List<Match> ();
          JsonArray matchArray = (JsonArray)value;
          foreach (object matchObject in matchArray) {
            Match match = new Match ((IDictionary<string, object>)matchObject);
            mList.Add (match);
          }
          Matches = mList.ToArray ();
          break;
        }
      }
    }

    public IEnumerator<Match> GetEnumerator ()
    {
      return (new List<Match> (Matches)).GetEnumerator ();
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
