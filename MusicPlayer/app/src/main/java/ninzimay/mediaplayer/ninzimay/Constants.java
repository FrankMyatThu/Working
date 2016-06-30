package ninzimay.mediaplayer.ninzimay;

/**
 * Created by myat on 21/6/2016.
 */
public class Constants {
    public interface ACTION {
        public static String PREV_ACTION = "ninzimay.mediaplayer.ninzimay.action.prev";
        public static String PLAY_ACTION = "ninzimay.mediaplayer.ninzimay.action.play";
        public static String PLAYBACK_ACTION = "ninzimay.mediaplayer.ninzimay.action.playBack";
        public static String PAUSE_ACTION = "ninzimay.mediaplayer.ninzimay.action.pause";
        public static String NEXT_ACTION = "ninzimay.mediaplayer.ninzimay.action.next";
        public static String INDEXED_SONG_ACTION = "ninzimay.mediaplayer.ninzimay.action.indexedSong";
        public static String INDEXED_SEEK_ACTION = "ninzimay.mediaplayer.ninzimay.action.indexedSeek";
        public static String INVOKE_ONDEMAND_ACTION = "ninzimay.mediaplayer.ninzimay.action.invokedOnDemand";
        public static String STOPFOREGROUND_ACTION = "ninzimay.mediaplayer.ninzimay.action.stopforeground";
    }
    public  interface  BROADCAST{
        public static String FOREVER_BROADCAST = "ninzimay.mediaplayer.ninzimay.Broadcast.forever";
        public static String ONDEMAND_BROADCAST = "ninzimay.mediaplayer.ninzimay.Broadcast.onDemand";
    }
    public  interface  CACHE{
        public static String NINZIMAY = "ninzimay.mediaplayer.ninzimay.Cache.Ninzimay";
    }
    public interface NOTIFICATION_ID {
        public static int FOREGROUND_SERVICE = 101;
    }
}