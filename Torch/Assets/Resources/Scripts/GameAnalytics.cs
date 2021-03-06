using System;
using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class GameAnalytics {

	public GameAnalytics (){

	}

	public void WriteStart(){
		string datetime = DateTime.UtcNow.ToString();
		string time = "\"" + datetime + "\"";
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "\"Event\"", "\"StartGame\""},
			{ "\"Time\"", time}
		};
		WriteToFile (eventData);
	}

	public void WriteTorchPickup(int dungeonLevel, float StartTime){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{"\"Event\"", "\"TorchPickup\""},
			{ "\"Level\"", dungeonLevel},
			{"\"Time\"", Time.time - StartTime}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("TorchPickup", eventData);
		WriteToFile (eventData);
	}

	public void WriteFinishLevel(int dungeonLevel, int levelScore, int totalScore, float StartTime){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "\"Event\"", "\"FinishLevel\""},
			{ "\"Level\"", dungeonLevel},
			{ "\"LevelScore\"", levelScore },
			{ "\"Score\"", totalScore },
			{ "\"LevelTime\"", Time.time - StartTime},
			{ "\"Time\"", Time.time}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("LevelComplete", eventData);
		WriteToFile (eventData);
	}

	public void WriteDeath(int totalScore, int dungeonLevel){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "\"Event\"", "\"Death\"" },
			{ "\"Score\"", totalScore },
			{ "\"Level\"", dungeonLevel},
			{ "\"Time\"", Time.time}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("Death", eventData);
		WriteToFile (eventData);

	}

	public void WritePuzzleStart(string RoomType, int dungeonLevel){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "\"Event\"", "\"PuzzleStart\"" },
			{ "\"Puzzle\"", "\"" + RoomType + "\""},
			{ "\"Level\"", dungeonLevel}
		};
		WriteToFile (eventData);
	}

	public void WritePuzzleComplete(string RoomType, float Time, int dungeonLevel){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "\"Event\"", "\"PuzzleComplete\"" },
			{ "\"Puzzle\"", "\"" + RoomType + "\"" },
			{"\"Time\"", Time},
			{ "\"Level\"", dungeonLevel}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("PuzzleComplete", eventData);
		WriteToFile (eventData);
	}

	public void WriteToFile(Dictionary<string, object> dict){
		int counter = 0;
		using (System.IO.StreamWriter file = new System.IO.StreamWriter ("data.txt", true)) {
			file.WriteLine("{");
			foreach (KeyValuePair<string, object> entry in dict) {
				if (counter == 0) {
					file.Write (entry.Key + ":" + entry.Value);
				} else {
					file.WriteLine (",");
					file.Write (entry.Key + ":" + entry.Value);	
				}
				counter++;
			}
			file.WriteLine ();
			file.WriteLine ("},");
		}
	}
}


