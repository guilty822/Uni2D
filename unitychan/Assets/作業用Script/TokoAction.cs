using UnityEngine;
using System.Collections;

public class TokoAction : ExtendsAnim {
	
	//bool flg = false;
	bool isDead = false;

	private BoxCollider2D box2d;

	// Use this for initialization
	void Awake () {
		//Extendsでやってる初期化をここで
		base.Start ();
		box2d = GetComponent<BoxCollider2D> ();
		//徘徊開始
		StartCoroutine ("Walk");
	}

	// Update is called once per frame
	void Update () {
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetBool (hashIsDead, isDead);
	}

	public void ChangeFlg(){
		if (!(isDead)) {
			//徘徊終了　Toko　ｵﾜﾀ
			StopCoroutine ("Walk");
			animator.SetTrigger ("Damage");
			isDead = true;
			box2d.isTrigger = true;
			rig2d.isKinematic = true;
		}
	}

	 /* Tokoの徘徊コルーチン
	  * 内容はfor文を使って40フレーム分だけ左右の移動を繰り返させてる
	  * 移動量は1
	 */
	/*
	IEnumerator Walk(){
		animator.SetFloat (hashSpeed, 1f);//パラメーターに数値をセット
		for (int i = 0; i <= 100; i++) {
			if (!flg) transform.Translate (Vector3.right * 1 * Time.deltaTime);
			else      transform.Translate (Vector3.left * 1 * Time.deltaTime);
			yield return null;
			if (i == 100){
				if (flg) {
					flg = false;
					i = 0;
					spriteRenderer.flipX = false;//反転
				} else {
					flg = true;
					i = 0;
					spriteRenderer.flipX = true;//反転
				}
			}
		}
	}
	*/

}
