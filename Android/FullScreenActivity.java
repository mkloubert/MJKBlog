// s. http://blog.marcel-kloubert.de

public class FullScreenActivity extends Activity {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
 
        this.requestWindowFeature(Window.FEATURE_NO_TITLE);
        this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, 
                                  WindowManager.LayoutParams.FLAG_FULLSCREEN);
 
        // Parameter muss ggf. angepasst werden
        this.setContentView(R.layout.main);
    }
}
