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
  /// Represents an achievement's data, also contains gamer progress if applicable.
  /// </summary>
  public class Achievement
  {
    /// <summary>
    /// Achievement Type, referring to how gamer interaction must occur.
    /// </summary>
    public enum AchievementType
    {
      /// <summary> Standard earned/unearned achievement. </summary>
      NORMAL,

      /// <summary>
      /// Incremental achievement, requiring some number of actions before it
      /// is awarded, subject to game logic.
      /// </summary>
      INCREMENTAL
    }

    /// <summary>
    /// Achievement State, referring to how display should be handled.
    /// </summary>
    public enum AchievementState
    {
      /// <summary> The achievement and all its details are available to the gamer. </summary>
      VISIBLE,

      /// <summary>
      /// The name and description are replaced with "???", unless the gamer
      /// has completed the achievement.
      /// </summary>
      SECRET,

      /// <summary>
      /// The achievement does not appear at all, unless the gamer has
      /// completed the achievement.
      /// </summary>
      HIDDEN
    }

    /// <summary> Game-unique public readonly identifier for this achievement. </summary>
    public readonly String PublicId ;

    /// <summary> Achievement name. </summary>
    public readonly String Name ;

    /// <summary> Achievement description or instructions. </summary>
    public readonly String Description ;

    /// <summary> The type of the achievement, referring to gamer interaction model. </summary>
    public readonly AchievementType Type ;

    /// <summary>
    /// Number of points that will be awarded for completing this achievement,
    /// or have already been awarded if it is already complete.
    /// </summary>
    public readonly long Points ;

    /// <summary> The state of this achievement, referring to display logic. </summary>
    public readonly AchievementState State ;

    /// <summary>
    /// Required number of actions to complete this achievement, subject to game
    /// logic. For "normal"-type achievements this will always be 1.
    /// </summary>
    public readonly long RequiredCount ;

    /// <summary>
    /// Current gamer progress towards the required count of this achievement,
    /// subject to the same game logic as the requiredCount field.
    /// </summary>
    public readonly long Count ;

    /// <summary>
    /// UTC timestamp in milliseconds when the gamer last made any progress
    /// towards this achievement, or 0 if no progress ever.
    /// </summary>
    public readonly long ProgressAt ;

    /// <summary>
    /// UTC timestamp in milliseconds when the gamer completed this achievement,
    /// or 0 if it has not yet been completed.
    /// </summary>
    public readonly long CompletedAt ;

    /// <returns>
    /// true if the gamer has completed this achievement,
    /// false otherwise.
    /// </returns>
    public bool IsCompleted ()
    {
      return CompletedAt > 0L;
    }

    internal Achievement (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();

        switch (key) {
        case "public_id":
          PublicId = valueString;
          break;
        case "name":
          Name = valueString;
          break;
        case "description":
          Description = valueString;
          break;
        case "type":
          switch (valueString) {
          case "incremental":
            Type = AchievementType.INCREMENTAL;
            break;
          case "normal":
            Type = AchievementType.NORMAL;
            break;
          }
          break;
        case "points":
          Points = (long)value;
          break;
        case "state":
          switch (valueString) {
          case "visible":
            State = AchievementState.VISIBLE;
            break;
          case "secret":
            State = AchievementState.SECRET;
            break;
          case "hidden":
            State = AchievementState.HIDDEN;
            break;
          }
          break;
        case "required_count":
          RequiredCount = (long)value;
          break;
        case "count":
          Count = (long)value;
          break;
        case "progress_at":
          ProgressAt = (long)value;
          break;
        case "completed_at":
          CompletedAt = (long)value;
          break;
        }
      }
    }
  }
}
