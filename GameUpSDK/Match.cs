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
  /// Represents a response containing GameUp Multiplayer Match with its metadata.
  /// </summary>
  public class Match
  {
    /// <summary> Nickname set for the current gamer in this given match
    /// If the current gamer's nickname is changed after the match is setup, 
    /// the old nickname is still used hence the use of this 'whoami' field. 
    /// </summary>
    public String WhoAmI { get ; set; }
    
    /// <summary> Match ID </summary>
    public String MatchId { get ; set; }
    
    /// <summary> Current turn number </summary>
    public long TurnCount { get ; set; }

    /// <summary> Name of gamer for the given turn  </summary>
    public String Turn { get ; set; }

    /// <summary> Nickname of all the gamers in the current match </summary>
    public String[] Gamers { get; set; }

    /// <summary> When the match was created </summary>
    public long CreatedAt { get ; set; } 

    /// <summary> Checks to see if the match is still ongoing or has ended. </summary>
    public bool Active { get ; set; } 
  }
}