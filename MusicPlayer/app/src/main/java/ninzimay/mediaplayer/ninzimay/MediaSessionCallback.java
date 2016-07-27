package ninzimay.mediaplayer.ninzimay;

import android.content.Intent;
import android.support.v4.media.session.MediaSessionCompat;
import android.util.Log;
import android.view.KeyEvent;

/**
 * Created by myat on 27/7/2016.
 */
final class MediaSessionCallback extends MediaSessionCompat.Callback {

    private String LoggerName = "NinZiMay";

    @Override
    public void onPlay() {
        Log.d(LoggerName, "MediaSessionCallback onPlay");
    }

    @Override
    public void onPause() {
        Log.d(LoggerName, "MediaSessionCallback onPause");
    }

    @Override
    public void onStop() {
        Log.d(LoggerName, "MediaSessionCallback onStop");
    }

    @Override
    public void onSkipToNext() {
        Log.d(LoggerName, "MediaSessionCallback onSkipToNext");
    }

    @Override
    public void onSkipToPrevious() {
        Log.d(LoggerName, "MediaSessionCallback onSkipToPrevious");
    }

    @Override
    public boolean onMediaButtonEvent(final Intent mediaButtonIntent) {
        Log.d(LoggerName, "MediaSessionCallback onMediaButtonEvent");
        KeyEvent keyEvent = (KeyEvent) mediaButtonIntent.getExtras().get(Intent.EXTRA_KEY_EVENT);
        // ...do something with keyEvent, super... does nothing.
        return super.onMediaButtonEvent(mediaButtonIntent);
    }
}
