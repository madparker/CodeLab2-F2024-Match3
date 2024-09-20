using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DabuLyu
{
      public class FixedTokenObjectPool : TokenObjectPool
      {

      
          
          void Awake()
          {
              tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //load all the token prefabs
              spriteTypes = Resources.LoadAll<Sprite>("_Core/Images");
          }

      }

}
