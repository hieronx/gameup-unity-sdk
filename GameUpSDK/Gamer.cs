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
  /// Represents a gamer in the GameUp service.
  /// </summary>
  public class Gamer
  {
    private readonly String nickname;
    private readonly String name;
    private readonly short timezone;
    private readonly String location;
    private readonly String locale;
    private readonly long createdAt;
    
    /// <summary> Nickname, intended for easy public display. </summary>
    public String Nickname { get { return nickname; } }
    
    /// <summary> A real name, if one was provided when the gamer signed up. </summary>
    public String Name { get { return name; } }
    
    /// <summary> Time zone of the gamer. </summary>
    public short Timezone { get { return timezone; } }
    
    /// <summary> Location of the gamer. </summary>
    public String Location { get { return location; } }
    
    /// <summary> Locale string for this gamer. </summary>
    public String Locale { get { return locale; } }
    
    /// <summary> When the gamer first registered with GameUp. </summary>
    public long CreatedAt { get { return createdAt ; } }

    internal Gamer (String nickname, String name, short timezone, String location, String locale, long createdAt)
    {
      this.nickname = nickname;
      this.name = name;
      this.timezone = timezone;
      this.location = location;
      this.locale = locale;
      this.createdAt = createdAt;
    }
  }
}

