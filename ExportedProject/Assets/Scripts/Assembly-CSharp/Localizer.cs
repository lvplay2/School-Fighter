using System.Collections.Generic;
using UnityEngine;

public class Localizer : MonoBehaviour
{
	private static Dictionary<string, string> dict = new Dictionary<string, string>
	{
		{ "School Fighter", "スクールファイター" },
		{ "Player", "プレイヤー" },
		{ "Taichi", "たいち" },
		{ "Aoi", "あおい" },
		{ "Honoka", "ほのか" },
		{ "Level", "レベル" },
		{ "Easy", "かんたん" },
		{ "Normal", "ふつう" },
		{ "Hard", "むずかしい" },
		{ "Mode", "操作" },
		{ "Simple", "シンプル" },
		{ "START", "スタート" },
		{ "SCORE", "スコア" },
		{ "Time Left", "残り時間" },
		{ "ROUND", "ラウンド" },
		{ "FIGHT!", "ファイト!" },
		{ "Times Up", "タイムアップ" },
		{ "CLEAR!", "クリア!" },
		{ "Congratulation!", "全ラウンドクリア!" },
		{ "DRAW", "引き分け" },
		{ "K.O.", "K.O. キミの負け" },
		{ "Failed.", "残念。キミの負け" },
		{ "TRY AGAIN", "もういちど" },
		{ "Guard", "ガード" },
		{ "Attack", "アタック" },
		{ "Rooftop Fighter", "屋上ファイター" },
		{ "AI Fighter", "対戦相手" },
		{ "Order", "順番" },
		{ "Random", "ランダム" }
	};

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static string LocalizedString(string key)
	{
		string result = key;
		if (Application.systemLanguage == SystemLanguage.Japanese && dict.ContainsKey(key))
		{
			result = dict[key];
		}
		return result;
	}
}
