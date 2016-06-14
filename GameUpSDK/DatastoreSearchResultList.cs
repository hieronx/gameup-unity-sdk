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
  /// <summary> Represents Datastore query result data </summary>
  public class DatastoreSearchResultList : IEnumerable<DatastoreObject>
  {
    /// <summary> The number of Datastore objects returned as part of this response. </summary>
    public readonly long Count;

    /// <summary> The total number of Datastore objects available on the server. </summary>
    public readonly long TotalCount;

    /// <summary> Retrieved search results </summary>
    public readonly DatastoreObject[] Results;

    internal DatastoreSearchResultList (IDictionary<string, object> dict)
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
        case "total_count":
          TotalCount = (long)value;
          break;
        case "results":
          List<DatastoreObject> rList = new List<DatastoreObject> ();
          JsonArray resultArray = (JsonArray)value;
          foreach (object resultObject in resultArray) {
            DatastoreObject datastoreObject = new DatastoreObject ((IDictionary<string, object>)resultObject);
            rList.Add (datastoreObject);
          }
          Results = rList.ToArray ();
          break;
        }
      }
    }

    public IEnumerator<DatastoreObject> GetEnumerator ()
    {
      return (new List<DatastoreObject> (Results)).GetEnumerator ();
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

