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
        player = new MediaPlayer();
        initializeMusicPlayer();
    }
    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d(LoggerName, "[MusicService].[onStartCommand]");
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
        //set player properties
        player.setWakeMode(getApplicationContext(),
                PowerManager.PARTIAL_WAKE_LOCK);
        player.setAudioStreamType(AudioManager.STREAM_MUSIC);
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
                player.prepareAsync();
                setCurrent_MusicDictionary(_MusicDictionary);
            }

        } catch (IllegalArgumentException e){
            Log.e(LoggerName, "[MusicService].[playSong] Error = "+ e.getMessage());
            e.printStackTrace();
        } catch (IOException e) {
            Log.e(LoggerName, "[MusicService].[playSong] Error = "+ e.getMessage());
            e.printStackTrace();
        }
    }
    public List<MusicDictionary> getList_MusicDictionary(){

        List_MusicDictionary = new ArrayList<MusicDictionary>();

        MusicDictionary _MusicDictionary_1 = new MusicDictionary();
        _MusicDictionary_1.ID = 1;
        _MusicDictionary_1.Srno = 1;
        _MusicDictionary_1.FileName = "a0171721";
        _MusicDictionary_1.EnglishTitle = "Ae Thal";
        _MusicDictionary_1.MyanmarTitle = "ဧည့္သည္";
        _MusicDictionary_1.AlbumName = "Ninzi May";
        _MusicDictionary_1.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_1.Length = "2:46";
        _MusicDictionary_1.Genre = "Pop";
        _MusicDictionary_1.Lyric = "ဧည့္သည္";;
        _MusicDictionary_1.IsFavorite = false;
        _MusicDictionary_1.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_1);

        MusicDictionary _MusicDictionary_2 = new MusicDictionary();
        _MusicDictionary_2.ID = 2;
        _MusicDictionary_2.Srno = 2;
        _MusicDictionary_2.FileName = "b0271721";
        _MusicDictionary_2.EnglishTitle = "Mu Yar Mar Yar Moe";
        _MusicDictionary_2.MyanmarTitle = "မူရာမာယာမိုး";
        _MusicDictionary_2.AlbumName = "Ninzi May";
        _MusicDictionary_2.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_2.Length = "1:45";
        _MusicDictionary_2.Genre = "Pop";
        _MusicDictionary_2.Lyric = "မူရာမာယာမိုး";
        _MusicDictionary_2.IsFavorite = false;
        _MusicDictionary_2.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_2);

        MusicDictionary _MusicDictionary_3 = new MusicDictionary();
        _MusicDictionary_3.ID = 3;
        _MusicDictionary_3.Srno = 3;
        _MusicDictionary_3.FileName = "c0371721";
        _MusicDictionary_3.EnglishTitle = "Ti Ah Mo";
        _MusicDictionary_3.MyanmarTitle = "တီအားမို";
        _MusicDictionary_3.AlbumName = "Ninzi May";
        _MusicDictionary_3.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_3.Length = "2:16";
        _MusicDictionary_3.Genre = "Pop";
        _MusicDictionary_3.Lyric = "တီအားမို";
        _MusicDictionary_3.IsFavorite = false;
        _MusicDictionary_3.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_3);

        MusicDictionary _MusicDictionary_4 = new MusicDictionary();
        _MusicDictionary_4.ID = 4;
        _MusicDictionary_4.Srno = 4;
        _MusicDictionary_4.FileName = "d0471721";
        _MusicDictionary_4.EnglishTitle = "Sane Yet Lai Ah";
        _MusicDictionary_4.MyanmarTitle = "စိမ္းရက္ေလအား";
        _MusicDictionary_4.AlbumName = "Ninzi May";
        _MusicDictionary_4.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_4.Length = "2:03";
        _MusicDictionary_4.Genre = "Pop";
        _MusicDictionary_4.Lyric = "စိမ္းရက္ေလအား";
        _MusicDictionary_4.IsFavorite = false;
        _MusicDictionary_4.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_4);

        MusicDictionary _MusicDictionary_5 = new MusicDictionary();
        _MusicDictionary_5.ID = 5;
        _MusicDictionary_5.Srno = 5;
        _MusicDictionary_5.FileName = "e0571721";
        _MusicDictionary_5.EnglishTitle = "Cherry Lan";
        _MusicDictionary_5.MyanmarTitle = "ခ်ယ္ရီလမ္း";
        _MusicDictionary_5.AlbumName = "Ninzi May";
        _MusicDictionary_5.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_5.Length = "3:21";
        _MusicDictionary_5.Genre = "Rock";
        _MusicDictionary_5.Lyric = "ခ်ယ္ရီလမ္း";
        _MusicDictionary_5.IsFavorite = false;
        _MusicDictionary_5.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_5);

        MusicDictionary _MusicDictionary_6 = new MusicDictionary();
        _MusicDictionary_6.ID = 6;
        _MusicDictionary_6.Srno = 6;
        _MusicDictionary_6.FileName = "f0671721";
        _MusicDictionary_6.EnglishTitle = "Nway Oo Pone Pyin";
        _MusicDictionary_6.MyanmarTitle = "ေႏြဦးပံုျပင္";
        _MusicDictionary_6.AlbumName = "Ninzi May";
        _MusicDictionary_6.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_6.Length = "3:24";
        _MusicDictionary_6.Genre = "Pop";
        _MusicDictionary_6.Lyric = "ေႏြဦးပံုျပင္";
        _MusicDictionary_6.IsFavorite = false;
        _MusicDictionary_6.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_6);

        MusicDictionary _MusicDictionary_7 = new MusicDictionary();
        _MusicDictionary_7.ID = 7;
        _MusicDictionary_7.Srno = 7;
        _MusicDictionary_7.FileName = "g0771721";
        _MusicDictionary_7.EnglishTitle = "Na Lone Thar Myoe Taw";
        _MusicDictionary_7.MyanmarTitle = "ႏွလံုးသားၿမိဳ႕ေတာ္";
        _MusicDictionary_7.AlbumName = "Ninzi May";
        _MusicDictionary_7.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_7.Length = "3:04";
        _MusicDictionary_7.Genre = "Pop";
        _MusicDictionary_7.Lyric = "ႏွလံုးသားၿမိဳ႕ေတာ္";
        _MusicDictionary_7.IsFavorite = false;
        _MusicDictionary_7.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_7);

        MusicDictionary _MusicDictionary_8 = new MusicDictionary();
        _MusicDictionary_8.ID = 8;
        _MusicDictionary_8.Srno = 8;
        _MusicDictionary_8.FileName = "h0871721";
        _MusicDictionary_8.EnglishTitle = "Ta Khar Ka Lat Saung";
        _MusicDictionary_8.MyanmarTitle = "တခါကလက္ေဆာင္";
        _MusicDictionary_8.AlbumName = "Ninzi May";
        _MusicDictionary_8.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_8.Length = "3:50";
        _MusicDictionary_8.Genre = "Pop";
        _MusicDictionary_8.Lyric = "တခါကလက္ေဆာင္";
        _MusicDictionary_8.IsFavorite = false;
        _MusicDictionary_8.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_8);

        MusicDictionary _MusicDictionary_9 = new MusicDictionary();
        _MusicDictionary_9.ID = 9;
        _MusicDictionary_9.Srno = 9;
        _MusicDictionary_9.FileName = "i0971721";
        _MusicDictionary_9.EnglishTitle = "Bae Thu Ko Lauk Chit Ma Lae";
        _MusicDictionary_9.MyanmarTitle = "ဘယ္သူကိုယ့္ေလာက္ခ်စ္သလဲ";
        _MusicDictionary_9.AlbumName = "Ninzi May";
        _MusicDictionary_9.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_9.Length = "5:10";
        _MusicDictionary_9.Genre = "Pop";
        _MusicDictionary_9.Lyric = "ဘယ္သူကိုယ့္ေလာက္ခ်စ္သလဲ";
        _MusicDictionary_9.IsFavorite = false;
        _MusicDictionary_9.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_9);

        MusicDictionary _MusicDictionary_10 = new MusicDictionary();
        _MusicDictionary_10.ID = 10;
        _MusicDictionary_10.Srno = 10;
        _MusicDictionary_10.FileName = "j1071721";
        _MusicDictionary_10.EnglishTitle = "Min Thi Naing Ma Lar";
        _MusicDictionary_10.MyanmarTitle = "မင္းသိႏိုင္မလား";
        _MusicDictionary_10.AlbumName = "Ninzi May";
        _MusicDictionary_10.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_10.Length = "5:00";
        _MusicDictionary_10.Genre = "Pop";
        _MusicDictionary_10.Lyric = "မင္းသိႏိုင္မလား";
        _MusicDictionary_10.IsFavorite = false;
        _MusicDictionary_10.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_10);

        MusicDictionary _MusicDictionary_11 = new MusicDictionary();
        _MusicDictionary_11.ID = 11;
        _MusicDictionary_11.Srno = 11;
        _MusicDictionary_11.FileName = "k1171721";
        _MusicDictionary_11.EnglishTitle = "A Chit Htet Ma Ka";
        _MusicDictionary_11.MyanmarTitle = "အခ်စ္ထက္မက";
        _MusicDictionary_11.AlbumName = "Ninzi May";
        _MusicDictionary_11.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_11.Length = "5:10";
        _MusicDictionary_11.Genre = "Pop";
        _MusicDictionary_11.Lyric = "အခ်စ္ထက္မက";
        _MusicDictionary_11.IsFavorite = false;
        _MusicDictionary_11.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_11);

        return List_MusicDictionary;
    }
    //<!-- End developer defined function(s).  -->
}