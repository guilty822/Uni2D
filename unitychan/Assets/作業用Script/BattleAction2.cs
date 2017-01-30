using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleAction2 : ExtendsAnim{

	bool JumpFlg = false;
	GameObject Cam;
	Vector3 Pos;
	Vector3 Uni;
	private GameObject Toko;
	Vector2 Position;

	public Vector2 SPEED = new Vector2(0.05f, 0.05f);
	private int moveflg = 0;
	private int attackflg = 0;

	void Awake ()
	{
		//ExtendsのStart()を呼んでいろいろ初期化
		base.Start ();
		Toko = GameObject.Find ("Toko");
		Cam = GameObject.FindWithTag ("MainCamera");
		Pos.y = 0.4f;
		Pos.z = -50;
	}

	void Update (){

		axis = Input.GetAxisRaw ("Horizontal");
		isDown = Input.GetAxisRaw ("Vertical") < 0;
		//unityちゃんのちょっと上にカメラを固定して追従
		Uni = transform.position + Pos;

		//Baseで継承元であるExtendsクラスのUnityAnim関数を呼んでる
		base.UnityAnim(axis,isDown);

		//axisを引数に
		Move(axis);


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


	void Move(float Axis){
		if (attackflg == 0) {
			// 現在位置をPositionに代入
			Position = transform.position;

			// 左キーを押し続けていたら
			if (Input.GetKey (KeyCode.A)) {
				// 代入したPositionに対して加算減算を行う
				Position.x -= SPEED.x;
				moveflg = 1; 
			}
			// 右キーを押し続けていたら
			if (Input.GetKey (KeyCode.D)) {
				
				if (Cam.transform.position.x < transform.position.x) {
					Cam.transform.position = new Vector3 (transform.position.x, Uni.y, -10);
				}
				// 代入したPositionに対して加算減算を行う
				Position.x += SPEED.x;
				moveflg = 1;
			}

			// 現在の位置に加算減算を行ったPositionを代入する
			transform.position = Position;
			if (Input.GetButtonDown ("Jump") && !JumpFlg){
				rig2d.velocity = new Vector2 (rig2d.velocity.x, 5);
			}
			// flip sprite
			if (Axis != 0) spriteRenderer.flipX = Axis < 0;

			// Unityちゃんの上にだけカメラ追従
			Cam.transform.position = new Vector3 (Cam.transform.position.x, Uni.y, -10);
		}

	}


	//ゴールとの当たり判定

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag ("Goal") == true) Debug.Log ("hit");
		if (collider.gameObject.CompareTag ("Toko") == true && attackflg == 1) {
			Toko.SendMessage ("ChangeFlg");
		}
		if (collider.gameObject.CompareTag ("gameover") == true)
			SceneManager.LoadScene ("作業用");
	}


	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.CompareTag ("Ground") == true) JumpFlg = false;
		if (col.gameObject.tag == ("Toko") || col.gameObject.tag == ("Cindy")) {
			animator.SetBool (hashDamage, true);
			animator.SetBool (hashIsDead, true);
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.CompareTag ("Ground") == true) JumpFlg = true;
	}


}
