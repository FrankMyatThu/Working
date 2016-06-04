package audio.mediaplayer.mediaplayer;

import android.content.res.AssetFileDescriptor;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
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

import java.io.File;
import java.io.FileDescriptor;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private String TagName = "LogMusicApp";
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

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        mediaPlayer = MediaPlayer.create(this, R.raw.nay_par_say_chit_lo);
        mediaPlayer.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
            public void onCompletion(MediaPlayer _MediaPlayer) {
                Log.e(TagName, "Finish");
                txtMessage.setText("Finish");
            }
        });
        finalTime = mediaPlayer.getDuration();
        Log.e(TagName, "finalTime = " + finalTime);
        startTime = mediaPlayer.getCurrentPosition();
        Log.e(TagName, "startTime = " + startTime);

        Seekbar = (SeekBar)findViewById(R.id.SeekBar);
        Seekbar.setMax((int) finalTime);
        Seekbar.setProgress((int) startTime);
        Seekbar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                Log.e(TagName, "onProgressChanged()");
                if (mediaPlayer != null && fromUser) {
                    Log.e(TagName, "onProgressChanged() progress = "+progress);
                    mediaPlayer.seekTo(progress);
                }
            }
        });

        btnForward = (ImageButton) findViewById(R.id.btnForward);
        btnPause = (ImageButton) findViewById(R.id.btnPause);
        btnPlay = (ImageButton) findViewById(R.id.btnPlay);
        btnBackward = (ImageButton) findViewById(R.id.btnBackward);
        txtMessage = (TextView) findViewById(R.id.txtMessage);

        btnForward.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.e(TagName, "btnForward");
                txtMessage.setText("Forward");

                InputStream ins = getResources().openRawResource(R.raw.min);

                /*
                AssetFileDescriptor _AssetFileDescriptor = getResources().openRawResourceFd(R.raw.min);
                if (_AssetFileDescriptor == null) return;
                try {
                    mediaPlayer.reset();
                    mediaPlayer.setDataSource(
                            _AssetFileDescriptor.getFileDescriptor(),
                            _AssetFileDescriptor.getStartOffset(),
                            _AssetFileDescriptor.getLength());
                    _AssetFileDescriptor.close();
                    mediaPlayer.start();
                } catch (IOException e) {
                    e.printStackTrace();
                }
                */

            }
        });

        btnPause.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.e(TagName, "btnPause");
                txtMessage.setText("Pause");
                mediaPlayer.pause();
            }
        });

        btnPlay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.e(TagName, "btnPlay");
                txtMessage.setText("Playing");
                mediaPlayer.start();
                Handler.postDelayed(UpdateSongTime, 100);
            }
        });

        btnBackward.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View _View) {
                Log.e(TagName, "btnBackward");
                txtMessage.setText("Backward");
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

    private FileDescriptor openFile(String path)
            throws FileNotFoundException, IOException {
        Log.e(TagName, "openFile() path = "+path);
        File file = new File(path);
        FileOutputStream fos = new FileOutputStream(file);
        fos.write((" Beginning of process...").getBytes());
        return fos.getFD();
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
