package ninzimay.mediaplayer.ninzimay;

import android.app.Activity;
import android.graphics.Color;
import android.graphics.Typeface;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;

public class MainActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Typeface font = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        Button btnShuffle = (Button)findViewById( R.id.btnShuffle );
        Button btnBackward = (Button)findViewById( R.id.btnBackward );
        Button btnPlay = (Button)findViewById( R.id.btnPlay );
        Button btnForward = (Button)findViewById( R.id.btnForward );
        Button btnRepeat = (Button)findViewById( R.id.btnRepeat );
        Button btnLyric = (Button)findViewById( R.id.btnLyric );
        Button btnFavorite = (Button)findViewById( R.id.btnFavorite );
        btnShuffle.setTypeface(font);
        btnBackward.setTypeface(font);
        btnPlay.setTypeface(font);
        btnForward.setTypeface(font);
        btnRepeat.setTypeface(font);
        btnLyric.setTypeface(font);
        btnFavorite.setTypeface(font);

        String[] SongList = {"a_chit_lo_khaw_ta_lar", "min", "nay_par_say_chit_lo", "twae_lat_myar"};
        ListAdapter _ListAdapter = new SongListingRowControl(this, SongList);
        ListView _ListView = (ListView) findViewById(R.id.listView);
        _ListView.setAdapter(_ListAdapter);

        _ListView.setOnItemClickListener(
                new AdapterView.OnItemClickListener() {
                    @Override
                    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                        String songInfo = String.valueOf(parent.getItemAtPosition(position));
                        //Log.i("", "songInfo = " + songInfo);
                    }
                }
        );
    }
}
