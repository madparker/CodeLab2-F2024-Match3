using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DabuLyu 
{
    public class FixedInputManagerScript : InputManagerScript
    {
        
        public GameObject selectedIndicatorPrefab;
        private GameObject selectedIndicator;

        public override void Start()
        {
            base.Start();
            selectedIndicator = Instantiate(selectedIndicatorPrefab, new Vector3(-10, -10, 0), Quaternion.identity);
            selectedIndicator.SetActive(false);
        }

        public  override  void SelectToken()
        {
            //base.SelectToken();
            
            if(Input.GetMouseButtonDown(0)){
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D tokenCollider = Physics2D.OverlapPoint(mousePos);

                if(tokenCollider != null){
                    if(selected == null){
                        selected = tokenCollider.gameObject;
                        
                        selectedIndicator.transform.position = selected.transform.position;
                        selectedIndicator.SetActive(true);
                        
                        
                        
                        
                    } else {
                        Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
                        Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);
            
                        if(Mathf.Abs((pos1.x - pos2.x) + (pos1.y - pos2.y)) == 1){ 
                            moveManager.SetupTokenExchange(selected, pos1, tokenCollider.gameObject, pos2, true);
                        }
                        selected = null;
                        selectedIndicator.SetActive(false);
                        
                        
                        
                    }
                }
            }

        
        }
    }
}

