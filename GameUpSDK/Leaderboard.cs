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
  /// Represents a GameUp leaderboard metadata and top ranked gamers.
  /// </summary>
  public class Leaderboard : IEnumerable<Leaderboard.Entry>
  {
    /// <summary> Leaderboard ID. </summary>
    public String LeaderboardId { get ; set ; }

    /// <summary> Leaderboard display name. </summary>
    public String Name { get ; set ; }

    /// <summary> Leaderboard public identifier. </summary>
    public String PublicId { get ; set ; }

    /// <summary> Sort order indicator. </summary>
    public LeaderboardSort Sort { get ; set ; }

    /// <summary> Type indicator. </summary>
    public LeaderboardType Type { get ; set ; }

    /// <summary> Leaderboard display hint. </summary>
    public String DisplayHint { get ; set ; }

    /// <summary> Leaderboard tags. </summary>
    public String[] Tags { get ; set ; }

    /// <summary> Leaderboard limit. </summary>
    public long Limit { get ; set ; }

    /// <summary> Leaderboard creation unix time. </summary>
    public long CreatedAt { get ; set ; }

    /// <summary> Leaderboard Reset configuration. </summary>
    public Reset LeaderboardReset { get ; set ;}

    /// <summary>
    /// The top ranked gamers on this board, up to 50. Already sorted according
    /// to the leaderboard sort settings.
    /// </summary>
    public Entry[] Entries { get ; set ; }

    /// <summary>
    /// Leaderboard sort order hint.
    /// </summary>
    public enum LeaderboardSort
    {

      /// <summary> Indicates the entries should be sorted ascending by score. </summary>
      ASC,

      /// <summary> Indicates the entries should be sorted descending by score. </summary>
      DESC
    }

    /// <summary>
    /// The type of a leaderboard.
    /// </summary>
    public enum LeaderboardType
    {
      /// <summary> Standard best score, one entry per gamer leaderboard type. </summary>
      RANK
    }

    /// <summary>
    /// An entry for a player in a leaderboard.
    /// </summary>
    public class Entry
    {
      /// <summary> The rank in a player's entry. </summary>
      public long Rank { get ; set ; }

      /// <summary> Nickname, suitable for public display. </summary>
      public String Name { get ; set ; }

      /// <summary> The score in a player's entry. </summary>
      public long Score { get ; set ; }

      /// <summary> When the score was submitted to this leaderboard. </summary>
      public long ScoreAt { get ; set ; }

      /// <summary> Scoretags in this entry. </summary>
      public IDictionary<String, Object> Scoretags { get ; set ; }

      /// <summary> Convert Scoretags to the specified user defined data type. </summary>
      public T ConvertScoretags<T>() {
        string json = SimpleJson.SerializeObject(Scoretags);
        return SimpleJson.DeserializeObject<T>(json, null);
      }
    }

    /// <summary>
    /// The reset information for a daily, weekly, or monthly leaderboard.
    /// </summary>
    public class Reset
    {

      /// <summary> Leaderboard Reset Type - daily, weekly or monthly. </summary>
      public ResetType Type { get ; set ; }

      /// <summary> Leaderboard Reset UTC Hour </summary>
      public long UtcHour { get ; set ; }

      /// <summary> Leaderboard Reset Day in a week; will be 0 if unset. </summary>
      public int DayOfWeek { get ; set ; }

      /// <summary> Leaderboard Reset Day in a month; will be 0 if unset. </summary>
      public int DayOfMonth { get ; set ; }

      /// <summary>
      /// The reset type for the leaderboard.
      /// </summary>
      public enum ResetType
      {
        
        /// <summary> Daily value. </summary>
        DAILY,
        
        /// <summary> Weekly value. </summary>
        WEEKLY,
        
        /// <summary> Monthly value. </summary>
        MONTHLY
      }
    }

    public IEnumerator<Leaderboard.Entry> GetEnumerator ()
    {
      return (new List<Entry>(Entries)).GetEnumerator ();
    }

    private IEnumerator GetEnumerator1 ()
    {
      return this.GetEnumerator ();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator1 ();
    }
  }
}

