/// <summary>
/// Native module for playing media files.  2017-06-27 BY YB
/// Wrapper for 'Tizen.Multimedia.MMPlayer'
/// </summary>
using System;
using System.Collections.Generic;
using Tizen;
using Tizen.Multimedia;
using ElmSharp;
using System.Threading;

namespace ReactNative.Modules.MediaPlayer
{
    public class DantePlayer
    {
        public const string DANTE_PLAYER = "DP";
        
        public const string INVALID_MEDIA_TYPE = "Invalid Media Type";

        private Player _player = null;
        
        private Window _win = new Window("PlayerWindow");

        private Timer _timer = null;   // play event

        private State _currentPlayerState = State.IDLE;     // state switch

        private DantePlayer() 
        { 
            if ( null == _player )
            {
                _player = new Player(); 
            }
        }

        private string _uri;
        
        public static readonly DantePlayer instance = new DantePlayer();

        public delegate void CallbackDelegate(int ret_code);   // delegate for Player state

        public static Dictionary<string, CallbackDelegate> PlayerCBList = new Dictionary<string, CallbackDelegate>();

        public string currentMediaUri
        {
            internal set
            {
                if ( value != null )   
                {
                    _uri = value; 
                }
            }

            get
            {
                return _uri;
            }
        }

        public string cookie 
        { 
            set
            {
                _player.Cookie = value;
            }

            get
            {
                return _player.Cookie;
            }
        }

        public State state
        { 
            get
            {
                //Log.Warn(DANTE_PLAYER, $"[INFO] state = '{_player.State}'");
                return _currentPlayerState;
            }
        }

        /// milliseceonds
        public int position
        {
            get
            {
                int position = 0;
                try
                {
                    position = _player.GetPlayPosition();
                }
                catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                }

                return position;
            }
        }

