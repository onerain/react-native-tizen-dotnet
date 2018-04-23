/// <summary>
/// Basic data structure.  2017-06-27 BY YB
/// </summary>

using ElmSharp;

namespace ReactNative.Modules.MediaPlayer
{
   public enum MediaType
    {
        Audio = 1,
        Video = 2,

        NONE
    }

    public enum DisplayType
    {
        Default = 1,        // use 'Window' only
        Customize = 2,      // customize parent widgets, ...  seems doesn't affect .

        NONE
    }

    public enum State
    {
        IDLE = 0,
        PREPARING,
        PREPARED,
        STARTED,
        PAUSED,
        STOPPED,
        PLAYBACKCOMPLETE,
        EXT
    }
    
    /// <summary>
    /// Enumeration for player display rotation
    /// </summary>
    public enum Rotation
    {
        /// <summary>
        /// Display is not rotated  
        /// </summary>
        RotationNone,

        /// <summary>
        ///  Display is rotated 90 degrees 
        /// </summary>
        Rotation90,

        /// <summary>
        /// Display is rotated 180 degrees  
        /// </summary>
        Rotation180,

        /// <summary>
        /// Display is rotated 270 degrees  
        /// </summary>
        Rotation270
    }


    /// <summary>
    /// Enumeration for player display mode
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Letter box 
        /// </summary>
        LetterBox,

        /// <summary>
        ///  Origin size
        /// </summary>
        OriginalSize,

        /// <summary>
        /// Full-screen 
        /// </summary>
        FullScreen,

        /// <summary>
        /// Cropped full-screen 
        /// </summary>
        CroppedFull,

        /// <summary>
        /// Origin size (if surface size is larger than video size(width/height)) or 
        /// Letter box (if video size(width/height) is larger than surface size) 
        /// </summary>
        OriginalOrFull,

    }
    public class TrackInfo
    {
        public int count;
        public string[] LanguageCode;
        public int selected;
    }

 
    public class MediaInfo
    {
        public int duration;
        public int position;
        public string ablum;
        public string codec;
    } 

    public enum RET_CODE
    {
        PLAYER_OK = 0,                          // Success
        PLAYER_INVALID_URI,              	// No source is set.
        PLAYER_INVALID_SOURCE,              	// No source is set.
        PLAYER_DUPLICATED_URI,              	// No source is set.
        PLAYER_INSTANCE_DISPOSED,           	// The player has already been disposed of.
        PLAYER_INVALID_STATE,          			// The player is not in the valid state.
        PLAYER_INVALID_INSTANCE_OR_POLICY,  	// The player has already been disposed of.\n /// -or-\n /// poilcy has already been disposed of.
        PLAYER_NONE_POLICY,                 	// policy is null
        PLAYER_FEATURE_NOT_SUPPORTED,       	// feature is not supported
        PLAYER_VALUE_IN_USE,                	// The value has already been assigned to another player.
        PLAYER_WRONG_RANGE_RATE,            	// less than -5.0 or greater than 5.0 or equals to 0
        PLAYER_POSITION_OUT_OF_RANGE,       	// The specified position is not valid.

        PLAYER_SUBTITLE_EMPTY_PATH,         	// subtitle's path is an empty string.
        PLAYER_SUBTITLE_FILE_PATH_NOT_EXIST,	// The specified path does not exist.
        PLAYER_SUBTITLE_NULL_PATH,          	// The path is null.
        PLAYER_SUBTITLE_FORMAT_NOT_SUPPORTED,	// feature is not supported
        PLAYER_PLAYBACK_INTERRUPTED,
        PLAYER_ERROR_OCCURED,
        PLAYER_TO_BE_EXTENDED,
    }  
}