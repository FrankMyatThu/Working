package listview.test.listviewer;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListAdapter;
import android.widget.ListView;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        String[] SongList = {"a_chit_lo_khaw_ta_lar", "min", "nay_par_say_chit_lo", "twae_lat_myar"};
        ListAdapter _ListAdapter = new ListViewAdapter(this, SongList);
        ListView _ListView = (ListView) findViewById(R.id.listView);
        _ListView.setAdapter(_ListAdapter);

        _ListView.setOnItemClickListener(
                new AdapterView.OnItemClickListener() {
                    @Override
                    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                        String songInfo = String.valueOf(parent.getItemAtPosition(position));
                        Log.i("", "songInfo = " + songInfo);
                    }
                }
        );
    }

}
