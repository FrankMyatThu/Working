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
implements
MediaPlayer.OnPreparedListener,
MediaPlayer.OnCompletionListener,
MediaPlayer.OnErrorListener
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
        Log.d(LoggerName, "[MusicService].[onCreate]");
        super.onCreate();
        initializeMusicPlayer();
    }
    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d(LoggerName, "[MusicService].[onStartCommand]");
        if(intent == null) {
            stopForeground(true);
            stopSelf();
            return START_STICKY;
        }
        if (intent.getAction().equals(Constants.ACTION.STARTFOREGROUND_ACTION)) {
            Log.d(LoggerName, "Received Start Foreground Intent ");
            playSong(GetSongToPlay(true, true, true));
        }else if (intent.getAction().equals(Constants.ACTION.COMING_BACK)) {
            Log.d(LoggerName, "Coming Back");
        } else if (intent.getAction().equals(Constants.ACTION.PREV_ACTION)) {
            Log.d(LoggerName, "Clicked Previous");
        } else if (intent.getAction().equals(Constants.ACTION.PLAY_ACTION)) {
            Log.d(LoggerName, "Clicked Play");
        } else if (intent.getAction().equals(Constants.ACTION.NEXT_ACTION)) {
            Log.d(LoggerName, "Clicked Next");
        } else if (intent.getAction().equals(Constants.ACTION.STOPFOREGROUND_ACTION)) {
            Log.d(LoggerName, "Received Stop Foreground Intent");
            stopForeground(true);
            stopSelf();
        }
        return START_STICKY;
    }
    @Override
    public IBinder onBind(Intent intent) {
        Log.d(LoggerName, "[MusicService].[onBind] _MusicBinder" + _MusicBinder);
        return _MusicBinder;
    }
    @Override
    public boolean onUnbind(Intent intent){
        if(player.isPlaying())
            return false;

        player.stop();
        player.release();
        player = null;
        return false;
    }
    @Override
    public void onPrepared(MediaPlayer _MediaPlayer) {
        //start playback
        _MediaPlayer.start();
    }
    @Override
    public void onCompletion(MediaPlayer _MediaPlayer) {
        _MediaPlayer.release();
        _MediaPlayer = null;
        Log.d(LoggerName, "[MusicService].[onCompletion] Finish and Released object.");
        playSong(GetSongToPlay(false, IsRepeatAlbum, IsShuffle));
    }
    @Override
    public boolean onError(MediaPlayer _MediaPlayer, int what, int extra) {
        return false;
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    public void initializeMusicPlayer(){
        player = new MediaPlayer();
        //set player properties
        player.setWakeMode(getApplicationContext(),
                PowerManager.PARTIAL_WAKE_LOCK);
        //set listeners
        player.setOnPreparedListener(this);
        player.setOnCompletionListener(this);
        player.setOnErrorListener(this);
    }
    public void setList(List<MusicDictionary> _List_MusicDictionary){
        List_MusicDictionary = _List_MusicDictionary;
    }
    public boolean IsMediaPlayerObjectAvailable(){
        return (player != null);
    }
    public boolean IsPlayingSong(){
        return player.isPlaying();
    }
    public int getMusicDuration(){
        /// Get total length using mediaplayer object.
        return player.getDuration();

        /// Get total length using MediaMetadataRetriever object.
        /*String path = "android.resource://"+getPackageName()+"/raw/"+ getCurrent_MusicDictionary().FileName;
        Uri uri = Uri.parse(path);
        MediaMetadataRetriever mmr = new MediaMetadataRetriever();
        mmr.setDataSource(getApplicationContext(),uri);
        String durationString = mmr.extractMetadata(MediaMetadataRetriever.METADATA_KEY_DURATION);
        int Duration_millSecond = Integer.parseInt(durationString);
        return  Duration_millSecond;*/

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
        Log.d(LoggerName, "Current_MusicDictionary.EnglishTitle = " + Current_MusicDictionary.EnglishTitle);
    }
    public MusicDictionary GetSongToPlay(Boolean IsFirstSong, Boolean IsRepeatAlbum, Boolean IsShuffle){
        Log.d(LoggerName, "GetSongToPlay List_MusicDictionary.size() ="+List_MusicDictionary.size());
        MusicDictionary ToReturn_MusicDictionary = null;
        if(IsFirstSong){
            for(int i=0; i<List_MusicDictionary.size(); i++){
                if(List_MusicDictionary.get(i).Srno == 1){
                    List_MusicDictionary.get(i).PlayingStatus = PlayingStatus_Playing;
                    ToReturn_MusicDictionary = List_MusicDictionary.get(i);
                    return ToReturn_MusicDictionary;
                }
            }
        }else
        {
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
                            Log.d(LoggerName, "Get first song of the album because of repeated flag is on.");
                            List_MusicDictionary.get(0).PlayingStatus = PlayingStatus_Playing;
                            ToReturn_MusicDictionary = List_MusicDictionary.get(0);
                            Log.d(LoggerName, "Get first song of the album because of repeated flag is on. ToReturn_MusicDictionary.EnglishTitle = "+ToReturn_MusicDictionary.EnglishTitle);
                            return ToReturn_MusicDictionary;
                        }
                        return  ToReturn_MusicDictionary; // Null value will be returned because of it arrives to end of the album.
                    }else
                    {
                        /// Next song
                        List_MusicDictionary.get(i+1).PlayingStatus = PlayingStatus_Playing;
                        ToReturn_MusicDictionary = List_MusicDictionary.get(i+1);
                        Log.d(LoggerName, "Next Song is "+ ToReturn_MusicDictionary.EnglishTitle);
                        return ToReturn_MusicDictionary;
                    }
                }else
                {
                    Log.d(LoggerName , "Title = "+ List_MusicDictionary.get(i).EnglishTitle +" : Status = "+ List_MusicDictionary.get(i).PlayingStatus);
                }
            }
        }
        return ToReturn_MusicDictionary;
    }
    public void playSong(MusicDictionary _MusicDictionary){
        String path = "android.resource://"+getPackageName()+"/raw/"+_MusicDictionary.FileName;
        Log.d(LoggerName, "[MusicService].[playSong] path = " + path);
        try {

            Log.d(LoggerName, "[MusicService].[playSong] CurrentPlayingLength = " + CurrentPlayingLength);

            if (CurrentPlayingLength > 0) {
                /// Play song after pause
                player.seekTo(CurrentPlayingLength);
                player.start();
                CurrentPlayingLength = 0;
            } else {

                Log.d(LoggerName, "[MusicService].[playSong] player = " + player);

                /// Just playing song from start of the length
                /// MediaPlayer initialization
                initializeMusicPlayer();
                player.setDataSource(getApplicationContext(), Uri.parse(path));
                player.setAudioStreamType(AudioManager.STREAM_MUSIC);
                setCurrent_MusicDictionary(_MusicDictionary);
                player.prepareAsync();
            }

        } catch (IllegalArgumentException e){
            Log.e(LoggerName, "[MusicService].[playSong] Error = "+ e.getMessage());
            e.printStackTrace();
        } catch (IOException e) {
            Log.e(LoggerName, "[MusicService].[playSong] Error = "+ e.getMessage());
            e.printStackTrace();
        }
    }
    //<!-- End developer defined function(s).  -->
}