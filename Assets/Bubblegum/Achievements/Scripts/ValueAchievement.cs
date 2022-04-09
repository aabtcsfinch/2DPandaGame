using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bubblegum.Achievements
{
    /// <summary>
    /// Value achievement
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Object/Achievements/Value Achievement")]
    public class ValueAchievement : BaseAchievement, IPoints
    {
		#region VARIABLES
		
		/// <summary>
		/// Methods to invoke when the score changes
		/// </summary>
		public System.Action onPointsChanged { get; set; }

		/// <summary>
		/// Current value
		/// </summary>
		public override int Value { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public override int DefaultValue
        {
            get
            {
                if (compareMode == ComparisonMode.LessThan)
                    return int.MaxValue;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Achievement progress
        /// </summary>
        public override float Progress
        {
            get
            {
                if (Completed)
                    return 1f;

                if (compareMode == ComparisonMode.GreaterThan)
                    return Mathf.Clamp01((float)Value / (requiredValue + 1));
                else
                    return 0f;
            }
        }

        /// <summary>
        /// Get the progress display text
        /// </summary>
        public override string ProgressDisplayText
        {
            get
            {
                if (compareMode == ComparisonMode.GreaterThan)
                    return Completed ? (requiredValue + 1) + "/" + (requiredValue + 1) : Value + "/" + (requiredValue + 1);
                else
                    return Completed ? "1/1" : "0/1";
            }
        }

        /// <summary>
        /// Required value to complete the achievement
        /// </summary>
        [SerializeField, Tooltip("Required value to complete the achievement")]
        private int requiredValue;

        /// <summary>
        /// Compare mode between current and required value
        /// </summary>
        [SerializeField, Tooltip("Compare mode between current and required value")]
        private ComparisonMode compareMode;

        #endregion

        #region METHODS

        /// <summary>
        /// Check if the value exists and loaded
        /// </summary>
        public override void Load()
        {
            base.Load();

            SetPoints(PersistentData.GetInt(PrefsKey, DefaultValue));
        }

        /// <summary>
        /// Save value
        /// </summary>
        public override void Save()
        {
            base.Save();

            PersistentData.SetInt(PrefsKey, Value);
        }

        /// <summary>
        /// Check achievement progress, whether it is completed
        /// </summary>
        public void SaveAndCheckProgress()
        {
            Save();

            switch (compareMode)
            {
                case ComparisonMode.Equal:
                    if (Value == requiredValue)
                    {
                        AchievementCompleted();
                    }
                    break;

                case ComparisonMode.NotEqual:
                    if (Value != requiredValue)
                    {
                        AchievementCompleted();
                    }
                    break;

                case ComparisonMode.GreaterThan:
                    if (Value > requiredValue)
                    {
                        AchievementCompleted();
                    }
                    break;

                case ComparisonMode.LessThan:
                    if (Value < requiredValue)
                    {
                        AchievementCompleted();
                    }
                    break;
            }

			onPointsChanged?.Invoke();
		}

		/// <summary>
		/// Add value
		/// </summary>
		public void AddPoints(int value)
        {
            Value += value;
            SaveAndCheckProgress();
        }

        /// <summary>
        /// Set value
        /// </summary>
        public void SetPoints(int value)
        {
            Value = value;
            SaveAndCheckProgress();
		}

		/// <summary>
		/// Set data from dictionary to the object
		/// </summary>
		public override void SetSaveData(Dictionary<string, SaveDataKeyValuePair> data)
        {
            base.SetSaveData(data);
            SetPoints(data.ContainsKey(PrefsKey) ? Convert.ToInt32(data[PrefsKey].value) : DefaultValue);
        }

        /// <summary>
        /// Get the save data from this object and save it to the dictionary
        /// </summary>
        public override void GetSaveData(Dictionary<string, SaveDataKeyValuePair> data)
        {
            base.GetSaveData(data);

            data[PrefsKey] = new SaveDataKeyValuePair(Name, Value);
        }

        #endregion
    }
}
