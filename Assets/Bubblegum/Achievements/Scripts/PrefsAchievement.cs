using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubblegum.Achievements
{
    /// <summary>
    /// Achievement that reads value from prefs
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Object/Achievements/Prefs Achievement")]
    public class PrefsAchievement : ValueAchievement
    {
        #region VARIABLES

        /// <summary>
        /// Key to read from player prefs
        /// </summary>
        public string prefsKey = "Achievement";

        #endregion

        #region METHODS

        /// <summary>
        /// Set the value by reading from player prefs
        /// </summary>
        public void SetValueFromPlayerPrefs()
        {
            SetPoints(PersistentData.GetInt(prefsKey));
        }

        #endregion

    }
}
