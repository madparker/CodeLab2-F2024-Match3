using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour {

	public int initSeed; //the initial Seed as integer
	//seed generator gets random numbers based on the start seed you choose

	void Awake(){
		Random.InitState(initSeed); //randomizing the initial seed on awake
	}
}
