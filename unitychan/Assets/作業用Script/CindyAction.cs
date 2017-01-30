using UnityEngine;
using System.Collections;

public class CindyAction : ExtendsAnim {
	// Use this for initialization
	void Awake() {
		base.Start ();
		/*コルーチンを呼び出す時に文字列を引数にして呼び出すとオーバーロードできない
		 * StartCoroutine("コルーチン名");
		 * 
		 * だから文字列指定しないで普通に関数を呼んで引数を渡すようにしてやる
		 * StartCoroutine(コルーチン名(引数));
		*/
		StartCoroutine ("Walk");
	}
	
	// Update is called once per frame
	void Update () {
	}

}
