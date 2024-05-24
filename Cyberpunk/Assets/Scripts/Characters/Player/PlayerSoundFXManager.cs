using UnityEngine;

namespace HL
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        [SerializeField] private AK.Wwise.Event Footsteps;
        [SerializeField] private AK.Wwise.Event PlasmaGunShoot;

        public void PlayFootsteps()
        {
            Footsteps.Post(gameObject);
        }

        public void PlayPlasmaGunShoot()
        {
            PlasmaGunShoot.Post(gameObject);
        }
    }
}