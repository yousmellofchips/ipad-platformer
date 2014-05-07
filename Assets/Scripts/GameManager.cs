using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager - handles the state and flow control for the entire game.
/// </summary>
public class GameManager : MonoBehaviour {

	/// <summary>
	/// The one and only instance of the game manager.
	/// </summary>
	/// <value>The instance.</value>
	public static GameManager Instance {
		get {
			if (_instance == null) {
				GameObject singleton = new GameObject ();
				_instance = singleton.AddComponent<GameManager> ();
				singleton.name = "(singleton) " + typeof(GameManager).ToString ();
				
				DontDestroyOnLoad (singleton);
			}
			return _instance;
		}
	}

	// Use this for initialization.
	void Start () {
		_gameState = GameState.Init;
	}
	
	// Update is called once per frame.
	void Update () {
		if (_debugPrevState != _gameState)
		{
			Debug.Log("Entered GameState: " + _gameState.ToString());
			_debugPrevState = _gameState;
		}

		switch (_gameState) {
		case GameState.Init:
			InitGame();
			break;
		case GameState.TitleScreen:
			TitleScreen();
			break;
		case GameState.StartGame:
			StartGame();
			break;
		case GameState.InitLevel:
			InitLevel();
			break;
		case GameState.InGame:
			InGame();
			break;
		case GameState.LevelCompleted:
			LevelCompleted();
			break;
		case GameState.LoseLife:
			LoseLife();
			break;
		case GameState.GameOver:
			GameOver();
			break;
		case GameState.GameCompleted:
			GameCompleted();
			break;
		case GameState.HiScores:
			HiScores();
			break;
		case GameState.Credits:
			Credits();
			break;
		}
	}

	void InitGame ()
	{
		// TODO: any "entire game" initial set up required.
		if (Application.loadedLevelName != "Startup") {
			Application.LoadLevel("Startup");
		}

		_gameState = GameState.TitleScreen;
	}

	void TitleScreen ()
	{
		// TODO: show fancy title intro and message saying "press any key to start".
		//		 if required, after timeout (or whatever), transition to HiScores state.
		if (Application.loadedLevelName != "TitleScreen") {
			Application.LoadLevel("TitleScreen");
		}

		if (Input.anyKey) {
			_gameState = GameState.StartGame;
		}
	}

	void StartGame ()
	{
		// TODO: set initial level, player state, etc.
		_currentLives = kInitialLives;
		_currentLevel = kStartingLevel;

		_gameState = GameState.InitLevel;
	}

	void InitLevel ()
	{
		Application.LoadLevel(_currentLevel);

		// TODO: set current level to default state, as required.

		_gameState = GameState.InGame;
	}

	void InGame ()
	{
		// TODO: main gameplay loop.

		// TODO: choose exit state transition from one of the following three states:-
		//_gameState = GameState.LoseLife;
		//_gameState = GameState.LevelCompleted;
		//_gameState = GameState.GameCompleted;
	}

	void LevelCompleted ()
	{
		// TODO: fancy animation, jingle, fireworks, etc.
		//		 once that's done, increase level and transition to state below.

		if (_currentLevel == kFinalLevel) {
			_gameState = GameState.GameCompleted;
		} else {
			_currentLevel++;
			_gameState = GameState.InitLevel;
		}
	}

	void LoseLife ()
	{
		// TODO: life losing animation, sequence, sounds, etc.
		//		 once that is done with, proceed to state change below.

		if (--_currentLives >= 0) { // equal to, so as to "play" the final life before dying.
			_gameState = GameState.InitLevel;
		} else {
			_gameState = GameState.GameOver;
		}
	}

	void GameOver ()
	{
		// TODO: Game Over message, music, animation, etc.
		//		 once that is done with, or timeout occurs, transition to state below.

		_gameState = GameState.Init;
	}

	void GameCompleted ()
	{
		// TODO: Game completion sequence, music, etc.
		//		 once that is done with, timed out or user bypasses, transition to state below.
		if (Application.loadedLevelName != "GameCompleted") {
			Application.LoadLevel("GameCompleted");
		}

		_gameState = GameState.Init; // or credits.
	}

	void HiScores ()
	{
		// TODO: present flashy hi scores tables (or equivalent, rankings, time completed, level reached, number of attempts, etc.)
		//		 once that is done, transition to title screen.
		if (Application.loadedLevelName != "HiScores") {
			Application.LoadLevel("HiScores");
		}

		_gameState = GameState.TitleScreen;
	}

	void Credits ()
	{
		// TODO: present credits
		//		 once that is done, user bypasses, etc. then transition to game init.
		if (Application.loadedLevelName != "Credits") {
			Application.LoadLevel("Credits");
		}

		_gameState = GameState.Init;
	}

	// Private data
	private const int kInitialLives = 4;	// starting number of lives.
	private const int kStartingLevel = 5;	// starting level.
	private const int kFinalLevel = 5;		// last level, after which GameCompleted state is entered.

	private static GameManager _instance = null;

	private GameState _gameState = GameState.Init;
	private GameState _debugPrevState = GameState.GameOver; // for debugging state messages
	private int _currentLives = 0;
	private int _currentLevel = 0;
}
