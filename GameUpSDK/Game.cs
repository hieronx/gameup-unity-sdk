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
  /// Represents a game on the GameUp service.
  /// </summary>
  public class Game
  {
    private readonly String name;
    private readonly String description;
    private readonly long createdAt;
    private readonly long updatedAt;
    
    /// <summary> Game's registered display name. </summary>
    public String Name { get { return name; } }
    
    /// <summary> Game description text. </summary>
    public String Description { get { return description; } }
    
    /// <summary> Timestamp when the game was created. </summary>
    public long CreatedAt { get { return createdAt; } }
    
    /// <summary> Timestamp when game details were last changed or updated. </summary>
    public long UpdatedAt { get { return updatedAt; } }

    internal Game (String name, String description, long createdAt, long updatedAt)
    {
      this.name = name;
      this.description = description;
      this.createdAt = createdAt;
      this.updatedAt = updatedAt;
    }
  }
}

