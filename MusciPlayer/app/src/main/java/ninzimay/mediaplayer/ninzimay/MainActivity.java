package ninzimay.mediaplayer.ninzimay;

import android.app.Activity;
import android.graphics.Color;
import android.graphics.Typeface;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Button;

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
        btnShuffle.setTypeface(font);
        btnBackward.setTypeface(font);
        btnPlay.setTypeface(font);
        btnForward.setTypeface(font);
        btnRepeat.setTypeface(font);

    }
}
