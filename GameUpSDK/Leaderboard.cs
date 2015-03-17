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

    /// <summary> Leaderboard display name. </summary>
    public String Name { get ; set ; }

    /// <summary> Leaderboard public identifier. </summary>
    public String PublicId { get ; set ; }

    /// <summary> Sort order indicator. </summary>
    public Sort SortOrder { get ; set ; }

    /// <summary> Type indicator. </summary>
    public Type LeaderboardType { get ; set ; }

    /// <summary>
    /// The top ranked gamers on this board, up to 50. Already sorted according
    /// to the leaderboard sort settings.
    /// </summary>
    public Entry[] Entries { get ; set ; }

    /// <summary>
    /// Leaderboard sort order hint.
    /// </summary>
    public enum Sort
    {

      /// <summary> Indicates the entries should be sorted ascending by score. </summary>
      ASC,
      
      /// <summary> Indicates the entries should be sorted descending by score. </summary>
      DESC
    }
    
    /// <summary>
    /// Leaderboard type hint.
    /// </summary>
    public enum Type 
    {
      
      /// <summary> Standard best score, one entry per gamer leaderboard type. </summary>
      RANK
    }

    public class Entry 
    {
     
      ///<summary> Nickname, suitable for public display. </summary>
      public String Name { get ; set ; }

      ///<summary> Score. </summary>
      public long Score { get ; set ; }
      
      ///<summary> When the score was submitted to this leaderboard. </summary>
      public long ScoreAt { get ; set ; }
     
    }

    // Must also implement IEnumerable.GetEnumerator, but implement as a private method.
    // When you implement IEnumerable(T), you must also implement IEnumerable and IEnumerator(T). 
    // see https://msdn.microsoft.com/en-us/library/s793z9y2(v=vs.110).aspx
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

