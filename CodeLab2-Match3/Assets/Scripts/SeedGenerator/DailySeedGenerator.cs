using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailySeedGenerator : SeedGenerator {

	void Awake(){ //on awake, when you open it up, randomizing the seed based on the current date
		int seed = System.DateTime.Now.Year * 1000 + System.DateTime.Now.DayOfYear;

		Random.InitState(seed); //picking a random seed based on the current date
	}
}
