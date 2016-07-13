package ninzimay.mediaplayer.ninzimay;

import android.content.Context;
import android.content.Intent;
import android.graphics.Typeface;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.LocalBroadcastManager;
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

import com.google.gson.Gson;

import java.util.ArrayList;
import java.util.List;


/**
 * Created by myat on 10/6/2016.
 */
public class SongListingRowControl extends BaseAdapter implements View.OnClickListener{
    //<!-- Start declaration area.  -->
    String LoggerName = "NinZiMay";
    private Context _Context;
    private Typeface _Font = null;
    private String PlayingStatus_New = "PlayingStatus_New";
    private String PlayingStatus_Playing = "PlayingStatus_Playing";
    private String PlayingStatus_Played = "PlayingStatus_Played";
    private ArrayList<MusicDictionary> ArrayList_MusicDictionary = new ArrayList<MusicDictionary>();
    private Gson gson = new Gson();
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
        TextView txtSongLength;
        ViewHolder(View _View){
            imgSongImage = (ImageView) _View.findViewById(R.id.imgSongImage);
            txtMyanmarInfo = (TextView) _View.findViewById(R.id.txtMyanmarInfo);
            txtEnglishInfo = (TextView) _View.findViewById(R.id.txtEnglishInfo);
            txtSongLength =  (TextView) _View.findViewById(R.id.txtSongLength);
            btnFavorite = (Button) _View.findViewById(R.id.btnFavorite);
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

        _ViewHolder.btnFavorite.setTypeface(_Font);
        if(_MusicDictionary.IsFavorite){
            _ViewHolder.btnFavorite.setText(_Context.getString(R.string.FavoriteOn));
        }else{
            _ViewHolder.btnFavorite.setText(_Context.getString(R.string.FavoriteOff));
        }

        _ViewHolder.btnFavorite.setTag(position);
        _ViewHolder.btnFavorite.setOnClickListener(this);

        //Log.d(LoggerName , "Title = "+ _MusicDictionary.EnglishTitle +" : Status = "+ _MusicDictionary.PlayingStatus);
        return _View_Row;
    }
    @Override
    public void onClick(View _View) {
        switch (_View.getId()) {
            case R.id.btnFavorite:
                btnFavorite_Click(_View);
                break;
            default:
                break;
        }
    }
    //<!-- Start system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    private void btnFavorite_Click(View _View){
        Boolean IsFavoriteNow = false;
        int position=(Integer)_View.getTag();
        Button _btnFavorite = (Button) _View.findViewById(R.id.btnFavorite);

        String Current_MusicDictionary = gson.toJson(ArrayList_MusicDictionary.get(position));
        Intent intent_Broadcast_Favorite = new Intent(Constants.BROADCAST.CLICK_FAVORITE);
        intent_Broadcast_Favorite.putExtra("Current_MusicDictionary", Current_MusicDictionary);

        if(_Context.getString(R.string.FavoriteOff).equalsIgnoreCase(_btnFavorite.getText().toString())){
            IsFavoriteNow = true;
            intent_Broadcast_Favorite.putExtra("IsFavoriteNow", IsFavoriteNow);
            _btnFavorite.setText(_Context.getString(R.string.FavoriteOn));
        }else{
            IsFavoriteNow = false;
            intent_Broadcast_Favorite.putExtra("IsFavoriteNow", IsFavoriteNow);
            _btnFavorite.setText(_Context.getString(R.string.FavoriteOff));
        }
        LocalBroadcastManager.getInstance(_Context).sendBroadcast(intent_Broadcast_Favorite);
    }
    public void addItems(ArrayList<MusicDictionary> List_MusicDictionary){
        ArrayList_MusicDictionary.clear();
        ArrayList_MusicDictionary.addAll(List_MusicDictionary);
    }
    //<!-- End developer defined function(s).  -->
}