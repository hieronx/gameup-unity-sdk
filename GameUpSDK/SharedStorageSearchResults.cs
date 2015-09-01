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
  /// <summary> Represents shared storage data </summary>
  public class SharedStorageSearchResults : IEnumerable<SharedStorageObject>
  {
    /// <summary> The number of shared storage data returned as part of this response. </summary>
    public int Count;

    /// <summary> The total number of shared storage data available on the server. </summary>
    public int TotalCount;

    /// <summary> Retrieved search results </summary>
    public SharedStorageObject[] Results;

    public IEnumerator<SharedStorageObject> GetEnumerator ()
    {
      return (new List<SharedStorageObject>(Results)).GetEnumerator ();
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

  /// <summary> Represents a shared storage values </summary>
  public class SharedStorageObject   
  {
    /// <summary> Public portion of search result. </summary>
    public IDictionary<string, object> Public;

    /// <summary> Protected portion of search result. </summary>
    public IDictionary<string, object> Protected;

    /// <summary> Convert Public data to the specified user defined data type. </summary>
    public T ConvertPublic<T> () {
      if (Public is JsonObject) {
        return SimpleJson.DeserializeObject<T> (Public.ToString());
      }
      return SimpleJson.DeserializeObject<T> (SimpleJson.SerializeObject(Public));
    }

    /// <summary> Convert Public data to the string. </summary>
    public string ConvertPublic () {
      if (Public is JsonObject) {
        return Public.ToString();
      }
      return SimpleJson.SerializeObject(Public);
    }

    /// <summary> Convert Protected data to the specified user defined data type. </summary>
    public T ConvertProtected<T> () {
      if (Protected is JsonObject) {
        return SimpleJson.DeserializeObject<T> (Protected.ToString());
      }
      return SimpleJson.DeserializeObject<T> (SimpleJson.SerializeObject(Protected));
    }

    /// <summary> Convert Protected data to the string. </summary>
    public string ConvertProtected () {
      if (Protected is JsonObject) {
        return Protected.ToString();
      }
      return SimpleJson.SerializeObject(Protected);
    }
  }
}

