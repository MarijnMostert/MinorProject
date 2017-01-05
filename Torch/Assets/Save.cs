using System;
using UnityEngine;
using System.IO;

public class Save {
	//FileStream fileStream;

	public Save (){
		//fileStream = new FileStream(@"save.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
	}

	public int Read(){
//		using (System.IO.StreamReader reader = new System.IO.StreamReader ("save.txt", true)) {
//			try {
//				String line = reader.ReadLine ();
//				int level = Int32.Parse (line);
//				reader.Close();
//				return level;
//			} catch (Exception e){
//				Debug.Log ("reading save file failed");
//				reader.Close ();
//				return 0;
//			}
//		}
		string temp = System.IO.File.ReadAllText("save.txt");
		int level = Int32.Parse (temp);
		Debug.Log ("Dungeon level: " + level);
		return level;
	}


	public void ToFile(int level){
//		using (System.IO.StreamWriter file = new System.IO.StreamWriter ("save.txt", true)) {
//			System.IO.File.WriteAllText ("save.txt", String.Empty);
//			file.WriteLine (level.ToString());
//			file.Close ();
//		}
		System.IO.File.WriteAllText ("save.txt", String.Empty);
		System.IO.File.WriteAllText ("save.txt", level.ToString ());
	}



}


