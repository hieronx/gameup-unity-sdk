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
  /// Represents the whole response from the matches endpoint, including meta
  /// information about the response.
  /// </summary>
  public class MatchList : IEnumerable<Match>
  {
    /// <summary> The number of matches returned as part of this response. </summary>
    public int Count { get ; set ; }
    
    /// <summary> The matches themselves. </summary>
    public Match[] Matches { get; set; }
    
    // Must also implement IEnumerable.GetEnumerator, but implement as a private method.
    // When you implement IEnumerable(T), you must also implement IEnumerable and IEnumerator(T). 
    // see https://msdn.microsoft.com/en-us/library/s793z9y2(v=vs.110).aspx
    public IEnumerator<Match> GetEnumerator ()
    {
      return (new List<Match>(Matches)).GetEnumerator ();
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
