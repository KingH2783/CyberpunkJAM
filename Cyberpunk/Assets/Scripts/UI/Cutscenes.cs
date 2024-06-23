using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HL
{
    public class Cutscenes : MonoBehaviour
    {
        [HideInInspector] public static Cutscenes Instance { get; private set; }

        [Header("INTRO")]
        [SerializeField] private GameObject FindingTheOpenDoor;
        [SerializeField] private GameObject FindingTheOpenDoorText1;
        [SerializeField] private GameObject FindingTheOpenDoorText2;
        [SerializeField] private GameObject CheckingPC;
        [SerializeField] private GameObject GuardOpensDoor;
        [SerializeField] private GameObject AlarmGoesOff;
        [SerializeField] private GameObject RunningWithArm;
        [SerializeField] private GameObject HopeOutOfWindow;
        [SerializeField] private GameObject HopeFallsDown;
        [SerializeField] private GameObject MikeEnters;
        [SerializeField] private GameObject MikeCarriesHope;
        [SerializeField] private GameObject HopeOnMedicalChair;
        [SerializeField] private GameObject HopeEyesClosed;
        [SerializeField] private GameObject HopeEyesOpen;
        [SerializeField] private GameObject MikeGivesGlasses;

        [Header("MID SCENE")]
        [SerializeField] private GameObject Malfunction;
        [SerializeField] private GameObject MJSavingHope;
        [SerializeField] private GameObject MJShootingBack;
        [SerializeField] private GameObject MJandHopeRunning;

        [Header("BOSS DEATH")]
        [SerializeField] private GameObject BossChoking;
        [SerializeField] private GameObject MJEntering;
        [SerializeField] private GameObject HopeOnGround;
        [SerializeField] private GameObject MJHoldingHopesHand;
        [SerializeField] private GameObject Ending;

        public bool isInCutscene = false;
        private CutscenesEnum currentCutscene;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            EnableCutscene(CutscenesEnum.FindingTheOpenDoor);
        }

        public void EnableCutscene(CutscenesEnum scene)
        {
            isInCutscene = true;
            currentCutscene = scene;
            switch (scene)
            {
                case CutscenesEnum.None:
                    break;
                case CutscenesEnum.FindingTheOpenDoor:
                    StartCoroutine(FindingTheOpenDoorWait());
                    break;
                case CutscenesEnum.CheckingPC:
                    FindingTheOpenDoor.SetActive(false);
                    CheckingPC.SetActive(true);
                    StartCoroutine(StartNextCutscene(10));
                    break;
                case CutscenesEnum.GuardOpensDoor:
                    CheckingPC.SetActive(false);
                    GuardOpensDoor.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.AlarmGoesOff:
                    GuardOpensDoor.SetActive(false);
                    AlarmGoesOff.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.RunningWithAarm:
                    AlarmGoesOff.SetActive(false);
                    RunningWithArm.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.HopeOutOfWindow:
                    RunningWithArm.SetActive(false);
                    HopeOutOfWindow.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.HopeFallsDown:
                    HopeOutOfWindow.SetActive(false);
                    HopeFallsDown.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MikeEnters:
                    HopeFallsDown.SetActive(false);
                    MikeEnters.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MikeCarriesHope:
                    MikeEnters.SetActive(false);
                    MikeCarriesHope.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.HopeOnMedicalChair:
                    MikeCarriesHope.SetActive(false);
                    HopeOnMedicalChair.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.HopeEyesClosed:
                    HopeOnMedicalChair.SetActive(false);
                    HopeEyesClosed.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.HopeEyesOpen:
                    HopeEyesClosed.SetActive(false);
                    HopeEyesOpen.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MikeGivesGlasses:
                    StartCoroutine(MikeGivesGlassesWait());
                    break;

                case CutscenesEnum.Malfunction:
                    Malfunction.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MJSavingHope:
                    Malfunction.SetActive(false);
                    MJSavingHope.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MJShootingBack:
                    MJSavingHope.SetActive(false);
                    MJShootingBack.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MJandHopeRunning:
                    StartCoroutine(MJandHopeRunningWait());
                    break;

                case CutscenesEnum.BossChoking:
                    BossChoking.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MJEntering:
                    BossChoking.SetActive(false);
                    MJEntering.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.HopeOnGround:
                    MJEntering.SetActive(false);
                    HopeOnGround.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.MJHoldingHopesHand:
                    HopeOnGround.SetActive(false);
                    MJHoldingHopesHand.SetActive(true);
                    StartCoroutine(StartNextCutscene(3));
                    break;
                case CutscenesEnum.Ending:
                    StartCoroutine(EndingWait());
                    break;
                default:
                    break;
            }
        }

        private IEnumerator FindingTheOpenDoorWait()
        {
            FindingTheOpenDoor.SetActive(true);
            yield return StartCoroutine(WaitForSecondsOrInput(15));
            FindingTheOpenDoorText1.SetActive(false);
            FindingTheOpenDoorText2.SetActive(true);
            yield return StartCoroutine(WaitForSecondsOrInput(10));
            EnableCutscene(CutscenesEnum.CheckingPC);
        }

        private IEnumerator MikeGivesGlassesWait()
        {
            HopeEyesOpen.SetActive(false);
            MikeGivesGlasses.SetActive(true);
            yield return StartCoroutine(WaitForSecondsOrInput(3));
            MikeGivesGlasses.SetActive(false);
            isInCutscene = false;
        }

        private IEnumerator MJandHopeRunningWait()
        {
            MJShootingBack.SetActive(false);
            MJandHopeRunning.SetActive(true);
            yield return StartCoroutine(WaitForSecondsOrInput(3));
            MJandHopeRunning.SetActive(false);
            isInCutscene = false;
        }

        private IEnumerator EndingWait()
        {
            MJHoldingHopesHand.SetActive(false);
            Ending.SetActive(true);
            yield return StartCoroutine(WaitForSecondsOrInput(10));
            isInCutscene = false;
            SceneManager.LoadScene(0);
        }

        private IEnumerator StartNextCutscene(float secondsToWait)
        {
            yield return StartCoroutine(WaitForSecondsOrInput(secondsToWait));
            EnableCutscene(currentCutscene + 1);
        }

        private IEnumerator WaitForSecondsOrInput(float seconds)
        {
            float timer = 0f;
            bool inputDetected = false;

            while (timer < seconds && !inputDetected)
            {
                if (Input.anyKey)
                {
                    inputDetected = true;
                }
                else
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            // Consume the input so it doesn't affect subsequent waits
            while (Input.anyKey)
            {
                yield return null;
            }
        }
    }
}