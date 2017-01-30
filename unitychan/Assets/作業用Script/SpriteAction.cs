using UnityEngine;
using System.Collections;

public class SpriteAction : MonoBehaviour
{

	//このクラスは戦闘モーションがないキャラ用に継承させるクラス

	protected int hashSpeed = Animator.StringToHash ("Speed");
	protected int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	protected int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	protected int hashIsCrouch = Animator.StringToHash ("IsCrouch");

	protected int hashDamage = Animator.StringToHash ("Damage");
	protected int hashIsDead = Animator.StringToHash ("IsDead");

	[SerializeField] private float characterHeightOffset = 0.2f;
	[SerializeField] private LayerMask groundMask;

	[SerializeField, HideInInspector] protected Animator animator;
	[SerializeField, HideInInspector] protected SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector] protected Rigidbody2D rig2d;

	[SerializeField] private int hp = 4;

	// sbyte 範囲　-128～127
	protected sbyte axis = 0;
	protected bool isDown = false;

	protected void Awake (){
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();
	}

	protected void SpriteAnim (){
		
		var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 1, groundMask);

		// update animator parameters
		animator.SetBool (hashIsCrouch, isDown);
		animator.SetFloat (hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetFloat (hashSpeed, Mathf.Abs (axis));

		//rig2d.velocity = new Vector2 (rig2d.velocity.x, 5);

	}

}
