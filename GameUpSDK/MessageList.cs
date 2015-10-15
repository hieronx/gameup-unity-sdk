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
  /// Represents a list of message from the mailbox.
  /// </summary>
  public class MessageList : IEnumerable<Message>
  {
    /// <summary> The number of messages returned as part of this response. </summary>
    public readonly long Count ;

    /// <summary> The messages themselves. </summary>
    public readonly Message[] Messages ;

    public IEnumerator<Message> GetEnumerator ()
    {
      return (new List<Message> (Messages)).GetEnumerator ();
    }

    private IEnumerator GetEnumerator1 ()
    {
      return this.GetEnumerator ();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator1 ();
    }

    internal MessageList (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        
        switch (key) {
        case "count":
          Count = (long) value;
          break;
        case "messages":
          List<Message> mList = new List<Message> ();
          JsonArray messageArray = (JsonArray)value;
          foreach (object messageObject in messageArray) {
            Message message = new Message ((IDictionary<string, object>)messageObject);
            mList.Add (message);
          }
          Messages = mList.ToArray ();
          break;
        }
      }
    }
  }
}

