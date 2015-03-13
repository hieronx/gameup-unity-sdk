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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameUp
{
  /// <summary>
  /// A session client for gamer actions in the GameUp service.
  /// </summary>
  public class SessionClient
  {
    private readonly string apiKey;
    private readonly string token;
    private readonly long createdAt;

    public long CreatedAt { get { return createdAt; } }
    public string Token { get { return token; } }

    internal SessionClient (string apiKey, string token)
    {
      this.apiKey = apiKey;
      this.token = token;
      this.createdAt = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
    }

    public delegate void GamerCallback (Gamer gamer);

    public delegate void StorageGetCallback (IDictionary<String, object> data);

    public delegate void AchievementCallback ();

    public delegate void UpdateLeaderboardCallback (Rank rank);

    public delegate void LeaderboardAndRankCallback (LeaderboardAndRank leaderboard);

    /// <summary>
    /// Ping the GameUp service to check it is reachable and the current session
    /// is still valid.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Ping (Client.SuccessCallback success, Client.ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, "/v0/");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", apiKey, token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success ();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Get information about the current logged in gamer.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Gamer (GamerCallback success, Client.ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, "/v0/gamer");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", apiKey, token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Gamer> (jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Store the supplied object with the given key into Cloud Storage.
    /// </summary>
    /// <param name="key">The name of the key.</param>
    /// <param name="data">The data dictionary to store.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StoragePut (string key, IDictionary<String, object> data, Client.SuccessCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/storage/" + Uri.EscapeUriString (key);
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "PUT", apiKey, token);

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success ();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Fetch the object for the given key from Cloud Storage.
    /// </summary>
    /// <param name="key">The name of the key.</param>
    /// <param name="type">The type to deserialize the JSON into.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StorageGet (string key, StorageGetCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/storage/" + Uri.EscapeUriString (key);
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", apiKey, token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<IDictionary<String, object>> (jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Delete the object with the supplied key from Cloud Storage.
    /// </summary>
    /// <param name="key">The name of the key.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StorageDelete (string key, Client.SuccessCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/storage/" + Uri.EscapeUriString (key);
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "DELETE", apiKey, token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success ();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Submit progress for an achievement.
    /// </summary>
    /// <param name="id">The ID of the achievement.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Achievement (string id, AchievementCallback success, Client.ErrorCallback error)
    {
      Achievement (id, 1, success, error);
    }

    /// <summary>
    /// Submit the supplied count as progress for an achievement.
    /// </summary>
    /// <param name="achievementId">The ID of the achievement.</param>
    /// <param name="count">The progress count to submit for the achievement.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Achievement (string achievementId, int count, AchievementCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/achievement/" + achievementId;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", apiKey, token);

      wwwRequest.SetBody("{'count':'" + System.Convert.ToString (count) + "'}");

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Get a list of achievements available in the game; with the gamer's progress
    /// and any completed achievements.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Achievements (Client.AchievementsCallback success, Client.ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, "/v0/gamer/achievement");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", apiKey, token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<AchievementList> (jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Submit the supplied score to the specified leaderboard. The new score will only
    /// overwrite the previous score if it is "better" according to the sorting rules;
    /// nevertheless the current gamer's rank will always be returned.
    /// </summary>
    /// <param name="id">The ID of the leaderboard.</param>
    /// <param name="score">The new score to submit to the leaderboard.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void UpdateLeaderboard (string id, long score, UpdateLeaderboardCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/leaderboard/" + id;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", apiKey, token);

      wwwRequest.SetBody("{'score':'" + System.Convert.ToString (score) + "'}");

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Rank> (jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Fetch the leaderboard with the top 50 ranked gamers and the current gamer's
    /// ranking.
    /// </summary>
    /// <param name="id">The ID of the leaderboard.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void LeaderboardAndRank (string id, LeaderboardAndRankCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/leaderboard/" + id;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", apiKey, token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        LeaderboardAndRank rank = SimpleJson.DeserializeObject<LeaderboardAndRank> (jsonResponse);
        success(rank);
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    public string Serialize ()
    {
      return SimpleJson.SerializeObject(this);
    }

    public static SessionClient Deserialize (string session)
    {
      return SimpleJson.DeserializeObject<SessionClient> (session);
    }
  }
}

