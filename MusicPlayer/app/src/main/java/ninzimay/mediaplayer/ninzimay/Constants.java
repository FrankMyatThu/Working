package ninzimay.mediaplayer.ninzimay;

/**
 * Created by myat on 21/6/2016.
 */
public class Constants {
    public interface ACTION {
        public static String PREV_ACTION = "ninzimay.mediaplayer.ninzimay.action.prev";
        public static String PLAY_ACTION = "ninzimay.mediaplayer.ninzimay.action.play";
        public static String PAUSE_ACTION = "ninzimay.mediaplayer.ninzimay.action.pause";
        public static String NEXT_ACTION = "ninzimay.mediaplayer.ninzimay.action.next";
        public static String INDEXED_SONG_ACTION = "ninzimay.mediaplayer.ninzimay.action.indexedSong";
        public static String INDEXED_SEEK_ACTION = "ninzimay.mediaplayer.ninzimay.action.indexedSeek";
        public static String STOPFOREGROUND_ACTION = "ninzimay.mediaplayer.ninzimay.action.stopforeground";
    }
    public interface NOTIFICATION_ID {
        public static int FOREGROUND_SERVICE = 101;
    }
}
