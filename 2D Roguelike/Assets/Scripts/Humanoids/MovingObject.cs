using System.Collections;
using UnityEngine;

/// <summary>
/// Moving object.
/// 
/// </summary>

public abstract class MovingObject : MonoBehaviour {

	//Time to move Object in Seconds
	public float moveTime = 0.1f;
	// Mask of Layer on which Collision is Detected
	public LayerMask blockingLayer;

	// Refrence to BoxCollider to trigger Collision
	private BoxCollider2D boxCollider2D;
	// Reference Rigidbody of Object to move
	private Rigidbody2D rigid2D;
	// Float used for MovementCalculation
	private float inverseMoveTime;

	// Use this for initialization (Can be overriden by InheritingClass)
	protected virtual void Start () {
		// Find BoxCollider Reference
		boxCollider2D = GetComponent <BoxCollider2D> ();
		// Find Rigidbody2D
		rigid2D = GetComponent <Rigidbody2D> ();
		// Set InverMoveTime
		inverseMoveTime = 1f / moveTime;
	}


	// Function to Check if Object can Move in Direction (X and Y Direction and RayCast to Check Collision)
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit) {
		// Start of Check (Current position)
		Vector2 start = transform.position;
		// Calculate EndPosition based on given X and Y Direction
		Vector2 end = start + new Vector2 (xDir, yDir);

		// Disable BoxCollider, so RayCast doesn't hit own Collider
		boxCollider2D.enabled = false;
		// Cast RayCastLine from Startpoint to Endpoint and Store Collider (on BlockingLayer) to Hit
		hit = Physics2D.Linecast (start, end, blockingLayer);
		// Enable BoxCollider
		boxCollider2D.enabled = true;
		// Check if OpenTile was Hit
		if (hit.transform == null) {
			// Move toward OpenTile
			StartCoroutine (SmoothMovement (end) );
			// Move was Possible
			return true;
		}
		// Otherwise if Something was Hit (No OpenTile) Move was Not Possible
		return false;
	}


	// Function to Attempt to Move in given Direction
	protected virtual void AttemptMove <T> (int xDir, int ydir) where T : Component {
		// RayCast to Check Collision
		RaycastHit2D hit;
		// Can the Player Move
		bool canMove = Move (xDir, ydir, out hit);
		// If OpenTile was Hit
		if (hit.transform == null) {
			// Dont do anything (movement is done in SmoothMovement)
			return;
		}
		// Otherwise: set the HitComponent to the Object that was Hit
		T hitComponent = hit.transform.GetComponent<T> ();
		// If the Player is Blocked and Hit something to Interact with
		if (!canMove && hitComponent != null) {
			// Call OnCantMove and give HitComponent
			OnCantMove (hitComponent);
		}
	}


	// Function to Smoothly move Object towards given End
	protected IEnumerator SmoothMovement (Vector3 end) {
		// Units between Current and End Position
		float sqrRemaining = (transform.position - end).sqrMagnitude;
		// While Square Remaining Distance is Smaller than Nearly Nothing
		while (sqrRemaining > float.Epsilon) {
			// New Position based on StraightLine toward TargetPoint, with Speed
			Vector3 newPosition = Vector3.MoveTowards (rigid2D.position, end, inverseMoveTime * Time.deltaTime);
			// Move the Rigidbody to new Position
			rigid2D.MovePosition (newPosition);
			// Recalculate the Remaining Distance, after movement
			sqrRemaining = (transform.position - end).sqrMagnitude;
			// Wait a Frame, before Re-Check of Loop
			yield return null;
		}
	}


	// Function if Object Cant Move in Direction 
	protected abstract void OnCantMove <T> (T component) where T : Component;
}
