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

namespace GameUp
{
  /// <summary>
  /// Represents an achievement's data, also contains gamer progress if applicable.
  /// </summary>
  public class Achievement
  {
    private readonly String publicId;
    private readonly String name;
    private readonly String description;
    private readonly Type type;
    private readonly int points;
    private readonly State state;
    private readonly int requiredCount;
    private readonly int count;
    private readonly long progressAt;
    private readonly long completedAt;
    
    /// <summary> Game-unique public identifier for this achievement. </summary>
    public String PublicId { get { return  publicId; } }
    
    /// <summary> Achievement name. </summary>
    public String Name { get { return  name; } }
    
    /// <summary> Achievement description or instructions. </summary>
    public String Description { get { return description; } }
    
    /// <summary> The type of the achievement, referring to gamer interaction model. </summary>
    public Type achievementType { get { return type; } }
    
    /// <summary>
    /// Number of points that will be awarded for completing this achievement,
    /// or have already been awarded if it is already complete.
    /// </summary>
    public int Points { get { return points ; } }
    
    /// <summary> The state of this achievement, referring to display logic. </summary>
    public State AchievementState { get { return state; } }
    
    /// <summary>
    /// Required number of actions to complete this achievement, subject to game
    /// logic. For "normal"-type achievements this will always be 1.
    /// </summary>
    public int RequiredCount { get { return requiredCount ; } }
    
    /// <summary>
    /// Current gamer progress towards the required count of this achievement,
    /// subject to the same game logic as the requiredCount field.
    /// </summary>
    public int Count { get { return count; } }
    
    /// <summary>
    /// UTC timestamp in milliseconds when the gamer last made any progress
    /// towards this achievement, or 0 if no progress ever.
    /// </summary>
    public long ProgressAt { get { return progressAt; } }
    
    /// <summary>
    /// UTC timestamp in milliseconds when the gamer completed this achievement,
    /// or 0 if it has not yet been completed.
    /// </summary>
    public long CompletedAt { get { return completedAt; } }
    
    /// <returns> 
    /// true if the gamer has completed this achievement,
    /// false otherwise.
    /// </returns>
    public bool IsCompleted ()
    {
      return completedAt > 0L;
    }

    /// <summary>
    /// Achievement Type, referring to how gamer interaction must occur.
    /// </summary>
    public enum Type
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
    public enum State
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

    internal Achievement (String publicId, String name, String description, Type type, int points, State state, int requiredCount, int count, long progressAt, long completedAt)
    {
      this.publicId = publicId;
      this.name = name;
      this.description = description;
      this.type = type;
      this.points = points;
      this.state = state;
      this.requiredCount = requiredCount;
      this.count = count;
      this.progressAt = progressAt;
      this.completedAt = completedAt;
    }
  }
}
