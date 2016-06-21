package ninzimay.mediaplayer.ninzimay;

/**
 * Created by myat on 21/6/2016.
 */
public class Constants {
    public interface ACTION {
        public static String MAIN_ACTION = "ninzimay.mediaplayer.ninzimay.action.main";
        public static String PREV_ACTION = "ninzimay.mediaplayer.ninzimay.action.prev";
        public static String PLAY_ACTION = "ninzimay.mediaplayer.ninzimay.action.play";
        public static String NEXT_ACTION = "ninzimay.mediaplayer.ninzimay.action.next";
        public static String COMING_BACK = "ninzimay.mediaplayer.ninzimay.action.comingback";
        public static String STARTFOREGROUND_ACTION = "ninzimay.mediaplayer.ninzimay.action.startforeground";
        public static String STOPFOREGROUND_ACTION = "ninzimay.mediaplayer.ninzimay.action.stopforeground";
    }

    public interface NOTIFICATION_ID {
        public static int FOREGROUND_SERVICE = 101;
    }
}
