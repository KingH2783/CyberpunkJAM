using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour

{
    public AK.Wwise.Event Footsteps;
    public AK.Wwise.Event PlasmaGunShoot;



    public void PlayFootsteps()
    {
        Footsteps.Post(gameObject);

    }

    public void PlayPlasmaGunShoot()
    {
        PlasmaGunShoot.Post(gameObject);

    }

}
