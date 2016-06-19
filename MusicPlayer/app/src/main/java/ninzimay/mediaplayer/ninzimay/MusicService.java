package ninzimay.mediaplayer.ninzimay;

import android.app.Service;
import android.content.ContentUris;
import android.content.Intent;
import android.media.AudioManager;
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

    private String LoggerName = "NinZiMay";
    private String PlayingStatus_New = "PlayingStatus_New";
    private String PlayingStatus_Playing = "PlayingStatus_Playing";
    private String PlayingStatus_Played = "PlayingStatus_Played";
    private MediaPlayer player;
    private List<MusicDictionary> List_MusicDictionary;
    private int SongID;
    private int CurrentPlayingLength = 0;
    private boolean IsRepeatAlbum = true;
    private boolean IsShuffle = false;
    private final IBinder _MusicBinder = new MusicBinder();

    public void onCreate(){
        super.onCreate();
        player = new MediaPlayer();
        initializeMusicPlayer();
    }

    public void initializeMusicPlayer(){
        //set player properties
        player.setWakeMode(getApplicationContext(),
                PowerManager.PARTIAL_WAKE_LOCK);
        player.setAudioStreamType(AudioManager.STREAM_MUSIC);
        //set listeners
        player.setOnPreparedListener(this);
        player.setOnCompletionListener(this);
        player.setOnErrorListener(this);
    }

    //pass song list
    public void setList(List<MusicDictionary> _List_MusicDictionary){
        List_MusicDictionary = _List_MusicDictionary;
    }

    //binder
    public class MusicBinder extends Binder {
        MusicService getService() {
            return MusicService.this;
        }
    }

    //activity will bind to service
    @Override
    public IBinder onBind(Intent intent) {
        Log.d(LoggerName, "[MusicService].[onBind] _MusicBinder" +_MusicBinder);
        return _MusicBinder;
    }

    //release resources when unbind
    @Override
    public boolean onUnbind(Intent intent){
        player.stop();
        player.release();
        player = null;
        return false;
    }

    // Get song to play
    public MusicDictionary GetSongToPlay(Boolean IsFirstSong, Boolean IsRepeatAlbum, Boolean IsShuffle)
    {
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
                            List_MusicDictionary.get(0).PlayingStatus = PlayingStatus_Playing;
                            ToReturn_MusicDictionary = List_MusicDictionary.get(0);
                        }
                        return ToReturn_MusicDictionary;

                    }else
                    {
                        /// Next song
                        List_MusicDictionary.get(i+1).PlayingStatus = PlayingStatus_Playing;
                        ToReturn_MusicDictionary = List_MusicDictionary.get(i+1);
                        return ToReturn_MusicDictionary;
                    }
                }
            }
        }
        return ToReturn_MusicDictionary;
    }

    //play a song
    public void playSong(MusicDictionary _MusicDictionary){
        String path = "android.resource://"+getPackageName()+"/raw/"+_MusicDictionary.FileName;
        Log.d(LoggerName, "[MusicService].[playSong] path = " + path);
        try {

            if (CurrentPlayingLength > 0) {
                /// Play song after pause
                player.seekTo(CurrentPlayingLength);
                player.start();
                CurrentPlayingLength = 0;
            } else {
                /// Just playing song from start of the length
                /// MediaPlayer initialization
                player.setDataSource(this, Uri.parse(path));
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

}