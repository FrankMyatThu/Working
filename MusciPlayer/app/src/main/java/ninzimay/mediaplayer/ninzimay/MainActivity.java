package ninzimay.mediaplayer.ninzimay;

import android.app.Activity;
import android.content.Context;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.media.AudioManager;
import android.os.Handler;
import android.graphics.Typeface;
import android.media.MediaPlayer;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SeekBar;
import android.widget.TextView;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

public class MainActivity extends Activity {

    /// <!-- Declaration area start. -->
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
    double startTime = 0;
    double finalTime = 0;
    boolean IsRepeatAlbum = true;
    boolean IsShuffle = false;
    boolean IsUserSeekingSliderBar = false;
    Handler Handler = new Handler();
    /// <!-- Declaration area end. -->

    /// <!-- Get music list start. -->
    public List<MusicDictionary> getList_MusicDictionary(){

        List_MusicDictionary = new ArrayList<MusicDictionary>();

        MusicDictionary _MusicDictionary_1 = new MusicDictionary();
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
    /// <!-- Get music list end. -->

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        /// <!-- Invoking background image loader start. -->
        ImageView imgBackgroundImage = (ImageView)findViewById(R.id.imgBackgroundImage);
        loadBitmap(R.drawable.background_1, imgBackgroundImage, this);
        /// <!-- Invoking background image loader end. -->

        /// <!-- Get customized font start. -->
        Typeface font_fontawesome = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        Typeface font_ailerons = Typeface.createFromAsset( getAssets(), "ailerons-typeface.otf" );
        /// <!-- Get customized font end. -->

        /// <!-- Bind basic controls start. -->
        txtTitle = (TextView)findViewById(R.id.txtTitle);
        txtStartPoint = (TextView)findViewById(R.id.txtStartPoint);
        txtEndPoint = (TextView)findViewById(R.id.txtEndPoint);
        txtCurrentPlayingMyanmarInfo = (TextView)findViewById(R.id.txtCurrentPlayingMyanmarInfo);
        txtCurrentPlayingEnglishInfo = (TextView)findViewById(R.id.txtCurrentPlayingEnglishInfo);
        Seekbar = (SeekBar)findViewById(R.id.SeekBar);
        btnShuffle = (Button)findViewById( R.id.btnShuffle );
        btnBackward = (Button)findViewById( R.id.btnBackward );
        btnPlay = (Button)findViewById( R.id.btnPlay );
        btnForward = (Button)findViewById( R.id.btnForward );
        btnRepeat = (Button)findViewById( R.id.btnRepeat );
        btnLyric = (Button)findViewById( R.id.btnLyric );
        btnFavorite = (Button)findViewById( R.id.btnFavorite );
        btnShuffle.setTypeface(font_fontawesome);
        btnBackward.setTypeface(font_fontawesome);
        btnPlay.setTypeface(font_fontawesome);
        btnForward.setTypeface(font_fontawesome);
        btnRepeat.setTypeface(font_fontawesome);
        btnLyric.setTypeface(font_fontawesome);
        btnFavorite.setTypeface(font_fontawesome);
        txtTitle.setTypeface(font_ailerons);

        /// <!-- Bind button event start. -->
        btnForward.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(LoggerName, "btnForward");
                //txtMessage.setText("Forward");
                //PlaySong(GetToPlaySong(false, IsRepeatAlbum));
            }
        });

        /*btnPause.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(TagName, "btnPause");
                txtMessage.setText("Pause");
                mediaPlayer.pause();
                CurrentPlayingLength = mediaPlayer.getCurrentPosition();
                ButtonEnableDisable("Pause");
            }
        });*/


        btnPlay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(LoggerName, "btnPlay");

                //<string name="Play">&#xf04b;</string>
                //<string name="Pause">&#xf04c;</string>
                /*if(btnPlay.getText().equals("&#xf04b;"))  /// Play
                {
                    /// Play from the start of the each song(s).
                    PlaySong(GetToPlaySong(true, IsRepeatAlbum, IsShuffle));
                    //btnPlay.setText("&#xf04c;"); /// Pause
                }
                else
                {
                    /// Play after pause.
                    //mediaPlayer.pause();
                    //CurrentPlayingLength = mediaPlayer.getCurrentPosition();
                    //PlaySong(_MusicDictionary);
                    //btnPlay.setText("&#xf04b;"); /// Play
                }*/

                if (CurrentPlayingLength > 0) {
                    /// Play after pause.
                    PlaySong(_MusicDictionary);
                } else {
                    /// Play from the start of the each song(s).
                    PlaySong(GetToPlaySong(true, IsRepeatAlbum, IsShuffle));
                }

                Handler.postDelayed(UpdateSongTime, 100);
                //ButtonEnableDisable("Play");
            }
        });

        btnBackward.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(LoggerName, "btnBackward");
                //txtMessage.setText("Backward");
                //PlaySong(List_MusicDictionary.get(0).Name);
            }
        });
        /// <!-- Bind button event end. -->
        /// <!-- Bind basic controls end. -->

        /// <!-- Bind List controls start. -->
        ListAdapter _ListAdapter = new SongListingRowControl(this, getList_MusicDictionary());
        ListView _ListView = (ListView) findViewById(R.id.listView);
        _ListView.setAdapter(_ListAdapter);
        _ListView.setScrollingCacheEnabled(false);
        _ListView.setOnItemClickListener(
                new AdapterView.OnItemClickListener() {
                    @Override
                    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                        String Clicked_FileName = ((MusicDictionary)parent.getItemAtPosition(position)).FileName;
                        Log.i(LoggerName, "Clicked_FileName = " + Clicked_FileName);
                    }
                }
        );
        /// <!-- Bind List controls end. -->


    }

    /// <!-- Update song and its info handler start. -->
    private Runnable UpdateSongTime = new Runnable() {
        public void run() {
            if(!IsUserSeekingSliderBar){
                startTime = mediaPlayer.getCurrentPosition();
                Seekbar.setProgress((int) startTime);
                setProgressText();
            }
            Handler.postDelayed(this, 100);
        }
    };
    /// <!-- Update song and its info handler end. -->

    /// <!-- Get current playing length start. -->
    protected void setProgressText() {

        final int HOUR = 60*60*1000;
        final int MINUTE = 60*1000;
        final int SECOND = 1000;

        int durationInMillis = mediaPlayer.getDuration();
        int curVolume = mediaPlayer.getCurrentPosition();

        int durationHour = durationInMillis/HOUR;
        int durationMint = (durationInMillis%HOUR)/MINUTE;
        int durationSec = (durationInMillis%MINUTE)/SECOND;

        int currentHour = curVolume/HOUR;
        int currentMint = (curVolume%HOUR)/MINUTE;
        int currentSec = (curVolume%MINUTE)/SECOND;

        txtStartPoint.setText(currentMint +":"+ currentSec);
        txtEndPoint.setText(durationMint +":"+ durationSec);
    }
    /// <!-- Get current playing length end. -->

    /// <!-- Loading bitmap in background thread start. -->
    public void loadBitmap(int resId, ImageView imageView, Context _Context) {
        BitmapWorkerTask task = new BitmapWorkerTask(imageView, _Context);
        task.execute(resId);
    }
    /// <!-- Loading bitmap in background thread end. -->

    /// <!-- Play song start. -->
    private void PlaySong(MusicDictionary _MusicDictionary){
        String path = "android.resource://"+getPackageName()+"/raw/"+_MusicDictionary.FileName;
        Log.i(LoggerName, "path = " + path);
        try {

            if (CurrentPlayingLength > 0) {
                /// Play song after pause
                mediaPlayer.seekTo(CurrentPlayingLength);
                mediaPlayer.start();
                CurrentPlayingLength = 0;
                //txtCurrentPlayingMyanmarInfo.setText(_MusicDictionary.MyanmarTitle);
                //txtCurrentPlayingEnglishInfo.setText(_MusicDictionary.EnglishTitle);
            } else {
                /// Just playing song from start of the length
                /// MediaPlayer initialization
                if (mediaPlayer != null)
                    mediaPlayer.reset();
                else
                    mediaPlayer = new MediaPlayer();

                mediaPlayer.setAudioStreamType(AudioManager.STREAM_MUSIC);
                mediaPlayer.setDataSource(this, Uri.parse(path));
                mediaPlayer.prepareAsync();
                mediaPlayer.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                    @Override
                    public void onPrepared(MediaPlayer mp) {
                        /// MediaPlayer
                        mediaPlayer.start();
                        mediaPlayer.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
                            public void onCompletion(MediaPlayer _MediaPlayer) {
                                txtCurrentPlayingMyanmarInfo.setText("");
                                txtCurrentPlayingEnglishInfo.setText("");
                                mediaPlayer.release();
                                mediaPlayer = null;
                                Log.i(LoggerName, "Finish and Released object.");
                                PlaySong(GetToPlaySong(false, IsRepeatAlbum, IsShuffle));
                            }
                        });
                        finalTime = mediaPlayer.getDuration();
                        Log.i(LoggerName, "finalTime = " + finalTime);
                        startTime = mediaPlayer.getCurrentPosition();
                        Log.i(LoggerName, "startTime = " + startTime);

                        /// SeekBar
                        Seekbar = (SeekBar) findViewById(R.id.SeekBar);
                        Seekbar.setMax((int) finalTime);
                        Seekbar.setProgress((int) startTime);
                        Seekbar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {

                            @Override
                            public void onStopTrackingTouch(SeekBar seekBar) {
                                IsUserSeekingSliderBar = false;
                                mediaPlayer.seekTo(seekBar.getProgress());
                                CurrentPlayingLength = 0;
                            }

                            @Override
                            public void onStartTrackingTouch(SeekBar seekBar) {
                                IsUserSeekingSliderBar = true;
                            }

                            @Override
                            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                                if (mediaPlayer != null && fromUser && IsUserSeekingSliderBar) {
                                    String _Progress = String.format("%d:%d",
                                            TimeUnit.MILLISECONDS.toMinutes(progress),
                                            TimeUnit.MILLISECONDS.toSeconds(progress) -
                                                    TimeUnit.MINUTES.toSeconds(TimeUnit.MILLISECONDS.toMinutes(progress))
                                    );
                                    Log.i(LoggerName, "progress = "+_Progress);
                                    txtStartPoint.setText(_Progress);
                                }
                            }
                        });
                    }
                });
                txtCurrentPlayingMyanmarInfo.setText(_MusicDictionary.MyanmarTitle);
                txtCurrentPlayingEnglishInfo.setText(_MusicDictionary.EnglishTitle);
            }

        } catch (IllegalArgumentException e){
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    /// <!-- Play song end. -->

    /// <!-- Get song start. -->
    private MusicDictionary GetToPlaySong(Boolean IsFirstSong, Boolean IsRepeatAlbum, Boolean IsShuffle)
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
    /// <!-- Get song end. -->

}