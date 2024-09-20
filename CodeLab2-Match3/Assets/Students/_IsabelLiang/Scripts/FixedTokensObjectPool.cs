using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IsabelLiang
{
    public class FixedTokensObjectPool : TokenObjectPool
    {
        void Awake()
        {
            tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //load all the token prefabs
            spriteTypes = Resources.LoadAll<Sprite>("_Core/Images");
        }
    }

}
