package ninzimay.mediaplayer.ninzimay;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.ContentUris;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.media.AudioManager;
import android.media.MediaMetadataRetriever;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Binder;
import android.os.Handler;
import android.os.IBinder;
import android.os.PowerManager;
import android.support.v4.app.NotificationCompat;
import android.support.v4.content.LocalBroadcastManager;
import android.telephony.PhoneStateListener;
import android.telephony.TelephonyManager;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.style.StyleSpan;
import android.text.style.TypefaceSpan;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ImageView;
import android.widget.RemoteViews;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.List;
import java.util.concurrent.TimeUnit;

/**
 * Created by Administrator on 2016/06/19.
 */
public class MusicService extends Service
implements AudioManager.OnAudioFocusChangeListener
{
    //<!-- Start declaration area.  -->
    private String LoggerName = "NinZiMay";
    private String PlayingStatus_New = "PlayingStatus_New";
    private String PlayingStatus_Playing = "PlayingStatus_Playing";
    private String PlayingStatus_Played = "PlayingStatus_Played";
    private enum PlayerEventName {
        NextSong, CurrentPlayingSong, PreviousSong;
    }
    private MediaPlayerState mediaPlayerState;
    private MediaPlayer player = null;
    private List<MusicDictionary> List_MusicDictionary;
    private List<MusicDictionary> ShuffledList;
    private MusicDictionary Current_MusicDictionary;
    private int CurrentPlayingLength = 0;
    private int CurrentVolumeLevel = 0;
    private int AudioFocusRequestCode = 0;
    private boolean IsUserPressedPause = false;
    private boolean IsUserUpdateList = false;
    private boolean IsStopSong = false;
    private Handler Handler_Music = null;
    private Runnable Runnable_Music = null;
    private Gson gson = new Gson();
    private AudioManager _AudioManager;
    private MusicIntentReceiver _MusicIntentReceiver;
    private DatabaseHandler _DatabaseHandler = null;
    private List<MusicDictionary> Filterable_List_MusicDictionary = new ArrayList<MusicDictionary>();
    //<!-- End declaration area.  -->

    //<!-- Start dependency object(s).  -->
    private class MusicIntentReceiver extends BroadcastReceiver {
        @Override public void onReceive(Context context, Intent intent) {
            if (intent.getAction().equals(Intent.ACTION_HEADSET_PLUG)) {
                int state = intent.getIntExtra("state", -1);
                switch (state) {
                    case 0:
                        //Log.d(LoggerName, "Headset is unplugged");
                        if(mediaPlayerState != MediaPlayerState.Started){
                            return;
                        }
                        pauseCurrentSong();
                        broadCast_OnDemand(false);
                        break;
                    case 1:
                        //Log.d(LoggerName, "Headset is plugged");
                        break;
                    default:
                        //Log.d(LoggerName, "I have no idea what the headset state is");
                        break;
                }
            }
        }
    }
    //<!-- End dependency object(s).  -->

    //<!-- Start system defined function(s).  -->
    public void onCreate(){
        //Log.d(LoggerName, "Service.onCreate");
        super.onCreate();
        _DatabaseHandler = new DatabaseHandler(getApplicationContext());
        getHandler();
        _MusicIntentReceiver = new MusicIntentReceiver();
        IntentFilter filter = new IntentFilter(Intent.ACTION_HEADSET_PLUG);
        registerReceiver(_MusicIntentReceiver, filter);
        _AudioManager = (AudioManager) getSystemService(Context.AUDIO_SERVICE);
        AudioFocusRequestCode = _AudioManager.requestAudioFocus(this, AudioManager.STREAM_MUSIC, AudioManager.AUDIOFOCUS_GAIN);
        CurrentVolumeLevel = _AudioManager.getStreamVolume(AudioManager.STREAM_MUSIC);
    }
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        if (intent.getAction().equals(Constants.ACTION.PREV_ACTION)) {
            CurrentPlayingLength = 0;
            switch (InvokeNavigator()._NavigatorValue){
                case ReplayCurrentSong:
                    playSong(getCurrent_ReInitialized_MusicDictionary());
                    break;
                default:
                    playSong(GetSongToPlay(PlayerEventName.PreviousSong));
                    break;
            }
        }else if (intent.getAction().equals(Constants.ACTION.PLAY_ACTION)) {
            if(List_MusicDictionary == null){
                String Initial_List_MusicDictionary = intent.getExtras().get("Initial_List_MusicDictionary").toString();
                List_MusicDictionary = gson.fromJson(Initial_List_MusicDictionary, new TypeToken<List<MusicDictionary>>(){}.getType());
            }
            playSong(GetSongToPlay(PlayerEventName.NextSong));
        }else if (intent.getAction().equals(Constants.ACTION.PLAYBACK_ACTION)) {
            playbackCurrentSong();
        }else if (intent.getAction().equals(Constants.ACTION.PAUSE_ACTION)) {
            IsUserPressedPause = true;
            pauseCurrentSong();
            broadCast_OnDemand(false);
        }else if (intent.getAction().equals(Constants.ACTION.NEXT_ACTION)) {
            if(List_MusicDictionary == null){
                String Initial_List_MusicDictionary = intent.getExtras().get("Initial_List_MusicDictionary").toString();
                List_MusicDictionary = gson.fromJson(Initial_List_MusicDictionary, new TypeToken<List<MusicDictionary>>(){}.getType());
            }
            CurrentPlayingLength = 0;
            switch (InvokeNavigator()._NavigatorValue){
                case ReplayCurrentSong:
                    playSong(getCurrent_ReInitialized_MusicDictionary());
                    break;
                case Stop:
                    stopSong();
                    break;
                default:
                    playSong(GetSongToPlay(PlayerEventName.NextSong));
                    break;
            }
        }else if (intent.getAction().equals(Constants.ACTION.INDEXED_SONG_ACTION)) {
            if(List_MusicDictionary == null){
                String Initial_List_MusicDictionary = intent.getExtras().get("Initial_List_MusicDictionary").toString();
                List_MusicDictionary = gson.fromJson(Initial_List_MusicDictionary, new TypeToken<List<MusicDictionary>>(){}.getType());
            }
            CurrentPlayingLength = 0;
            String Current_MusicDictionary = intent.getExtras().get("Current_MusicDictionary").toString();
            setCurrent_MusicDictionary(gson.fromJson(Current_MusicDictionary, MusicDictionary.class));
            playSong(getCurrent_ReInitialized_MusicDictionary());
        }else if (intent.getAction().equals(Constants.ACTION.INDEXED_SEEK_ACTION)) {
            CurrentPlayingLength = Integer.parseInt(intent.getExtras().get("seekBarIndex").toString());
            if(mediaPlayerState == MediaPlayerState.Started)
                playIndexedSong();
        }if (intent.getAction().equals(Constants.ACTION.INVOKE_ONDEMAND_ACTION)) {
            broadCast_OnDemand(false);
        }if (intent.getAction().equals(Constants.ACTION.UPDATE_MUSICDICTIONARY_ACTION)) {
            String ToUpdate_MusicDictionary = intent.getExtras().get("ToUpdate_MusicDictionary").toString();
            update_MusicDictionary(gson.fromJson(ToUpdate_MusicDictionary, MusicDictionary.class));
        }else if (intent.getAction().equals(Constants.ACTION.STOPFOREGROUND_ACTION)) {
            broadCast_OnDemand(true);
            stopForeground(true);
            stopSelf();
        }
        return START_STICKY;
    }
    @Override
    public void onDestroy() {
        player.release();
        player = null;
        mediaPlayerState = MediaPlayerState.End;
        SharedPreferences _SharedPreferences = getSharedPreferences(Constants.CACHE.NINZIMAY, MODE_PRIVATE);
        _SharedPreferences.edit().clear().commit();
        _AudioManager.abandonAudioFocus(this);
        if(_MusicIntentReceiver != null)
            unregisterReceiver(_MusicIntentReceiver);
    }
    @Override
    public void onAudioFocusChange(int focusChange) {
        switch (focusChange)
        {
            case AudioManager.AUDIOFOCUS_GAIN:
                // TODO : resume playback
                Log.d(LoggerName, "AUDIOFOCUS_GAIN = "+focusChange);
                _AudioManager.setStreamVolume(AudioManager.STREAM_MUSIC, CurrentVolumeLevel, 0);
                if(CurrentPlayingLength > 0 && IsUserPressedPause == false ){
                    playbackCurrentSong();
                }
                break;
            case AudioManager.AUDIOFOCUS_LOSS:
                // TODO : stop playback and release media player
                //Log.d(LoggerName, "AUDIOFOCUS_LOSS = "+focusChange);
                CurrentVolumeLevel = _AudioManager.getStreamVolume(AudioManager.STREAM_MUSIC);
                pauseCurrentSong();
                broadCast_OnDemand(false);
                //Log.d(LoggerName, "Paused");
                break;
            case AudioManager.AUDIOFOCUS_LOSS_TRANSIENT:
                // TODO : pause palyback
                //Log.d(LoggerName, "AUDIOFOCUS_LOSS_TRANSIENT = "+focusChange);
                CurrentVolumeLevel = _AudioManager.getStreamVolume(AudioManager.STREAM_MUSIC);
                pauseCurrentSong();
                broadCast_OnDemand(false);
                break;
            case AudioManager.AUDIOFOCUS_LOSS_TRANSIENT_CAN_DUCK:
                // TODO : continue playing at an attenuated level
                // TODO : pause palyback
                //Log.d(LoggerName, "AUDIOFOCUS_LOSS_TRANSIENT_CAN_DUCK = "+focusChange);
                CurrentVolumeLevel = _AudioManager.getStreamVolume(AudioManager.STREAM_MUSIC);
                pauseCurrentSong();
                broadCast_OnDemand(false);
                break;
        }
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    public int getMusicCurrrentPosition(){
        if(mediaPlayerState == MediaPlayerState.Paused)
            return CurrentPlayingLength;

        return player.getCurrentPosition();
    }
    public int getMusicDuration(){
        /// Get total length using mediaplayer object.
        return player.getDuration();
    }
    public MusicDictionary getCurrent_MusicDictionary(){
        return Current_MusicDictionary;
    }
    public MusicDictionary getCurrent_ReInitialized_MusicDictionary(){
        MusicDictionary _MusicDictionary = getCurrent_MusicDictionary();
        for(int i=0; i<List_MusicDictionary.size(); i++){
            if(List_MusicDictionary.get(i).ID == _MusicDictionary.ID) {
                List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Playing;
            }else if(List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Played;
            }
        }
        return _MusicDictionary;
    }
    public List<MusicDictionary> getList(){
        return List_MusicDictionary;
    }
    private void getHandler(){
        //Music Handler for methods
        Handler_Music = new Handler();
        Runnable_Music = new Runnable() {
            @Override
            public void run() {
                Runtime.getRuntime().gc();
                if(mediaPlayerState == MediaPlayerState.Started || mediaPlayerState == MediaPlayerState.Paused){
                    broadCast_Forever();
                }
                Handler_Music.postDelayed(this, 100);
            }
        };
        Handler_Music.postDelayed(Runnable_Music, 100);
    }
    public void setCurrent_MusicDictionary(MusicDictionary _MusicDictionary){
        Current_MusicDictionary = _MusicDictionary;
    }
    private void showCustomNotifications(){
        if(IsStopSong){
            stopForeground(true);
            stopSelf();
            return;
        }

        MusicDictionary _MusicDictionary = getCurrent_MusicDictionary();

        Intent notificationIntent = new Intent(this, MainActivity.class);
        notificationIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        PendingIntent PendingIntent_OpenMainActivity = PendingIntent.getActivity(this, NotificationID.getID(), notificationIntent, 0);

        Intent previousIntent = new Intent(this, MusicService.class);
        previousIntent.setAction(Constants.ACTION.PREV_ACTION);
        PendingIntent PendingIntent_previousIntent = PendingIntent.getService(this, NotificationID.getID(), previousIntent, 0);

        Intent playBackIntent = new Intent(this, MusicService.class);
        playBackIntent.setAction(Constants.ACTION.PLAYBACK_ACTION);
        PendingIntent PendingIntent_playBackIntent = PendingIntent.getService(this, NotificationID.getID(), playBackIntent, 0);

        Intent pauseIntent = new Intent(this, MusicService.class);
        pauseIntent.setAction(Constants.ACTION.PAUSE_ACTION);
        PendingIntent PendingIntent_pauseIntent = PendingIntent.getService(this, NotificationID.getID(), pauseIntent, 0);

        Intent nextIntent = new Intent(this, MusicService.class);
        nextIntent.setAction(Constants.ACTION.NEXT_ACTION);
        PendingIntent PendingIntent_nextIntent = PendingIntent.getService(this, NotificationID.getID(), nextIntent, 0);

        Intent closeIntent = new Intent(this, MusicService.class);
        closeIntent.setAction(Constants.ACTION.STOPFOREGROUND_ACTION);
        PendingIntent PendingIntent_closeIntent = PendingIntent.getService(this, NotificationID.getID(), closeIntent, 0);

        RemoteViews _RemoteViews = new RemoteViews(getApplicationContext().getPackageName(), R.layout.customized_notification);
        _RemoteViews.setImageViewResource(R.id.imgSongImage, getResources().getIdentifier( _MusicDictionary.AlbumArt, "drawable", getPackageName()));
        _RemoteViews.setTextViewText(R.id.txtMyanmarInfo, _MusicDictionary.MyanmarTitle);
        _RemoteViews.setTextViewText(R.id.txtEnglishInfo, _MusicDictionary.EnglishTitle);
        _RemoteViews.setImageViewResource(R.id.btnClose, R.drawable.font_awesome_btnclose);
        _RemoteViews.setImageViewResource(R.id.btnBackward, R.drawable.font_awesome_btnbackward);
        _RemoteViews.setImageViewResource(R.id.btnForward, R.drawable.font_awesome_btnforward);
        _RemoteViews.setOnClickPendingIntent(R.id.imgSongImage, PendingIntent_OpenMainActivity);
        _RemoteViews.setOnClickPendingIntent(R.id.btnClose, PendingIntent_closeIntent);
        _RemoteViews.setOnClickPendingIntent(R.id.btnBackward, PendingIntent_previousIntent);
        _RemoteViews.setOnClickPendingIntent(R.id.btnForward, PendingIntent_nextIntent);

        if(player.isPlaying()){
            _RemoteViews.setImageViewResource(R.id.btnPlayPause, R.drawable.font_awesome_btnpause);
            _RemoteViews.setOnClickPendingIntent(R.id.btnPlayPause, PendingIntent_pauseIntent);
        }else{
            _RemoteViews.setImageViewResource(R.id.btnPlayPause, R.drawable.font_awesome_btnplay);
            _RemoteViews.setOnClickPendingIntent(R.id.btnPlayPause, PendingIntent_playBackIntent);
        }


        RemoteViews _RemoteViews_Minimized = new RemoteViews(getApplicationContext().getPackageName(), R.layout.customized_notification_minimized);
        _RemoteViews_Minimized.setImageViewResource(R.id.imgSongImage, getResources().getIdentifier( _MusicDictionary.AlbumArt, "drawable", getPackageName()));
        _RemoteViews_Minimized.setTextViewText(R.id.txtInfo, _MusicDictionary.MyanmarTitle +" - "+ _MusicDictionary.EnglishTitle );
        _RemoteViews_Minimized.setImageViewResource(R.id.btnClose, R.drawable.font_awesome_btnclose);
        _RemoteViews_Minimized.setImageViewResource(R.id.btnBackward, R.drawable.font_awesome_btnbackward);
        _RemoteViews_Minimized.setImageViewResource(R.id.btnForward, R.drawable.font_awesome_btnforward);
        _RemoteViews_Minimized.setOnClickPendingIntent(R.id.imgSongImage, PendingIntent_OpenMainActivity);
        _RemoteViews_Minimized.setOnClickPendingIntent(R.id.btnCloseLayout, PendingIntent_closeIntent);
        _RemoteViews_Minimized.setOnClickPendingIntent(R.id.btnBackwardLayout, PendingIntent_previousIntent);
        _RemoteViews_Minimized.setOnClickPendingIntent(R.id.btnForwardLayout, PendingIntent_nextIntent);

        if(player.isPlaying()){
            _RemoteViews_Minimized.setImageViewResource(R.id.btnPlayPause, R.drawable.font_awesome_btnpause);
            _RemoteViews_Minimized.setOnClickPendingIntent(R.id.btnPlayPauseLayout, PendingIntent_pauseIntent);
        }else{
            _RemoteViews_Minimized.setImageViewResource(R.id.btnPlayPause, R.drawable.font_awesome_btnplay);
            _RemoteViews_Minimized.setOnClickPendingIntent(R.id.btnPlayPauseLayout, PendingIntent_playBackIntent);
        }

        int icon = R.drawable.small_white_icon;
        long when = System.currentTimeMillis();
        Notification notification = new NotificationCompat.Builder(this)
                .setPriority(NotificationCompat.PRIORITY_MAX)
                .setSmallIcon(icon)
                .setOngoing(true)
                .setWhen(when)
                .setDeleteIntent(PendingIntent_closeIntent)
                .build();

        notification.bigContentView = _RemoteViews;
        notification.contentView = _RemoteViews_Minimized;

        notification.flags |= Notification.DEFAULT_SOUND;
        startForeground(Constants.NOTIFICATION_ID.FOREGROUND_SERVICE, notification);
    }
    private void broadCast_Forever(){
        /// Every seconds
        /// --------------------
        /// CurrentSongPlayingIndex
        /// IsSeekbarSeekable
        MusicDictionary Current_MusicDictionary = getCurrent_MusicDictionary();
        Intent intent_Broadcast_Forever = new Intent(Constants.BROADCAST.FOREVER_BROADCAST);
        intent_Broadcast_Forever.putExtra("CurrentSongPlayingIndex", getMusicCurrrentPosition());
        intent_Broadcast_Forever.putExtra("IsSeekbarSeekable", mediaPlayerState == MediaPlayerState.Started || mediaPlayerState == MediaPlayerState.Paused);
        LocalBroadcastManager.getInstance(getApplicationContext()).sendBroadcast(intent_Broadcast_Forever);
    }
    private void broadCast_OnDemand(Boolean IsClose) {
        //// Once for a song
        /// ---------------------
        /// CurrentSongTotalLength
        /// Updated List_MusicDictionary
        ///     MyanmarTitle
        ///     EnglishTitle
        String Using_List_MusicDictionary = gson.toJson(getList());
        MusicDictionary Current_MusicDictionary = getCurrent_MusicDictionary();
        Intent intent_Broadcast_OnDemand = new Intent(Constants.BROADCAST.ONDEMAND_BROADCAST);
        intent_Broadcast_OnDemand.putExtra("CurrentSongTotalLength", IsStopSong ? "0" : getMusicDuration());
        intent_Broadcast_OnDemand.putExtra("Using_List_MusicDictionary", Using_List_MusicDictionary);
        intent_Broadcast_OnDemand.putExtra("MyanmarTitle", IsStopSong ? "" : Current_MusicDictionary.MyanmarTitle);
        intent_Broadcast_OnDemand.putExtra("EnglishTitle", IsStopSong ? "" : Current_MusicDictionary.EnglishTitle);
        intent_Broadcast_OnDemand.putExtra("IsClose", IsClose);
        intent_Broadcast_OnDemand.putExtra("IsStopSong", IsStopSong);
        intent_Broadcast_OnDemand.putExtra("IsPause", CurrentPlayingLength > 0);
        LocalBroadcastManager.getInstance(getApplicationContext()).sendBroadcast(intent_Broadcast_OnDemand);
        if(!IsClose)
            showCustomNotifications();
    }
    private void update_MusicDictionary(MusicDictionary _MusicDictionary){
        for(int i=0; i<List_MusicDictionary.size(); i++){
            if(List_MusicDictionary.get(i).ID == _MusicDictionary.ID){
                List_MusicDictionary.get(i).IsFavorite = _MusicDictionary.IsFavorite;
            }
        }
        IsUserUpdateList = true;
    }
    public void playbackCurrentSong(){
        /// Play song after pause
        //Log.d(LoggerName, "Invoke playback");
        playSong(GetSongToPlay(PlayerEventName.CurrentPlayingSong));
    }
    public void playIndexedSong(){
        playSong(getCurrent_ReInitialized_MusicDictionary());
    }
    public void pauseCurrentSong(){
        player.pause();
        mediaPlayerState = MediaPlayerState.Paused;
        CurrentPlayingLength = player.getCurrentPosition();
    }
    private MusicDictionary getCurrentAndNextSong(List<MusicDictionary> List_MusicDictionary,  int index){
        List_MusicDictionary.get(index).PlayingStatus = PlayingStatus_Playing;
        return List_MusicDictionary.get(index);
    }
    private MusicDictionary getPreviousSong(List<MusicDictionary> List_MusicDictionary,  int index){
        List_MusicDictionary.get(index).PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.get(index-1).PlayingStatus = PlayingStatus_Playing;
        return List_MusicDictionary.get(index-1);
    }
    private MusicDictionary GetSongToPlay(PlayerEventName _PlayerEventName){
        Boolean IsFavoriteOn = false;
        Boolean IsShuffleOn = false;
        Boolean IsPlayingSongAlreadyExist = false;
        MusicDictionary ToReturn_MusicDictionary = null;
        Filterable_List_MusicDictionary = List_MusicDictionary;

        Setting _Setting = _DatabaseHandler.getPlayerSetting();
        IsFavoriteOn = _Setting.IsFavoriteOn;
        IsShuffleOn = _Setting.IsShuffleOn;

        if(IsFavoriteOn && CurrentPlayingLength <= 0){
            Filterable_List_MusicDictionary = new ArrayList<MusicDictionary>();

            for(int i=0; i<List_MusicDictionary.size(); i++){
                if(List_MusicDictionary.get(i).IsFavorite) {
                    MusicDictionary Favorite_MusicDictionary = List_MusicDictionary.get(i);
                    Filterable_List_MusicDictionary.add(Favorite_MusicDictionary);
                }else if(List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                    List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Played;
                }
            }

        }

        if(IsShuffleOn && CurrentPlayingLength <= 0)
        {
            Filterable_List_MusicDictionary = ShuffleSongs(Filterable_List_MusicDictionary);
        }


        switch (_PlayerEventName) {
            case NextSong:
                // Play next song(s)
                //Log.d(LoggerName, "Filterable_List_MusicDictionary.size() "+ Filterable_List_MusicDictionary.size());
                for(int i=0; i < Filterable_List_MusicDictionary.size(); i++)
                {
                    if(Filterable_List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                        IsPlayingSongAlreadyExist = true;
                        Filterable_List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Played;

                        if((i+1) >= Filterable_List_MusicDictionary.size() )
                            return getCurrentAndNextSong(Filterable_List_MusicDictionary, 0);

                        /// Next song
                        return getCurrentAndNextSong(Filterable_List_MusicDictionary, (i+1));
                    }
                }

                if(!IsPlayingSongAlreadyExist){
                    /// Play first song(s)
                    return getCurrentAndNextSong(Filterable_List_MusicDictionary, 0);
                }
                break;
            case CurrentPlayingSong:
                for(int i=0; i<Filterable_List_MusicDictionary.size(); i++)
                {
                    if(Filterable_List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                        return getCurrentAndNextSong(Filterable_List_MusicDictionary, i);
                    }
                }
                break;
            case PreviousSong:
                if(Filterable_List_MusicDictionary == null) return null;
                for(int i=0; i<Filterable_List_MusicDictionary.size(); i++)
                {
                    if(Filterable_List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                        if(i == 0) {
                            return null;
                        }
                        return getPreviousSong(Filterable_List_MusicDictionary, i);
                    }
                }
                break;
            default:
                throw new IllegalArgumentException("Invalid PlayerEvent(s).");
        }
        return ToReturn_MusicDictionary;
    }
    private void stopSong(){
        IsStopSong = true;
        CurrentPlayingLength = 0;
        MusicDictionary Current_MusicDictionary = getCurrent_MusicDictionary();
        for(int i=0; i<List_MusicDictionary.size(); i++)
        {
            if(List_MusicDictionary.get(i).ID == Current_MusicDictionary.ID){
                List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Played;
            }
        }
        setCurrent_MusicDictionary(new MusicDictionary());
        broadCast_OnDemand(false);
    }
    private void playSong(MusicDictionary _MusicDictionary){
        try {
            if(_MusicDictionary == null){
                return;
            }

            _AudioManager.abandonAudioFocus(this);
            AudioFocusRequestCode = _AudioManager.requestAudioFocus(this, AudioManager.STREAM_MUSIC, AudioManager.AUDIOFOCUS_GAIN);
            if(AudioFocusRequestCode != AudioManager.AUDIOFOCUS_REQUEST_GRANTED)
                return;

            //Log.d(LoggerName, "playSong AudioManager.AUDIOFOCUS_REQUEST_GRANTED and code is " + AudioFocusRequestCode);
            String path = "android.resource://"+getPackageName()+"/raw/"+_MusicDictionary.FileName;
            if (CurrentPlayingLength > 0) {
                player.seekTo(CurrentPlayingLength);
                player.setOnSeekCompleteListener(new MediaPlayer.OnSeekCompleteListener() {
                    @Override
                    public void onSeekComplete(MediaPlayer MediaPlayer_onSeekComplete) {
                        CurrentPlayingLength = 0;
                        player.start();
                        mediaPlayerState = MediaPlayerState.Started;
                        IsUserPressedPause = false;
                        broadCast_OnDemand(false);
                    }
                });
            }
            else
            {
                /// Just playing song from start of the length
                /// MediaPlayer initialization
                if (player != null){
                    player.reset();
                    mediaPlayerState = MediaPlayerState.Idle;
                }
                else{
                    player = new MediaPlayer();
                    player.setOnErrorListener(new MediaPlayer.OnErrorListener() {
                        public boolean onError(MediaPlayer _MediaPlayer, int what, int extra) {
                            Log.e(LoggerName, String.format("[player.setOnErrorListener] Error(%s%s)", what, extra));
                            mediaPlayerState = MediaPlayerState.Error;
                            StringBuilder sb = new StringBuilder();
                            sb.append("Media Player Error: ");
                            switch (what) {
                                case MediaPlayer.MEDIA_ERROR_NOT_VALID_FOR_PROGRESSIVE_PLAYBACK:
                                    sb.append("Not Valid for Progressive Playback");
                                    break;
                                case MediaPlayer.MEDIA_ERROR_SERVER_DIED:
                                    sb.append("Server Died");
                                    break;
                                case MediaPlayer.MEDIA_ERROR_UNKNOWN:
                                    sb.append("Unknown");
                                    break;
                                default:
                                    sb.append(" Non standard (");
                                    sb.append(what);
                                    sb.append(")");
                            }
                            sb.append(" (" + what + ") ");
                            sb.append(extra);
                            Log.e(LoggerName, sb.toString());
                            return true;
                        }
                    });
                    player.setAudioStreamType(AudioManager.STREAM_MUSIC);
                    player.setWakeMode(getApplicationContext(), PowerManager.PARTIAL_WAKE_LOCK);
                }
                player.setDataSource(getApplicationContext(), Uri.parse(path));
                mediaPlayerState = MediaPlayerState.Initialized;
                setCurrent_MusicDictionary(_MusicDictionary);
                player.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                    @Override
                    public void onPrepared(MediaPlayer MediaPlayer_onPrepared) {
                        mediaPlayerState = MediaPlayerState.Prepared;
                        /// MediaPlayer
                        player.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
                            public void onCompletion(MediaPlayer MediaPlayer_onCompletion) {
                                mediaPlayerState = MediaPlayerState.Completed;
                                player.reset();
                                mediaPlayerState = MediaPlayerState.Idle;
                                CommitNavigation(InvokeNavigator());
                            }
                        });
                        player.start();
                        mediaPlayerState = MediaPlayerState.Started;
                        IsUserPressedPause = false;
                        broadCast_OnDemand(false);
                    }
                });
                player.prepareAsync();
                mediaPlayerState = MediaPlayerState.Preparing;
            }
        } catch (IllegalArgumentException e){
            Log.e(LoggerName, "[MusicService].[playSong] IllegalArgumentException Error = "+ e.getMessage());
            e.printStackTrace();
        } catch (SecurityException e) {
            Log.e(LoggerName, "[MusicService].[playSong] SecurityException Error = "+ e.getMessage());
            e.printStackTrace();
        } catch (IllegalStateException e) {
            Log.e(LoggerName, "[MusicService].[playSong] IllegalStateException Error = "+ e.getMessage());
            e.printStackTrace();
        } catch (IOException e) {
            Log.e(LoggerName, "[MusicService].[playSong] IOException Error = "+ e.getMessage());
            e.printStackTrace();
        }
    }
    private void CommitNavigation(Navigator _Navigator){
        switch (_Navigator._NavigatorValue){
            case ReplayCurrentSong:
                playSong(getCurrent_ReInitialized_MusicDictionary());
                break;
            case Next:
                playSong(GetSongToPlay(PlayerEventName.NextSong));
                break;
            case Stop:
                stopSong();
                break;
        }
    }
    private Navigator InvokeNavigator(){
        Navigator _Navigator = new Navigator();
        String _RepeatStatus = _DatabaseHandler.getPlayerSetting().RepeatStatus;
        _Navigator._NavigatorValue = NavigatorValue.Next;
        if(_RepeatStatus.equalsIgnoreCase(RepeatStatus.Single) && getCurrent_MusicDictionary() != null){
            _Navigator._NavigatorValue = NavigatorValue.ReplayCurrentSong;
        }else if(_RepeatStatus.equalsIgnoreCase(RepeatStatus.ALL)){
            _Navigator._NavigatorValue = NavigatorValue.Next;
        }else if(_RepeatStatus.equalsIgnoreCase(RepeatStatus.None)){
            if(IsLastSongOfTheList()){
                _Navigator._NavigatorValue = NavigatorValue.Stop;
            }else{
                _Navigator._NavigatorValue = NavigatorValue.Next;
            }
        }
        return  _Navigator;
    }
    private Boolean IsLastSongOfTheList(){
        Boolean IsLastSong = false;
        MusicDictionary _MusicDictionary = getCurrent_MusicDictionary();
        List<MusicDictionary> _ToCheckList = null;
        if(Filterable_List_MusicDictionary.size() > 0){
            _ToCheckList = Filterable_List_MusicDictionary;
        }else{
            _ToCheckList = List_MusicDictionary;
        }

        if(_MusicDictionary == null)
            return IsLastSong;

        for(int i=0; i<_ToCheckList.size(); i++){
            if(_MusicDictionary.ID == _ToCheckList.get(i).ID
                    && (i+1) == _ToCheckList.size() ){
                IsLastSong = true;
                return IsLastSong;
            }
        }
        return IsLastSong;

    }
    private List<MusicDictionary> ShuffleSongs(List<MusicDictionary> ToShuffleList){
        if(ShuffledList == null){
            ShuffledList = new ArrayList<MusicDictionary>(ToShuffleList);
            Collections.shuffle(ShuffledList);
            //Log.d(LoggerName, "ShuffleSongs() (ShuffledList == null)");
        }else if(IsUserUpdateList){
            ShuffledList = new ArrayList<MusicDictionary>(ToShuffleList);
            Collections.shuffle(ShuffledList);
            IsUserUpdateList = false;
            //Log.d(LoggerName, "ShuffleSongs() (IsUserUpdateList)");
        }else if(ToShuffleList.size() !=  ShuffledList.size()){
            ShuffledList = new ArrayList<MusicDictionary>(ToShuffleList);
            Collections.shuffle(ShuffledList);
            //Log.d(LoggerName, "ShuffleSongs() (ToShuffleList.size() !=  ShuffledList.size())");
        }
        return  ShuffledList;
    }
    private void InvokeMediaSessions(){

    }
    //<!-- End developer defined function(s).  -->
}