﻿#if UNITY_EDITOR

/*! \cond PRIVATE */
namespace DarkTonic.MasterAudio {
    public class MasterAudioSettings : SingletonScriptable<MasterAudioSettings> {
        public const string AssetName = "MasterAudioSettings.asset";
        public const string AssetFolder = "Assets/Resources/MasterAudio";
        public const string ResourcePath = "MasterAudio/MasterAudioSettings";

        public bool UseDbScale;
		public bool UseCentsPitch;
		public bool HideLogoNav;
		public bool EditMAFolder;
		public string InstallationFolderPath = MasterAudio.MasterAudioDefaultFolder;
		public MasterAudio.MixerWidthMode MixerWidthSetting = MasterAudio.MixerWidthMode.Narrow;
        public bool BusesShownInNarrow = true;
        public bool ShowGizmos = true;

        public bool StopZeroVolumeVariations;
        public bool StopZeroVolumeGroups;
        public bool StopZeroVolumeBuses;
        public bool StopZeroVolumePlaylists;

        public bool ResourceClipsPauseDoNotUnload;
        public bool ResourceClipsAllLoadAsync = true;

        static MasterAudioSettings() {
            AssetNameToLoad = string.Format("{0}/{1}", AssetFolder , AssetName);
            ResourceNameToLoad = ResourcePath;
            FoldersToCreate = new System.Collections.Generic.List<string> {
                "Assets/Resources",
                "Assets/Resources/MasterAudio"
            };
        }
    }
}
/*! \endcond */

#endif