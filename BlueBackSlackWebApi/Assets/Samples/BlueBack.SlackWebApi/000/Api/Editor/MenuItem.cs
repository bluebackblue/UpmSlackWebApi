

/** BlueBack.SlackWebApi.Samples.Api.Editor
*/
namespace BlueBack.SlackWebApi.Samples.Api.Editor
{
	/** MenuItem
	*/
	#if(UNITY_EDITOR)
	public static class MenuItem
	{
		/** FileUpdate
		*/
		public sealed class FileUpdate
		{
			/** TextureUpload
			*/
			private BlueBack.SlackWebApi.Api.TextureUpload fileupload;

			/** constructor
			*/
			public FileUpdate()
			{
				UnityEngine.Debug.Log("Start");

				//サンプル用。
				//「https://api.slack.com/apps」でOAuthTokenを取得して下記のトークンを差し替える。
				//ファイルのアップロードには「files:write」のパーミッションが必要。
				string t_oauthtoken = "xoxb-000000000000-0000000000000-000000000000000000000000";

				//投稿先のチャンネル名。
				string t_channel = "#general";

				//開発用。
				#if(USERDEF_BLUEBACK)
				{
					BlueBack.JsonItem.JsonItem t_jsonitem = new BlueBack.JsonItem.JsonItem(BlueBack.AssetLib.Editor.LoadText.LoadTextFromAssetsPath("../../../config/SlackWebApi.txt",null));
					t_oauthtoken = t_jsonitem.GetItem("xoxb").GetStringData();
				}
				#endif

				UnityEngine.Texture2D t_texture = new UnityEngine.Texture2D(128,128);
				{
					for(int yy=0;yy<t_texture.height;yy++){
						for(int xx=0;xx<t_texture.width;xx++){
							t_texture.SetPixel(xx,yy,new UnityEngine.Color((float)xx/t_texture.width,(float)xx/t_texture.height,1.0f));
						}
					}
					t_texture.Apply();
				}

				//fileupload
				this.fileupload = new BlueBack.SlackWebApi.Api.TextureUpload(t_oauthtoken,t_channel,t_texture,"タイトル","コメント",(UnityEngine.Time.time % 10000).ToString());
			}

			/** Update
			*/
			public void Update()
			{
				if(this.fileupload != null){
					this.fileupload.Update();
					switch(this.fileupload.mode){
					case BlueBack.SlackWebApi.Api.TextureUpload.Mode.Request:
					case BlueBack.SlackWebApi.Api.TextureUpload.Mode.Work:
						{
						}return;
					}
				}

				//終了。
				UnityEditor.EditorApplication.update -= Update;

				{
					UnityEngine.Debug.Log("mode : " + this.fileupload.mode.ToString());

					if(this.fileupload.result != null){
						UnityEngine.Debug.Log("result : " + this.fileupload.result);
					}

					if(this.fileupload.errorstring != null){
						UnityEngine.Debug.Log("errorstring : " + this.fileupload.errorstring);
					}

					UnityEngine.Debug.Log(this.fileupload.webrequest.responseCode.ToString());
					foreach(var t_pair in this.fileupload.webrequest.GetResponseHeaders()){
						UnityEngine.Debug.Log(t_pair.Key + " : " + t_pair.Value);
					}
				}

				UnityEngine.Debug.Log("End");
				this.fileupload.DisposeWebRequest();
				this.fileupload = null;
			}
		}

		/** ファイルアップロード。
		*/
		[UnityEditor.MenuItem("Samples/BlueBack.SlackWebApi/Api/FileUpdate")]
		private static void MenuItem_FileUpdate()
		{
			UnityEditor.EditorApplication.update += new FileUpdate().Update;
		}
	}
	#endif
}

