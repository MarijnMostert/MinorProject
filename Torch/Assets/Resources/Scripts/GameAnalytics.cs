using System;
using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class GameAnalytics {

	public GameAnalytics (){

	}

	public void WriteStart(){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "Event", "StartGame"},
			{ "Time", DateTime.UtcNow}
		};
		WriteToFile (eventData);
	}

	public void WriteTorchPickup(){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{"Event", "TorchPickup"},
			{"Time", Time.time}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("TorchPickup", eventData);
		WriteToFile (eventData);
	}

	public void WriteFinishLevel(int dungeonLevel, int score, float StartTime){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "Event", "FinishLevel"},
			{ "Level", dungeonLevel},
			{ "LevelScore", score },
			{ "TimeSpent", Time.time - StartTime}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("LevelComplete", eventData);
		WriteToFile (eventData);
	}

	public void WriteDeath(int totalScore, int dungeonLevel){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "Event", "Death" },
			{ "Score", totalScore },
			{ "Level", dungeonLevel},
			{ "TotalTime", Time.time}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("Death", eventData);
		WriteToFile (eventData);

	}

	public void WriteToFile(Dictionary<string, object> dict){
		using (System.IO.StreamWriter file = new System.IO.StreamWriter ("data.txt", true)) {
			file.WriteLine ("{");
			foreach (KeyValuePair<string, object> entry in dict) {
				file.WriteLine (entry.Key + ":" + entry.Value);	
			}
			file.WriteLine ("}");
		}
	}
}


