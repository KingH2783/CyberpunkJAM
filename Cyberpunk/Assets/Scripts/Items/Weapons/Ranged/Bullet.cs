using UnityEngine;

namespace HL
{
    public class Bullet : MonoBehaviour
    {
        Rigidbody2D rb;
        [SerializeField][Tooltip("This is additive to Weapon Damage")] private float bulletDamage;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletLifeTime;
        [SerializeField] private LayerMask environmentLayer;

        [HideInInspector] public CharacterManager characterWhoFiredMe;
        [HideInInspector] public RangedWeapon weapon;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.velocity = transform.right * bulletSpeed;
            Destroy(gameObject, bulletLifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.TryGetComponent(out CharacterManager characterGettingShot) &&
                    characterGettingShot.characterStatsManager.teamID != characterWhoFiredMe.characterStatsManager.teamID && 
                    !characterGettingShot.isDead && 
                    !characterGettingShot.isDashing)
                {
                    characterGettingShot.characterStatsManager.TakeDamage(Mathf.RoundToInt(weapon.weaponDamage + bulletDamage), characterWhoFiredMe);

                    Destroy(gameObject);
                }

                // It hit the environment
                else if ((environmentLayer.value & (1 << collision.gameObject.layer)) > 0)
                    Destroy(gameObject);
            }
        }
    }
}