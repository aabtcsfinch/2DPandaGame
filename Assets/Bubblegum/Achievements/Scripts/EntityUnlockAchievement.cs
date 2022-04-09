using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubblegum.Achievements
{
    /// <summary>
    /// Achievement for entity unlock
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Object/Achievements/Entity Unlock Achievement")]
    public class EntityUnlockAchievement : BaseAchievement
    {
        #region VARIABLES

        /// <summary>
        /// Entity to check for unlock (unlock specific entity)
        /// or the entity parent to check for unlock (unlock specific type of entity)
        /// </summary>
        public Entity entityType;

        #endregion

        #region METHODS

        /// <summary>
        /// Register onEntityUnlocked event
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Entity.onEntityUnlocked += OnEntityUnlocked;
        }

        /// <summary>
        /// On Entity unlocked
        /// </summary>
        private void OnEntityUnlocked(Entity entity)
        {
            if (entity.IsEntity(entityType))
                AchievementCompleted();
        }

        #endregion

    }
}
