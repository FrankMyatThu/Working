package ninzimay.mediaplayer.ninzimay;

import android.util.Log;

import java.util.concurrent.atomic.AtomicInteger;

/**
 * Created by myat on 7/7/2016.
 */
public class NotificationID {
    private static String LoggerName = "NinZiMay";
    public static int getID() {
        /// http://www.fileformat.info/tip/java/date2millis.htm
        //int _UniqueID = (int) (System.currentTimeMillis() % Integer.MAX_VALUE);
        //int _UniqueID = (int)(System.currentTimeMillis() / 1000);
        int _UniqueID = Math.abs((int)(System.currentTimeMillis()));
        Log.d(LoggerName, "_UniqueID = "+_UniqueID + " | System.currentTimeMillis() = "+ System.currentTimeMillis());
        return _UniqueID;
    }
}
