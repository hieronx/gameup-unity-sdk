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
using System.Net;
using UnityEngine;

namespace GameUp
{
  /// <summary>
  /// The game client for the GameUp service.
  /// </summary>
  public class Client
  {
    private readonly static IJsonSerializerStrategy serializerStrategy = new GameUpPocoJsonSerializerStrategy();

    public static readonly string SCHEME = "https";
    public static readonly int PORT = 443;
    public static readonly string ACCOUNTS_SERVER = "accounts.gameup.io";
    public static readonly string API_SERVER = "api.gameup.io";

    /// <summary>
    /// Gets or sets the GameUp API key.
    /// </summary>
    /// <value>The GameUp API key.</value>
    public static string ApiKey { get; set; }

    private Client ()
    {
    }

    public delegate void ServerCallback (ServerInfo info);

    public delegate void GameCallback (Game game);

    public delegate void AchievementsCallback (AchievementList achievements);

    public delegate void LeaderboardsCallback (LeaderboardList leaderboards);

    public delegate void LeaderboardCallback (Leaderboard leaderboard);

    public delegate void LoginCallback (SessionClient session);

    public delegate void SuccessCallback ();

    public delegate void GenericSuccessCallback<T> (T data);

    public delegate void ErrorCallback (int statusCode, string reason);

    /// <summary>
    /// Ping the GameUp service to check it is reachable.
    /// </summary>
    /// <param name="error">The callback to execute on error.</param>
    public static void Ping (ErrorCallback error)
    {
      Ping (() => {}, error);
    }

    /// <summary>
    /// Ping the GameUp service to check it is reachable.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Ping (SuccessCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, API_SERVER, PORT, "/v0/");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, "");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success ();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Retrieve global service information from GameUp; includes server
    /// time and other information.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Server (ServerCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, API_SERVER, PORT, "/v0/server");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, "");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<ServerInfo> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Retrieve information about the Game associated to the API Key. API Keys
    /// are obtained from the GameUp Dashboard - https://dashboard.gameup.io
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Game (GameCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, API_SERVER, PORT, "/v0/game");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, "");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Game> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Get a list of achievements available in the game. This excludes any gamer
    /// information for the achievements; their progress or completion timestamps.
    /// See SessionClient.Achievements (...) for Gamer information.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Achievements (AchievementsCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, API_SERVER, PORT, "/v0/game/achievement");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, "");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success (SimpleJson.DeserializeObject<AchievementList> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Retrieve the list of available leaderboards for this game.
    /// </summary>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Leaderboards (LeaderboardsCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, API_SERVER, PORT, "/v0/game/leaderboard");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, "");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success (SimpleJson.DeserializeObject<LeaderboardList> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Get the leaderboard with the supplied ID; this will contain the top ranked
    /// gamers on the leaderboard.
    /// </summary>
    /// <param name="id">The ID of the Leaderboard.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Leaderboard (string id, LeaderboardCallback success, ErrorCallback error)
    {
      Leaderboard(id, 0, 50, false, success, error);
    }

