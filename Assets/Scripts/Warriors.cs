using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warriors : MonoBehaviour
{
    public Animator anim;
  
    public  void Die()
    {
        anim.GetComponent<Animator>();
        anim.SetTrigger("Die");
    }
}
