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
  /// Represents a response containing GameUp global and/or server instance info.
  /// </summary>
  public class MatchTurn
  {
    /// <summary> Turn type </summary>
    public String Type { get ; set; }
    
    /// <summary> Current turn number </summary>
    public int TurnNumber { get ; set; }

    /// <summary> Name of gamer for this turn </summary>
    public String Gamer { get ; set; }

    /// <summary> Data stored for this turn </summary>
    public String Data { get ; set; }
    
    /// <summary> When the match was created </summary>
    public long CreatedAt { get ; set; }

  }
}
