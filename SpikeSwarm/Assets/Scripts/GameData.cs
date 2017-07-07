using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
	[System.Serializable]
	public class CommandIconSpriteInfo
	{
		public Command.CommandType Type;
		public Color BackgroundColor;
		public Sprite Icon;
	};

	public CommandIconSpriteInfo[] CommandIconSpriteInfos;
}
