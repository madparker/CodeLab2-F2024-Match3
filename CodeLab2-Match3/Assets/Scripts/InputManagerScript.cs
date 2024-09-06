﻿using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager;
	protected MoveTokensScript moveManager;
	protected GameObject selected = null;

	public virtual void Start () {
		moveManager = GetComponent<MoveTokensScript>();
		gameManager = GetComponent<GameManagerScript>();
	}

	public virtual void SelectToken(){
		if(Input.GetMouseButtonDown(0)){
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			Collider2D tokenCollider = Physics2D.OverlapPoint(mousePos);

			if(tokenCollider != null){
				if(selected == null){
					selected = tokenCollider.gameObject;
				} else {
					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);

					if(Mathf.Abs((pos1.x - pos2.x) + (pos1.y - pos2.y)) == 1){
						moveManager.SetupTokenExchange(selected, pos1, tokenCollider.gameObject, pos2, true);
					}

					selected = null;
				}
			}
		}

	}
}
