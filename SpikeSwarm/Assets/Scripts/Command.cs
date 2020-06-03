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
	public int UniqueId;
    public AnimationCurve DropCurve;
    public float DropDuration;

    private CommandType _commandType;
    private bool _dropping;
    private float _dropTime;
    private float _dropStartY;

    public CommandType Type { get { return _commandType; } }

	public void Setup(Color bgColor, Sprite icon, int uniqueId, CommandType type)
	{
		Background.color = bgColor;
		Icon.sprite = icon;
		UniqueId = uniqueId;
        _commandType = type;
    }

    public void Drop()
    {
        _dropping = true;
        _dropTime = 0.0f;
        _dropStartY = transform.position.y;
    }

    private void Update()
    {
        if(_dropping)
        {
            _dropTime += Time.deltaTime;
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(0.0f, _dropStartY, _dropStartY * DropCurve.Evaluate(_dropTime / DropDuration));
            transform.position = position;

            if (_dropTime >= DropDuration)
                _dropping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Combatant combatant = other.GetComponentInParent<Combatant>();
            combatant.OnEnterCommandPickup(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Combatant combatant = other.GetComponentInParent<Combatant>();
            combatant.OnExitCommandPickup(this);
        }
    }
}
