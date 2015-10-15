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
  /// Represents the whole response from an achievement endpoint, including meta
  /// information about the response.
  /// </summary>
  public class AchievementList : IEnumerable<Achievement>
  {
    /// <summary> The number of achievements returned as part of this response. </summary>
    public readonly long Count ;

    /// <summary> The achievements themselves. </summary>
    public readonly Achievement[] Achievements ;

    internal AchievementList (IDictionary<string, object> dict)
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
        case "achievements":
          List<Achievement> aList = new List<Achievement> ();
          JsonArray achievementArray = (JsonArray)value;
          foreach (object achievementObject in achievementArray) {
            Achievement achievement = new Achievement ((IDictionary<string, object>)achievementObject);
            aList.Add (achievement);
          }
          Achievements = aList.ToArray ();
          break;
        }
      }
    }

    public IEnumerator<Achievement> GetEnumerator ()
    {
      return (new List<Achievement> (Achievements)).GetEnumerator ();
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
