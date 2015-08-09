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
  /// Represents a combined response which contains both a Leaderboard and a Rank.
  /// </summary>
  public class LeaderboardAndRank
  {
    /// <summary> The Rank portion of the response. </summary>
    public Rank Rank { get ; set ;}

    /// <summary> The Leaderboard portion of the response. </summary>
    public Leaderboard Leaderboard { get ; set ;}

  }
}