        public int duration
        {
            get
            {
                int duration = 0;
                try
                {
                    duration = _player.StreamInfo.GetDuration();
                }
                catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    duration = 0;
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                    duration = 0;
                }

                return duration;
            }
        }

        public string AlbumInfo
        {
            get
            {
                string albumInfo = "";

                try
                {
                    if ( null != _player.StreamInfo.GetAlbumArt() )
                    {
                        albumInfo = System.Text.Encoding.Unicode.GetString(_player.StreamInfo.GetAlbumArt());
                    }
                }
                catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                }

                return albumInfo;
            }
        }


        public bool looping 
        { 
            set
            {
                _player.IsLooping = value;
            }

            get
            {
                return _player.IsLooping;
            }
        }

        public bool muted
        { 
            set
            {
                _player.Muted = value;
            }

            get
            {
                return _player.Muted;
            }
        }

        public float volume
        { 
            set
            {
                if ( value < 0.0f || value > 1.0f )
                {
                    _player.Volume = 0.2f;
                }
                else
                {
                    _player.Volume = value;
                }
            }

            get
            {
                return _player.Volume;
            }
        }


        public string AudioCodecFormat
        {
            get
            {
                if ( _player.State == PlayerState.Idle )
                {
                    Log.Error(DANTE_PLAYER, $"couldn't obtain audio codec format in state:{_player.State}");
                    return "";
                }
                return _player.StreamInfo.GetAudioCodec();
            }
        }

        public string VideoCodecFormat
        {
            get
            {
                if ( _player.State == PlayerState.Idle )
                {
                    Log.Error(DANTE_PLAYER, $"couldn't obtain video codec format in state:{_player.State}");
                    return "";
                }
               return _player.StreamInfo.GetVideoCodec();
            }
        }

        public float playbackRate
        {
            set
            {
                try
                {
                    _player.SetPlaybackRate(value);
                }
                catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);  
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_WRONG_RANGE_RATE);
                }
            }
        }

        private void BeginTimer(int dueTime, int period)
        {
            if ( null == _timer )
            {
                Log.Warn(DANTE_PLAYER, "[INFO] create the timer ...");
                _timer = new Timer(UpdatePlayInfo, "UpdatePlayInfo Timer ...", dueTime, period);
            }
            else
            {
                Log.Warn(DANTE_PLAYER, "[WARNNING] Timer is running ...");
            }
        }

        private void EndTimer()
        {
            if ( _timer != null )
            {
                Log.Warn(DANTE_PLAYER, "[INFO] destroy the timer ...");
                _timer.Dispose();
                _timer = null;
            }
        }

        private static void InvokeFunction(string func, int ret_code)
        {
            if (PlayerCBList.ContainsKey(func))
            {
                PlayerCBList[func].Invoke(ret_code);
            }
            else
            {
                Log.Info(DANTE_PLAYER, $"Event:{func} hasn't been registered yet!");
            }

            return ;
        }

        // {
        // vedio settings
        //_player.Display = new Display(_win);
        //_player.Display = new Display(new MediaView(box)); // The feature(http://tizen.org/feature/multimedia.raw_video) is not supported.
        // }

        private void DestroyWindow()
        {
            // Destroy player window
            if ( null != _win )
            {
                Log.Info(DANTE_PLAYER, "[INFO] Destroy the player window now ... ");
                _win.Unrealize();
                _win = null;
            }
        }

        //public async void Prepare(string url)
        public RET_CODE Prepare(string url)
        {
            if ( null == url || "" == url )
            {
                Log.Error(DANTE_PLAYER, $"[WARNING] Invalid uri: {url} ");
                InvokeFunction("OnException", (int)RET_CODE.PLAYER_INVALID_URI);
                return RET_CODE.PLAYER_INVALID_URI;
            }

            // 1. Reset previos source , because of singleton player
            if ( currentMediaUri != null )
            {
                if ( currentMediaUri != url )
                {
                    Log.Info(DANTE_PLAYER, $"[WARNING] new source:{url} ");

                    // destroy timer
                    EndTimer();

                    // unprepare
                    try
                    {
                        _player.Unprepare();
                    }
                    catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
                    {
                        Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                        return RET_CODE.PLAYER_INSTANCE_DISPOSED;
                    }
                    catch (InvalidOperationException ex)
                    {
                        Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                        return RET_CODE.PLAYER_INVALID_STATE;
                    }
                }
                else
                {
                    Log.Info(DANTE_PLAYER, $"[WARNING] already initialized source: {url} ");
                    return RET_CODE.PLAYER_DUPLICATED_URI;
                }
            }
            else
            {
                //  only one time 
                _player.PlaybackCompleted += OnPlaybackComplete;
                _player.ErrorOccurred += OnErrorOccurred;
                _player.PlaybackInterrupted += OnPlaybackInterrupted;
            }
        
            // 2. 'Preparing'  event
            _currentPlayerState = State.PREPARING;
            InvokeFunction("OnPreparing", (int)RET_CODE.PLAYER_OK);

            // 3. set source & init
            try
            {
                _player.SetSource(new MediaUriSource(url));
                //await _player.PrepareAsync();
                _player.PrepareAsync().Wait();
            }
            catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                _currentPlayerState = State.IDLE;
                //return RET_CODE.PLAYER_INSTANCE_DISPOSED;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                _currentPlayerState = State.IDLE;
                return RET_CODE.PLAYER_INVALID_STATE;
            }
            catch (Exception ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_ERROR_OCCURED);
                return RET_CODE.PLAYER_ERROR_OCCURED;
            }

            // 4. switch state
            _currentPlayerState = State.PREPARED;

            // 5. save uri
            currentMediaUri = url;

            // 6. trigger 'prepared' event
            InvokeFunction("OnPrepared", (int)RET_CODE.PLAYER_OK);

            //Log.Info(DANTE_PLAYER, $"[INFO] Inited, state = {_player.State}");
            return RET_CODE.PLAYER_OK;
        }

        public RET_CODE Play()
        {
            if ( State.PREPARED == _currentPlayerState || State.PAUSED == _currentPlayerState )
            {
                // Play
                try
                {
                    _player.Start();
                }
                catch(ObjectDisposedException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    return RET_CODE.PLAYER_INSTANCE_DISPOSED;
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                    return RET_CODE.PLAYER_INVALID_STATE;
                }

                // update state
                _currentPlayerState = State.STARTED;

                // Begin timer ..
                BeginTimer(10, 200);

                // 'Playing' event
                InvokeFunction("OnStarted", (int)RET_CODE.PLAYER_OK);
            }
            else
            {
                Log.Warn(DANTE_PLAYER, $"[WARNNING] couldn't Play in state:{_currentPlayerState}");
            }

            return RET_CODE.PLAYER_OK;
        }

        public RET_CODE Pause()
        {
            if ( State.STARTED == _currentPlayerState )
            {
                try
                {
                    _player.Pause();
                }
                catch(ObjectDisposedException ex)   // [Reserved] FOR the further,  singleton player -> multi player instance
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    return RET_CODE.PLAYER_INSTANCE_DISPOSED;
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                    return RET_CODE.PLAYER_INVALID_STATE;
                }      

                // Switch state
                _currentPlayerState = State.PAUSED;

                // 'Paused' event
                InvokeFunction("OnPaused", (int)RET_CODE.PLAYER_OK);  
            }
            else
            {
                Log.Warn(DANTE_PLAYER, $"[WARNNING] couldn't Play in state:{_currentPlayerState}");
            }

            return RET_CODE.PLAYER_OK;
        }

        public async void SeekTo(int millisec)
        {
            try
            {
                InvokeFunction("OnSeeking", (int)RET_CODE.PLAYER_OK);
                //_player.SetPlayPositionAsync(value, true);
                await _player.SetPlayPositionAsync(millisec, false);
                InvokeFunction("OnSeeked", (int)RET_CODE.PLAYER_OK);
            }
            catch(ObjectDisposedException ex)       // [Reserved] FOR the further,  singleton player -> multi player instance
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_POSITION_OUT_OF_RANGE);
            }  
        }

        public RET_CODE Stop()
        {
            if ( State.PREPARING != _currentPlayerState &&  State.IDLE != _currentPlayerState )
            {
                // Destory Timer
                EndTimer();

                // Pause
                try
                {
                    _player.Stop();
                }
                catch(ObjectDisposedException ex)   // [Reserved] FOR the further,  singleton player -> multi player instance
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    return RET_CODE.PLAYER_INSTANCE_DISPOSED;
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                    InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                    return RET_CODE.PLAYER_INVALID_STATE;
                } 

                // update state
                _currentPlayerState = State.STOPPED;               
            }
            else
            {
                Log.Warn(DANTE_PLAYER, $"[WARNNING] couldn't Stop in state:{_currentPlayerState}");
            }

            return RET_CODE.PLAYER_OK;
        }

        public RET_CODE UnPrepare()
        {
            // Destory Timer
            EndTimer();
 
            // register basic event handler
            _player.PlaybackCompleted -= OnPlaybackComplete;
            _player.ErrorOccurred -= OnErrorOccurred;
            _player.PlaybackInterrupted -= OnPlaybackInterrupted;

            // stop & Unpreapre
            try
            {
                _player.Unprepare();
            }
            catch(ObjectDisposedException ex)   // [Reserved] FOR the further,  singleton player -> multi player instance
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                return RET_CODE.PLAYER_INSTANCE_DISPOSED;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                InvokeFunction("OnThrowException", (int)RET_CODE.PLAYER_INVALID_STATE);
                return RET_CODE.PLAYER_INVALID_STATE;
            } 

            // reset media uri
            currentMediaUri = null;

            // update state
            _currentPlayerState = State.IDLE;

            return RET_CODE.PLAYER_OK;
        }

        public State GetCurrentState()
        {
            Log.Info(DANTE_PLAYER, $"[INFO] Current State={_player.State}");
            return (State)_player.State;
        }

        public RET_CODE SetSubtile(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            if ( extension != ".sub" &&  extension != ".smi" &&  extension != ".srt" )
            {
                Log.Warn(DANTE_PLAYER, $"[WARNING] Does not support subtile type:{extension}");
                return RET_CODE.PLAYER_SUBTITLE_FORMAT_NOT_SUPPORTED;
            }

            try
            {
                _player.SetSubtitle(path);
            }
            catch(ObjectDisposedException ex)   // [Reserved] FOR the further,  singleton player -> multi player instance
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                return RET_CODE.PLAYER_INSTANCE_DISPOSED;
            }
            catch (ArgumentException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}]");
                return RET_CODE.PLAYER_SUBTITLE_NULL_PATH;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                return RET_CODE.PLAYER_SUBTITLE_FILE_PATH_NOT_EXIST;
            }

            return RET_CODE.PLAYER_OK;
        }

        public RET_CODE ClearSubtile()
        {
            try
            {
                _player.ClearSubtitle();
            }
            catch(ObjectDisposedException ex)   // [Reserved] FOR the further,  singleton player -> multi player instance
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                return RET_CODE.PLAYER_INSTANCE_DISPOSED;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
                return RET_CODE.PLAYER_INVALID_STATE;
            }

            return RET_CODE.PLAYER_OK;
        }
        
        public void GetDownlaodProgress(out int start, out int current)
        {
            start = 0;
            current = 0;

            try
            {
                DownloadProgress download_progress = _player.GetDownloadProgress();
                start = download_progress.Start;
                current = download_progress.Current;
            }
            catch(ObjectDisposedException ex)   // [Reserved] FOR the further,  singleton player -> multi player instance
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(DANTE_PLAYER, $"Exception Info:[{ex.ToString()}] ");
            }
        }