    /// <summary>
    /// Get the leaderboard with the supplied ID; this will contain the top ranked
    /// gamers on the leaderboard.
    /// </summary>
    /// <param name="id">The ID of the Leaderboard.</param>
    /// <param name="limit">Number of entries to return. Default is 50.</param>
    /// <param name="offset">Entries to start from with the leaderboard list. Default is 0.</param>
    /// <param name="withScoretags">Include Scoretags in the leaderboard entries.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void Leaderboard (string id, int limit, int offset, bool withScoretags, LeaderboardCallback success, ErrorCallback error)
    {
      string path = "/v0/game/leaderboard/" + id;
      string queryParam = "?offset=" + offset + "&limit=" + limit + "&with_scoretags=" + withScoretags;
      // HttpUtility.ParseQueryString is not available in Unity.
      UriBuilder b = new UriBuilder (SCHEME, API_SERVER, PORT, path, queryParam);
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "GET", ApiKey, "");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<Leaderboard> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Use a unique ID to generate an account for a gamer. This ID should be cached on the
    /// gaming device so it can be re-used to re-login the gamer to the same account. See
    /// the documentation for more information.
    /// </summary>
    /// <param name="id">The unique ID used for the gamer (could be a device ID).</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginAnonymous (string id, LoginCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, ACCOUNTS_SERVER, PORT, "/v0/gamer/login/anonymous");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, "");
      wwwRequest.SetBody ("{\"id\": \"" + id + "\"}");
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(createSessionClient(jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Login a gamer using a GameUp Email / Password combination.
    /// </summary>
    /// <param name="email">Gamer's Email.</param>
    /// <param name="password">Gamer's Password.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginGameUp (string email, string password, LoginCallback success, ErrorCallback error)
    {
      LoginGameUp(email, password, null, success, error);
    }

    /// <summary>
    /// Login a gamer using a GameUp Email / Password combination and link an existing gamer token.
    /// </summary>
    /// <param name="email">Gamer's Email.</param>
    /// <param name="password">Gamer's Password.</param>
    /// <param name="session">An existing session client.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginGameUp (string email, string password, SessionClient session, LoginCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, ACCOUNTS_SERVER, PORT, "/v0/gamer/login/gameup");
      String token = session == null ? "" : session.Token;
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, token);

      wwwRequest.SetBody ("{\"email\": \"" + email + "\", \"password\":\"" + password +"\"}");

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(createSessionClient(jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Create a new account for the gamer using a GameUp Email / Password combination and link an existing gamer token.
    /// </summary>
    /// <param name="email">Gamer's Email.</param>
    /// <param name="password">Gamer's Password.</param>
    /// <param name="confirm_password">Gamer's Password Confirmation</param>
    /// <param name="name">Gamer's Name - Optional</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void CreateGameUpAccount (string email, string password, string confirm_password, string name, LoginCallback success, ErrorCallback error)
    {
      CreateGameUpAccount(email, password, confirm_password, name, null, success, error);
    }

    /// <summary>
    /// Create a new account for the gamer using a GameUp Email / Password combination and link an existing gamer token.
    /// </summary>
    /// <param name="email">Gamer's Email.</param>
    /// <param name="password">Gamer's Password.</param>
    /// <param name="confirm_password">Gamer's Password Confirmation</param>
    /// <param name="name">Gamer's Name</param>
    /// <param name="session">An existing session client.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void CreateGameUpAccount (string email, string password, string confirm_password, string name, SessionClient session, LoginCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, ACCOUNTS_SERVER, PORT, "/v0/gamer/account/gameup/create");
      String token = session == null ? "" : session.Token;
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, token);

      wwwRequest.SetBody ("{\"email\": \"" + email + "\", \"password\":\"" + password + "\", \"confirm_password\":\"" + confirm_password + "\"}");
      if (name.Trim().Length > 0) {
        wwwRequest.SetBody ("{\"email\": \"" + email + "\", \"password\":\"" + password + "\", \"confirm_password\":\"" + confirm_password + "\", \"name\":\"" + name + "\"}");
      }

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(createSessionClient(jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Send a password recovery email to the gamer
    /// </summary>
    /// <param name="email">Gamer's Email.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void ResetEmailGameUp (string email, SuccessCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, ACCOUNTS_SERVER, PORT, "/v0/gamer/account/gameup/reset/send");
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, "");

      wwwRequest.SetBody ("{\"email\": \"" + email + "\"}");

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success();
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();  
    }


    /// <summary>
    /// Login a gamer using a Facebook OAuth Token. An OAuth token can be obtained from the
    /// Facebook SDK.
    /// </summary>
    /// <param name="accessToken">The Facebook OAuth token.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginOAuthFacebook (string accessToken, LoginCallback success, ErrorCallback error)
    {
      LoginOAuthFacebook (accessToken, null, success, error);
    }

    /// <summary>
    /// Login a gamer using a Facebook OAuth Token; or link a Facebook account to their
    /// current session.
    /// </summary>
    /// <param name="accessToken">The Facebook OAuth token.</param>
    /// <param name="session">An existing gamer session client.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginOAuthFacebook (string accessToken, SessionClient session, LoginCallback success, ErrorCallback error)
    {
      LoginOAuth ("facebook", accessToken, session, success, error);
    }

    /// <summary>
    /// Login a gamer using a Google OAuth Token. An OAuth token can be obtained from the
    /// Google SDK.
    /// </summary>
    /// <param name="accessToken">The Google OAuth token.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginOAuthGoogle (string accessToken, LoginCallback success, ErrorCallback error)
    {
      LoginOAuthGoogle (accessToken, null, success, error);
    }

    /// <summary>
    /// Login a gamer using a Google OAuth token; or link a Google account to their
    /// current session.
    /// </summary>
    /// <param name="accessToken">The Google OAuth token.</param>
    /// <param name="session">An existing gamer session client.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void LoginOAuthGoogle (string accessToken, SessionClient session, LoginCallback success, ErrorCallback error)
    {
      LoginOAuth ("google", accessToken, session, success, error);
    }

    private static void LoginOAuth (string type, string accessToken, SessionClient session, LoginCallback success, ErrorCallback error)
    {
      UriBuilder b = new UriBuilder (SCHEME, ACCOUNTS_SERVER, PORT, "/v0/gamer/login/oauth2");
      String token = session == null ? "" : session.Token;
      WWWRequest wwwRequest = new WWWRequest (b.Uri, "POST", ApiKey, token);

      wwwRequest.SetBody ("{\"type\": \"" + type + "\", \"access_token\":\"" + accessToken +"\"}");

      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(createSessionClient(jsonResponse));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    /// <summary>
    /// Make a generic request with pre-set ApiKey.
    /// </summary>
    /// <param name="uri">The URI for the request.</param>
    /// <param name="method">The method for the request.</param>
    /// <param name="body">The body for the request, may be null.</param>
    /// <param name="success">The callback to execute on success.</param>
    /// <param name="error">The callback to execute on error.</param>
    public static void MakeRequest <T>(Uri uri, string method, string body, GenericSuccessCallback<T> success, ErrorCallback error)
    {
      WWWRequest wwwRequest = new WWWRequest (uri, method, ApiKey, "");
      wwwRequest.SetBody(body);
      
      wwwRequest.OnSuccess = (String jsonResponse) => {
        success(SimpleJson.DeserializeObject<T> (jsonResponse, serializerStrategy));
      };
      wwwRequest.OnFailure = (int statusCode, string reason) => {
        error (statusCode, reason);
      };
      wwwRequest.Execute ();
    }

    private static SessionClient createSessionClient(String jsonResponse) {
      JsonObject json = SimpleJson.DeserializeObject<JsonObject> (jsonResponse, serializerStrategy);
      String token = System.Convert.ToString (json["token"]);
      SessionClient sessionClient = new SessionClient();
      sessionClient.ApiKey = ApiKey;
      sessionClient.Token = token;
      return sessionClient;
    }
  }
}
