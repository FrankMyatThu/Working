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
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.Typeface;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
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

import com.facebook.stetho.Stetho;
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
    ImageView imgBackgroundImage = null;
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
    Gson _Gson = new Gson();
    //<!-- End declaration area.  -->

    //<!-- Start dependency object(s).  -->
    private BroadcastReceiver broadcastReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            Runtime.getRuntime().gc();
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

                String Using_List_MusicDictionary = intent.getExtras().get("Using_List_MusicDictionary").toString();
                CurrentSongTotalLength = Integer.parseInt(intent.getExtras().get("CurrentSongTotalLength").toString());
                Seekbar.setMax(CurrentSongTotalLength);
                txtCurrentPlayingEnglishInfo.setText(intent.getExtras().get("EnglishTitle").toString());
                txtCurrentPlayingMyanmarInfo.setText(intent.getExtras().get("MyanmarTitle").toString());
                ArrayList<MusicDictionary> ArrayList_Using_List_MusicDictionary =  _Gson.fromJson(Using_List_MusicDictionary, new TypeToken<ArrayList<MusicDictionary>>(){}.getType());
                ListView_Rebind(ArrayList_Using_List_MusicDictionary);
            }

            if (Constants.BROADCAST.CLICK_FAVORITE.equals(action)){
                String Current_MusicDictionary = intent.getExtras().get("Current_MusicDictionary").toString();
                Boolean _IsFavoriteNow = Boolean.parseBoolean(intent.getExtras().get("IsFavoriteNow").toString());
                MusicDictionary _Current_MusicDictionary =  _Gson.fromJson(Current_MusicDictionary, new TypeToken<MusicDictionary>(){}.getType());
                btnFavorite_Click(_Current_MusicDictionary, _IsFavoriteNow);
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
        Stetho.initializeWithDefaults(this);
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
        _IntentFilter.addAction(Constants.BROADCAST.CLICK_FAVORITE);
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
        /// Clean up memory
        Runtime.getRuntime().gc();
    }
    @Override
    public void onDetachedFromWindow() {
        //Log.d(LoggerName, "In the onDetachedFromWindow() event");
        super.onDetachedFromWindow();
        /// Clean up memory
        unloadBitmap();
    }
    @Override
    public void onBackPressed(){
        //Log.d(LoggerName, "In the onBackPressed() event");
        // code here to show dialog
        super.onBackPressed();
        /// Clean up memory
        finish();
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
    //@Override
    public void onItemClick(AdapterView<?> parent, View _View, int position, long id) {
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
    private void btnFavorite_Click(MusicDictionary _MusicDictionary, Boolean IsFavoriteNow){
        //Log.d(LoggerName, "btnFavorite_Click _MusicDictionary.EnglishTitle = "+_MusicDictionary.EnglishTitle + " | _IsFavoriteNow = "+ IsFavoriteNow);
        DatabaseHandler _DatabaseHandler = new DatabaseHandler(this);
        _DatabaseHandler.updateMusicDictionary(_MusicDictionary.ID, IsFavoriteNow);

        Gson _Gson = new Gson();
        String ToUpdate_MusicDictionary = _Gson.toJson(_MusicDictionary);
        IntentMusicService = null;
        IntentMusicService = new Intent(this, MusicService.class);
        IntentMusicService.setAction(Constants.ACTION.UPDATE_MUSICDICTIONARY_ACTION);
        IntentMusicService.putExtra("ToUpdate_MusicDictionary", ToUpdate_MusicDictionary);
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
        unloadBitmap();
        imgBackgroundImage = (ImageView)findViewById(R.id.imgBackgroundImage);
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
    private void unloadBitmap(){
        if(imgBackgroundImage == null)  return;
        Drawable drawable = imgBackgroundImage.getDrawable();
        if (drawable instanceof BitmapDrawable) {
            BitmapDrawable bitmapDrawable = (BitmapDrawable) drawable;
            Bitmap bitmap = bitmapDrawable.getBitmap();
            bitmap.recycle();
        }
    }
    public ArrayList<MusicDictionary> getList_MusicDictionary(){
        List_MusicDictionary = new ArrayList<MusicDictionary>();

        DatabaseHandler _DatabaseHandler = new DatabaseHandler(this);
        List_MusicDictionary = _DatabaseHandler.getAllMusicDictionary();

        return List_MusicDictionary;
    }
    //<!-- End developer defined function(s).  -->
}