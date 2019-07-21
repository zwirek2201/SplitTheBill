using System;

namespace SplitTheBill
{
	public class LoadingOverlay extends Activity implements View.OnClickListener {

		String myLog = "myLog";

		AlphaAnimation inAnimation;
		AlphaAnimation outAnimation;

		FrameLayout progressBarHolder;
		Button button;

		@Override
		protected void onCreate(Bundle savedInstanceState) {
			super.onCreate(savedInstanceState);
			setContentView(R.layout.activity_main);

			button = (Button) findViewById(R.id.button);
			progressBarHolder = (FrameLayout) findViewById(R.id.progressBarHolder);

			button.setOnClickListener(this);

		}

		@Override
		public void onClick(View v) {
			switch (v.getId()) {
			case R.id.button:
				new MyTask().execute();
				break;
			}

		}
	}

		private class MyTask extends AsyncTask <Void, Void, Void> {

			@Override
			protected void onPreExecute() {
				super.onPreExecute();
				button.setEnabled(false);
				inAnimation = new AlphaAnimation(0f, 1f);
				inAnimation.setDuration(200);
				progressBarHolder.setAnimation(inAnimation);
				progressBarHolder.setVisibility(View.VISIBLE);
			}

			@Override
			protected void onPostExecute(Void aVoid) {
				super.onPostExecute(aVoid);
				outAnimation = new AlphaAnimation(1f, 0f);
				outAnimation.setDuration(200);
				progressBarHolder.setAnimation(outAnimation);
				progressBarHolder.setVisibility(View.GONE);
				button.setEnabled(true);
			}

			@Override
			protected Void doInBackground(Void... params) {
				try {
					for (int i = 0; i < 5; i++) {
						Log.d(myLog, "Emulating some task.. Step " + i);
						TimeUnit.SECONDS.sleep(1);
					}
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				return null;
			}
		}
			
}

