using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour

{
    public AK.Wwise.Event Footsteps;
    public AK.Wwise.Event PlasmaGunShoot;

    [Header("Enemies")]

    public AK.Wwise.Event enemy_range1_running_concrete;
    public AK.Wwise.Event enemy_range1_damaged;
    public AK.Wwise.Event enemy_range1_death;
    public AK.Wwise.Event enemy_range1_fall;
    public AK.Wwise.Event enemy_range1_idle;
    public AK.Wwise.Event enemy_range1_jump;
    public AK.Wwise.Event enemy_range1_land;
    public AK.Wwise.Event enemy_range1_shooting;


    public void PlayFootsteps()
    {
        Footsteps.Post(gameObject);

    }

    public void PlayPlasmaGunShoot()
    {
        PlasmaGunShoot.Post(gameObject);

    }

    // -------- ENEMIES ------------

    public void PlayEnemy_range1_running_concrete()
    {
        enemy_range1_running_concrete.Post(gameObject);

    }

    public void PlayEnemy_range1_damaged()
    {
        enemy_range1_damaged.Post(gameObject);

    }

    public void PlayEnemy_range1_death()
    {
        enemy_range1_death.Post(gameObject);

    }

    public void PlayEnemy_range1_fall()
    {
        enemy_range1_fall.Post(gameObject);

    }

    public void Playenemy_range1_idle()
    {
        enemy_range1_idle.Post(gameObject);

    }

    public void PlayEnemy_range1_jump()
    {
        enemy_range1_jump.Post(gameObject);

    }

    public void PlayEnemy_range1_land()
    {
        enemy_range1_land.Post(gameObject);

    }

    public void PlayEnemy_range1_shooting()
    {
        enemy_range1_shooting.Post(gameObject);

    }



}
