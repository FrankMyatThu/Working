package ninzimay.mediaplayer.ninzimay;

import android.app.Service;
import android.content.ContentUris;
import android.content.Intent;
import android.media.AudioManager;
import android.media.MediaMetadataRetriever;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Binder;
import android.os.IBinder;
import android.os.PowerManager;
import android.util.Log;

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
        FirstPlaying, NextSong, CurrentPlayingSong, PreviousSong, IndexedSong;
    }
    private MediaPlayer player;
    private List<MusicDictionary> List_MusicDictionary;
    private MusicDictionary Current_MusicDictionary;
    private int CurrentPlayingLength = 0;
    private boolean IsRepeatAlbum = true;
    private boolean IsShuffle = false;
    private boolean IsMediaPlayerReady = false;
    private final IBinder _MusicBinder = new MusicBinder();
    //<!-- End declaration area.  -->

    //<!-- Start dependency object(s).  -->
    public class MusicBinder extends Binder {
        MusicService getService() {
            return MusicService.this;
        }
    }
    //<!-- End dependency object(s).  -->

    //<!-- Start system defined function(s).  -->
    public void onCreate(){
        super.onCreate();
    }
    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        if(intent == null) {
            stopForeground(true);
            stopSelf();
            return START_STICKY;
        }
        if (intent.getAction().equals(Constants.ACTION.STARTFOREGROUND_ACTION)) {
            playSong(GetSongToPlay(PlayerEventName.FirstPlaying, IsRepeatAlbum, IsShuffle));
        }else if (intent.getAction().equals(Constants.ACTION.INDEXED_ACTION)) {
            playSong(getCurrent_ReInitialized_MusicDictionary());
        }else if (intent.getAction().equals(Constants.ACTION.COMING_BACK)) {
        } else if (intent.getAction().equals(Constants.ACTION.PREV_ACTION)) {
        } else if (intent.getAction().equals(Constants.ACTION.PAUSE_ACTION)) {
        } else if (intent.getAction().equals(Constants.ACTION.NEXT_ACTION)) {
        } else if (intent.getAction().equals(Constants.ACTION.STOPFOREGROUND_ACTION)) {
            stopForeground(true);
            stopSelf();
        }
        return START_STICKY;
    }
    @Override
    public IBinder onBind(Intent intent) {
        return _MusicBinder;
    }
    @Override
    public boolean onUnbind(Intent intent){
        if(player == null) return false;
        if(player.isPlaying()) return false;

        player.stop();
        player.release();
        player = null;
        return false;
    }
    @Override
    public void onDestroy() {
        if(player == null) return;
        if(player.isPlaying()) return;

        player.stop();
        player.release();
        player = null;
        return;
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    public void playbackCurrentSong(){
        /// Play song after pause
        IsMediaPlayerReady = false;
        if(player!=null){
            player.stop();
            player.release();
            player = null;
        }
        playSong(GetSongToPlay(PlayerEventName.CurrentPlayingSong, IsRepeatAlbum, IsShuffle));
    }
    public void playIndexedSong(){
        IsMediaPlayerReady = false;
        if(player!=null){
            player.stop();
            player.release();
            player = null;
        }
        playSong(getCurrent_ReInitialized_MusicDictionary());
    }
    public void pauseCurrentSong(){
        if(player == null) return;
        if(player.isPlaying()){
            player.pause();
            CurrentPlayingLength = player.getCurrentPosition();
            IsMediaPlayerReady = false;
            player.stop();
            player.release();
            player = null;
        }else
        {
            IsMediaPlayerReady = false;
            player.release();
            player = null;
        }

    }
    public void playNextSong(){
        IsMediaPlayerReady = false;
        if(player!=null){
            player.stop();
            player.release();
            player = null;
        }
        playSong(GetSongToPlay(PlayerEventName.NextSong, IsRepeatAlbum, IsShuffle));
    }
    public void playPreviousSong(){
        IsMediaPlayerReady = false;
        if(player!=null){
            player.stop();
            player.release();
            player = null;
        }
        MusicDictionary _MusicDictionary = GetSongToPlay(PlayerEventName.PreviousSong, IsRepeatAlbum, IsShuffle);
        if(_MusicDictionary == null)
            return;
        playSong(_MusicDictionary);
    }
    public void setList(List<MusicDictionary> _List_MusicDictionary){
        List_MusicDictionary = _List_MusicDictionary;
    }
    public List<MusicDictionary> getList(){
        return List_MusicDictionary;
    }
    public boolean IsMediaPlayerObjectAvailable(){
        return IsMediaPlayerReady;
    }
    public boolean IsPlayingSong(){
        if(player == null) return  false;
        if(!IsMediaPlayerReady) return false;
        return player.isPlaying();
    }
    public boolean IsPauseSong(){
        return CurrentPlayingLength > 0;
    }
    public int getMusicDuration(){
        /// Get total length using mediaplayer object.
        return player.getDuration();
    }
    public int getMusicCurrrentPosition(){
        return player.getCurrentPosition();
    }
    public void seekToMusic(int Location){
        player.seekTo(Location);
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
    public void setCurrent_MusicDictionary(MusicDictionary _MusicDictionary){
        Current_MusicDictionary = _MusicDictionary;
    }
    public MusicDictionary GetSongToPlay(PlayerEventName _PlayerEventName,
                                        Boolean IsRepeatAlbum,
                                        Boolean IsShuffle){
        MusicDictionary ToReturn_MusicDictionary = null;
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
                            return  ToReturn_MusicDictionary; // Null value will be returned because of it arrives to end of the album.
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
            case IndexedSong:
            default:
                throw new IllegalArgumentException("Invalid PlayerEvent(s).");
        }
        return ToReturn_MusicDictionary;
    }
    public void playSong(MusicDictionary _MusicDictionary){
        String path = "android.resource://"+getPackageName()+"/raw/"+_MusicDictionary.FileName;
        try {
            /// Just playing song from start of the length
            /// MediaPlayer initialization
            if (player != null){
                player.reset();
            }
            else{
                player = new MediaPlayer();
                player.reset();
            }
            player.setAudioStreamType(AudioManager.STREAM_MUSIC);
            player.setWakeMode(getApplicationContext(), PowerManager.PARTIAL_WAKE_LOCK);
            player.setDataSource(getApplicationContext(), Uri.parse(path));
            setCurrent_MusicDictionary(_MusicDictionary);
            player.setOnErrorListener(new MediaPlayer.OnErrorListener() {
                public boolean onError(MediaPlayer _MediaPlayer, int what, int extra) {
                    Log.e(LoggerName, String.format("[player.setOnErrorListener] Error(%s%s)", what, extra));
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
                    /// MediaPlayer
                    player.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
                        public void onCompletion(MediaPlayer MediaPlayer_onCompletion) {
                            player.stop();
                            player.release();
                            player = null;

                            IsMediaPlayerReady = false;
                            playSong(GetSongToPlay(PlayerEventName.NextSong, IsRepeatAlbum, IsShuffle));
                        }
                    });
                    if (CurrentPlayingLength > 0) {
                        player.seekTo(CurrentPlayingLength);
                        player.setOnSeekCompleteListener(new MediaPlayer.OnSeekCompleteListener() {
                            @Override
                            public void onSeekComplete(MediaPlayer MediaPlayer_onSeekComplete) {
                                CurrentPlayingLength = 0;
                                player.start();
                                IsMediaPlayerReady = true;
                            }
                        });
                    }else
                    {
                        player.start();
                        IsMediaPlayerReady = true;
                    }
                }
            });
            player.prepareAsync();

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