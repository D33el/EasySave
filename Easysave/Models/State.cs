using System;
namespace EasySave.Models
{
	public class State
	{
		//private State stateInstance = new State() ;
		public string saveId;
		public bool saveState;
		public int saveFilesNumber;
		public long saveSize;
		public string[] saveProgress;

		public State(string SaveId, bool SaveState, int SaveFilesNumber, long SaveSize, string[] SaveProgress)
		{
			saveId = SaveId;
            saveState = SaveState;
			saveFilesNumber = SaveFilesNumber;
            saveSize = SaveSize;
			saveProgress = SaveProgress;

        }
		//public State GetState()
		//{
		//	return new State();
		//}

		public void UpdateState()
		{

		}

	}
}

