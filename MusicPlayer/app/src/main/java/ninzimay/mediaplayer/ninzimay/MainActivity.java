package ninzimay.mediaplayer.ninzimay;

import android.app.Activity;
import android.app.ActivityManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.graphics.Typeface;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SeekBar;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

import ninzimay.mediaplayer.ninzimay.MusicService.MusicBinder;

public class MainActivity extends Activity
implements SeekBar.OnSeekBarChangeListener,
View.OnClickListener,
AdapterView.OnItemClickListener
{
    //<!-- Start declaration area.  -->
    MusicService musicService;
    Intent playIntent;
    boolean IsMusicServiceConnected = false;
    List<MusicDictionary> List_MusicDictionary = null;
    MusicDictionary _MusicDictionary = null;
    String LoggerName = "NinZiMay";
    String PlayingStatus_New = "PlayingStatus_New";
    String PlayingStatus_Playing = "PlayingStatus_Playing";
    String PlayingStatus_Played = "PlayingStatus_Played";
    TextView txtTitle = null;
    TextView txtCurrentPlayingMyanmarInfo = null;
    TextView txtCurrentPlayingEnglishInfo = null;
    TextView txtStartPoint = null;
    TextView txtEndPoint = null;
    int CurrentPlayingLength = 0;
    MediaPlayer mediaPlayer = null;
    Button btnShuffle = null;
    Button btnBackward = null;
    Button btnPlay = null;
    Button btnForward = null;
    Button btnRepeat = null;
    Button btnLyric = null;
    Button btnFavorite = null;
    SeekBar Seekbar = null;
    int MusicTime_CurrentPlaying = 0;
    int MusicTime_TotalLength = 0;
    int CurrentSongID = 0;
    boolean IsRepeatAlbum = true;
    boolean IsShuffle = false;
    boolean IsUserSeekingSliderBar = false;
    boolean IsComingBack = false;
    Handler Handler_Music = null;
    Runnable Runnable_Music = null;
    //<!-- End declaration area.  -->

    //<!-- Start dependency object(s).  -->
    private ServiceConnection Music_ServiceConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName name, IBinder service) {
            MusicBinder binder = (MusicBinder)service;
            //get service
            musicService = binder.getService();
            //pass list
            if(!IsComingBack){
                musicService.setList(getList_MusicDictionary());
            }
            IsMusicServiceConnected = true;
            Log.d(LoggerName, "[onServiceConnected] IsMusicServiceConnected = "+ IsMusicServiceConnected);
        }

        @Override
        public void onServiceDisconnected(ComponentName name) {
            IsMusicServiceConnected = false;
            Log.d(LoggerName, "[onServiceDisconnected] IsMusicServiceConnected = "+ IsMusicServiceConnected);
        }
    };
    //<!-- End dependency object(s).  -->

    //<!-- Start system defined function(s).  -->
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Log.d(LoggerName, "In the onCreate() event");
        super.onCreate(savedInstanceState);
        if( savedInstanceState != null ) {
            IsComingBack = savedInstanceState.getBoolean("IsComingBack");
            Log.d(LoggerName, "IsComingBack after orientation changed = " + IsComingBack);
        }

        if(isMyServiceRunning(MusicService.class)){
            IsComingBack = true;
            Log.d(LoggerName, "IsComingBack after back button pressed = " + IsComingBack);
        }
        setContentView(R.layout.activity_main);
        initializer();
    }
    public void onStart(){
    super.onStart();
    Log.d(LoggerName, "In the onStart() event");

        if(playIntent == null){
            playIntent = new Intent(this, MusicService.class);
            bindService(playIntent, Music_ServiceConnection, Context.BIND_AUTO_CREATE);
            if(IsComingBack){
                playIntent.setAction(Constants.ACTION.COMING_BACK);
                startService(playIntent);
            }

            //Music Handler for methods
            Handler_Music = new Handler();
            Runnable_Music = new Runnable() {
                @Override
                public void run() {
                    if (IsMusicServiceConnected){ // Check if service bounded

                        if(!musicService.IsMediaPlayerObjectAvailable()){ MusicTime_TotalLength = 0; return; }

                        if (MusicTime_TotalLength == 0){ // Put data in it one time
                            MusicTime_TotalLength = musicService.getMusicDuration();
                            Seekbar.setMax(MusicTime_TotalLength);
                            MusicDictionary _MusicDictionary = musicService.getCurrent_MusicDictionary();
                            txtCurrentPlayingMyanmarInfo.setText(_MusicDictionary.MyanmarTitle);
                            txtCurrentPlayingEnglishInfo.setText(_MusicDictionary.EnglishTitle);
                        }

                        if(!IsUserSeekingSliderBar){
                            MusicTime_CurrentPlaying = musicService.getMusicCurrrentPosition();
                            Seekbar.setProgress(MusicTime_CurrentPlaying);
                            setProgressText();
                        }

                        if(CurrentSongID != musicService.getCurrent_MusicDictionary().ID){
                            CurrentSongID = musicService.getCurrent_MusicDictionary().ID;
                            MusicTime_TotalLength = 0;
                        }

                    }else if(!IsMusicServiceConnected){ // if service is not bounded log it
                        Log.d(LoggerName, "Waiting to get connection from service...");
                    }
                    Handler_Music.postDelayed(this, 100);
                }
            };
        }
    }
    public void onResume(){
        super.onResume();
        Log.d(LoggerName, "In the onResume() event");
        if(IsComingBack) {
            Handler_Music.postDelayed(Runnable_Music, 100);
        }
    }
    public void onRestart() {
        super.onRestart();
        Log.d(LoggerName, "In the onRestart() event");
    }
    public void onPause(){
        super.onPause();
        Log.d(LoggerName, "In the onPause() event");
    }
    public void onStop(){
        super.onStop();
        Log.d(LoggerName, "In the onStop() event");
    }
    public void onDestroy() {
        Log.d(LoggerName, "In the onDestroy() event");
        super.onDestroy();
        if (Music_ServiceConnection != null) {
            unbindService(Music_ServiceConnection);
        }
    }
    @Override
    public void onBackPressed(){
        Log.d(LoggerName, "In the onBackPressed() event");
        // code here to show dialog
        super.onBackPressed();  // optional depending on your needs
    }
    @Override
    protected void onSaveInstanceState(Bundle outState) {
        Log.d(LoggerName, "onSaveInstanceState");
        outState.putBoolean("IsComingBack", true);
        super.onSaveInstanceState(outState);
    }
    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.btnShuffle:
                break;
            case R.id.btnBackward:
                break;
            case R.id.btnPlay:
                btnPlay_Click();
                break;
            case R.id.btnForward:
                break;
            case R.id.btnRepeat:
                break;
            case R.id.btnLyric:
                break;
            case R.id.btnFavorite:
                break;
            default:
                break;
        }

    }
    @Override
    public void onStopTrackingTouch(SeekBar seekBar) {
        IsUserSeekingSliderBar = false;
        Log.d(LoggerName, "seekBar.getProgress() ="+seekBar.getProgress());
        musicService.seekToMusic(seekBar.getProgress());
        CurrentPlayingLength = 0;
    }
    @Override
    public void onStartTrackingTouch(SeekBar seekBar) {
        Log.d(LoggerName, "onStartTrackingTouch");
        IsUserSeekingSliderBar = true;
    }
    @Override
    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
        if (IsMusicServiceConnected && fromUser && IsUserSeekingSliderBar) {
            String _Progress = String.format("%02d:%02d",
                    TimeUnit.MILLISECONDS.toMinutes(progress),
                    TimeUnit.MILLISECONDS.toSeconds(progress) -
                            TimeUnit.MINUTES.toSeconds(TimeUnit.MILLISECONDS.toMinutes(progress))
            );
            Log.d(LoggerName, "progress = "+_Progress);
            txtStartPoint.setText(_Progress);
        }
    }
    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        String Clicked_FileName = ((MusicDictionary)parent.getItemAtPosition(position)).FileName;
        //Log.d(LoggerName, "Clicked_FileName = " + Clicked_FileName);
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    private void btnPlay_Click(){
        Log.d(LoggerName, "Play Clicked.");
        playIntent.setAction(Constants.ACTION.STARTFOREGROUND_ACTION);
        startService(playIntent);
        Handler_Music.postDelayed(Runnable_Music, 1000);
    }
    private  void initializer(){

        /// Start Invoking background image loader.
        ImageView imgBackgroundImage = (ImageView)findViewById(R.id.imgBackgroundImage);
        loadBitmap(R.drawable.background_1, imgBackgroundImage, this);
        /// End Invoking background image loader.

        /// Start setting customized font(s).
        Typeface font_fontawesome = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        Typeface font_ailerons = Typeface.createFromAsset( getAssets(), "ailerons-typeface.otf" );
        /// End setting customized font(s).

        /// Start binding basic control(s)
        /// Text control(s)
        txtTitle = (TextView)findViewById(R.id.txtTitle);
        txtStartPoint = (TextView)findViewById(R.id.txtStartPoint);
        txtEndPoint = (TextView)findViewById(R.id.txtEndPoint);
        txtCurrentPlayingMyanmarInfo = (TextView)findViewById(R.id.txtCurrentPlayingMyanmarInfo);
        txtCurrentPlayingEnglishInfo = (TextView)findViewById(R.id.txtCurrentPlayingEnglishInfo);
        txtTitle.setTypeface(font_ailerons);
        /// Button control(s)
        btnShuffle = (Button)findViewById( R.id.btnShuffle );
        btnBackward = (Button)findViewById( R.id.btnBackward );
        btnPlay = (Button)findViewById( R.id.btnPlay );
        btnForward = (Button)findViewById( R.id.btnForward );
        btnRepeat = (Button)findViewById( R.id.btnRepeat );
        btnLyric = (Button)findViewById( R.id.btnLyric );
        btnFavorite = (Button)findViewById( R.id.btnFavorite );
        btnShuffle.setOnClickListener(this);
        btnBackward.setOnClickListener(this);
        btnPlay.setOnClickListener(this);
        btnForward.setOnClickListener(this);
        btnRepeat.setOnClickListener(this);
        btnLyric.setOnClickListener(this);
        btnFavorite.setOnClickListener(this);
        btnShuffle.setTypeface(font_fontawesome);
        btnBackward.setTypeface(font_fontawesome);
        btnPlay.setTypeface(font_fontawesome);
        btnForward.setTypeface(font_fontawesome);
        btnRepeat.setTypeface(font_fontawesome);
        btnLyric.setTypeface(font_fontawesome);
        btnFavorite.setTypeface(font_fontawesome);
        /// Seekbar control
        Seekbar = (SeekBar)findViewById(R.id.SeekBar);
        Seekbar.setOnSeekBarChangeListener(this);
        /// End binding basic control(s)

        /// Start binding listview control
        ListAdapter _ListAdapter = new SongListingRowControl(this, getList_MusicDictionary());
        ListView _ListView = (ListView) findViewById(R.id.listView);
        _ListView.setAdapter(_ListAdapter);
        _ListView.setScrollingCacheEnabled(false);
        _ListView.setOnItemClickListener(this);
        /// End binding listview control
    }
    protected void setProgressText() {

        final int HOUR = 60*60*1000;
        final int MINUTE = 60*1000;
        final int SECOND = 1000;

        int durationInMillis = musicService.getMusicDuration();
        int curVolume = musicService.getMusicCurrrentPosition();

        int durationHour = durationInMillis/HOUR;
        int durationMint = (durationInMillis%HOUR)/MINUTE;
        int durationSec = (durationInMillis%MINUTE)/SECOND;

        int currentHour = curVolume/HOUR;
        int currentMint = (curVolume%HOUR)/MINUTE;
        int currentSec = (curVolume%MINUTE)/SECOND;

        txtStartPoint.setText(String.format("%02d:%02d", currentMint, currentSec));
        txtEndPoint.setText(String.format("%02d:%02d", durationMint, durationSec));
    }
    private boolean isMyServiceRunning(Class<?> serviceClass) {
        ActivityManager manager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }
    public void loadBitmap(int resId, ImageView imageView, Context _Context) {
        BitmapWorkerTask task = new BitmapWorkerTask(imageView, _Context);
        task.execute(resId);
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