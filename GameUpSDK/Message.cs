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
  /// Represents a message from the mailbox.
  /// </summary>
  public class Message
  {
    /// <summary> ID of the message. </summary>
    public readonly String MessageId ;

    /// <summary> Tags associated with this message. </summary>
    public readonly String[] Tags ;

    /// <summary> Subject of the message. </summary>
    public readonly String Subject ;

    /// <summary> Leaderboard creation unix time. </summary>
    public readonly long CreatedAt ;

    /// <summary> Message expiration unix time. </summary>
    public readonly long ExpiresAt ;

    /// <summary> When the message was first read unix time. </summary>
    public readonly long ReadAt ;

    /// <summary> Raw Key-Value representing the body of the message. </summary>
    public readonly IDictionary<String, Object> Body ;

    internal Message (IDictionary<string, object> dict)
    {
      foreach (string key in dict.Keys) {
        object value;
        dict.TryGetValue (key, out value);
        if (value == null) {
          continue;
        }
        string valueString = value.ToString ();
        
        switch (key) {
        case "message_id":
          MessageId = valueString;
          break;
        case "tags":
          List<string> tagList = new List<string> ();
          JsonArray tagArray = (JsonArray)value;
          foreach (string entryObject in tagArray) {
            tagList.Add (entryObject);
          }
          Tags = tagList.ToArray ();
          break;
        case "subject":
          Subject = valueString;
          break;
        case "created_at":
          CreatedAt = (long)value;
          break;
        case "updated_at":
          CreatedAt = (long)value;
          break;
        case "body":
          Body = (IDictionary<string,object>)value;
          break;
        }
      }
    }
  }
}

