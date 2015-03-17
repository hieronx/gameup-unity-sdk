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
  /// Represents a gamer's detailed standing on a leaderboard.
  /// </summary>
  public class Rank
  {

    /// <summary> Nickname, suitable for public display. </summary>
    public String Name { get ; set ; }
    
    /// <summary> Most up to date rank. </summary>
    public long Ranking { get ; set ; }
    
    /// <summary> When the latest rank was calculated. </summary>
    public long RankAt { get ; set ; }
    
    /// <summary> The best score the gamer has entered on this leaderboard. </summary>
    public long Score { get ; set ; }
    
    /// <summary> When the best score was recorded. </summary>
    public long ScoreAt { get ; set ; }
    
    /// <summary>
    /// If this data is in response to a leaderboard submission, and the score
    /// submitted replaces the previous one, this field will contain that
    /// previous value.
    /// </summary>
    public long LastScore { get ; set ; }
    
    /// <summary> When the previous score was submitted. </summary>
    public long LastScoreAt { get ; set ; }
    
    /// <summary> What the rank on this leaderboard was when it was previously checked. </summary>
    public long LastRank { get ; set ; }
    
    /// <summary> When the previous rank was calculated. </summary>
    public long LastRankAt { get ; set ; }
    
    /// <summary> The highest rank this gamer has ever had on this leaderboard. </summary>
    public long BestRank { get ; set ; }
    
    /// <summary> When the highest rank was recorded. </summary>
    public long BestRankAt { get ; set ; }

    /// <returns>
    /// true if this is the first time the current gamer appears on this
    ///         leaderboard, false otherwise.
    /// </returns>
    public bool isNew() {
      return LastRank == 0;
    }
    
    /// <returns>
    /// true if the response indicates the gamer has a new best score on
    /// this leaderboard, false otherwise.
    /// </returns>
    public bool isNewScore() {
      return Score != LastScore;
    }
    
    /// <returns>
    /// true if the rank has changed since it was last checked,
    /// regardless if it's now higher or lower, false otherwise.
    /// </returns>
    public bool isNewRank() {
      return Ranking != LastRank;
    }
    
    /// <returns>
    /// true if this response contains a new all-time best rank on this
    /// leaderboard, false otherwise.
    /// </returns>
    public bool isNewBestRank() {
      return Ranking == BestRank && RankAt == BestRankAt;
    }

  }
}
