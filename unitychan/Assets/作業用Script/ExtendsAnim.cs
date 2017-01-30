using UnityEngine;
using System.Collections;

public class ExtendsAnim : MonoBehaviour {

	/*
	 * このExtendsクラスでは各キャラ共通のアニメーション・・
	 * 走る・落ちる・しゃがむ・攻撃・・etc
	 * をまとめたもの
	 */

	protected int hashSpeed = Animator.StringToHash ("Speed");
	protected int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	protected int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	protected int hashIsCrouch = Animator.StringToHash ("IsCrouch");
	protected int hashAttack1 = Animator.StringToHash ("Attack1");
	protected int hashAttack2 = Animator.StringToHash ("Attack2");
	protected int hashAttack3 = Animator.StringToHash ("Attack3");

	protected int hashDamage = Animator.StringToHash ("Damage");
	protected int hashIsDead = Animator.StringToHash ("IsDead");

	[SerializeField] protected float characterHeightOffset = 0.2f;

	//特定のレイヤーとの当たり判定をとるために使う｡今回は地面
	[SerializeField] protected LayerMask groundMask;


	protected Animator animator;
	protected SpriteRenderer spriteRenderer;
	protected Rigidbody2D rig2d;

	protected float axis;
	protected bool isDown;

	// Use this for initialization
	protected void Start () {
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame


	// Unityちゃん用のアニメーション制御関数

	protected void UnityAnim(float Axis,bool Down){
		var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 1, groundMask);

		// update animator parameters
		animator.SetBool (hashIsCrouch, isDown);
		animator.SetFloat(hashGroundDistance,distanceFromGround.distance == 0 ? 99 :distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetFloat (hashSpeed, Mathf.Abs (axis));
	}

	protected IEnumerator Walk(){
		bool Toko_Walk = false;
		animator.SetFloat (hashSpeed, 1f);//パラメーターに数値をセット
		for (int i = 0; i <= 100; i++) {
			if (!Toko_Walk) transform.Translate (Vector3.right * 1 * Time.deltaTime);
			else      transform.Translate (Vector3.left * 1 * Time.deltaTime);
			yield return null;
			if (i == 100){
				if (Toko_Walk) {
					Toko_Walk = false;
					i = 0;
					spriteRenderer.flipX = false;//反転
				} else {
					Toko_Walk = true;
					i = 0;
					spriteRenderer.flipX = true;//反転
				}
			}
		}
	}

	//こっちのwalkは徘徊途中でジャンプさせる｡引数にジャンプパワーを設定
	protected IEnumerator Walk(sbyte JmpPower){
		animator.SetFloat (hashSpeed, 1f);//パラメーターに数値をセット
		bool Cindy_Walk = false;
		for (int i = 0; i <= 100; i++) {
			if (!Cindy_Walk) rig2d.velocity = Vector2.right;
			else          rig2d.velocity = Vector2.left;
			yield return null;
			if (i == 100){
				if (Cindy_Walk) {
					rig2d.velocity = new Vector2(rig2d.velocity.x,JmpPower);
					animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
					Cindy_Walk = false;
					i = 0;
					spriteRenderer.flipX = false;//反転
				} else {
					Cindy_Walk = true;
					i = 0;
					spriteRenderer.flipX = true;//反転
				}
			}
		}
	}

}
