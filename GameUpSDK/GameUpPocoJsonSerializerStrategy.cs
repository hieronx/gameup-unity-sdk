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
using System.Text.RegularExpressions;
using UnityEngine;

namespace GameUp
{
  public class GameUpPocoJsonSerializerStrategy : PocoJsonSerializerStrategy
  {

    private readonly Regex rx = new Regex(@"\B[A-Z]");
    private readonly Dictionary<string, string> specials = new Dictionary<string, string>();

    internal GameUpPocoJsonSerializerStrategy() {
      specials.Add("ranking", "rank");
    }

    protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
    {
      String output = clrPropertyName;
      output = rx.Replace(output, m => "_" + m.ToString().ToLower());
      output = output.ToLower();

      string specialOutput;
      specials.TryGetValue(clrPropertyName.ToLower(), out specialOutput);
      if (specialOutput != null) {
        output = specialOutput;
      }

      return output;
    }
  }
}

