using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DabuLyu
{
    public class FixedGameManagerScript : GameManagerScript
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            

        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();


            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }
    }


}
