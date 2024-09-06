using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MattParker
{

    public class FixedMatchManagerScript : MatchManagerScript
    {

        public override bool GridHasMatch(){
            bool match = base.GridHasMatch();
            
            //Maybe do some other stuff?
		      
            return match;
        }
        
        
        public override List<GameObject> GetAllMatchTokens(){
            List<GameObject> tokensToRemove = base.GetAllMatchTokens();

            //Maybe do some other stuff?
		
            return tokensToRemove;
        }
        
    }
}
