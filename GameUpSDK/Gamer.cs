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

namespace GameUp
{
  /// <summary>
  /// Represents a gamer in the GameUp service.
  /// </summary>
  public class Gamer
  {
    /// <summary> Nickname, intended for easy public display. </summary>
    public readonly String Nickname;

    /// <summary> A real name, if one was provided when the gamer signed up. </summary>
    public readonly String Name;

    /// <summary> Time zone of the gamer. </summary>
    public readonly long Timezone;

    /// <summary> Location of the gamer. </summary>
    public readonly String Location;

    /// <summary> Locale string for this gamer. </summary>
    public readonly String Locale;

    /// <summary> When the gamer first registered with GameUp. </summary>
    public readonly long CreatedAt;

    internal Gamer (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();

        switch (key) {
        case "nickname":
          Nickname = valueString;
          break;
        case "name":
          Name = valueString;
          break;
        case "timezone":
          Timezone = (long) value;
          break;
        case "location":
          Location = valueString;
          break;
        case "locale":
          Locale = valueString;
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        }
      }
    }
  }
}

