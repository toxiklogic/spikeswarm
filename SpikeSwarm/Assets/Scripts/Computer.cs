using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public Sprite[] Frames;
    public int MechIndex;

    private SpriteRenderer _spriteRenderer;
    private int _currentFrame;
    private float _frameTime;
    private float _frameDuration = 0.3f;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _currentFrame = 0;
    }

    private void Update()
    {
        _frameTime += Time.deltaTime;
        if(_frameTime > _frameDuration)
        {
            _spriteRenderer.sprite = Frames[_currentFrame++];
            if (_currentFrame == Frames.Length)
                _currentFrame = 0;
            _frameTime = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Command")
            return;

        Command command = other.gameObject.GetComponent<Command>();
        GameEvents.TriggerExecuteCommand(MechIndex, command.Type, command.UniqueId);
    }
}
