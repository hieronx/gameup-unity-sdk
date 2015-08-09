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
  /// Represents a list of leaderboards with meta information.
  /// </summary>
  public class LeaderboardList : IEnumerable<Leaderboard>
  {
    /// <summary> The number of leaderboards returned. </summary>
    public int Count { get ; set ; }
    
    /// <summary> The leaderboard items. </summary>
    public Leaderboard[] Leaderboards { get; set; }

    public IEnumerator<Leaderboard> GetEnumerator ()
    {
      return (new List<Leaderboard>(Leaderboards)).GetEnumerator ();
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
