package ninzimay.mediaplayer.ninzimay;

import android.content.Context;
import android.graphics.Typeface;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

/**
 * Created by myat on 10/6/2016.
 */
public class SongListingRowControl extends ArrayAdapter<String> {
    SongListingRowControl(Context context, String[] _List){
        super(context, R.layout.song_listing_row_control, _List);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent){
        LayoutInflater _LayoutInflater = LayoutInflater.from(getContext());
        View _SongLisingRowControl = _LayoutInflater.inflate(R.layout.song_listing_row_control, parent, false);
        Typeface font = Typeface.createFromAsset(this.getContext().getAssets(), "fontawesome-webfont.ttf");

        String singleRow = getItem(position);
        ImageView imgSongImage = (ImageView) _SongLisingRowControl.findViewById(R.id.imgSongImage);

        TextView txtMyanmarInfo = (TextView) _SongLisingRowControl.findViewById(R.id.txtMyanmarInfo);
        TextView txtEnglishInfo = (TextView) _SongLisingRowControl.findViewById(R.id.txtEnglishInfo);

        Button btnFavorite = (Button) _SongLisingRowControl.findViewById(R.id.btnFavorite);
        Button btnRunningSong = (Button) _SongLisingRowControl.findViewById(R.id.btnRunningSong);

        btnFavorite.setTypeface(font);
        btnRunningSong.setTypeface(font);

        txtEnglishInfo.setText(singleRow);
        imgSongImage.setImageResource(R.drawable.album_art);

        return _SongLisingRowControl;
    }
}