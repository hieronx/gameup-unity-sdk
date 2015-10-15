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
  /// Represents a game on the GameUp service.
  /// </summary>
  public class Game
  {
    /// <summary> Game's registered display name. </summary>
    public readonly String Name ;

    /// <summary> Game description text. </summary>
    public readonly String Description ;

    /// <summary> Timestamp when the game was created. </summary>
    public readonly long CreatedAt ;

    /// <summary> Timestamp when game details were last changed or updated. </summary>
    public readonly long UpdatedAt ;

    internal Game (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();
        
        switch (key) {
        case "name":
          Name = valueString;
          break;
        case "description":
          Description = valueString;
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        case "updated_at":
          UpdatedAt = (long)value;
          break;
        }
      }
    }
  }

}

