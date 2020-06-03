using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Arena : NetworkBehaviour
{
    public class ArenaState : StateMachine.State
    {
        protected Arena _context;
        public ArenaState(Arena context) { _context = context; }
    }

    public class PregameState : ArenaState
    {
        public PregameState(Arena context) : base(context) { }

        public override void Enter()
        {
            GameEvents.TriggerUpdateCountdownStatus("Waiting for combatant!");
        }
    }

    public class CountdownState : ArenaState
    {
        public CountdownState(Arena context) : base(context) { }

        float _stateTime;
        float _nextCountdownUpdate;

        public override void Enter()
        {
            _stateTime = 5.0f;
            GameEvents.TriggerUpdateCountdownStatus(((int)_stateTime).ToString());
            _nextCountdownUpdate = 4.0f;
        }

        public override void Update()
        {
            if (_context.isServer && _stateTime <= 0.0f)
            {
                _context.RpcStartGame();
                return;
            }

            _stateTime -= Time.deltaTime;

            if(_stateTime < _nextCountdownUpdate)
            {
                GameEvents.TriggerUpdateCountdownStatus(((int)_stateTime).ToString());
                _nextCountdownUpdate = _stateTime - 1.0f;
            }
        }

        public override void Exit()
        {
            GameEvents.TriggerUpdateCountdownStatus("");
        }
    }

    public class PlayState : ArenaState
    {
        public PlayState(Arena context) : base(context) { }

        public override void Enter()
        {
            GameEvents.TriggerGameStarted();
        }
    }

    public static Arena Instance;

    public CommandSpawner CommandSpawner;
	public GameObject MechPrefab;
	public Transform[] MechSpawnPoints;

    private StateMachine _stateMachine;
    private List<Combatant> _combatants = new List<Combatant>();
    private Mech[] _mechs = new Mech[PLAYER_COUNT];

    static int PLAYER_COUNT = 2;

	void Awake()
	{
		Instance = this;
        InitializeStateMachine();
    }

    private void OnEnable()
    {
        GameEvents.OnExecuteCommand += GameEvents_OnExecuteCommand;
    }

    private void OnDisable()
    {
        GameEvents.OnExecuteCommand -= GameEvents_OnExecuteCommand;
    }

    void Update ()
	{
        _stateMachine.Update();
	}

    public bool GameStarted { get { return _stateMachine.CurrentState.GetType() == typeof(PlayState); } }

    private void InitializeStateMachine()
    {
        if (_stateMachine != null)
            return;

        _stateMachine = new StateMachine();
        _stateMachine.AddState(new PregameState(this));
        _stateMachine.AddState(new CountdownState(this));
        _stateMachine.AddState(new PlayState(this));
        _stateMachine.ChangeState<PregameState>();
    }

	public void ServerOnCombatantJoined(Combatant combatant)
	{
        _combatants.Add(combatant);
		RpcSpawnMechs(_combatants.Count);

        if (_combatants.Count == PLAYER_COUNT)
        {
            RpcStartCountdown();
        }
	}

	[ClientRpc]
	private void RpcSpawnMechs(int playerCount)
	{
        for (int i = 0; i < playerCount; i++)
        {
            if (_mechs[i] != null)
                continue;

            GameObject go = Instantiate(MechPrefab, MechSpawnPoints[i].position, MechSpawnPoints[i].rotation);
            go.transform.position = MechSpawnPoints[i].position;

            _mechs[i] = go.GetComponent<Mech>();
        }
	}

    [ClientRpc]
    private void RpcStartCountdown()
    {
        InitializeStateMachine();
        _stateMachine.ChangeState<CountdownState>();
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        _stateMachine.ChangeState<PlayState>();
    }

    [ClientRpc]
    private void RpcExecuteCommand(int mechIndex, Command.CommandType type)
    {
        _mechs[mechIndex].ExecuteCommand(type);
    }

    private void GameEvents_OnExecuteCommand(int mechIndex, Command.CommandType type, int id)
    {
        if(isServer)
        {
            RpcExecuteCommand(mechIndex, type);
            CommandSpawner.ServerRemoveCommand(id);
        }
    }
}
