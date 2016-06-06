package audio.mediaplayer.mediaplayer;

import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ImageButton;
import android.widget.SeekBar;
import android.widget.TextView;
import java.io.IOException;
import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private String TagName = "LogMusicApp";
    private int CurrentPlayingLength = 0;
    private MediaPlayer mediaPlayer;
    private ImageButton btnForward;
    private ImageButton btnPause;
    private ImageButton btnPlay;
    private ImageButton btnBackward;
    private TextView txtMessage;
    private SeekBar Seekbar;
    private double startTime = 0;
    private double finalTime = 0;
    private Handler Handler = new Handler();
    private List<MusicDictionary> List_MusicDictionary = null;

    public class MusicDictionary{
        public int Srno;
        public String Name;
        public String Status;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        Field[] fields=R.raw.class.getFields();
        List_MusicDictionary = new ArrayList<MusicDictionary>();
        for(int count=0; count < fields.length; count++){
            Log.i(TagName, "Raw Asset: " + fields[count].getName());
            MusicDictionary _MusicDictionary = new MusicDictionary();
            _MusicDictionary.Srno = count + 1 ;
            _MusicDictionary.Name = fields[count].getName();
            _MusicDictionary.Status = "New";
            List_MusicDictionary.add(_MusicDictionary);
        }

        /// Button(s)
        btnForward = (ImageButton) findViewById(R.id.btnForward);
        btnPause = (ImageButton) findViewById(R.id.btnPause);
        btnPlay = (ImageButton) findViewById(R.id.btnPlay);
        btnBackward = (ImageButton) findViewById(R.id.btnBackward);
        txtMessage = (TextView) findViewById(R.id.txtMessage);
        btnPause.setEnabled(false);

        btnForward.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(TagName, "btnForward");
                txtMessage.setText("Forward");
                PlaySong(List_MusicDictionary.get(3).Name);
            }
        });

        btnPause.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(TagName, "btnPause");
                txtMessage.setText("Pause");
                mediaPlayer.pause();
                CurrentPlayingLength = mediaPlayer.getCurrentPosition();
                ButtonEnableDisable("Pause");
            }
        });

        btnPlay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(TagName, "btnPlay");
                txtMessage.setText("Playing");
                PlaySong(List_MusicDictionary.get(0).Name);
                Handler.postDelayed(UpdateSongTime, 100);
                ButtonEnableDisable("Play");

            }
        });

        btnBackward.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.i(TagName, "btnBackward");
                txtMessage.setText("Backward");
                PlaySong(List_MusicDictionary.get(0).Name);
            }
        });


        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
    }

    private void ButtonEnableDisable(String State){
        if(State.equalsIgnoreCase("Play")){
            btnPause.setEnabled(true);
            btnPlay.setEnabled(false);
        }else if(State.equalsIgnoreCase("Pause")){
            btnPause.setEnabled(false);
            btnPlay.setEnabled(true);
        }
    }


    private void PlaySong(String Name){
        String path = "android.resource://"+getPackageName()+"/raw/"+Name;
        Log.i(TagName, "path = "+ path);
        try {

            if(CurrentPlayingLength > 0){
                /// Play song after pause
                mediaPlayer.seekTo(CurrentPlayingLength);
                mediaPlayer.start();
                CurrentPlayingLength = 0;
                txtMessage.setText(Name);
            }else
            {
                /// Just playing song from start of the length
                /// MediaPlayer initialization
                if(mediaPlayer != null)
                    mediaPlayer.reset();
                else
                    mediaPlayer = new MediaPlayer();
                mediaPlayer.setDataSource(this, Uri.parse(path));
                mediaPlayer.prepare();
                mediaPlayer.start();
                txtMessage.setText(Name);
            }

            /// MediaPlayer
            mediaPlayer.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
                public void onCompletion(MediaPlayer _MediaPlayer) {
                    Log.i(TagName, "Finish");
                    txtMessage.setText("Finish");
                    PlaySong("nay_par_say_chit_lo");
                }
            });
            finalTime = mediaPlayer.getDuration();
            Log.i(TagName, "finalTime = " + finalTime);
            startTime = mediaPlayer.getCurrentPosition();
            Log.i(TagName, "startTime = " + startTime);

            /// SeekBar
            Seekbar = (SeekBar)findViewById(R.id.SeekBar);
            Seekbar.setMax((int) finalTime);
            Seekbar.setProgress((int) startTime);
            Seekbar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
                @Override
                public void onStopTrackingTouch(SeekBar seekBar) {
                }

                @Override
                public void onStartTrackingTouch(SeekBar seekBar) {
                }

                @Override
                public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                    //Log.i(TagName, "onProgressChanged()");
                    if (mediaPlayer != null && fromUser) {
                        //Log.i(TagName, "onProgressChanged() progress = "+progress);
                        mediaPlayer.seekTo(progress);
                    }
                }
            });

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private Runnable UpdateSongTime = new Runnable() {
        public void run() {
            startTime = mediaPlayer.getCurrentPosition();
            Seekbar.setProgress((int)startTime);
            Handler.postDelayed(this, 100);
        }
    };

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        if (id == R.id.nav_camera) {
            // Handle the camera action
        } else if (id == R.id.nav_gallery) {

        } else if (id == R.id.nav_slideshow) {

        } else if (id == R.id.nav_manage) {

        } else if (id == R.id.nav_share) {

        } else if (id == R.id.nav_send) {

        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }
}
