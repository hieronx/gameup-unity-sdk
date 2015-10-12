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
    public String MessageId { get ; set ; }

    /// <summary> Tags associated with this message. </summary>
    public String[] Tags { get ; set ; }

    /// <summary> Subject of the message. </summary>
    public String Subject { get ; set ; }

    /// <summary> Leaderboard creation unix time. </summary>
    public long CreatedAt { get ; set ; }

    /// <summary> Message expiration unix time. </summary>
    public long ExpiresAt { get ; set ; }

    /// <summary> When the message was first read unix time. </summary>
    public long ReadAt { get ; set ; }

    /// <summary> Raw Key-Value representing the body of the message. </summary>
    public IDictionary<String, Object> Body { get ; set ; }
  }
}

