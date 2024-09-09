using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DabuLyu
{
    public class FixedGameManagerScript : GameManagerScript
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            
            matchManager = GetComponent<FixedMatchManagerScript>();
            inputManager = GetComponent<FixedInputManagerScript>();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();


        }
    }


}
