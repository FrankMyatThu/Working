package ninzimay.mediaplayer.ninzimay;

import android.app.Activity;
import android.content.Context;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Typeface;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;

public class MainActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ImageView imgBackgroundImage = (ImageView)findViewById(R.id.imgBackgroundImage);
        loadBitmap(R.drawable.background_1, imgBackgroundImage, this);

        Typeface font_fontawesome = Typeface.createFromAsset( getAssets(), "fontawesome-webfont.ttf" );
        Typeface font_ailerons = Typeface.createFromAsset( getAssets(), "ailerons-typeface.otf" );

        TextView txtTitle = (TextView)findViewById(R.id.txtTitle);
        Button btnShuffle = (Button)findViewById( R.id.btnShuffle );
        Button btnBackward = (Button)findViewById( R.id.btnBackward );
        Button btnPlay = (Button)findViewById( R.id.btnPlay );
        Button btnForward = (Button)findViewById( R.id.btnForward );
        Button btnRepeat = (Button)findViewById( R.id.btnRepeat );
        Button btnLyric = (Button)findViewById( R.id.btnLyric );
        Button btnFavorite = (Button)findViewById( R.id.btnFavorite );
        btnShuffle.setTypeface(font_fontawesome);
        btnBackward.setTypeface(font_fontawesome);
        btnPlay.setTypeface(font_fontawesome);
        btnForward.setTypeface(font_fontawesome);
        btnRepeat.setTypeface(font_fontawesome);
        btnLyric.setTypeface(font_fontawesome);
        btnFavorite.setTypeface(font_fontawesome);
        txtTitle.setTypeface(font_ailerons);

        String[] SongList = {"a_chit_lo_khaw_ta_lar", "min", "nay_par_say_chit_lo", "twae_lat_myar"};
        ListAdapter _ListAdapter = new SongListingRowControl(this, SongList);
        ListView _ListView = (ListView) findViewById(R.id.listView);
        _ListView.setAdapter(_ListAdapter);
        _ListView.setScrollingCacheEnabled(false);

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

    public void loadBitmap(int resId, ImageView imageView, Context _Context) {
        BitmapWorkerTask task = new BitmapWorkerTask(imageView, _Context);
        task.execute(resId);
    }
}
