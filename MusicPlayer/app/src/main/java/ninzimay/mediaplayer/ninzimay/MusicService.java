package ninzimay.mediaplayer.ninzimay;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.ContentUris;
import android.content.Context;
import android.content.Intent;
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
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.style.StyleSpan;
import android.text.style.TypefaceSpan;
import android.util.DisplayMetrics;
import android.util.Log;
import android.widget.RemoteViews;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Date;
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
            CurrentPlayingLength = Integer.parseInt(intent.getExtras().get("seekBarIndex").toString());
            playIndexedSong();
        }if (intent.getAction().equals(Constants.ACTION.INVOKE_ONDEMAND_ACTION)) {
            broadCast_OnDemand();
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
    public void setCurrent_MusicDictionary(MusicDictionary _MusicDictionary){
        Current_MusicDictionary = _MusicDictionary;
    }

    private Bitmap getCustomFont_For_RemoteView(String ToDrawText, Typeface _TypeFace){
        DisplayMetrics _DisplayMetrics = getApplicationContext().getResources().getDisplayMetrics();
        int width = _DisplayMetrics.widthPixels;
        Bitmap _Bitmap = Bitmap.createBitmap(width - 72, 84, Bitmap.Config.ARGB_8888);
        Canvas _Canvas = new Canvas(_Bitmap);
        Paint paint =  new Paint();
        paint.setAntiAlias(true);
        paint.setSubpixelText(true);
        paint.setTypeface(_TypeFace);
        paint.setStyle(Paint.Style.FILL);
        paint.setTextSize(50);
        paint.setTextAlign(Paint.Align.LEFT);
        _Canvas.drawText(ToDrawText, 80, 60, paint);
        return _Bitmap;
    }

    private void showCustomNotifications(){
        Typeface font_fontawesome = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        Typeface font_ailerons = Typeface.createFromAsset( getAssets(), "ailerons-typeface.otf" );
        Typeface font_ninzimay = Typeface.createFromAsset( getAssets(), "ninzimay.ttf" );

        Intent notificationIntent = new Intent(this, MainActivity.class);
        notificationIntent.setAction("");
        notificationIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        PendingIntent _PendingIntent = PendingIntent.getActivity(this, 0, notificationIntent, 0);

        Intent closeIntent = new Intent(this, MusicService.class);
        closeIntent.setAction(Constants.ACTION.STOPFOREGROUND_ACTION);
        PendingIntent PendingIntent_closeIntent = PendingIntent.getService(this, 0, closeIntent, 0);

        RemoteViews _RemoteViews = new RemoteViews(getApplicationContext().getPackageName(), R.layout.custome_notification);
        _RemoteViews.setImageViewBitmap(R.id.notification_img, getCustomFont_For_RemoteView(getString(R.string.Close), font_fontawesome));
        _RemoteViews.setOnClickPendingIntent(R.id.notification_img, PendingIntent_closeIntent);


        int icon = R.drawable.logo;
        long when = System.currentTimeMillis();
        Notification notification = new NotificationCompat.Builder(this)
                .setSmallIcon(icon)
                .setContentText("Testing font awesome")
                .setContentIntent(_PendingIntent)
                .setOngoing(true)
                .setWhen(when)
                .build();

        notification.contentView = _RemoteViews;
        //notification.bigContentView = _RemoteViews;

        notification.flags |= Notification.FLAG_AUTO_CANCEL;
        startForeground(Constants.NOTIFICATION_ID.FOREGROUND_SERVICE, notification);
    }
    /*public Bitmap getBitmapFont(String ToDraw, Typeface _Typeface)
    {
        Bitmap myBitmap = Bitmap.createBitmap(20, 20, Bitmap.Config.ARGB_8888);
        Canvas myCanvas = new Canvas(myBitmap);
        Paint paint = new Paint();
        paint.setAntiAlias(true);
        paint.setSubpixelText(true);
        paint.setTypeface(_Typeface);
        paint.setStyle(Paint.Style.FILL);
        paint.setColor(Color.BLACK);
        paint.setTextSize(15);
        myCanvas.drawText(ToDraw, 0, 20, paint);
        return myBitmap;
    }*/
    private void invokeNotificationView(){
        MusicDictionary _MusicDictionary = getCurrent_MusicDictionary();
        Intent notificationIntent = new Intent(this, MainActivity.class);
        notificationIntent.setAction("");
        notificationIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        PendingIntent _PendingIntent = PendingIntent.getActivity(this, 0, notificationIntent, 0);

        Intent previousIntent = new Intent(this, MusicService.class);
        previousIntent.setAction(Constants.ACTION.PREV_ACTION);
        PendingIntent PendingIntent_previousIntent = PendingIntent.getService(this, 0, previousIntent, 0);

        Intent playIntent = new Intent(this, MusicService.class);
        playIntent.setAction(Constants.ACTION.PLAY_ACTION);
        PendingIntent PendingIntent_playIntent = PendingIntent.getService(this, 0, playIntent, 0);

        Intent nextIntent = new Intent(this, MusicService.class);
        nextIntent.setAction(Constants.ACTION.NEXT_ACTION);
        PendingIntent PendingIntent_nextIntent = PendingIntent.getService(this, 0, nextIntent, 0);

        Intent closeIntent = new Intent(this, MusicService.class);
        closeIntent.setAction(Constants.ACTION.STOPFOREGROUND_ACTION);
        PendingIntent PendingIntent_closeIntent = PendingIntent.getService(this, 0, closeIntent, 0);

        Typeface font_fontawesome = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        Typeface font_ailerons = Typeface.createFromAsset( getAssets(), "ailerons-typeface.otf" );
        Typeface font_ninzimay = Typeface.createFromAsset( getAssets(), "ninzimay.ttf" );

        RemoteViews _RemoteViews = new RemoteViews(getPackageName(), R.layout.customized_notification);
        _RemoteViews.setImageViewResource(R.id.imgSongImage, R.drawable.album_art);
        _RemoteViews.setTextViewText(R.id.txtMyanmarInfo, _MusicDictionary.MyanmarTitle);
        _RemoteViews.setTextViewText(R.id.txtEnglishInfo, _MusicDictionary.EnglishTitle);
        _RemoteViews.setOnClickPendingIntent(R.id.btnClose, PendingIntent_closeIntent);
        Log.d(LoggerName, "R.string.Close = "+R.string.Close);
        Log.d(LoggerName, "getString(R.string.Close) = "+getString(R.string.Close));

        SpannableStringBuilder _SpannableStringBuilder = new SpannableStringBuilder();
        _SpannableStringBuilder.append(getString(R.string.Close));
        int start = _SpannableStringBuilder.length();
        _SpannableStringBuilder.setSpan(
                new CustomTypefaceSpan("", font_fontawesome),
                start,
                _SpannableStringBuilder.length(),
                Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
        _RemoteViews.setTextViewText(R.id.btnClose, _SpannableStringBuilder);


        //_RemoteViews.setImageViewBitmap(R.id.btnClose, getBitmapFont(getString(R.string.Close), font_fontawesome));
        _RemoteViews.setOnClickPendingIntent(R.id.btnBackward, PendingIntent_previousIntent);
        _RemoteViews.setOnClickPendingIntent(R.id.btnPlayPause, PendingIntent_playIntent);
        _RemoteViews.setOnClickPendingIntent(R.id.btnForward, PendingIntent_nextIntent);

        Bitmap icon = BitmapFactory.decodeResource(getResources(), R.drawable.album_art);
        Notification notification = new NotificationCompat.Builder(this)
                .setSmallIcon(R.drawable.logo)
                .setContentIntent(_PendingIntent)
                .setOngoing(true)
                .setWhen(System.currentTimeMillis())
                .build();



        notification.bigContentView = _RemoteViews;

        startForeground(Constants.NOTIFICATION_ID.FOREGROUND_SERVICE, notification);
    }
    private void attatchForeground(){
        Intent notificationIntent = new Intent(this, MainActivity.class);
        notificationIntent.setAction("");
        notificationIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);

        PendingIntent _PendingIntent = PendingIntent.getActivity(this, 0, notificationIntent, 0);

        Intent previousIntent = new Intent(this, MusicService.class);
        previousIntent.setAction(Constants.ACTION.PREV_ACTION);
        PendingIntent PendingIntent_previousIntent = PendingIntent.getService(this, 0, previousIntent, 0);

        Intent playIntent = new Intent(this, MusicService.class);
        playIntent.setAction(Constants.ACTION.PLAY_ACTION);
        PendingIntent PendingIntent_playIntent = PendingIntent.getService(this, 0, playIntent, 0);

        Intent nextIntent = new Intent(this, MusicService.class);
        nextIntent.setAction(Constants.ACTION.NEXT_ACTION);
        PendingIntent PendingIntent_nextIntent = PendingIntent.getService(this, 0, nextIntent, 0);

        Intent closeIntent = new Intent(this, MusicService.class);
        closeIntent.setAction(Constants.ACTION.STOPFOREGROUND_ACTION);
        PendingIntent PendingIntent_closeIntent = PendingIntent.getService(this, 0, closeIntent, 0);

        Bitmap icon = BitmapFactory.decodeResource(getResources(), R.drawable.album_art);
        Notification notification = new NotificationCompat.Builder(this)
                .setContentTitle("Truiton Music Player")
                .setTicker("Truiton Music Player")
                .setContentText("My Music")
                .setSmallIcon(R.drawable.logo)
                .setLargeIcon(Bitmap.createScaledBitmap(icon, 128, 128, false))
                .setContentIntent(_PendingIntent)
                .setOngoing(true)
                .addAction(android.R.drawable.ic_media_previous, "", PendingIntent_previousIntent)
                .addAction(android.R.drawable.ic_media_play, "", PendingIntent_closeIntent)
                .addAction(android.R.drawable.ic_media_next, "", PendingIntent_nextIntent)
                //.addAction(android.R.drawable.ic_media_next , "", PendingIntent_closeIntent)
                .build();

        //notification.bigContentView = ...;
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
        //attatchForeground();
        //invokeNotificationView();
        showCustomNotifications();
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
                        if(i == 0) {
                            return null;
                        }
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