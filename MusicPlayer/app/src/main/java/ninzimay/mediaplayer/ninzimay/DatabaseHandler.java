package ninzimay.mediaplayer.ninzimay;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.DatabaseUtils;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

import java.io.IOException;
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

    //<!-- Start constructor.  -->
    public DatabaseHandler(Context context) {
        super(context, DATABASE_NAME, null,
                Integer.parseInt(context.getResources().getString(R.string.DATABASE_VERSION)));
        _Context = context;
    }
    //<!-- End constructor.  -->

    //<!-- Start system defined function(s).  -->
    @Override
    public void onCreate(SQLiteDatabase db) {
        Log.d(LoggerName, "DatabaseHandler onCreate");
        db.beginTransaction();
        try {
            InputStream _InputStream = _Context.getResources().getAssets().open("sql_script_initial.sql");
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
    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        Log.d(LoggerName, "DatabaseHandler onUpgrade");
        InputStream _InputStream = null;
        db.beginTransaction();
        try {
            /// http://stackoverflow.com/a/26916986/900284
            /// http://stackoverflow.com/a/8133640/900284
            ArrayList<String> List_FileName = new ArrayList<String>();

            switch(oldVersion) {
                case 1:
                    Log.d(LoggerName, "DatabaseHandler onUpgrade sql_script_update_20160715_0615pm");
                    List_FileName.add("sql_script_update_20160715_0615pm.sql");
                case 2:
                    Log.d(LoggerName, "DatabaseHandler onUpgrade sql_script_update_20160716_1201am");
                    List_FileName.add("sql_script_update_20160716_1201am.sql");
                    break;
                default:
                    throw new IllegalStateException("onUpgrade() with unknown oldVersion " + oldVersion);
            }

            Log.d(LoggerName, "DatabaseHandler onUpgrade List_FileName.size() = " + List_FileName.size());
            for(int i=0; i < List_FileName.size(); i++){
                _InputStream = _Context.getResources().getAssets().open(List_FileName.get(i));
                String[] statements = FileHelper.parseSqlFile(_InputStream);
                for (String statement : statements) {
                    db.execSQL(statement);
                }
                _InputStream.close();
            }

            db.setTransactionSuccessful();
        } catch (Exception ex) {
            ex.printStackTrace();
        }finally {
            db.endTransaction();
            if(_InputStream != null){
                try {
                    _InputStream.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
    }
    //<!-- End system defined function(s).  -->

    //<!-- Start developer defined function(s).  -->
    //<!-- Start Create function(s).  -->
    //<!-- End Create function(s).  -->
    //<!-- Start Retrieve function(s).  -->
    public Boolean IsFavoriteOn() {
        //Log.d(LoggerName, "DatabaseHandler IsFavoriteOn()");
        long RowCount = 0;
        try{
            String selectQuery = "SELECT COUNT(*) FROM tbl_MusicDictionary WHERE IsFavorite = 'true'";
            SQLiteDatabase db = this.getWritableDatabase();
            RowCount = DatabaseUtils.longForQuery(db, selectQuery, null);
        }catch (Exception ex){
            ex.printStackTrace();
        }
        return RowCount > 0;
    }
    public Setting getPlayerSetting(){
        //Log.d(LoggerName, "DatabaseHandler getPlayerSetting()");
        Cursor cursor = null;
        Setting _Setting = new Setting();
        try{
            String selectQuery = "SELECT  * FROM tbl_Setting";
            SQLiteDatabase db = this.getWritableDatabase();
            cursor = db.rawQuery(selectQuery, null);
            if (cursor.moveToFirst()) {
                do {
                    _Setting.IsFavoriteOn = Boolean.parseBoolean(cursor.getString(cursor.getColumnIndex("IsFavoriteOn")));
                    _Setting.RepeatStatus = cursor.getString(cursor.getColumnIndex("RepeatStatus"));
                    _Setting.IsShuffleOn = Boolean.parseBoolean(cursor.getString(cursor.getColumnIndex("IsShuffleOn")));
                    _Setting.MyanmarFontName = cursor.getString(cursor.getColumnIndex("MyanmarFontName"));
                } while (cursor.moveToNext());
            }
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            if(cursor != null)
                cursor.close();
        }
        return _Setting;
    }
    public ArrayList<MusicDictionary> getAllMusicDictionary() {
        //Log.d(LoggerName, "DatabaseHandler getAllMusicDictionary()");
        ArrayList<MusicDictionary> List_MusicDictionary = new ArrayList<MusicDictionary>();
        Cursor cursor = null;
        try{
            String selectQuery = "SELECT  * FROM tbl_MusicDictionary ORDER BY Srno ASC;";
            SQLiteDatabase db = this.getWritableDatabase();
            cursor = db.rawQuery(selectQuery, null);
            if (cursor.moveToFirst()) {
                do {
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
                    _MusicDictionary.IsFavorite = Boolean.parseBoolean(cursor.getString(cursor.getColumnIndex("IsFavorite")));
                    _MusicDictionary.PlayingStatus = cursor.getString(cursor.getColumnIndex("PlayingStatus"));
                    List_MusicDictionary.add(_MusicDictionary);
                } while (cursor.moveToNext());
            }
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            if(cursor != null)
                cursor.close();
        }
        return List_MusicDictionary;
    }
    //<!-- End Retrieve function(s).  -->
    //<!-- Start Update function(s).  -->
    public void updateMusicDictionary(int ID, boolean IsFavoriteOn){
        SQLiteDatabase db = this.getWritableDatabase();
        db.beginTransaction();
        try{
            String strFilter = "ID=" + ID;
            ContentValues _ContentValues = new ContentValues();
            _ContentValues.put("IsFavorite",  IsFavoriteOn ? "true" : "false");
            int UpdateReturnValue = db.update("tbl_MusicDictionary", _ContentValues, strFilter, null);
            db.setTransactionSuccessful();
            //Log.d(LoggerName, " ID = " + ID +" | IsFavoriteOn = "+ IsFavoriteOn + " | UpdateReturnValue = " + UpdateReturnValue );
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }
    public void updateSetting_Favorite(boolean IsPlayerFavoriteOn){
        SQLiteDatabase db = this.getWritableDatabase();
        db.beginTransaction();
        try{
            ContentValues _ContentValues = new ContentValues();
            _ContentValues.put("IsFavoriteOn",  IsPlayerFavoriteOn ? "true" : "false");
            int UpdateReturnValue = db.update("tbl_Setting", _ContentValues, null, null);
            db.setTransactionSuccessful();
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }
    public void updateSetting_Shuffle(boolean IsPlayerShuffleOn){
        SQLiteDatabase db = this.getWritableDatabase();
        db.beginTransaction();
        try{
            ContentValues _ContentValues = new ContentValues();
            _ContentValues.put("IsShuffleOn",  IsPlayerShuffleOn ? "true" : "false");
            int UpdateReturnValue = db.update("tbl_Setting", _ContentValues, null, null);
            db.setTransactionSuccessful();
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }
    public void updateSetting_MyanmarFont(String MyanmarFontName){
        SQLiteDatabase db = this.getWritableDatabase();
        db.beginTransaction();
        try{
            ContentValues _ContentValues = new ContentValues();
            _ContentValues.put("MyanmarFontName",  MyanmarFontName);
            int UpdateReturnValue = db.update("tbl_Setting", _ContentValues, null, null);
            db.setTransactionSuccessful();
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }
    public void updateSetting_RepeatStatus(String RepeatStatus){
        // ALL
        // None
        // Single
        SQLiteDatabase db = this.getWritableDatabase();
        db.beginTransaction();
        try{
            ContentValues _ContentValues = new ContentValues();
            _ContentValues.put("RepeatStatus",  RepeatStatus);
            int UpdateReturnValue = db.update("tbl_Setting", _ContentValues, null, null);
            db.setTransactionSuccessful();
        }catch (Exception ex){
            ex.printStackTrace();
        }finally {
            db.endTransaction();
        }
    }
    //<!-- End Update function(s).  -->
    //<!-- Start Delete function(s).  -->
    //<!-- End Delete function(s).  -->
    //<!-- End developer defined function(s).  -->
}
