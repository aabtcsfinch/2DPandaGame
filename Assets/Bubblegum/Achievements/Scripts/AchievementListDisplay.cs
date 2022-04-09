using UnityEngine;

namespace Bubblegum.Achievements
{
    /// <summary>
    /// Display for all achievements found in the application
    /// </summary>
    public class AchievementListDisplay : DisplayMonoBehaviour
    {
        #region VARIABLES

        [Header("Achievements")]

        /// <summary>
        /// For generating the achievement list items
        /// </summary>
        [SerializeField, Tooltip("For generating the achievement list items")]
        private AchievementDisplay displayPrefab;

        /// <summary>
        /// The layout to create achievments under
        /// </summary>
        [SerializeField, Tooltip("The layout to create achievments under")]
        private Transform layout;

        /// <summary>
        /// If we should automatically find all achievement objects
        /// </summary>
        [SerializeField, Tooltip("If we should automatically find all achievement objects")]
        private bool autoFindAchievements = true;

        /// <summary>
        /// All of the achievements that we find
        /// </summary>
        [SerializeField, Tooltip("All of the achievements that we find")]
        private BaseAchievement[] achievements;

        #endregion

        #region METHODS

        /// <summary>
        /// Start this behaviour
        /// </summary>
        protected override void Start()
        {
            base.Start();

			if (autoFindAchievements)
				Key.InvokeOnKeysReady(() =>
				{
					achievements = Key.FindAll<BaseAchievement>();
					CreateAchievementDisplays();
				});
			else
			{
				foreach (BaseAchievement achievement in achievements)
					achievement.Initialize();

				CreateAchievementDisplays();
			}
        }

        /// <summary>
        /// Initialize this object
        /// </summary>
        void CreateAchievementDisplays()
        {
            layout.DestroyChildren();

            foreach (BaseAchievement achievement in achievements)
            {
                AchievementDisplay display = Instantiate(displayPrefab, layout);
                display.Initialize(achievement);
            }
        }

        #endregion
    }
}