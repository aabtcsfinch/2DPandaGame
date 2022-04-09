using Bubblegum.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum.Achievements
{
    /// <summary>
    /// Base achievement object
    /// </summary>
    public abstract class BaseAchievement : StateKey, IPopupData, ISequenced, ISaveData
    {
        /// <summary>
        /// Popup related variables, used for notifying user when they completed an achievement
        /// </summary>
        #region POPUP_VARIABLES

        /// <summary>
        /// Popup display priority
        /// </summary>
        public int Priority => priority;

        /// <summary>
        /// Popup title
        /// </summary>
        public string Title => Name;

        /// <summary>
        /// Message to be displayed on the popup
        /// </summary>
        public string Message => description;

        /// <summary>
        /// Image to be displayed, achievement icon
        /// </summary>
        public Sprite Icon => icon;

        /// <summary>
        /// Popup audio
        /// </summary>
        public AudioClip Audio => audio;

        /// <summary>
        /// Type of popup
        /// </summary>
        public string PopupType => popupType;

        /// <summary>
        /// No additional content
        /// </summary>
        public GameObject AdditionalContent => null;

        /// <summary>
        /// No confirm action
        /// </summary>
        public Action ConfirmAction => null;

        /// <summary>
        /// No cancel action
        /// </summary>
        public Action CancelAction => null;

        /// <summary>
        /// Get the description
        /// </summary>
        public string Description => description;

        #endregion

        #region VARIABLES

        /// <summary>
        /// Prefs prefix for this achievement
        /// </summary>
        private const string PREFS_PREFIX = "Achievement";

        /// <summary>
        /// Prefs key for completed status
        /// </summary>
        private const string COMPLETED_PREFS_PREFIX = "AchievementCompleted";
        
        /// <summary>
        /// Prefs key for reward collection
        /// </summary>
        private const string REWARD_COLLECTED_PREFS_PREFIX = "AchievementRewardCollected";

        /// <summary>
        /// Achievement value
        /// </summary>
        public virtual int Value
        {
            get
            {
                return Completed ? 1 : 0;
            }
            set
            {
                Completed = value == 1;
            }
        }

        /// <summary>
        /// Default value for the achievement
        /// </summary>
        public virtual int DefaultValue
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Achievement progress string
        /// </summary>
        public virtual float Progress
        {
            get
            {
                return Completed ? 1f : 0f;
            }
        }

        /// <summary>
        /// Get the progress display text
        /// </summary>
        public virtual string ProgressDisplayText
        {
            get
            {
                return Completed ? "1/1" : "0/1";
            }
        }

        /// <summary>
        /// If this sequence object is complete
        /// </summary>
        public bool Complete => throw new NotImplementedException();

        /// <summary>
        /// If this achievement is completed
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// If this achievement reward is collected
        /// </summary>
        public bool RewardCollected { get; set; }

        [Header("Achievement")]

        /// <summary>
        /// The priority of this display
        /// </summary>
        [SerializeField, Tooltip("The priority of this display")]
        private int priority = 1;

        /// <summary>
        /// Type of popup
        /// </summary>
        [SerializeField, Tooltip("Type of popup")]
        private string popupType;

        /// <summary>
        /// Description for the achievement
        /// </summary>
        [SerializeField, Tooltip("Description for the achievement"), TextArea]
        private string description;

        /// <summary>
        /// Icon for the achievement
        /// </summary>
        [SerializeField, Tooltip("Icon for the achievement")]
        private Sprite icon;

        /// <summary>
		/// The audio to play when this achievement unlocks
		/// </summary>
		[SerializeField, Tooltip("The audio to play when this achievement unlocks")]
        private AudioClip audio;

        /// <summary>
        /// Prefs key for storing achievement value
        /// </summary>
        protected string PrefsKey
        {
            get
            {
                if (string.IsNullOrEmpty(cachedPrefsKey))
                    cachedPrefsKey = PREFS_PREFIX + ID;

                return cachedPrefsKey;
            }
        }
        private string cachedPrefsKey;

        /// <summary>
        /// Prefs key for storing achievement completed status
        /// </summary>
        protected string CompletedPrefsKey
        {
            get
            {
                if (string.IsNullOrEmpty(cachedCompletedPrefsKey))
                    cachedCompletedPrefsKey = COMPLETED_PREFS_PREFIX + ID;

                return cachedCompletedPrefsKey;
            }
        }
        protected string cachedCompletedPrefsKey;

        /// <summary>
        /// Prefs key for storing achievement reward collected status
        /// </summary>
        protected string RewardCollectedPrefsKey
        {
            get
            {
                if (string.IsNullOrEmpty(cachedRewardCollectedPrefsKey))
                    cachedRewardCollectedPrefsKey = REWARD_COLLECTED_PREFS_PREFIX + ID;

                return cachedRewardCollectedPrefsKey;
            }
        }
        protected string cachedRewardCollectedPrefsKey;

        #endregion

        #region METHODS

        /// <summary>
        /// Load achievement completed status
        /// </summary>
        public override void Load()
        {
            Completed = PersistentData.GetInt(CompletedPrefsKey) == 1;
            RewardCollected = PersistentData.GetInt(RewardCollectedPrefsKey) == 1;
        }

        /// <summary>
        /// Save achievement completed status
        /// </summary>
        public override void Save()
        {
            PersistentData.SetInt(CompletedPrefsKey, Completed ? 1 : 0);
            PersistentData.SetInt(RewardCollectedPrefsKey, RewardCollected ? 1 : 0);
        }

        /// <summary>
        /// Invoke achievement completed event
        /// </summary>
        public void AchievementCompleted()
        {
            if (Completed)
                return;

            Completed = true;
            RewardCollected = false;
            Save();

            SequenceManager.Instance.Queue(this);
        }

        /// <summary>
        /// Mark achievement reward as collected
        /// </summary>
        public void CollectAchievementReward()
        {
            if (RewardCollected)
                return;
            
            RewardCollected = true;
            Save();
        }

        /// <summary>
        /// Before start this task
        /// </summary>
        public void BeforeTask() { }

        /// <summary>
        /// Skip this task
        /// </summary>
        public void SkipTask() { }

        /// <summary>
        /// Start Task
        /// </summary>
        public IEnumerator StartTask()
        {
            Popup.ShowPopup(this);

            while (Popup.GetPopup(popupType).gameObject.activeSelf)
                yield return null;

            SequenceManager.Instance.StartNext(this);
        }

        /// <summary>
        /// Set data from dictionary to the object
        /// </summary>
        public virtual void SetSaveData(Dictionary<string, SaveDataKeyValuePair> data)
        {
            Completed = data.ContainsKey(CompletedPrefsKey) ? (bool)data[CompletedPrefsKey].value : false;
            RewardCollected = data.ContainsKey(RewardCollectedPrefsKey) ? (bool)data[RewardCollectedPrefsKey].value : false;
        }

        /// <summary>
        /// Get the save data from this object and save it to the dictionary
        /// </summary>
        public virtual void GetSaveData(Dictionary<string, SaveDataKeyValuePair> data)
        {
            data[CompletedPrefsKey] = new SaveDataKeyValuePair(Name, Completed);
            data[RewardCollectedPrefsKey] = new SaveDataKeyValuePair(Name, RewardCollected);
        }

        #endregion
    }

#if UNITY_EDITOR

    /// <summary>
    /// Editor script for achievement objects
    /// </summary>
    [CustomEditor(typeof(BaseAchievement), true)]
    public class BaseAchievementEditor : Key.KeyEditor
    {
        #region METHODS

        /// <summary>
        /// Draw the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Trigger Complete"))
                ((BaseAchievement)target).AchievementCompleted();
        }

        #endregion
    }

#endif
}