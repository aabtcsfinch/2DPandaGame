using UnityEngine;
using UnityEngine.UI;

namespace Bubblegum.Achievements
{
    /// <summary>
    /// Displays an achievement to the user
    /// </summary>
    public class AchievementDisplay : MonoBehaviour
    {
        #region VARIABLES

        /// <summary>
        /// The achievement that we want to show
        /// </summary>
        [SerializeField, Tooltip("The achievement that we want to show")]
        private BaseAchievement achievement;

        /// <summary>
        /// The image to display the icon
        /// </summary>
        [SerializeField, Tooltip("The image to display the icon")]
        private Image icon;

        /// <summary>
        /// Locked sprite if we want to show one
        /// </summary>
        [SerializeField, Tooltip("Locked sprite if we want to show one")]
        private Sprite lockedSprite;

        /// <summary>
        /// The name text to display
        /// </summary>
        [SerializeField, Tooltip("The name text to display")]
        private Text nameDisplay;

        /// <summary>
        /// The description text to display
        /// </summary>
        [SerializeField, Tooltip("The description text to display")]
        private Text description;

        /// <summary>
        /// The progress text to display
        /// </summary>
        [SerializeField, Tooltip("The progress text to display")]
        private Text progressText;

        /// <summary>
        /// Progress slider to display
        /// </summary>
        [SerializeField, Tooltip("Progress slider to display")]
        private Slider progressSlider;

        /// <summary>
        /// Button to collect achievement reward
        /// </summary>
        [SerializeField, Tooltip("Button to collect achievement reward")]
        private Button rewardCollectButton;

        /// <summary>
        /// Icon to indicate the reward has been collected
        /// </summary>
        [SerializeField, Tooltip("Icon to indicate the reward has been collected")]
        private GameObject rewardCollectedIcon;

        /// <summary>
        /// To toggle reward button collectable state
        /// </summary>
        public UnityBoolEvent onRewardCollectable;

        /// <summary>
        /// The animator component
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Locked key to set in the animator
        /// </summary>
        private const string LOCKED_KEY = "Locked";

        #endregion

        #region METHODS

        /// <summary>
        /// Awaken this object
        /// </summary>
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Enable this object
        /// </summary>
        void OnEnable()
        {
            if (achievement)
                Initialize(achievement);
        }

        /// <summary>
        /// Initialize this display
        /// </summary>
        /// <param name="achievement"></param>
        public void Initialize(BaseAchievement achievement)
        {
            this.achievement = achievement;

            if (nameDisplay)
                nameDisplay.text = achievement.Name;

            if (description)
                description.text = achievement.Description;

            if (icon)
            {
                if (lockedSprite && !achievement.Completed)
                    icon.sprite = lockedSprite;
                else
                    icon.sprite = achievement.Icon;
            }

            if (animator)
                animator.SetBool(LOCKED_KEY, !achievement.Completed);

            if (progressText)
                progressText.text = achievement.ProgressDisplayText;

            if (progressSlider)
                progressSlider.value = achievement.Progress;

            if (rewardCollectButton)
            {
                if (rewardCollectedIcon)
                    rewardCollectedIcon.SetActive(achievement.RewardCollected);

                var rewardCollectable = achievement.Completed && !achievement.RewardCollected;
                rewardCollectButton.gameObject.SetActive(!achievement.RewardCollected);
                rewardCollectButton.interactable = rewardCollectable;
                onRewardCollectable.Invoke(rewardCollectable);

                rewardCollectButton.onClick.AddListener(() =>
                {
                    achievement.CollectAchievementReward();
                    rewardCollectButton.gameObject.SetActive(false);

                    if (rewardCollectedIcon)
                        rewardCollectedIcon.SetActive(true);
                });
            }
        }

        #endregion
    }
}