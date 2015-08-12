using UnityEngine;
using System.Collections;
using GameUp;
using System.Collections.Generic;
using System;

public class GameUpTest : MonoBehaviour
{
  Client.ErrorCallback failure = (status, reason) => {
    if (status >= 500) {
      Debug.LogError (status + ": " + reason);
    } else {
      Debug.LogWarning (status + ": " + reason);
    }
  };
  readonly string achievementId = "70c99a8e6dff4a6fac7e517a8dd4e83f";
  readonly string leaderboardId = "6ded1e8dbf104faba384bb659069ea69";
  readonly string facebookToken = "invalid-token-1234";
  readonly string storage_key = "profile_info";
  
  SessionClient session;
  
  #if UNITY_IOS
  bool tokenSent = false;
  #endif
  
  void Start ()
  {
    string deviceId = SystemInfo.deviceUniqueIdentifier;
    Match match = null;
    
    Client.ApiKey = "9e87fc40a177490f95e734750f6b513e";
    
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

    Debug.Log ("All Leaderboards...");
    Client.Leaderboards ((LeaderboardList list) => {
      IEnumerator<Leaderboard> en = list.GetEnumerator ();
      en.MoveNext ();
      Leaderboard l = en.Current;

      Debug.Log ("Leaderboard Name: " + l.Name + " " + l.PublicId + " " + l.SortOrder + " " + l.LeaderboardType + " " + en.Current.Name);
    }, failure);

    Debug.Log ("Single Leaderboard with 10 entries and 20 offset...");
    Client.Leaderboard (leaderboardId, 10, 20, (Leaderboard l) => {
      IEnumerator<Leaderboard.Entry> en = l.GetEnumerator ();
      en.MoveNext ();
      
      Debug.Log ("Leaderboard Name: " + l.Name + " " + l.PublicId + " " + l.SortOrder + " " + l.LeaderboardType + " " + l.Entries.Length + " " + en.Current.Name);
    }, failure);
    
    Debug.Log ("Anonymous Login with Id : " + deviceId + " ...");
    Client.LoginAnonymous (deviceId, (SessionClient s) => {
      session = s;
      Debug.Log ("Successfully logged in anonymously: " + session.Token);

      Client.CreateGameUpAccount("unitysdk@gameup.io", "password", "password", "UnitySDK Test", session, (SessionClient gus) => {
        session = gus;
        Debug.Log ("Successfully created GameUp Account: " + session.Token);
      }, (status, reason) => { 
        Client.LoginGameUp("unitysdk@gameup.io", "password", session, (SessionClient gus) => {
          session = gus;
          Debug.Log ("Successfully logged in with GameUp Account: " + session.Token);
        }, failure);
      });

      //Let's assume that we are about to save the session 
      //for later usage without having to re-login the user.
      String serializedSession = session.Serialize();
      Debug.Log ("Saved session: " + serializedSession);

      //Let's assume that some time has passed and
      //that we are about to restore the session 
      s = SessionClient.Deserialize(serializedSession);
      Debug.Log ("Restored session: " + s.Token);

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
      }, (Achievement a) => {
        Debug.Log ("Successfully unlocked achievement");
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

      Debug.Log ("LeaderboardUpdate with scoretags...");
      ScoretagTest scoretagtest = new ScoretagTest();
      scoretagtest.Datetime = DateTime.Now.Millisecond;
      session.UpdateLeaderboard (leaderboardId, DateTime.Now.Millisecond, scoretagtest, (Rank r) => {
        Debug.Log ("Updated leaderboard with scoretags. New rank " + r.Ranking + " for " + r.Name + " with tags " + r.Scoretags.ToString());
      }, failure);
      
      Debug.Log ("LeaderboardAndRank...");
      session.LeaderboardAndRank (leaderboardId, (LeaderboardAndRank lr) => {
        Debug.Log ("1-Retrieved Leaderboard and Rank: " + lr.Leaderboard.Name);
        Debug.Log ("1-Retrieved Leaderboard and Rank: " + lr.Rank.Name + " " + lr.Rank.Ranking);
      }, failure);

      Debug.Log ("LeaderboardAndRank of 10 with autooffset...");
      session.LeaderboardAndRank (leaderboardId, 10, (LeaderboardAndRank lr) => {
        Debug.Log ("2-Retrieved Leaderboard Entries count:  " + lr.Leaderboard.Entries.Length);
        Debug.Log ("2-Retrieved Leaderboard and Rank: " + lr.Leaderboard.Name);
        Debug.Log ("2-Retrieved Leaderboard and Rank: " + lr.Rank.Name);
        if (lr.Rank.Scoretags != null) {
          Debug.Log ("2-ScoreTags: " + lr.Rank.Scoretags.ToString());
        }
      }, failure);

      Debug.Log ("LeaderboardAndRank of 10 starting from position 20 (offset)...");
      session.LeaderboardAndRank (leaderboardId, 10, 20, (LeaderboardAndRank lr) => {
        Debug.Log ("3-Retrieved Leaderboard Entries count:  " + lr.Leaderboard.Entries.Length);
        Debug.Log ("3-Retrieved Leaderboard and Rank: " + lr.Leaderboard.Name);
        Debug.Log ("3-Retrieved Leaderboard and Rank: " + lr.Rank.Name);
        if (lr.Rank.Scoretags != null) {
          Debug.Log ("3-ScoreTags: " + lr.Rank.Scoretags.ToString());
        }
      }, failure);
      
      Debug.Log ("Get All Matches...");
      session.GetAllMatches ((MatchList matches) => {
        Debug.Log ("Retrieved Matches. Size: " + matches.Count);
      }, failure);
      
      Debug.Log ("Create match for 2...");
      session.CreateMatch (2, (Match neMatch) => {
        match = neMatch;
        Debug.Log ("New match created. Match ID: " + match.MatchId);
      }, () => {
        Debug.Log ("Gamer queued");
      }, failure);
      
      if (match != null) {
        String matchId = match.MatchId;
        Debug.Log ("Get match details...");
        session.GetMatch (matchId, (Match newMatch) => {
          Debug.Log ("Got match details. Match turn count: " + newMatch.TurnCount);
        }, failure);
        
        Debug.Log ("Get turn data...");
        session.GetTurnData (matchId, 0, (MatchTurnList turns) => {
          Debug.Log ("Got Turns. Count is: " + turns.Count);
          IEnumerator<MatchTurn> turnEnum = turns.GetEnumerator();
          if (turnEnum.MoveNext()) {
            Debug.Log ("Got data for " + turnEnum.Current.Gamer + ". Data: " + turnEnum.Current.Data);
          } else {
            Debug.Log ("No turn list available");
          }
        }, failure);
        
        if (match.Turn.Equals(match.WhoAmI)) {
          Debug.Log ("Submitting a turn...");
          session.SubmitTurn (matchId, (int)match.TurnCount, match.WhoAmI, "Unity SDK Turn Data", () => {
            Debug.Log ("Submitted turn");
          }, failure);
          
          Debug.Log ("Ending match...");
          session.EndMatch(matchId, (String id) => {
            Debug.Log ("Match ended: " + id);
          }, failure);
        } else {
          Debug.Log ("Leaving match...");
          session.LeaveMatch(matchId, (String id) => {
            Debug.Log ("Left match: " + id);
          }, failure);
        }
      }
      
      #if UNITY_IOS
      UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |  UnityEngine.iOS.NotificationType.Badge |  UnityEngine.iOS.NotificationType.Sound);
      #endif
      
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
  
  #if UNITY_IOS
  void FixedUpdate () {
    if (!tokenSent && session != null) { // tokenSent needs to be defined somewhere global so this code is trigged everytime (bool tokenSent = false)
      byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
      if(token != null) {
        tokenSent = true;
        string tokenString = System.BitConverter.ToString(token).Replace("-", "").ToLower();
        
        Debug.Log ("Attempting to subscribe to Push Notifications.");
        String[] segments = {};
        session.SubscribePush(tokenString, segments, () => {
          Debug.Log ("Successfully subscribed to push notifications");
        }, failure);
      }
    }
  }
  #endif
}

class ScoretagTest {
  public long Datetime { get ; set ; }
}
