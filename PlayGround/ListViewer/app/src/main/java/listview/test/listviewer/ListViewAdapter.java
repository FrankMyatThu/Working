package listview.test.listviewer;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

/**
 * Created by myat on 8/6/2016.
 */
public class ListViewAdapter extends ArrayAdapter<String> {
    ListViewAdapter(Context context, String[] _List){
        super(context, R.layout.row_elements, _List);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent){
        LayoutInflater _LayoutInflater = LayoutInflater.from(getContext());
        View ListViewRow = _LayoutInflater.inflate(R.layout.row_elements, parent, false);

        String singleRow = getItem(position);
        TextView txtSongInfo = (TextView) ListViewRow.findViewById(R.id.txtSongInfo);
        ImageView imgSongImage = (ImageView) ListViewRow.findViewById(R.id.imgSongImage);

        txtSongInfo.setText(singleRow);
        imgSongImage.setImageResource(R.drawable.album_art);

        return ListViewRow;
    }
}
