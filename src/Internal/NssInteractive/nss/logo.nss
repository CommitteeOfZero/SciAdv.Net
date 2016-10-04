#include "nss/function.nss"
#include "nss/function_select.nss"


//=============================================================================//
//◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆
.//★遊戯円環★バージョン1.00
//◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆
//=============================================================================//
chapter main
{

	#SYSTEM_product_code="CHAOS;HEAD 1.00 VERSION";
	#SCRIPT_VERSION="1.00";
	#SYSTEM_loading_image="cg/sys/save/loading.jpg";
	#SYSTEM_loading_image_x=298;
	#SYSTEM_loading_image_y=213;

	$ChapterName = "boot";

	{

		$GameStart = 1;

		//★：フラグ初期化
		InitTrigger();

		//★：定義
		SystemInit();

		if($GameContiune == 1)
		{
			#play_speed_plus = 3;
			$GameContiune = 0;
			Delete("*");
			call_chapter nss/boot_開始スクリプト.nss;
		}

		//★：システム変数系のクリア
		#play_speed_plus += 0;
		if(#play_speed_plus != 0)
		{
			/*
				初回起動時ではないときは、プレイ速度をバックアップ
			*/
			#play_speed_plus = #SYSTEM_play_speed;
		}
		else
		{
			/*
				初回起動時は、プレイ速度のバックアップを３に固定
			*/
			#play_speed_plus = 3;
		}

		$GameName = 0;

		$PLACE_badend = 0;
		$PLACE_title = 1;

		$SYSTEM_skip=0;
		$SYSTEM_text_auto=0;
		#SYSTEM_play_speed = 3;
		$SYSTEM_menu_lock = 1;

		//★タイトルで何を選択したかのリセット
		$TitleSelect = 0;

		CreateColor("タイトルカラー", 1000, 0, 0, 800, 600, "BLACK");
		Fade("タイトルカラー", 0, 0, null, true);
		Fade("@box11",0,0,null,false);
		Fade("@box12",0,0,null,true);

		//■：ロゴとエキストラBGM判定
		TitleLogo();
		//■：タイトル定義
		
		while (1)
		{
			Wait(1);
		}

	}
	//->end while

}

//============================================================================//
..//■ロゴ判定■
//============================================================================//
function TitleLogo()
{
//★：一度観たらゲーム中は出さないようにする判定

	$Logo += 0;

	if($Logo == 0)
	{
		CreateTexture("タイトルニトロプラス", 100, 0, 0, "cg/sys/title/boot_nitroplus.jpg");
		CreateTexture("タイトル5GK", 100, 0, 0, "cg/sys/title/boot_5gk.jpg");
		CreateTexture("タイトル注意事項", 100, 0, 0, "cg/sys/title/注意事項.jpg");
		Fade("タイトル*", 0, 0, null, true);

		Fade("タイトルニトロプラス", 800, 1000, null, true);
		WaitKey(3000);
		Fade("タイトルニトロプラス", 800, 0, null, true);

		CreateSE("タイトル前サウンド１","SE_日常_PC_ハードディスク_Loop");
		SoundPlay("タイトル前サウンド１",0,1000,true);

		Fade("タイトル注意事項", 800, 1000, null, true);
		WaitKey(10000);

		CreateSE("タイトル前サウンド２","SE_日常_PC_マウスクリック");
		SoundPlay("タイトル前サウンド２",0,1000,false);
		SetVolume("タイトル前サウンド１", 100, 0, NULL);

		Fade("タイトル注意事項", 800, 0, null, true);

		Delete("タイトルニトロプラス");
		Delete("タイトル5GK");
		Delete("タイトル注意事項");
	}


	if($エキストラタイトル == 1)
	{
		if($エキストラＢＧＭ != "@CH01")
		{
			//★ＢＧＭプレイ
			SetVolume("@CH*", 1000, 0, NULL);
			SoundPlay("@CH01",3000,1000,true);
		}
		$エキストラタイトル = 0;
	}
	else
	{
		//★ＢＧＭプレイ
		SoundPlay("@CH01",0,1000,true);
	}


}
//============================================================================//






