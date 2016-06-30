package ninzimay.mediaplayer.ninzimay;

import android.app.Service;
import android.content.ContentUris;
import android.content.Intent;
import android.content.SharedPreferences;
import android.media.AudioManager;
import android.media.MediaMetadataRetriever;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Binder;
import android.os.Handler;
import android.os.IBinder;
import android.os.PowerManager;
import android.support.v4.content.LocalBroadcastManager;
import android.util.Log;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

/**
 * Created by Administrator on 2016/06/19.
 */
public class MusicService extends Service
{
    //<!-- Start declaration area.  -->
    private String LoggerName = "NinZiMay";
    private String PlayingStatus_New = "PlayingStatus_New";
    private String PlayingStatus_Playing = "PlayingStatus_Playing";
    private String PlayingStatus_Played = "PlayingStatus_Played";
    private enum PlayerEventName {
        FirstPlaying, NextSong, CurrentPlayingSong, PreviousSong;
    }
    private MediaPlayerState mediaPlayerState;
    private MediaPlayer player = null;
    private List<MusicDictionary> List_MusicDictionary;
    private MusicDictionary Current_MusicDictionary;
    private int CurrentPlayingLength = 0;
    private boolean IsRepeatAlbum = true;
    private boolean IsShuffle = false;
    private Handler Handler_Music = null;
    private Runnable Runnable_Music = null;
    private Gson gson = new Gson();
    //<!-- End declaration area.  -->

    //<!-- Start dependency object(s).  -->
    //<!-- End dependency object(s).  -->

