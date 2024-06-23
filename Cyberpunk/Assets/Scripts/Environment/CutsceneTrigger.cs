using UnityEngine;

namespace HL
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] private Transform TPPoint;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.TryGetComponent(out PlayerManager player))
                {
                    player.isPerformingAction = true;
                    player.isInvulnerable = true;
                    player._transform.position = TPPoint.position;
                    Cutscenes.Instance.EnableCutscene(CutscenesEnum.Malfunction);
                }
            }
        }
    }
}