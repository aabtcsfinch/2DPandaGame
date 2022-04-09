using UnityEngine;

namespace Bubblegum.Achievements
{
    /// <summary>
    /// A tag to unlock entities when an achievement is complete
    /// </summary>
    public class TagAchievementUnlock : Tag
    {
        #region PUBLIC_VARIABLES

        /// <summary>
        /// The achievement for the player to complete
        /// </summary>
        public BaseAchievement achievement;

        #endregion

        #region PUBLIC_METHODS

        /// <summary>
        /// Apply this tag to the object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="entity"></param>
        protected override void Run(PrefabConnection connection, GameObject gameObject)
        {
            if (!connection.IsUnlocked() && achievement.Completed)
                connection.Unlock();
        }

        #endregion

    }
}