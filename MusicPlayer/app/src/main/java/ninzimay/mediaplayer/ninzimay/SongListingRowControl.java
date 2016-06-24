package ninzimay.mediaplayer.ninzimay;

import android.content.Context;
import android.graphics.Typeface;
import android.support.v4.content.ContextCompat;
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

import java.util.ArrayList;
import java.util.List;


/**
 * Created by myat on 10/6/2016.
 */
public class SongListingRowControl extends ArrayAdapter<MusicDictionary> {
    Typeface _Font = null;
    String LoggerName = "NinZiMay";
    private String PlayingStatus_New = "PlayingStatus_New";
    private String PlayingStatus_Playing = "PlayingStatus_Playing";
    private String PlayingStatus_Played = "PlayingStatus_Played";
    
    SongListingRowControl(Context context, List<MusicDictionary> _List){
        super(context, R.layout.song_listing_row_control, _List);
        _Font = Typeface.createFromAsset(this.getContext().getAssets(), "fontawesome-webfont.ttf");
    }

    class ViewHolder{
        ImageView imgSongImage;
        TextView txtMyanmarInfo;
        TextView txtEnglishInfo;
        Button btnFavorite;
        Button btnRunningSong;
        TextView txtSongLength;
        //ImageView imgRunningSong;
        ViewHolder(View _View){
            imgSongImage = (ImageView) _View.findViewById(R.id.imgSongImage);
            txtMyanmarInfo = (TextView) _View.findViewById(R.id.txtMyanmarInfo);
            txtEnglishInfo = (TextView) _View.findViewById(R.id.txtEnglishInfo);
            txtSongLength =  (TextView) _View.findViewById(R.id.txtSongLength);
            btnFavorite = (Button) _View.findViewById(R.id.btnFavorite);
            btnRunningSong = (Button) _View.findViewById(R.id.btnRunningSong);
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
            //Log.d("NinZiMay", "SetTag");
        }
        else
        {
            _ViewHolder = (ViewHolder) _View_Row.getTag();
            //Log.d("NinZiMay", "GetTag");
        }


        MusicDictionary _MusicDictionary = getItem(position);
        _ViewHolder.btnFavorite.setTypeface(_Font);
        _ViewHolder.btnRunningSong.setTypeface(_Font);
        _ViewHolder.txtEnglishInfo.setText(_MusicDictionary.EnglishTitle);
        _ViewHolder.txtMyanmarInfo.setText(_MusicDictionary.MyanmarTitle);
        _ViewHolder.txtSongLength.setText(_MusicDictionary.Length);
        _ViewHolder.imgSongImage.setImageResource(R.drawable.album_art);

        if(_MusicDictionary.PlayingStatus == PlayingStatus_Playing){
            _ViewHolder.txtEnglishInfo.setTextColor(ContextCompat.getColor(getContext(), R.color.lightOrange));
            _ViewHolder.txtMyanmarInfo.setTextColor(ContextCompat.getColor(getContext(), R.color.lightOrange));
        }else{
            _ViewHolder.txtEnglishInfo.setTextColor(ContextCompat.getColor(getContext(), R.color.lightgray));
            _ViewHolder.txtMyanmarInfo.setTextColor(ContextCompat.getColor(getContext(), R.color.lightgray));
        }

        //Log.d(LoggerName , "Title = "+ _MusicDictionary.EnglishTitle +" : Status = "+ _MusicDictionary.PlayingStatus);

        return _View_Row;
    }
}