using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// movement config
	public float gravity = -25f;
	public float runSpeed = 4f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 1f;

	//private Animator _animator;
	private CharacterController2D _controller;

	private Vector3 _velocity;
	private float _normalizedHorizontalSpeed = 0;

	// input
	private bool _right;
	private bool _left;
	private bool _up;

	void Awake()
	{
		//_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		
//		// Find out how many fag packets there are to collect for this level.
//		numFags = GameObject.FindGameObjectsWithTag("Fags").Length;
	}

	#region Event Listeners
	
	void onControllerCollider( CharacterController2D.RayWithVelocityInfo hit )
	{
	}

	void onTriggerEnterEvent( Collider2D col )
	{
	}

	void onTriggerExitEvent( Collider2D col )
	{
	}

	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// the Update loop only gathers input. Actual movement is handled in FixedUpdate because we are using the Physics system for movement
	void Update()
	{
		// a minor bit of trickery here. FixedUpdate sets _up to false so to ensure we never miss any jump presses we leave _up
		// set to true if it was true the previous frame
		_up = _up || Input.GetKey( KeyCode.UpArrow ) ;//|| Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space);
		_right = Input.GetKey( KeyCode.RightArrow );
		_left = Input.GetKey( KeyCode.LeftArrow );

		foreach (Touch touch in Input.touches){
			if (touch.phase == TouchPhase.Began){
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
				if (hit.collider != null) {
					if (hit.collider.gameObject.name == "left_arrow_64x64") {
						_left = true;
					} else if (hit.collider.gameObject.name == "right_arrow_64x64") {
						_right = true;
					} else if (hit.collider.gameObject.name == "up_arrow_64x64") {
						_up = true;
					}
				}
			}
		}

		// Only if keys aren't being used, check joysticks etc.
		if (_right == false) {
			_right = Input.GetAxis("Horizontal") > 0;
		}
		if (_left == false) {
			_left = Input.GetAxis("Horizontal") < 0;
		}
	}

	void FixedUpdate()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;

		if( _controller.isGrounded ) {
			_velocity.y = 0;
		}

		if( _left )
		{
			_normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f ) {
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			}
		} else if( _right )
		{
			_normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f ) {
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			}
		} else {
			_normalizedHorizontalSpeed = 0;
		}

		// we can only jump whilst grounded
		if( _controller.isGrounded && _up )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
		}

		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, _normalizedHorizontalSpeed * runSpeed, Time.fixedDeltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.fixedDeltaTime;
		
		_controller.move( _velocity * Time.fixedDeltaTime );
		
		// reset input
		_up = false;
	}
}
