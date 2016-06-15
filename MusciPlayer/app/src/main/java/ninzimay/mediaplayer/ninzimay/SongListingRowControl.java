package ninzimay.mediaplayer.ninzimay;

import android.content.Context;
import android.graphics.Typeface;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.animation.Animation;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.VideoView;

/**
 * Created by myat on 10/6/2016.
 */
public class SongListingRowControl extends ArrayAdapter<String> {
    Typeface _Font = null;
    SongListingRowControl(Context context, String[] _List){
        super(context, R.layout.song_listing_row_control, _List);
        _Font = Typeface.createFromAsset(this.getContext().getAssets(), "fontawesome-webfont.ttf");
    }

    class ViewHolder{
        ImageView imgSongImage;
        TextView txtMyanmarInfo;
        TextView txtEnglishInfo;
        Button btnFavorite;
        //Button btnRunningSong;
        //ImageView imgRunningSong;
        ViewHolder(View _View){
            imgSongImage = (ImageView) _View.findViewById(R.id.imgSongImage);
            txtMyanmarInfo = (TextView) _View.findViewById(R.id.txtMyanmarInfo);
            txtEnglishInfo = (TextView) _View.findViewById(R.id.txtEnglishInfo);
            btnFavorite = (Button) _View.findViewById(R.id.btnFavorite);
            //btnRunningSong = (Button) _View.findViewById(R.id.btnRunningSong);
            //imgRunningSong = (ImageView) _View.findViewById(R.id.imgRunningSong);
        }
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent){

        View _View_Row = convertView;
        ViewHolder _ViewHolder = null;

        if(_View_Row == null)
        {
            LayoutInflater _LayoutInflater = LayoutInflater.from(getContext());
            _View_Row = _LayoutInflater.inflate(R.layout.song_listing_row_control, parent, false);
            _ViewHolder = new ViewHolder(_View_Row);
            _View_Row.setTag(_ViewHolder);
            Log.d("NinZiMay", "SetTag");
        }
        else
        {
            _ViewHolder = (ViewHolder) _View_Row.getTag();
            Log.d("NinZiMay", "GetTag");
        }


        String singleRow = getItem(position);
        _ViewHolder.btnFavorite.setTypeface(_Font);
        //_ViewHolder.btnRunningSong.setTypeface(_Font);
        _ViewHolder.txtEnglishInfo.setText(singleRow);
        _ViewHolder.txtMyanmarInfo.setText(singleRow);
        _ViewHolder.imgSongImage.setImageResource(R.drawable.album_art);

        return _View_Row;
    }
}