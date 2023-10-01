using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAnim : MonoBehaviour
{
    public Animator anim;

    public void StartShovel()
    {
        anim.SetTrigger("shovel");
    }
}
