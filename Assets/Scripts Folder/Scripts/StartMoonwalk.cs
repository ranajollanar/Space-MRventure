using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMoonwalk : MonoBehaviour
{
    public Animator astroAnimator;
    // Start is called before the first frame update
    void Start()
    {
        astroAnimator.SetBool("isWalking", true);
   
    }
}
