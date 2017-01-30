using UnityEngine;
using System.Collections;

public class BattleSpriteAction : MonoBehaviour{

	int JumpFlg = 0;
	GameObject Cam;
	Vector3 Pos;
	Vector3 Uni;
	private GameObject Toko;

	static int hashSpeed = Animator.StringToHash ("Speed");
	static int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	static int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	static int hashIsCrouch = Animator.StringToHash ("IsCrouch");
	static int hashAttack1 = Animator.StringToHash ("Attack1");
	static int hashAttack2 = Animator.StringToHash ("Attack2");
	static int hashAttack3 = Animator.StringToHash ("Attack3");


	static int hashDamage = Animator.StringToHash ("Damage");
	static int hashIsDead = Animator.StringToHash ("IsDead");

	[SerializeField] private float characterHeightOffset = 0.2f;

	//特定のレイヤーとの当たり判定をとるために使う｡今回は地面
	[SerializeField] LayerMask groundMask;


	[SerializeField, HideInInspector] Animator animator;
	[SerializeField, HideInInspector]SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector]Rigidbody2D rig2d;

	public int hp = 4;

	public Vector2 SPEED = new Vector2(0.05f, 0.05f);
	private int moveflg = 0;
	private int attackflg = 0;

	void Awake ()
	{
		Toko = GameObject.Find ("Toko");
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();
		Cam = GameObject.FindWithTag ("MainCamera");
		Pos.y = 0.8f;
	}

	void Update (){
		//unityちゃんのちょっと上にカメラを固定して追従
		Uni = transform.position + Pos;
		Cam.transform.position = new Vector3(transform.position.x,Uni.y,-15);
        float axis = Input.GetAxisRaw ("Horizontal");
		bool isDown = Input.GetAxisRaw ("Vertical") < 0;


		/* 
		  Raycastで地面とのあたり判定をとっている､わかりやすく文章にするなら､
		  Raycast(自身のポジションの,真下に,1(｢真下｣の具体的な数値)ぶん,groundMaskレイヤー)
		  が存在するか･･･って感じ｡
		  戻り値は、地面に触れていたらtrue違うならfalse
		*/
		var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 1, groundMask);

		// update animator parameters
		animator.SetBool (hashIsCrouch, isDown);
		animator.SetFloat (hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetFloat (hashSpeed, Mathf.Abs (axis));


		/*
		  引数axisはmove関数の中のunityちゃんの振り向きを制御しているspriteRenderer.flip.xに
		  必要な引数｡Update関数の中で宣言されたローカル変数のaxisはUpdate関数でしか使えないので
		  それを引数としてmove関数に渡してあげて処理している
		  
		  引数distanceFromGroundはRaycastの戻り値
		*/
		Move(axis,distanceFromGround);


		if (moveflg == 0 || moveflg == 1) {
			if (attackflg == 0) {
				if (Input.GetKeyDown (KeyCode.Z)) animator.SetTrigger (hashAttack1);
				if (Input.GetKeyDown (KeyCode.X)) animator.SetTrigger (hashAttack2);
				if (Input.GetKeyDown (KeyCode.C)) animator.SetTrigger (hashAttack3);
			}
		}



		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("SpecialAttack") == true ||
			animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack1") == true ||
			animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack2") == true) {
			attackflg = 1;
		} else attackflg = 0;


	}

	void Move(float Axis,bool DFGround){

		if (attackflg == 0) {
			// 現在位置をPositionに代入
			Vector2 Position = transform.position;

			// 左キーを押し続けていたら
			if (Input.GetKey (KeyCode.A)) {
				// 代入したPositionに対して加算減算を行う
				Position.x -= SPEED.x;
				moveflg = 1; 
			}

			// 右キーを押し続けていたら
			if (Input.GetKey (KeyCode.D)) {
				// 代入したPositionに対して加算減算を行う
				Position.x += SPEED.x;
				moveflg = 1;
			}

			// 現在の位置に加算減算を行ったPositionを代入する
			transform.position = Position;

			if (Input.GetButtonDown ("Jump") && (JumpFlg < 1)){
				JumpFlg += 1;
				rig2d.velocity = new Vector2 (rig2d.velocity.x, 5);
			}
			// flip sprite
			if (Axis != 0) spriteRenderer.flipX = Axis < 0;
			//地面に触れている間はジャンプフラグ0
			if (DFGround) JumpFlg = 0;
		}

	}


	//ゴールとの当たり判定
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag ("Goal") == true) Debug.Log ("hit");
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.CompareTag ("Toko")) Toko.SendMessage ("ChangeFlg");
	}

}
