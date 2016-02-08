using UnityEngine;
using GameUp;
using System.Collections;
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

  readonly string achievementId = "9fcd83327951475caf818d59156a23c2";
  readonly string leaderboardId = "b540acd249384c1784a95912a3f157c0";
  readonly string scriptId = "6ecc280b5b2b4c678eb53401c7133811";
  readonly string mailboxScriptId = "567417e01bd64533bb4677d997cc98bf";
  readonly string facebookToken = "invalid-token-1234";
  readonly string storage_key = "profile_info";
  readonly string shared_storage_key = "ArmyInfo";
  SessionClient session;
  
  #if UNITY_IOS
  bool tokenSent = false;
  #endif

  void Start ()
  {

    string deviceId = SystemInfo.deviceUniqueIdentifier;
    Client.ApiKey = "cd711f5ef5804365a11120897f5d137e";
    Client.EnableGZip = true;

    testClientMethods ();

    Debug.Log ("Anonymous Login with Id : " + deviceId + " ...");
    Client.LoginAnonymous (deviceId, (SessionClient s) => {
      session = s;
      Debug.Log ("Logged in anonymously: " + session.Token);

      //Let's assume that we are about to save the session 
      //for later usage without having to re-login the user.
      String serializedSession = session.Serialize ();
      Debug.Log ("Saved session: " + serializedSession);

      //Let's assume that some time has passed and
      //that we are about to restore the session 
      s = SessionClient.Deserialize (serializedSession);
      Debug.Log ("Restored session: " + s.Token);

      testSessionClientMethods (session);

    }, failure);
  }

  void testClientMethods ()
  {
    Client.Ping ((PingInfo server) => {
      Debug.Log ("Server Time: " + server.Time);
    }, failure);
    
    Client.Game ((Game game) => {
      Debug.Log (game.Name + " " + game.Description + " " + game.CreatedAt);
    }, failure);
    
    Client.Achievements ((AchievementList a) => {
      IEnumerator<Achievement> en = a.GetEnumerator ();
      en.MoveNext ();
      
      Debug.Log ("Achievements Count: " + a.Count + " " + en.Current.Name);
    }, failure);

    Client.Leaderboards ((LeaderboardList list) => {
      foreach (Leaderboard entry in list.Leaderboards) {
        Debug.LogFormat ("Name: " + entry.Name + " sort: " + entry.Sort);
        if (entry.LeaderboardReset != null) {
          Debug.LogFormat ("Name: " + entry.Name + " reset type: " + entry.LeaderboardReset.Type + " reset hr: " + entry.LeaderboardReset.UtcHour);
        } else {
          Debug.LogFormat ("Name: " + entry.Name + " has no reset!");
        }

        // you could add each leaderboard entry to a UI element and show it
      }
    }, failure);

    Client.Leaderboard (leaderboardId, 10, 20, false, (Leaderboard l) => {
      foreach (Leaderboard.Entry en in l.Entries) {
        Debug.Log ("Leaderboard Name: " + l.Name + " " + l.PublicId + " " + l.Sort + " " + l.Type + " " + l.Entries.Length + " " + en.Name);
      }
    }, failure);
  }

  void testSessionClientMethods (SessionClient session)
  {
    Client.CreateEmailAccount ("unitysdk@gameup.io", "password", "password", "UnitySDK Test", session, (SessionClient gus) => {
      session = gus;
      Debug.Log ("Created GameUp Account: " + session.Token);
    }, (status, reason) => { 
      Client.LoginEmail ("unitysdk@gameup.io", "password", (SessionClient gus) => {
        Debug.Log ("Logged in with GameUp Account: " + session.Token);
      }, failure);
    });
    
    Client.linkFacebook (session, facebookToken, () => {
      Debug.Log ("Facebook Linking successful: ");
    }, (status, reason) => {
      Debug.Log ("[Expected Failure] Facebook Linking Failed: " + status + " " + reason);
    });
    
    session.Gamer ((Gamer gamer) => {
      Debug.Log ("Gamer Name: " + gamer.Name);
      testMatch (gamer);
    }, failure);
    
    session.UpdateGamer ("UnitySDKTest", () => {
      Debug.Log ("Updated gamer's profile");
    }, failure);
    
    session.StorageDelete (storage_key, () => {
      Debug.Log ("Deleted storage: " + storage_key);
    }, failure);
    
    Dictionary<string, string> data = new Dictionary<string, string> ();
    data.Add ("boss", "chris, andrei, mo");
    data.Add ("coins_collected", "2000");
    session.StoragePut (storage_key, data, () => {
      Debug.Log ("Stored: " + storage_key);

      SessionClient.StorageGetCallback callback = (IDictionary<string, string> dic) => {
        string value;
        dic.TryGetValue ("coins_collected", out value);
        Debug.Log ("Retrieved storage coins_collected: " + value);
      };

      session.StorageGet (storage_key, callback, failure);
    }, failure);

    session.Achievement (achievementId, 5, () => {
      Debug.Log ("Updated achievement");
    }, (Achievement a) => {
      Debug.Log ("Unlocked achievement");
    }, failure);
    
    session.Achievements ((AchievementList al) => {
      Debug.Log ("Retrieved achievements " + al.Count);
      foreach (Achievement entry in al.Achievements) {
        Debug.LogFormat ("Name: " + entry.Name + " state: " + entry.State);
      }
    }, failure);
    
    session.UpdateLeaderboard (leaderboardId, DateTime.Now.Millisecond, (Rank r) => {
      Debug.Log ("Updated leaderboard. New rank " + r.Ranking + " for " + r.Name);
    }, failure);
    
    ScoretagTest scoretagtest = new ScoretagTest ();
    scoretagtest.Datetime = DateTime.Now.Millisecond;
    session.UpdateLeaderboard (leaderboardId, DateTime.Now.Millisecond, scoretagtest, (Rank r) => {
      Debug.Log ("Updated leaderboard with scoretags. New rank " + r.Ranking + " for " + r.Name + " with tags " + r.Scoretags.ToString ());
    }, failure);
    
    session.LeaderboardAndRank (leaderboardId, (LeaderboardAndRank lr) => {
      Debug.Log ("1-Retrieved Leaderboard and Rank: " + lr.Leaderboard.Name);
      Debug.Log ("1-Retrieved Leaderboard and Rank: " + lr.Rank.Name + " " + lr.Rank.Ranking);
    }, failure);
    
    session.LeaderboardAndRank (leaderboardId, 10, (LeaderboardAndRank lr) => {
      Debug.Log ("2-Retrieved Leaderboard Entries count:  " + lr.Leaderboard.Entries.Length);
      Debug.Log ("2-Retrieved Leaderboard and Rank: " + lr.Leaderboard.Name);
      Debug.Log ("2-Retrieved Leaderboard and Rank: " + lr.Rank.Name);
      if (lr.Rank.Scoretags != null) {
        Debug.Log ("2-ScoreTags: " + lr.Rank.Scoretags.ToString ());
      }
    }, failure);
    
    session.LeaderboardAndRank (leaderboardId, 10, 20, (LeaderboardAndRank lr) => {
      Debug.Log ("3-Retrieved Leaderboard Entries count:  " + lr.Leaderboard.Entries.Length);
      Debug.Log ("3-Retrieved Leaderboard and Rank: " + lr.Leaderboard.Name);
      Debug.Log ("3-Retrieved Leaderboard and Rank: " + lr.Rank.Name);
      if (lr.Rank.Scoretags != null) {
        Debug.Log ("3-ScoreTags: " + lr.Rank.Scoretags.ToString ());
      }
    }, failure);
    
    session.StorageDelete (shared_storage_key, () => {
      Debug.Log ("Deleted shared storage: " + shared_storage_key);
    }, failure);
    
    Dictionary<string, object> armyData = new Dictionary<string, object> ();
    armyData.Add ("soldiers", 1000);
    armyData.Add ("tombstones", 10);
    
    session.SharedStoragePut (shared_storage_key, armyData, () => {
      Debug.Log ("Stored shared data: " + shared_storage_key);
      
      session.SharedStorageGet (shared_storage_key, (SharedStorageObject sso) => {
        Debug.Log ("Retrieved shared storage: " + sso.ConvertPublic ());
        
        armyData.Remove ("soldiers");
        armyData.Add ("soldiers", 1);
        session.SharedStorageUpdate (shared_storage_key, armyData, () => {
          Debug.Log ("Updated shared data: " + shared_storage_key);
          
          session.SharedStorageSearchGet ("value.tombstones > 5", (SharedStorageSearchResults results) => {
            Debug.Log ("Searched shared storage: " + results.Count);
            
            foreach (SharedStorageObject result in results) {
              Debug.Log ("Shared Storage Object: " + result.ConvertPublic ());
            }
          }, failure);
        }, failure);
      }, failure);
    }, failure);
    
    IDictionary<string, object> scriptData = new Dictionary<string, object> ();
    scriptData.Add ("a", 1);
    scriptData.Add ("b", 2);
    session.executeScript (scriptId, scriptData, (IDictionary<string, object> response) => {
      Debug.Log ("Executed script with result:" + GameUp.SimpleJson.SerializeObject (response));
    }, failure);

    session.executeScript (mailboxScriptId, scriptData, (IDictionary<string, object> response) => {
      Debug.Log ("Executed mailbox");
      session.MessageList (false, (MessageList ms) => {
        Debug.Log ("Got " + ms.Count + " messages back");
        IEnumerator<Message> messages = ms.GetEnumerator ();
        messages.MoveNext ();
        session.MessageGet (messages.Current.MessageId, true, (Message message) => {
          Debug.Log ("Got message with ID" + message.MessageId + " with subject: " + message.Subject);
          session.MessageDelete (messages.Current.MessageId, () => {
            Debug.Log ("Deleted message with ID" + message.MessageId);
          }, failure);
        }, failure);
      }, failure);
    }, failure);

  

    #if UNITY_IOS
    UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |  UnityEngine.iOS.NotificationType.Badge |  UnityEngine.iOS.NotificationType.Sound);
    #endif
  }

  void testMatch (Gamer gamer)
  {
    session.GetAllMatches ((MatchList matches) => {
      Debug.Log ("Retrieved Matches. Size: " + matches.Count);
    }, failure);
    
    List<String> matchFilters = new List<string> ();
    matchFilters.Add ("device=unity");
    matchFilters.Add ("rank=7");
    session.CreateMatch (2, matchFilters, (Match match) => {
      Debug.Log ("New match created. Match ID: " + match.MatchId);
      String matchId = match.MatchId;

      session.GetMatch (matchId, (Match newMatch) => {
        Debug.Log ("Got match details. Match turn count: " + newMatch.TurnCount);
      }, failure);
      
      if (match.TurnGamerId.Equals (gamer.GamerId)) {
        Debug.Log ("Match details : " + match.MatchId + ". Submitting a turn for " + match.WhoamiGamerId);
        IDictionary turnData = new Dictionary<string, string> {{"turndata", "Unity SDK Turn Data"}};
        session.SubmitTurnGamerId (matchId, (int)match.TurnCount, gamer.GamerId, turnData, () => {
          session.GetTurnData (matchId, 0, (MatchTurnList turns) => {
            Debug.Log ("Got Turns. Count is: " + turns.Count);
            foreach (MatchTurn matchTurn in turns) {
              // we can update the match state to sync it with the most recent turns
              Debug.LogFormat ("User '{0}' played turn number '{1}'.", matchTurn.Gamer, matchTurn.TurnNumber);
              Debug.LogFormat ("Turn data: '{0}'.", matchTurn.Data);
            }
            session.EndMatch (matchId, (String id) => {
              Debug.Log ("Match ended: " + id);
            }, failure);
          }, failure);
        }, failure);
      } else {
        session.LeaveMatch (matchId, (String id) => {
          Debug.Log ("Left match: " + id);
        }, failure);
      }
      
    }, () => {
      Debug.Log ("Gamer queued");
    }, failure);

    //instead of '0' use Match.UpdatedAt ...
    session.GetChangedMatches (0, (MatchChangeList list) => {
      Debug.Log (list.Count + " matches have changed.");
    }, failure);
  }

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

class ScoretagTest
{
  public long Datetime { get ; set ; }
}
