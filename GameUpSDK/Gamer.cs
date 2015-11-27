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
  /// Gamer Profile Types
  /// </summary>
  public enum GamerProfileType
  {
    _UNKNOWN,
    ANONYMOUS,
    EMAIL,
    FACEBOOK,
    GOOGLE,
    TANGO
  }

  /// <summary>
  /// Represents a gamer profile in the GameUp service.
  /// </summary>
  public class GamerProfile
  {
    /// <summary> Unique Gamer Profile ID. </summary>
    public readonly String ProfileId;

    /// <summary> Type of Gamer Profile. </summary>
    public readonly GamerProfileType Type;

    /// <summary> When the gamer profile was created. </summary>
    public readonly long CreatedAt;

    internal GamerProfile (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();
        
        switch (key) {
        case "id":
          ProfileId = valueString;
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        case "type":
          switch (valueString) {
          case "anonymous":
            Type = GamerProfileType.ANONYMOUS;
            break;
          case "email":
            Type = GamerProfileType.EMAIL;
            break;
          case "facebook":
            Type = GamerProfileType.FACEBOOK;
            break;
          case "google":
            Type = GamerProfileType.GOOGLE;
            break;
          case "tango":
            Type = GamerProfileType.TANGO;
            break;
          default:
            Type = GamerProfileType._UNKNOWN;
            break;
          }
          break;
        }
      }
    }
  }

  /// <summary>
  /// Represents a gamer in the GameUp service.
  /// </summary>
  public class Gamer
  {
    /// <summary> Unique Gamer ID. </summary>
    public readonly String GamerId;

    /// <summary> Nickname, intended for easy public display. </summary>
    public readonly String Nickname;

    /// <summary> A real name, if one was provided when the gamer signed up. </summary>
    public readonly String Name;

    /// <summary> Email address of the gamer. </summary>
    public readonly String Email;

    /// <summary> Time zone of the gamer. </summary>
    public readonly long Timezone;

    /// <summary> Location of the gamer. </summary>
    public readonly String Location;

    /// <summary> Locale string for this gamer. </summary>
    public readonly String Locale;

    /// <summary> Gender of this gamer. </summary>
    public readonly String Gender;

    /// <summary> When the gamer first registered with GameUp. </summary>
    public readonly long CreatedAt;

    /// <summary> When the gamer's profile was last updated. </summary>
    public readonly long UpdatedAt;

    /// <summary> Linked profiles of this gamer. </summary>
    public readonly GamerProfile[] Profiles;

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
        case "gamer_id":
          GamerId = valueString;
          break;
        case "nickname":
          Nickname = valueString;
          break;
        case "name":
          Name = valueString;
          break;
        case "email":
          Email = valueString;
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
        case "gender":
          Gender = valueString;
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        case "updated_at":
          UpdatedAt = (long)value;
          break;
        case "profiles":
          List<GamerProfile> pList = new List<GamerProfile> ();
          JsonArray profileArray = (JsonArray)value;
          foreach (object profileObject in profileArray) {
            GamerProfile profile = new GamerProfile ((IDictionary<string, object>)profileObject);
            pList.Add (profile);
          }
          Profiles = pList.ToArray ();
          break;
        }
      }
    }
  }
}