    //<!-- Start system defined function(s).  -->
    public void onCreate(){
        //Log.d(LoggerName, "Service.onCreate");
        super.onCreate();
        getHandler();
    }
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        if (intent.getAction().equals(Constants.ACTION.PREV_ACTION)) {
            playSong(GetSongToPlay(PlayerEventName.PreviousSong, IsRepeatAlbum, IsShuffle));
        }else if (intent.getAction().equals(Constants.ACTION.PLAY_ACTION)) {
            if(List_MusicDictionary == null){
                String Initial_List_MusicDictionary = intent.getExtras().get("Initial_List_MusicDictionary").toString();
                List_MusicDictionary = gson.fromJson(Initial_List_MusicDictionary, new TypeToken<List<MusicDictionary>>(){}.getType());
            }
            playSong(GetSongToPlay(PlayerEventName.FirstPlaying, IsRepeatAlbum, IsShuffle));
        }else if (intent.getAction().equals(Constants.ACTION.PLAYBACK_ACTION)) {
            playbackCurrentSong();
        }else if (intent.getAction().equals(Constants.ACTION.PAUSE_ACTION)) {
            pauseCurrentSong();
        }else if (intent.getAction().equals(Constants.ACTION.NEXT_ACTION)) {
            if(List_MusicDictionary == null){
                String Initial_List_MusicDictionary = intent.getExtras().get("Initial_List_MusicDictionary").toString();
                List_MusicDictionary = gson.fromJson(Initial_List_MusicDictionary, new TypeToken<List<MusicDictionary>>(){}.getType());
            }
            playSong(GetSongToPlay(PlayerEventName.NextSong, IsRepeatAlbum, IsShuffle));
        }else if (intent.getAction().equals(Constants.ACTION.INDEXED_SONG_ACTION)) {
            if(List_MusicDictionary == null){
                String Initial_List_MusicDictionary = intent.getExtras().get("Initial_List_MusicDictionary").toString();
                List_MusicDictionary = gson.fromJson(Initial_List_MusicDictionary, new TypeToken<List<MusicDictionary>>(){}.getType());
            }
            String Current_MusicDictionary = intent.getExtras().get("Current_MusicDictionary").toString();
            setCurrent_MusicDictionary(gson.fromJson(Current_MusicDictionary, MusicDictionary.class));
            playSong(getCurrent_ReInitialized_MusicDictionary());
        }else if (intent.getAction().equals(Constants.ACTION.INDEXED_SEEK_ACTION)) {
            CurrentPlayingLength =  Integer.parseInt(intent.getExtras().get("seekBarIndex").toString());
            playIndexedSong();
        }else if (intent.getAction().equals(Constants.ACTION.STOPFOREGROUND_ACTION)) {
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
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    public int getMusicCurrrentPosition(){
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
                if(mediaPlayerState == MediaPlayerState.Started || mediaPlayerState == MediaPlayerState.Paused){
                    broadCast_Forever();
                }
                Handler_Music.postDelayed(this, 100);
            }
        };
        Handler_Music.postDelayed(Runnable_Music, 100);
    }
    public void setList(List<MusicDictionary> _List_MusicDictionary){
        //SQLiteOpenHelper http://hmkcode.com/android-simple-sqlite-database-tutorial/
        List_MusicDictionary = _List_MusicDictionary;
    }
    public void setCurrent_MusicDictionary(MusicDictionary _MusicDictionary){
        Current_MusicDictionary = _MusicDictionary;
    }
    private void broadCast_Forever(){
        /// Every seconds
        /// --------------------
        /// CurrentSongPlayingIndex
        /// IsSeekbarSeekable
        MusicDictionary Current_MusicDictionary = getCurrent_MusicDictionary();
        Intent intent_Broadcast_Forever = new Intent(Constants.BROADCAST.FOREVER_BROADCAST);
        intent_Broadcast_Forever.putExtra("CurrentSongPlayingIndex", getMusicCurrrentPosition());
        //intent_Broadcast_Forever.putExtra("CurrentSongID", Current_MusicDictionary.ID);
        intent_Broadcast_Forever.putExtra("IsSeekbarSeekable", mediaPlayerState == MediaPlayerState.Started);
        LocalBroadcastManager.getInstance(getApplicationContext()).sendBroadcast(intent_Broadcast_Forever);
    }
    private void broadCast_OnDemand() {
        //// Once for a song
        /// ---------------------
        /// CurrentSongTotalLength
        /// Updated List_MusicDictionary
        ///     MyanmarTitle
        ///     EnglishTitle
        String Using_List_MusicDictionary = gson.toJson(getList());
        MusicDictionary Current_MusicDictionary = getCurrent_MusicDictionary();
        Intent intent_Broadcast_OnDemand = new Intent(Constants.BROADCAST.ONDEMAND_BROADCAST);
        intent_Broadcast_OnDemand.putExtra("CurrentSongTotalLength", getMusicDuration());
        intent_Broadcast_OnDemand.putExtra("Using_List_MusicDictionary", Using_List_MusicDictionary);
        intent_Broadcast_OnDemand.putExtra("MyanmarTitle", Current_MusicDictionary.MyanmarTitle);
        intent_Broadcast_OnDemand.putExtra("EnglishTitle", Current_MusicDictionary.EnglishTitle);
        LocalBroadcastManager.getInstance(getApplicationContext()).sendBroadcast(intent_Broadcast_OnDemand);
    }
    public void playbackCurrentSong(){
        /// Play song after pause
        playSong(GetSongToPlay(PlayerEventName.CurrentPlayingSong, IsRepeatAlbum, IsShuffle));
    }
    public void playIndexedSong(){
        playSong(getCurrent_ReInitialized_MusicDictionary());
    }
    public void pauseCurrentSong(){
        player.pause();
        mediaPlayerState = MediaPlayerState.Paused;
        CurrentPlayingLength = player.getCurrentPosition();
    }
    public void playNextSong(){
        playSong(GetSongToPlay(PlayerEventName.NextSong, IsRepeatAlbum, IsShuffle));
    }
    public void playPreviousSong(){
        MusicDictionary _MusicDictionary = GetSongToPlay(PlayerEventName.PreviousSong, IsRepeatAlbum, IsShuffle);
        if(_MusicDictionary == null)
            return;
        playSong(_MusicDictionary);
    }
    private MusicDictionary GetSongToPlay(PlayerEventName _PlayerEventName,
                                        Boolean IsRepeatAlbum,
                                        Boolean IsShuffle){
        MusicDictionary ToReturn_MusicDictionary = null;
        boolean IsPlayingSongAlreadyExist = false;
        switch (_PlayerEventName) {
            case FirstPlaying:
                for(int i=0; i<List_MusicDictionary.size(); i++){
                    if(List_MusicDictionary.get(i).Srno == 1){
                        List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Playing;
                        ToReturn_MusicDictionary = List_MusicDictionary.get(i);
                        return ToReturn_MusicDictionary;
                    }
                }
                break;
            case NextSong:
                // Play next song(s)
                for(int i=0; i<List_MusicDictionary.size(); i++)
                {
                    if(List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                        IsPlayingSongAlreadyExist = true;
                        List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Played;

                        // Checking if current index is last index
                        if((i+1) >= List_MusicDictionary.size())
                        {
                            if(IsRepeatAlbum)
                            {
                                List_MusicDictionary.get(0).PlayingStatus = PlayingStatus_Playing;
                                ToReturn_MusicDictionary = List_MusicDictionary.get(0);
                                return ToReturn_MusicDictionary;
                            }
                            return  null;
                        }else
                        {
                            /// Next song
                            List_MusicDictionary.get(i+1).PlayingStatus = PlayingStatus_Playing;
                            ToReturn_MusicDictionary = List_MusicDictionary.get(i+1);
                            return ToReturn_MusicDictionary;
                        }
                    }else
                    {
                        //Log.d(LoggerName , "Title = "+ List_MusicDictionary.get(i).EnglishTitle +" : Status = "+ List_MusicDictionary.get(i).PlayingStatus);
                    }
                }
                if(!IsPlayingSongAlreadyExist){
                    /// Next song
                    List_MusicDictionary.get(0).PlayingStatus = PlayingStatus_Playing;
                    ToReturn_MusicDictionary = List_MusicDictionary.get(0);
                    return ToReturn_MusicDictionary;
                }
                break;
            case CurrentPlayingSong:
                for(int i=0; i<List_MusicDictionary.size(); i++)
                {
                    if(List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                        ToReturn_MusicDictionary = List_MusicDictionary.get(i);
                        return ToReturn_MusicDictionary;
                    }
                }
                break;
            case PreviousSong:
                if(List_MusicDictionary == null) return null;
                for(int i=0; i<List_MusicDictionary.size(); i++)
                {
                    if(List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                        if(i == 0){ return null; }
                        List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_New;
                        List_MusicDictionary.get(i-1).PlayingStatus = PlayingStatus_Playing;
                        ToReturn_MusicDictionary = List_MusicDictionary.get(i-1);
                        return ToReturn_MusicDictionary;
                    }
                }
                break;
            default:
                throw new IllegalArgumentException("Invalid PlayerEvent(s).");
        }
        return ToReturn_MusicDictionary;
    }
    private void playSong(MusicDictionary _MusicDictionary){
        try {
            if(_MusicDictionary == null){
                return;
            }
            String path = "android.resource://"+getPackageName()+"/raw/"+_MusicDictionary.FileName;
            if (CurrentPlayingLength > 0) {
                player.seekTo(CurrentPlayingLength);
                player.setOnSeekCompleteListener(new MediaPlayer.OnSeekCompleteListener() {
                    @Override
                    public void onSeekComplete(MediaPlayer MediaPlayer_onSeekComplete) {
                        CurrentPlayingLength = 0;
                        player.start();
                        mediaPlayerState = MediaPlayerState.Started;
                        broadCast_OnDemand();
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
                    player.setAudioStreamType(AudioManager.STREAM_MUSIC);
                    player.setWakeMode(getApplicationContext(), PowerManager.PARTIAL_WAKE_LOCK);
                }
                player.setDataSource(getApplicationContext(), Uri.parse(path));
                mediaPlayerState = MediaPlayerState.Initialized;
                setCurrent_MusicDictionary(_MusicDictionary);
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
                                playSong(GetSongToPlay(PlayerEventName.NextSong, IsRepeatAlbum, IsShuffle));
                            }
                        });
                        player.start();
                        mediaPlayerState = MediaPlayerState.Started;
                        broadCast_OnDemand();
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
    //<!-- End developer defined function(s).  -->
}