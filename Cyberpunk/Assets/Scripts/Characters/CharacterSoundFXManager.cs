using AK.Wwise;
using UnityEngine;

namespace HL
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event Footsteps;
        [SerializeField] private AK.Wwise.Event PlasmaGunShoot;

        
        
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


        [Header("Player")]

        public AK.Wwise.Event player_ranged_attack_attack1_stationary;
        public AK.Wwise.Event player_reload;
        public AK.Wwise.Event player_dash;
        public AK.Wwise.Event player_death;
        public AK.Wwise.Event player_fall;
        public AK.Wwise.Event player_damaged;
        public AK.Wwise.Event player_idle;
        public AK.Wwise.Event player_jump;
        public AK.Wwise.Event player_land;
        public AK.Wwise.Event player_melee_attack1;
        public AK.Wwise.Event player_raised_gun_running;
        public AK.Wwise.Event player_ranged_attack_attack1_running;

        

        public void Playplayer_reload()
        {
            player_reload.Post(gameObject);

        }

        public void Playplayer_ranged_attack_attack1_stationary()
        {
            player_ranged_attack_attack1_stationary.Post(gameObject);

        }

        public void Playplayer_dash()
        {
            player_dash.Post(gameObject);

        }

        public void Playplayer_death()
        {
            player_death.Post(gameObject);

        }

        public void Playplayer_fall()
        {
            player_fall.Post(gameObject);

        }

        public void Playplayer_damaged()
        {
            player_damaged.Post(gameObject);

        }

        public void Playplayer_idle()
        {
            player_idle.Post(gameObject);

        }

        public void Playplayer_jump()
        {
            player_jump.Post(gameObject);

        }

        public void Playplayer_land()
        {
            player_land.Post(gameObject);

        }

        public void Playplayer_melee_attack1()
        {
            player_melee_attack1.Post(gameObject);

        }

        public void Playplayer_raised_gun_running()
        {
            player_raised_gun_running.Post(gameObject);

        }

        public void Playplayer_ranged_attack_attack1_running()
        {
            player_ranged_attack_attack1_running.Post(gameObject);

        }
    }
}