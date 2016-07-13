package ninzimay.mediaplayer.ninzimay;

import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

import java.io.InputStream;
import java.io.InterruptedIOException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by myat on 13/7/2016.
 */
public class DatabaseHandler extends SQLiteOpenHelper {
    private static final String LoggerName = "NinZiMay";
    private static final String DATABASE_NAME = "ninzimay.sqlite";
    private Context _Context;

    public DatabaseHandler(Context context) {
        super(context, DATABASE_NAME, null,
                Integer.parseInt(context.getResources().getString(R.string.DATABASE_VERSION)));
        _Context = context;
        Log.d(LoggerName, "DatabaseHandler DatabaseHandler()");
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        Log.d(LoggerName, "DatabaseHandler onCreate");
        db.beginTransaction();
        try {
            InputStream _InputStream = _Context.getResources().getAssets().open("sql_script_initial.sql");
            String[] statements = FileHelper.parseSqlFile(_InputStream);
            for (String statement : statements) {
                Log.d(LoggerName, "DatabaseHandler statement = "+statement);
                db.execSQL(statement);
                Log.d(LoggerName, "DatabaseHandler statement = "+statement+" Finish.");
            }
            db.setTransactionSuccessful();
        } catch (Exception ex) {
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        Log.d(LoggerName, "DatabaseHandler onUpgrade");
        db.beginTransaction();
        try {
            InputStream _InputStream = _Context.getResources().getAssets().open("sql_script_update.sql");
            String[] statements = FileHelper.parseSqlFile(_InputStream);
            for (String statement : statements) {
                db.execSQL(statement);
            }
            db.setTransactionSuccessful();
        } catch (Exception ex) {
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }

    public ArrayList<MusicDictionary> getAllMusicDictionary() {
        Log.d(LoggerName, "DatabaseHandler getAllMusicDictionary()");
        ArrayList<MusicDictionary> List_MusicDictionary = new ArrayList<MusicDictionary>();
        try{
            String selectQuery = "SELECT  * FROM tbl_MusicDictionary ORDER BY Srno ASC;";
            SQLiteDatabase db = this.getWritableDatabase();
            Cursor cursor = db.rawQuery(selectQuery, null);
            if (cursor.moveToFirst()) {
                do {

                    Log.d(LoggerName, "DatabaseHandler EnglishTitle = "+ cursor.getString(cursor.getColumnIndex("EnglishTitle")));

                    MusicDictionary _MusicDictionary = new MusicDictionary();
                    _MusicDictionary.ID = cursor.getInt(cursor.getColumnIndex("ID"));
                    _MusicDictionary.Srno = cursor.getInt(cursor.getColumnIndex("Srno"));
                    _MusicDictionary.FileName = cursor.getString(cursor.getColumnIndex("FileName"));
                    _MusicDictionary.EnglishTitle = cursor.getString(cursor.getColumnIndex("EnglishTitle"));
                    _MusicDictionary.MyanmarTitle = cursor.getString(cursor.getColumnIndex("MyanmarTitle"));
                    _MusicDictionary.AlbumName = cursor.getString(cursor.getColumnIndex("AlbumName"));
                    _MusicDictionary.AlbumArt = cursor.getString(cursor.getColumnIndex("AlbumArt"));
                    _MusicDictionary.Length = cursor.getString(cursor.getColumnIndex("Length"));
                    _MusicDictionary.Genre = cursor.getString(cursor.getColumnIndex("Genre"));
                    _MusicDictionary.Lyric = cursor.getString(cursor.getColumnIndex("Lyric"));
                    _MusicDictionary.IsFavorite = Boolean.parseBoolean(cursor.getString(cursor.getColumnIndex("IsFavorite"))) ;
                    _MusicDictionary.PlayingStatus = cursor.getString(cursor.getColumnIndex("PlayingStatus"));
                    List_MusicDictionary.add(_MusicDictionary);
                } while (cursor.moveToNext());
            }
        }catch (Exception ex){
            ex.printStackTrace();
        }
        return List_MusicDictionary;
    }
}
