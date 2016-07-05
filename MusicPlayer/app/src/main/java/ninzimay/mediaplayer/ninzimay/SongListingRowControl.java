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
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.VideoView;

import java.util.ArrayList;
import java.util.List;


/**
 * Created by myat on 10/6/2016.
 */
public class SongListingRowControl extends BaseAdapter {
    //<!-- Start declaration area.  -->
    String LoggerName = "NinZiMay";
    private Context _Context;
    private Typeface _Font = null;
    private String PlayingStatus_New = "PlayingStatus_New";
    private String PlayingStatus_Playing = "PlayingStatus_Playing";
    private String PlayingStatus_Played = "PlayingStatus_Played";
    private ArrayList<MusicDictionary> ArrayList_MusicDictionary = new ArrayList<MusicDictionary>();
    //<!-- End declaration area.  -->

    //<!-- Start constructor.  -->
    SongListingRowControl(Context context, ArrayList<MusicDictionary> List_MusicDictionary){
        this._Context = context;
        ArrayList_MusicDictionary.clear();
        ArrayList_MusicDictionary.addAll(List_MusicDictionary);
        _Font = Typeface.createFromAsset(_Context.getAssets(), "fontawesome-webfont.ttf");
    }
    //<!-- End constructor.  -->

    //<!-- Start dependency object(s).  -->
    class ViewHolder{
        ImageView imgSongImage;
        TextView txtMyanmarInfo;
        TextView txtEnglishInfo;
        Button btnFavorite;
        Button btnRunningSong;
        TextView txtSongLength;
        ViewHolder(View _View){
            imgSongImage = (ImageView) _View.findViewById(R.id.imgSongImage);
            txtMyanmarInfo = (TextView) _View.findViewById(R.id.txtMyanmarInfo);
            txtEnglishInfo = (TextView) _View.findViewById(R.id.txtEnglishInfo);
            txtSongLength =  (TextView) _View.findViewById(R.id.txtSongLength);
            btnFavorite = (Button) _View.findViewById(R.id.btnFavorite);
            btnRunningSong = (Button) _View.findViewById(R.id.btnRunningSong);
        }
    }
    //<!-- End dependency object(s).  -->

    //<!-- Start system defined function(s).  -->
    @Override
    public int getCount() {
        return ArrayList_MusicDictionary.size();
    }
    @Override
    public Object getItem(int position) {
        return ArrayList_MusicDictionary.get(position);
    }
    @Override
    public long getItemId(int position) {
        return position;
    }
    @Override
    public View getView(int position, View convertView, ViewGroup parent){

        View _View_Row = convertView;
        ViewHolder _ViewHolder = null;

        if(_View_Row == null)
        {
            LayoutInflater _LayoutInflater = LayoutInflater.from(_Context);
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

        MusicDictionary _MusicDictionary = ArrayList_MusicDictionary.get(position);
        _ViewHolder.btnFavorite.setTypeface(_Font);
        _ViewHolder.btnRunningSong.setTypeface(_Font);
        _ViewHolder.txtEnglishInfo.setText(_MusicDictionary.EnglishTitle);
        _ViewHolder.txtMyanmarInfo.setText(_MusicDictionary.MyanmarTitle);
        _ViewHolder.txtSongLength.setText(_MusicDictionary.Length);
        _ViewHolder.imgSongImage.setImageResource(R.drawable.album_art);

        if (_MusicDictionary.PlayingStatus.equalsIgnoreCase(PlayingStatus_Playing)){
            _ViewHolder.txtEnglishInfo.setTextColor(ContextCompat.getColor(_Context, R.color.lightOrange));
            _ViewHolder.txtMyanmarInfo.setTextColor(ContextCompat.getColor(_Context, R.color.lightOrange));
        }else{
            _ViewHolder.txtEnglishInfo.setTextColor(ContextCompat.getColor(_Context, R.color.lightgray));
            _ViewHolder.txtMyanmarInfo.setTextColor(ContextCompat.getColor(_Context, R.color.lightgray));
        }
        //Log.d(LoggerName , "Title = "+ _MusicDictionary.EnglishTitle +" : Status = "+ _MusicDictionary.PlayingStatus);
        return _View_Row;
    }
    //<!-- Start system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    public void addItems(ArrayList<MusicDictionary> List_MusicDictionary){
        ArrayList_MusicDictionary.clear();
        ArrayList_MusicDictionary.addAll(List_MusicDictionary);
    }
    //<!-- End developer defined function(s).  -->
}