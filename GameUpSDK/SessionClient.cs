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
    private readonly IJsonSerializerStrategy serializerStrategy = new GameUpPocoJsonSerializerStrategy();

    public long CreatedAt { get ; set ; }
    public string Token { get ; set ; }
    public string ApiKey { get ; set ; }

    public SessionClient ()
    {
      this.CreatedAt = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
    }

    public delegate void GamerCallback (Gamer gamer);

    public delegate void StorageGetCallback (IDictionary<String, string> data);
    public delegate void StorageGetRawCallback (string value);

    public delegate void AchievementCallback ();
    public delegate void AchievementUnlockedCallback (Achievement achievement);

    public delegate void UpdateLeaderboardCallback (Rank rank);

    public delegate void LeaderboardAndRankCallback (LeaderboardAndRank leaderboard);

    public delegate void MatchesCallback (MatchList matches);
    public delegate void MatchCallback (Match match);
    public delegate void TurnCallback (MatchTurnList turns);
    public delegate void TurnSubmitSuccessCallback ();
    public delegate void MatchCreateCallback (Match match);
    public delegate void MatchQueueCallback ();
    public delegate void MatchEndCallback (String matchId);
    public delegate void MatchLeaveCallback (String matchId);

    public delegate void PurchaseVerifyCallback (PurchaseVerification purchaseVerification);

    /// <summary>
    /// Ping the GameUp service to check it is reachable and the current session
    /// is still valid.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Ping (Client.SuccessCallback success, Client.ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, "/v0/");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
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
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Gamer> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Store the supplied IDictionary with the given key into Cloud Storage.
    /// </summary>
    /// <param name="key">The name of the key.</param>
    /// <param name="data">The data dictionary to store.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StoragePut (string key, IDictionary<string, string> data, Client.SuccessCallback success, Client.ErrorCallback error)
    {
      string value = SimpleJson.SerializeObject (data);
      StoragePut (key, value, success, error);
    }

    /// <summary>
    /// Store the supplied value with the given key into Cloud Storage.
    /// </summary>
    /// <param name="key">The name of the key.</param>
    /// <param name="value">The string value to store.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StoragePut (string key, string value, Client.SuccessCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/storage/" + Uri.EscapeUriString (key);
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "PUT", ApiKey, Token);
      wwwRequest.SetBody (value);
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
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StorageGet (string key, StorageGetCallback success, Client.ErrorCallback error)
    {
      WWWRequest wwwRequest = BuildStorageGetRequest (key, error);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        IDictionary<string, string> rawResponse = SimpleJson.DeserializeObject<IDictionary<String, string>> (jsonResponse);
        string data;
        rawResponse.TryGetValue("value", out data);
        success(SimpleJson.DeserializeObject<Dictionary<string, string>>(data));
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Fetch the string object for the given key from Cloud Storage.
    /// </summary>
    /// <param name="key">The name of the key.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void StorageGet (string key, StorageGetRawCallback success, Client.ErrorCallback error)
    {
      WWWRequest wwwRequest = BuildStorageGetRequest (key, error);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        IDictionary<string, string> rawResponse = SimpleJson.DeserializeObject<IDictionary<String, string>> (jsonResponse);
        string data;
        rawResponse.TryGetValue("value", out data);
        success(data);
      };
      wwwRequest.Execute ();
    }

    private WWWRequest BuildStorageGetRequest (string key, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/storage/" + Uri.EscapeUriString (key);
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      return wwwRequest;
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
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "DELETE", ApiKey, Token);
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
    /// <param name="successUnlocked">The callback to execute on success and unlock of an achievement</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Achievement (string id, AchievementCallback success, AchievementUnlockedCallback successUnlock, Client.ErrorCallback error)
    {
      Achievement (id, 1, success, successUnlock, error);
    }

    /// <summary>
    /// Submit the supplied count as progress for an achievement.
    /// </summary>
    /// <param name="achievementId">The ID of the achievement.</param>
    /// <param name="count">The progress count to submit for the achievement.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="successUnlocked">The callback to execute on success and unlock of an achievement</param>
    /// <param name="error">The callback to execute on error.</param>
    public void Achievement (string achievementId, int count, AchievementCallback success, AchievementUnlockedCallback successUnlock, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/achievement/" + achievementId;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      wwwRequest.SetBody("{\"count\":" + count + "}");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        if (jsonResponse.Length == 0) {
          success();
        } else {
          successUnlock(SimpleJson.DeserializeObject<Achievement> (jsonResponse, serializerStrategy));
        }
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
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<AchievementList> (jsonResponse, serializerStrategy));
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
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);

      wwwRequest.SetBody("{\"score\":" + score + "}");

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Rank> (jsonResponse, serializerStrategy));
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
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<LeaderboardAndRank> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Retrieve a list of matches the gamer is part of, along with the metadata for each match. 
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void GetAllMatches (MatchesCallback success, Client.ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, "/v0/gamer/match");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<MatchList> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();  
    }

    /// <summary>
    /// Retrieve a particular match's status and metadata.
    /// </summary>
    /// <param name="matchId">The match identifier</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void GetMatch (string matchId, MatchCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/match/" + matchId;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Match> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Get turn data for a particular match, only returning turns newer than the identified one.
    /// </summary>
    /// <param name="matchId">The match identifier</param>
    /// <param name="turnNumber">The turn number to start from, not inclusive. Use '0' to get all the turns in the match</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void GetTurnData (string matchId, int turnNumber, TurnCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/match/" + matchId + "/turn/" + turnNumber;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, Token);
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<MatchTurnList> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Submit turn data to the specified match.
    /// </summary>
    /// <param name="matchId">The match identifier</param>
    /// <param name="turn">Last seen turn number - this is used as a basic consistency check</param>
    /// <param name="nextGamer">Which gamer the next turn goes to</param>
    /// <param name="turnData">Turn data to submit</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void SubmitTurn (string matchId, int turn, string nextGamer, string turnData, Client.SuccessCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/match/" + matchId + "/turn";
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      
      wwwRequest.SetBody("{\"last_turn\":" + turn + "," +
                         "\"next_gamer\":\"" + nextGamer + "\"," +
                         "\"data\":\"" + turnData + "\"}");
      
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success ();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Request a new match. If there are not enough waiting gamers, the current gamer will be added to the queue instead.
    /// </summary>
    /// <param name="requiredGamers">The minimal required number of gamers needed to create a new match</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void CreateMatch (int requiredGamers, MatchCreateCallback success, MatchQueueCallback queued, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/match/";
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      
      wwwRequest.SetBody("{\"players\":" + requiredGamers + "}");
      
      wwwRequest.OnSuccess = (String jsonResponse) => {
        if (jsonResponse.Length == 0) {
          queued ();
        } else {
          success (SimpleJson.DeserializeObject<Match> (jsonResponse, serializerStrategy));
        }
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// End match. This will only work if it's the current gamer's turn.
    /// </summary>
    /// <param name="matchId">The match identifier</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void EndMatch (string matchId, MatchEndCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/match/" + matchId;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      
      wwwRequest.SetBody("{\"action\":\"end\"}");
      
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(matchId);
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Leave match. This will only work if it's NOT the current gamer's turn.
    /// </summary>
    /// <param name="matchId">The match identifier</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void LeaveMatch (string matchId, MatchLeaveCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/match/" + matchId;
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      
      wwwRequest.SetBody("{\"action\":\"leave\"}");
      
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(matchId);
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Subscribes to the GameUp Push Notification using a Device Token.
    /// It will automatically detect whether the device is Android or iOS (default).
    /// </summary>
    /// <param name="deviceToken">The device token</param>
    /// <param name="segments">List of Segments to subscribe to. An empty list is acceptable.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void SubscribePush (String deviceToken, String[] segments, Client.SuccessCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/push/";
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "PUT", ApiKey, Token);

      String segmentsString = "[" ;
      if (segments.Length > 0) {
        segmentsString += segments[0];
        for (int i = 1; i < segments.Length; i++) {
          segmentsString += "," + segments[i] ;
        }
      }
      segmentsString += "]" ;

      String platform = "ios" ;
      #if UNITY_ANDROID
        platform = "android" ;
      #endif

      String requestBody = "{" +
                            "\"platform\":\"" + platform +"\"," +
                            "\"id\":\"" + deviceToken +"\"," +
                            "\"multiplayer\":false," +
                            "\"segments\":" + segmentsString +
                            "}";
      wwwRequest.SetBody(requestBody);
      
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Verify an Apple purchase with the remote service.
    /// </summary>
    /// <param name="receipt">The encoded receipt data returned by the purchase.</param>
    /// <param name="productId">The ID of the product that was purchased.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void PurchaseVerifyApple (string receipt, string productId, PurchaseVerifyCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/purchase/verify/apple";
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      wwwRequest.SetBody("{\"product_id\":\"" + productId + "\",\"receipt_data\":\"" + receipt + "\"}");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<PurchaseVerification> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Verify a Google product purchase with the remote service.
    /// </summary>
    /// <param name="token">The purchase token returned by the purchase.</param>
    /// <param name="productId">The ID of the product that was purchased.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void PurchaseVerifyGoogleProduct (string token, string productId, PurchaseVerifyCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/purchase/verify/google";
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      wwwRequest.SetBody("{\"product_id\":\"" + productId + "\",\"purchase_token\":\"" + token + "\",\"type\":\"product\"}");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<PurchaseVerification> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Verify a Google subscription purchase with the remote service.
    /// </summary>
    /// <param name="token">The purchase token returned by the purchase.</param>
    /// <param name="subscriptionId">The ID of the subscription that was purchased.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public void PurchaseVerifyGoogleSubscription (string token, string subscriptionId, PurchaseVerifyCallback success, Client.ErrorCallback error)
    {
      string path = "/v0/gamer/purchase/verify/google";
      UriBuilder b = new UriBuilder (Client.SCHEME, Client.API_SERVER, Client.PORT, path);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, Token);
      wwwRequest.SetBody("{\"product_id\":\"" + subscriptionId + "\",\"purchase_token\":\"" + token + "\",\"type\":\"subscription\"}");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<PurchaseVerification> (jsonResponse, serializerStrategy));
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

