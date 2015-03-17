using UnityEngine;
using System.Collections;
using GameUp;
using System.Collections.Generic;
using System;

public class GameUpTest : MonoBehaviour
{
  Client.ErrorCallback failure = (status, reason) => {
    Debug.LogError (status + " " + reason);
  };
  readonly string achievementId = "70c99a8e6dff4a6fac7e517a8dd4e83f";
  readonly string leaderboardId = "6ded1e8dbf104faba384bb659069ea69";
  readonly string facebookToken = "invalid-token-1234";
  readonly string storage_key = "profile_info";
  
  void Start ()
  {
    string deviceId = SystemInfo.deviceUniqueIdentifier;
    Client.ApiKey = "6fb004d4289748199cb858ab0905f657";
    
    Debug.Log ("Ping...");
    Client.Ping (failure);
    
    Debug.Log ("Server Info...");
    Client.Server ((ServerInfo server) => {
      Debug.Log ("Server Time: " + server.Time);
    }, failure);
    
    Debug.Log ("Game...");
    Client.Game ((Game game) => {
      Debug.Log (game.Name + " " + game.Description + " " + game.CreatedAt);
    }, failure);
    
    Debug.Log ("Achievement...");
    Client.Achievements ((AchievementList a) => {
      IEnumerator<Achievement> en = a.GetEnumerator ();
      en.MoveNext ();
      
      Debug.Log ("Achievements Count: " + a.Count + " " + en.Current.Name);
    }, failure);
    
    Debug.Log ("Leaderboard...");
    Client.Leaderboard (leaderboardId, (Leaderboard l) => {
      IEnumerator<Leaderboard.Entry> en = l.GetEnumerator ();
      en.MoveNext ();
      
      Debug.Log ("Leaderboard Name: " + l.Name + " " + l.PublicId + " " + l.SortOrder + " " + l.LeaderboardType + " " + l.Entries.Length + " " + en.Current.Name);
    }, failure);
    
    Debug.Log ("Anonymous Login with Id : " + deviceId + " ...");
    Client.LoginAnonymous (deviceId, (SessionClient session) => {
      Debug.Log ("Successfully logged in anonymously: " + session.Token);
      
      Debug.Log ("Gamer...");
      session.Gamer ((Gamer gamer) => {
        Debug.Log ("Gamer Name: " + gamer.Name);
      }, failure);
      
      Debug.Log ("Storage Delete...");
      session.StorageDelete (storage_key, () => {
        Debug.Log ("Successfully deleted storage: " + storage_key);
      }, failure);
      
      Debug.Log ("Storage Put...");
      Dictionary<string, string> data = new Dictionary<string, string> ();
      data.Add ("boss", "chris, andrei, mo");
      data.Add ("coins_collected", "2000");
      session.StoragePut (storage_key, data, () => {
        Debug.Log ("Successfully stored: " + storage_key);
        
        Debug.Log ("Storage Get...");
        session.StorageGet (storage_key, (IDictionary<string, string> dic) => {
          string value;
          dic.TryGetValue ("coins_collected", out value);
          Debug.Log ("Successfully retrieved storage coins_collected: " + value);
        }, failure);
      }, failure);
      
      Debug.Log ("AchievementUpdate...");
      session.Achievement (achievementId, 5, () => {
        Debug.Log ("Successfully updated achievement");
      }, failure);
      
      Debug.Log ("Achievements...");
      session.Achievements ((AchievementList al) => {
        Debug.Log ("Retrieved achievements " + al.Count);
      }, failure);
      //
      Debug.Log ("LeaderboardUpdate...");
      session.UpdateLeaderboard (leaderboardId, DateTime.Now.Millisecond, (Rank r) => {
        Debug.Log ("Updated leaderboard. New rank " + r.Ranking + " for " + r.Name);
      }, failure);
      
      Debug.Log ("LeaderboardAndRank...");
      session.LeaderboardAndRank (leaderboardId, (LeaderboardAndRank lr) => {
        Debug.Log ("Retrieved Leaderboard and Rank. " + lr.Leaderboard.Name);
        Debug.Log ("Retrieved Leaderboard and Rank. " + lr.Rank.Name);
      }, failure);
      
      Debug.Log ("Facebook OAuth Login");
      Client.LoginOAuthFacebook (facebookToken, session, (SessionClient facebookSession) => {
        Debug.Log ("Facebook Login Successful: " + facebookSession.Token);
      }, (status, reason) => {
        Debug.Log ("[Expected Failure] Facebook Login Failed: " + status + " " + reason);
      });
      
    }, failure);
  }
  
  // Update is called once per frame
  void Update ()
  {
    
  }
}
