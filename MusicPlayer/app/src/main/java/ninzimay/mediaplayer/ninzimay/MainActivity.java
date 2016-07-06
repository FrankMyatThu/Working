package ninzimay.mediaplayer.ninzimay;

import android.app.Activity;
import android.app.ActivityManager;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.Typeface;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.os.Parcelable;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.LocalBroadcastManager;
import android.util.Log;
import android.view.View;
import android.widget.Adapter;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SeekBar;
import android.widget.TextView;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

public class MainActivity extends Activity
implements SeekBar.OnSeekBarChangeListener,
View.OnClickListener,
AdapterView.OnItemClickListener
{
    //<!-- Start declaration area.  -->
    String LoggerName = "NinZiMay";
    String PlayingStatus_New = "PlayingStatus_New";
    Intent IntentMusicService = null;
    ArrayList<MusicDictionary> List_MusicDictionary = null;
    TextView txtTitle = null;
    TextView txtCurrentPlayingMyanmarInfo = null;
    TextView txtCurrentPlayingEnglishInfo = null;
    TextView txtStartPoint = null;
    TextView txtEndPoint = null;
    ListView _ListView = null;
    SongListingRowControl Adapter_SongListingRowControl = null;
    Button btnShuffle = null;
    Button btnBackward = null;
    Button btnPlayPause = null;
    Button btnForward = null;
    Button btnRepeat = null;
    Button btnLyric = null;
    Button btnFavorite = null;
    SeekBar Seekbar = null;
    boolean IsUserSeekingSliderBar = false;
    boolean IsComingBack = false;
    boolean IsPauseSong = false;
    boolean IsSeekbarSeekable = false;
    Typeface font_fontawesome = null;
    Typeface font_ailerons = null;
    Typeface font_ninzimay = null;
    int CurrentSongTotalLength = 0;
    //<!-- End declaration area.  -->

    //<!-- Start dependency object(s).  -->
    private BroadcastReceiver broadcastReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();

            if (Constants.BROADCAST.FOREVER_BROADCAST.equals(action)){
                //Log.d(LoggerName, "ACTIVITY.FOREVER_BROADCAST");
                int CurrentSongPlayingIndex = Integer.parseInt(intent.getExtras().get("CurrentSongPlayingIndex").toString());
                IsSeekbarSeekable = Boolean.parseBoolean(intent.getExtras().get("IsSeekbarSeekable").toString());
                if(!IsUserSeekingSliderBar){
                    Seekbar.setProgress(CurrentSongPlayingIndex);
                    setProgressText(CurrentSongPlayingIndex);
                }
            }

            if (Constants.BROADCAST.ONDEMAND_BROADCAST.equals(action)){
                //Log.d(LoggerName, "ACTIVITY.ONDEMAND_BROADCAST");
                if(Boolean.parseBoolean(intent.getExtras().get("IsClose").toString())){
                    finish();
                    System.exit(0);
                }
                if(Boolean.parseBoolean(intent.getExtras().get("IsPause").toString())){
                    btnPlayPause.setText(getString(R.string.Play));
                    IsPauseSong = true;
                }else{
                    btnPlayPause.setText(getString(R.string.Pause));
                    IsPauseSong = false;
                }
                Gson _Gson = new Gson();
                String Using_List_MusicDictionary = intent.getExtras().get("Using_List_MusicDictionary").toString();
                CurrentSongTotalLength = Integer.parseInt(intent.getExtras().get("CurrentSongTotalLength").toString());
                Seekbar.setMax(CurrentSongTotalLength);
                txtCurrentPlayingEnglishInfo.setText(intent.getExtras().get("EnglishTitle").toString());
                txtCurrentPlayingMyanmarInfo.setText(intent.getExtras().get("MyanmarTitle").toString());
                ArrayList<MusicDictionary> ArrayList_Using_List_MusicDictionary =  _Gson.fromJson(Using_List_MusicDictionary, new TypeToken<ArrayList<MusicDictionary>>(){}.getType());
                ListView_Rebind(ArrayList_Using_List_MusicDictionary);
            }
        }
    };
    //<!-- End dependency object(s).  -->

    //<!-- Start system defined function(s).  -->
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        //Log.d(LoggerName, "In the onCreate() event");
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initializer();
    }
    public void onStart(){
        //Log.d(LoggerName, "In the onStart() event");
        super.onStart();
    }
    public void onResume(){
        //Log.d(LoggerName, "In the onResume() event");
        super.onResume();
        getCacheAndBind();
        IntentFilter _IntentFilter = new IntentFilter();
        _IntentFilter.addAction(Constants.BROADCAST.FOREVER_BROADCAST);
        _IntentFilter.addAction(Constants.BROADCAST.ONDEMAND_BROADCAST);
        LocalBroadcastManager.getInstance(this).registerReceiver(broadcastReceiver, _IntentFilter);
        if(IsComingBack){
            IntentMusicService = null;
            IntentMusicService = new Intent(this, MusicService.class);
            IntentMusicService.setAction(Constants.ACTION.INVOKE_ONDEMAND_ACTION);
            startService(IntentMusicService);
        }
    }
    public void onRestart() {
        //Log.d(LoggerName, "In the onRestart() event");
        super.onRestart();
    }
    public void onPause(){
        //Log.d(LoggerName, "In the onPause() event");
        super.onPause();
        LocalBroadcastManager.getInstance(this).unregisterReceiver(broadcastReceiver);
        setCache();
    }
    public void onStop(){
        //Log.d(LoggerName, "In the onStop() event");
        super.onStop();
    }
    public void onDestroy() {
        //Log.d(LoggerName, "In the onDestroy() event");
        super.onDestroy();
    }
    @Override
    public void onBackPressed(){
        //Log.d(LoggerName, "In the onBackPressed() event");
        // code here to show dialog
        super.onBackPressed();  // optional depending on your needs
    }
    @Override
    protected void onSaveInstanceState(Bundle outState) {
        //Log.d(LoggerName, "onSaveInstanceState");
        super.onSaveInstanceState(outState);
    }
    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.btnShuffle:
                break;
            case R.id.btnBackward:
                btnBackward_Click();
                break;
            case R.id.btnPlayPause:
                btnPlayPause_Click();
                break;
            case R.id.btnForward:
                btnForward_Click();
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
        if(!IsSeekbarSeekable) return;
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        IntentMusicService.setAction(Constants.ACTION.INDEXED_SEEK_ACTION);
        IntentMusicService.putExtra("seekBarIndex", seekBar.getProgress());
        startService(IntentMusicService);
    }
    @Override
    public void onStartTrackingTouch(SeekBar seekBar) {
        //Log.d(LoggerName, "onStartTrackingTouch");
        IsUserSeekingSliderBar = true;
    }
    @Override
    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
        if (fromUser && IsUserSeekingSliderBar) {
            String _Progress = String.format("%02d:%02d",
                    TimeUnit.MILLISECONDS.toMinutes(progress),
                    TimeUnit.MILLISECONDS.toSeconds(progress) -
                            TimeUnit.MINUTES.toSeconds(TimeUnit.MILLISECONDS.toMinutes(progress))
            );
            //Log.d(LoggerName, "progress = "+_Progress);
            txtStartPoint.setText(_Progress);
        }
    }
    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        Gson _Gson = new Gson();
        MusicDictionary _MusicDictionary = ((MusicDictionary)parent.getItemAtPosition(position));
        String Current_MusicDictionary = _Gson.toJson(_MusicDictionary);
        String Initial_List_MusicDictionary = _Gson.toJson(getList_MusicDictionary());
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        IntentMusicService.setAction(Constants.ACTION.INDEXED_SONG_ACTION);
        IntentMusicService.putExtra("Current_MusicDictionary", Current_MusicDictionary);
        IntentMusicService.putExtra("Initial_List_MusicDictionary", Initial_List_MusicDictionary);
        startService(IntentMusicService);
        btnPlayPause.setText(getString(R.string.Pause));
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    private void setCache(){
        if(!isMyServiceRunning(MusicService.class)) return;

        SharedPreferences sharedPreferences = getSharedPreferences(Constants.CACHE.NINZIMAY, MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();

        /// 1.IsCached
        editor.putBoolean("IsCached", true);

        /// 2.btnPlayPause_IsPlayLogoShowing
        if(btnPlayPause.getText().equals(getString(R.string.Play))){
            editor.putBoolean("btnPlayPause_IsPlayLogoShowing", true);
        }else{
            editor.putBoolean("btnPlayPause_IsPlayLogoShowing", false);
        }

        /// 3.ListViewFirstVisiblePosition
        int ListViewFirstVisiblePosition = _ListView.getFirstVisiblePosition();
        editor.putInt("ListViewFirstVisiblePosition", ListViewFirstVisiblePosition);

        /// 4.ListViewOffset
        View _View = _ListView.getChildAt(0);
        int Offset = (_View == null) ? 0 : (_View.getTop() - _ListView.getPaddingTop());
        editor.putInt("ListViewOffset", Offset);

        editor.commit();
        //Log.d(LoggerName, "SetListViewScrolledPosition ListViewFirstVisiblePosition = "+ ListViewFirstVisiblePosition +" | Offset = "+Offset);
    }
    private void getCacheAndBind(){
        if(!isMyServiceRunning(MusicService.class)) return;

        SharedPreferences sharedPreferences = getSharedPreferences(Constants.CACHE.NINZIMAY, MODE_PRIVATE);
        /// 1.IsCached
        if(!sharedPreferences.getBoolean("IsCached", false))
            return;
        else
            IsComingBack = true;

        /// 2.btnPlayPause_IsPlayLogoShowing
        if(sharedPreferences.getBoolean("btnPlayPause_IsPlayLogoShowing", true)){
            btnPlayPause.setText(getString(R.string.Play));
            if(IsComingBack) IsPauseSong = true;
        }else{
            btnPlayPause.setText(getString(R.string.Pause));
        }

        /// 3.ListViewFirstVisiblePosition
        int ListViewFirstVisiblePosition = sharedPreferences.getInt("ListViewFirstVisiblePosition", 0);
        /// 4.ListViewOffset
        int ListViewOffset = sharedPreferences.getInt("ListViewOffset", 0);
        _ListView.setSelectionFromTop(ListViewFirstVisiblePosition, ListViewOffset);
    }
    private void btnPlayPause_Click(){
        if(getString(R.string.Play).equalsIgnoreCase(btnPlayPause.getText().toString())){
            //Log.d(LoggerName, "Play is clicked");
            /// Play
            btnPlay_Click();
        }else{
            //Log.d(LoggerName, "Pause is clicked");
            /// Pause
            btnPause_Click();
        }
    }
    private void btnPlay_Click(){
        if(IsPauseSong){
            //Log.d(LoggerName, "[btnPlay_Click] Playback event");
            IntentMusicService = null;
            IntentMusicService = new Intent(this, MusicService.class);
            IntentMusicService.setAction(Constants.ACTION.PLAYBACK_ACTION);
            startService(IntentMusicService);
        }else{
            //Log.d(LoggerName, "[btnPlay_Click] play event");
            playSongInService(false);
        }
        btnPlayPause.setText(getString(R.string.Pause));
    }
    private void btnPause_Click(){
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        IntentMusicService.setAction(Constants.ACTION.PAUSE_ACTION);
        startService(IntentMusicService);
    }
    private void btnForward_Click(){
        Gson _Gson = new Gson();
        String Initial_List_MusicDictionary = _Gson.toJson(getList_MusicDictionary());
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        IntentMusicService.setAction(Constants.ACTION.NEXT_ACTION);
        IntentMusicService.putExtra("Initial_List_MusicDictionary", Initial_List_MusicDictionary);
        startService(IntentMusicService);
    }
    private void btnBackward_Click(){
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        IntentMusicService.setAction(Constants.ACTION.PREV_ACTION);
        startService(IntentMusicService);
    }
    private void playSongInService(Boolean IsIndexed){
        Gson _Gson = new Gson();
        String Initial_List_MusicDictionary = _Gson.toJson((List)getList_MusicDictionary());
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        if(IsIndexed){
            IntentMusicService.setAction(Constants.ACTION.INDEXED_SONG_ACTION);
        }else{
            IntentMusicService.setAction(Constants.ACTION.PLAY_ACTION);
        }
        IntentMusicService.putExtra("Initial_List_MusicDictionary", Initial_List_MusicDictionary);
        startService(IntentMusicService);
    }
    private  void initializer(){

        /// Start Invoking background image loader.
        ImageView imgBackgroundImage = (ImageView)findViewById(R.id.imgBackgroundImage);
        loadBitmap(R.drawable.background_1, imgBackgroundImage, this);
        /// End Invoking background image loader.

        /// Start setting customized font(s).
        font_fontawesome = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        font_ailerons = Typeface.createFromAsset( getAssets(), "ailerons-typeface.otf" );
        font_ninzimay = Typeface.createFromAsset( getAssets(), "ninzimay.ttf" );
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
        btnPlayPause = (Button)findViewById( R.id.btnPlayPause );
        btnForward = (Button)findViewById( R.id.btnForward );
        btnRepeat = (Button)findViewById( R.id.btnRepeat );
        btnLyric = (Button)findViewById( R.id.btnLyric );
        btnFavorite = (Button)findViewById( R.id.btnFavorite );
        btnShuffle.setOnClickListener(this);
        btnBackward.setOnClickListener(this);
        btnPlayPause.setOnClickListener(this);
        btnForward.setOnClickListener(this);
        btnRepeat.setOnClickListener(this);
        btnLyric.setOnClickListener(this);
        btnFavorite.setOnClickListener(this);
        btnShuffle.setTypeface(font_fontawesome);
        btnBackward.setTypeface(font_fontawesome);
        btnPlayPause.setTypeface(font_ninzimay);
        btnForward.setTypeface(font_fontawesome);
        btnRepeat.setTypeface(font_fontawesome);
        btnLyric.setTypeface(font_fontawesome);
        btnFavorite.setTypeface(font_fontawesome);
        /// Seekbar control
        Seekbar = (SeekBar)findViewById(R.id.SeekBar);
        Seekbar.setOnSeekBarChangeListener(this);
        /// End binding basic control(s)

        /// Start binding listview control
        Adapter_SongListingRowControl = new SongListingRowControl(this, getList_MusicDictionary());
        _ListView = (ListView) findViewById(R.id.listView);
        _ListView.setAdapter(Adapter_SongListingRowControl);
        _ListView.setScrollingCacheEnabled(false);
        _ListView.setOnItemClickListener(this);
        /// End binding listview control
    }
    protected void setProgressText(int CurrentSongPlayingIndex) {

        final int HOUR = 60*60*1000;
        final int MINUTE = 60*1000;
        final int SECOND = 1000;

        int durationInMillis = CurrentSongTotalLength;
        int curVolume = CurrentSongPlayingIndex;

        int durationHour = durationInMillis/HOUR;
        int durationMint = (durationInMillis%HOUR)/MINUTE;
        int durationSec = (durationInMillis%MINUTE)/SECOND;

        int currentHour = curVolume/HOUR;
        int currentMint = (curVolume%HOUR)/MINUTE;
        int currentSec = (curVolume%MINUTE)/SECOND;

        txtStartPoint.setText(String.format("%02d:%02d", currentMint, currentSec));
        txtEndPoint.setText(String.format("%02d:%02d", durationMint, durationSec));
    }
    protected void ListView_Rebind(List<MusicDictionary> List_MusicDictionary){
        Adapter_SongListingRowControl.addItems((ArrayList) List_MusicDictionary);
        Adapter_SongListingRowControl.notifyDataSetChanged();
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
    public ArrayList<MusicDictionary> getList_MusicDictionary(){

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
        _MusicDictionary_2.Length = "1:46";
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
        _MusicDictionary_4.Length = "2:00";
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
        _MusicDictionary_6.Length = "5:22";
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
        _MusicDictionary_8.MyanmarTitle = "တစ္ခါကလက္ေဆာင္";
        _MusicDictionary_8.AlbumName = "Ninzi May";
        _MusicDictionary_8.AlbumArt = "AlbumArt.jpg";
        _MusicDictionary_8.Length = "3:55";
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
        _MusicDictionary_9.Length = "5:01";
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
        _MusicDictionary_10.Length = "4:51";
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
        _MusicDictionary_11.Length = "5:01";
        _MusicDictionary_11.Genre = "Pop";
        _MusicDictionary_11.Lyric = "အခ်စ္ထက္မက";
        _MusicDictionary_11.IsFavorite = false;
        _MusicDictionary_11.PlayingStatus = PlayingStatus_New;
        List_MusicDictionary.add(_MusicDictionary_11);

        return List_MusicDictionary;
    }
    //<!-- End developer defined function(s).  -->
}