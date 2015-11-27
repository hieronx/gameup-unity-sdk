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
  /// <summary> Represents match changes with new turn information. </summary>
  public class MatchChange
  {
    /// <summary> Changed Match. </summary>
    public readonly Match Match;
    
    /// <summary> New turns in the match. </summary>
    public readonly MatchTurn[] Turns;
    
    internal MatchChange (IDictionary<string, object> dict)
    {
      Match = new Match(dict);

      object value;
      dict.TryGetValue ("turns", out value);
      List<MatchTurn> tList = new List<MatchTurn> ();
      JsonArray turnsArray = (JsonArray)value;
      foreach (object turnObject in turnsArray) {
        MatchTurn turn = new MatchTurn ((IDictionary<string, object>)turnObject);
        tList.Add (turn);
      }

      Turns = tList.ToArray ();
    }
  }

  /// <summary>
  /// Represents a list of changes in a match with turn information.
  /// </summary>
  public class MatchChangeList : IEnumerable<MatchChange>
  {
    /// <summary> Unix Timestamp of where the changes are calculated from. </summary>
    public readonly long Since ;

    /// <summary> The number of changed matches. </summary>
    public readonly long Count ;
    
    /// <summary> Changed matches. </summary>
    public readonly MatchChange[] MatchChanges ;

    internal MatchChangeList (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        
        switch (key) {
        case "count":
          Count = (long)value;
          break;
        case "since":
          Since = (long)value;
          break;
        case "matches":
          List<MatchChange> mList = new List<MatchChange> ();
          JsonArray matchChanges = (JsonArray)value;
          foreach (object matchChangeObject in matchChanges) {
            MatchChange matchChange = new MatchChange ((IDictionary<string, object>)matchChangeObject);
            mList.Add (matchChange);
          }
          MatchChanges = mList.ToArray ();
          break;
        }
      }
    }
    
    public IEnumerator<MatchChange> GetEnumerator ()
    {
      return (new List<MatchChange> (MatchChanges)).GetEnumerator ();
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
