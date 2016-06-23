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
    private MediaPlayer player;
    private List<MusicDictionary> List_MusicDictionary;
    private MusicDictionary Current_MusicDictionary;
    private int SongID;
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
            playSong(GetSongToPlay(true, false, false, true, false));
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
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    public void playbackCurrentSong(){
        //playSong(GetSongToPlay(true, true, true));
        /// Play song after pause
        if(player!=null){
            player.stop();
            player.release();
            player = null;
        }
        playSong(GetSongToPlay(false, false, true, true, false));
    }
    public void pauseCurrentSong(){
        player.pause();
        CurrentPlayingLength = player.getCurrentPosition();
        IsMediaPlayerReady = false;
        player.stop();
        player.release();
        player = null;
    }
    public void setList(List<MusicDictionary> _List_MusicDictionary){
        List_MusicDictionary = _List_MusicDictionary;
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
    public  MusicDictionary getCurrent_MusicDictionary(){
        return Current_MusicDictionary;
    }
    private void setCurrent_MusicDictionary(MusicDictionary _MusicDictionary){
        Current_MusicDictionary = _MusicDictionary;
    }
    public MusicDictionary GetSongToPlay(
                                        Boolean IsFirstSong,
                                        Boolean IsNextSong,
                                        Boolean IsCurrentPlayingSong,
                                        Boolean IsRepeatAlbum,
                                        Boolean IsShuffle){
        MusicDictionary ToReturn_MusicDictionary = null;
        if(IsFirstSong){
            for(int i=0; i<List_MusicDictionary.size(); i++){
                if(List_MusicDictionary.get(i).Srno == 1){
                    List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Playing;
                    ToReturn_MusicDictionary = List_MusicDictionary.get(i);
                    return ToReturn_MusicDictionary;
                }
            }
        }else if(IsNextSong){
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
        }else if(IsCurrentPlayingSong){
            for(int i=0; i<List_MusicDictionary.size(); i++)
            {
                if(List_MusicDictionary.get(i).PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
                    ToReturn_MusicDictionary = List_MusicDictionary.get(i);
                    return ToReturn_MusicDictionary;
                }
            }
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
                player.setAudioStreamType(AudioManager.STREAM_MUSIC);
            }
            player.setWakeMode(getApplicationContext(), PowerManager.PARTIAL_WAKE_LOCK);
            player.setDataSource(getApplicationContext(), Uri.parse(path));
            setCurrent_MusicDictionary(_MusicDictionary);
            player.setOnErrorListener(new MediaPlayer.OnErrorListener() {
                public boolean onError(MediaPlayer _MediaPlayer, int what, int extra) {
                    Log.e(LoggerName, String.format("[player.setOnErrorListener] Error(%s%s)", what, extra));
                    return true;
                }
            });
            player.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                @Override
                public void onPrepared(MediaPlayer MediaPlayer_onPrepared) {
                    /// MediaPlayer
                    if (CurrentPlayingLength > 0) {
                        player.seekTo(CurrentPlayingLength);
                        CurrentPlayingLength = 0;
                    }

                    player.start();
                    IsMediaPlayerReady = true;
                    player.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
                        public void onCompletion(MediaPlayer MediaPlayer_onCompletion) {
                            player.stop();
                            player.release();
                            player = null;

                            IsMediaPlayerReady = false;
                            playSong(GetSongToPlay(false, true, false, IsRepeatAlbum, IsShuffle));
                        }
                    });
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