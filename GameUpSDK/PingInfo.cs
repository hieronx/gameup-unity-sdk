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

namespace GameUp
{
  
  /// <summary>
  /// Represents a response containing GameUp global and/or ping info.
  /// </summary>
  public class PingInfo
  {
    
    /// <summary>
    /// The current time in UTC milliseconds according to the GameUp service.
    /// Note: This represents the time as it was when the server processed this
    /// request and does not account for request round trip time.
    /// </summary>
    public long Time { get; set; }
  }
}