#region " Events about "
        public void SetOnPrepared(CallbackDelegate cb)
        {
           AddEventListener("OnPrepared", cb); 
        }

        public void SetOnUpdatePlayInfo(CallbackDelegate cb)
        {
           AddEventListener("OnUpdatePlayInfo", cb); 
        }

        public void SetOnPlaybackComplete(CallbackDelegate cb)
        {
           AddEventListener("OnPlaybackComplete", cb); 
        }

        public void SetOnErrorOccurred(CallbackDelegate cb)
        {
           AddEventListener("OnErrorOccurred", cb); 
        }

        public void SetOnThrowException(CallbackDelegate cb)
        {
           AddEventListener("OnThrowException", cb); 
        }

        public void SetOnPlaybackInterrupted(CallbackDelegate cb)
        {
           AddEventListener("OnPlaybackInterrupted", cb); 
        }

        public void SetOnPreparing(CallbackDelegate cb)
        {
           AddEventListener("OnPreparing", cb); 
        }

        public void SetOnPaused(CallbackDelegate cb)
        {
           AddEventListener("OnPaused", cb); 
        }

        public void SetOnSeeking(CallbackDelegate cb)
        {
           AddEventListener("OnSeeking", cb); 
        }

        public void SetOnSeeked(CallbackDelegate cb)
        {
           AddEventListener("OnSeeked", cb); 
        }

        public void SetOnStarted(CallbackDelegate cb)
        {
           AddEventListener("OnStarted", cb); 
        }

        public void SetOnIdle(CallbackDelegate cb)
        {
           AddEventListener("OnIdle", cb); 
        }

        private static void AddEventListener(string eventType, CallbackDelegate cb)
        {
            //Log.Info(DANTE_PLAYER, $"[INFO] register '{eventType}'");

            if (PlayerCBList.ContainsKey(eventType))
            {
                PlayerCBList.Remove(eventType);
            }
            PlayerCBList[eventType] = cb;
        }

        // Timer CB
        private void UpdatePlayInfo(object sender)
        {
            // trigger 'prepared' event
            InvokeFunction("OnUpdatePlayInfo", (int)RET_CODE.PLAYER_OK);
        }

        private void OnPlaybackComplete(object sender, EventArgs e)
        {
            //Log.Info(DANTE_PLAYER, "[Event] OnPlaybackComplete Occurred");

            // update 'state'
            _currentPlayerState = State.PLAYBACKCOMPLETE;

            // Destory Timer
            EndTimer();

            // invoke 'OnPlaybackComplete'
            InvokeFunction("OnPlaybackComplete", (int)RET_CODE.PLAYER_OK);

            // TODO: destory the window for player
        }

        private void OnErrorOccurred(object sender, EventArgs e)
        {
            //Log.Info(DANTE_PLAYER, "[INFO] OnErrorOccur Occurred ");

            // Destory Timer
            EndTimer();

            // invoke 'OnErrorOccurred' 
            InvokeFunction("OnErrorOccurred", (int)RET_CODE.PLAYER_ERROR_OCCURED);

            // deinitialized the player
            UnPrepare();

            // TODO: destory the window for player
        }

        private void OnPlaybackInterrupted(object sender, EventArgs e)
        {
            //Log.Info(DANTE_PLAYER, "[INFO] OnPlaybackInterrupted Occurred ");

            // Destory Timer
            EndTimer();

            // invoke 'OnPlaybackInterrupted'
            InvokeFunction("OnPlaybackInterrupted", (int)RET_CODE.PLAYER_PLAYBACK_INTERRUPTED);

            // deinitialized the player
            UnPrepare();

            // TODO: destory the window for player       
        }
     
#endregion

    };
}