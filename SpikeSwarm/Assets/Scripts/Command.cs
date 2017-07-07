using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
	public enum CommandType
	{
		DIR_UP,
		DIR_DOWN,
		DIR_LEFT,
		DIR_RIGHT,
		BULLET,
		MINE,
		MINIBOT,
		STOP,
		MAX,
	};

	public SpriteRenderer Background;
	public SpriteRenderer Icon;

	public void Setup(Color bgColor, Sprite icon)
	{
		Background.color = bgColor;
		Icon.sprite = icon;
	}
}
