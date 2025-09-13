using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Dev-Controlled variables
	[SerializeField] private float speed;
	

	// Movement Variables
	private float movement;
	private Vector2 moveDir;
	
	// Necessary components
	private Rigidbody2D rb;
	private Collider2D col;
	PlayerControls inputs;
	
	// Screen related variables
	private Vector2 screenBounds;
	private float objectWidth;

	void Awake()
	{
		inputs = new PlayerControls();
		
		inputs.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
		inputs.Player.Movement.canceled += ctx => movement = ctx.ReadValue<float>();
	}
	
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		objectWidth = col.bounds.size.x / 2f;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!GameManager.Instance.GameInProgress) return;
		
		moveDir.x = movement;
		rb.AddForce(moveDir * speed * Time.deltaTime);
	}

	void LateUpdate()
	{	
		Vector3 viewPos = transform.position;
		viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
		transform.position = viewPos;
	}

	

	#region Enable/Disable Stuff


	
	void OnEnable()
	{
		inputs.Enable();
	}

	void OnDisable()
	{
		inputs.Disable();
	}
	
	#endregion
}
