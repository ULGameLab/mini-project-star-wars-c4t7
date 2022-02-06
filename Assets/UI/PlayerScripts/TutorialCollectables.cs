using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCollectables : MonoBehaviour
{
    public GameObject Prompt;

    public static int Collected;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Triggering")
        {
            Collected += 1;
            Destroy(gameObject);

            switch (Collected)
            {
                case 1:
                    Prompt.GetComponent<Text>().text = "Press 'SPACE' to jump over obstacles and hazards.";
                    break;
                case 2:
                    Prompt.GetComponent<Text>().text = "Hold 'LEFT SHIFT' to sprint. \n\nSpeed will enhance your jump as well.";
                    break;
                case 3:
                    Prompt.GetComponent<Text>().text = "Hold 'C' to crouch. \n\nYou will move slower but become more compact.";
                    break;
                case 4:
                    Prompt.GetComponent<Text>().text = "Press 'E' or 'RIGHT CLICK' with the mouse to activate the Repulsor. \n\nThis blast of kinetic energy will send any blue-colored objects in a short radius in front of you flying in the direction you are aiming.";
                    break;
                case 5:
                    Prompt.GetComponent<Text>().text = "This world is dangerous, and you are not alone in it. \n\nPress 'LEFT CLICK' on the mouse to fire your Pistol towards your targetting reticle. \n\nUse your Repulsor to deflect enemy projectiles before they hit you.";
                    break;
                default:
                    Prompt.GetComponent<Text>().text = "";
                    break;
            }
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCollected()
    {
        return Collected;
    }


}
