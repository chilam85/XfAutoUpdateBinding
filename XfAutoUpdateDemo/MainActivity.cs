using Android.App;
using Android.Widget;
using Android.OS;
using Com.Iflytek.Autoupdate;
using Android.Content;

namespace XfAutoUpdateDemo
{
	[Activity (Label = "XfAutoUpdateDemo", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class MainActivity : Activity , IFlytekUpdateListener
	{
		Context mContext;
		private IFlytekUpdate updManager;
		Handler mHandler;
		private Toast mToast;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mContext = ApplicationContext;
			mHandler = new Handler ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			updManager = IFlytekUpdate.GetInstance(mContext);
			updManager.SetDebugMode(true);
			updManager.SetParameter(UpdateConstants.ExtraWifionly, "true");
			updManager.SetParameter (UpdateConstants.ExtraStyle,UpdateConstants.UpdateUiDialog);
			updManager.ForceUpdate (this,this);


			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

			mToast = Toast.MakeText(this, "", ToastLength.Short);

		
		}

		public void OnResult (int errorcode, UpdateInfo result)
		{
			if(errorcode == UpdateErrorCode.Ok && result!= null) {
				if(result.UpdateType == UpdateType.NoNeed) {
					ShowTip("已经是最新版本！");
					return;
				}
				updManager.ShowUpdateInfo(this, result);
			}
			else
			{
				ShowTip("请求更新失败！\n更新错误码：" + errorcode);
			}

		}

		private void ShowTip(string str) {
			mHandler.Post (()=>{
				mToast.SetText(str);
				mToast.Show();
			});
		}
	}
}


