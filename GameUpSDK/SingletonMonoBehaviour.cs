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

using UnityEngine;

namespace GameUp
{
  public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
  {
    private static T _instance;

    public static T Instance {
      get {
        if (_instance == null) {
          _instance = GameObject.FindObjectOfType<T> ();
          if (_instance == null) {
            GameObject gameObject = new GameObject (typeof(T).Name);
            _instance = gameObject.AddComponent<T> ();
            DontDestroyOnLoad (gameObject);
          }

          if (!_instance.Initialized) {
            _instance.Initialize ();
            _instance.Initialized = true;
          }
        }
        return _instance;
      }
    }

    private void Awake ()
    {
      if (_instance != null) {
        DestroyImmediate (gameObject);
      }
    }

    protected bool Initialized { get; set; }

    protected virtual void Initialize ()
    {
    }
  }
}
