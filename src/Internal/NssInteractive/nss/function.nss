
//■マクロをさらにマクロにてまとめて定義
//=============================================================================//
.//まとめ定義
//=============================================================================//

..SystemInit
function SystemInit()
{

	$SYSTEM_spt_name = $ChapterName;
	$SYSTEM_text_interval = 34;


	//■一度読み込んだら再度読み込んでしまわないように。
	if(! $BGM_Init)
	{
		$BGM_Init = true;
		InitBGM();
	}

	if(! $BOX_Init)
	{
		$BOX_Init = true;
		LoadBox();
	}

}

..SystemSet
function SystemSet()
{
	Request("box00", UnLock);
	Fade("@box11",0,1000,null,false);
	Fade("@box12",0,1000,null,true);
	CreateWindow("box00", 20500, 0, 470, 800, 130, true);
	Request("box00", Lock);
}


//■テキストボックス・フォント、スクリプト中よく定義するものをマクロにてまとめて定義。
//=============================================================================//
.//ボックス定義
//=============================================================================//
..Box
function LoadBox()
{
	//シネスコもどき
	CreateColor("box11", 20000, 0, 0, 800, 50, "BLACK");
	CreateColor("box12", 20000, 0, 470, 800, 130, "BLACK");
	SetAlias("box11", "box11");
	SetAlias("box12", "box12");
	Fade("box11",0,0,null,false);
	Fade("box12",0,0,null,true);
	Request("box11", Lock);
	Request("box12", Lock);

	LoadFont("フォント１Ａ", "ＭＳ ゴシック", 20, #FFFFFF, #000000, 500, LEFTDOWN, "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽぁぃぅぇぉっゃゅょアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポァィゥェォッャュョ、。ー…！？");
	Request("フォント１Ａ", Lock);
}



//■スクリプトにおいてテキストを定義するマクロコマンド
//=============================================================================//
.//テキスト定義
//=============================================================================//

..SetText
function SetText("ボックス名","$テキスト名")
{
	SetFont("ＭＳ ゴシック", 20, #FFFFFF, #000000, 500, LEFTDOWN);
	LoadText("$構文名","ボックス名","$テキスト名",720,130,0,29);

	Request("$テキスト名", Hideable);
	Request("$テキスト名", Lock);
	Request("$テキスト名", Erase);

	Move("$テキスト名",0,@40,@0,null,true);
}

//■定義したテキストの位置をまとめて調整
//=============================================================================//
.//テキスト位置補正
//=============================================================================//


//■ボックスを描画するのとテキストをタイピングする動作をまとめて実行するマクロ
//=============================================================================//
.//タイピングマクロ
//=============================================================================//

..TypeBegin
function TypeBegin()
{
	$boxtype = $SYSTEM_present_preprocess;
	$textnumber = $SYSTEM_present_text;

	$SYSTEM_position_x_text_icon = -32768;
	$SYSTEM_position_y_text_icon = -32768;

	Request("$textnumber", Enter);
	WaitText("$textnumber", null);

	Fade("$textnumber", 0, 0, null, true);
	Request("$textnumber", UnLock);
	Request("$textnumber", Disused);
}

..TypeBegin2
function TypeBegin2()
{
	$boxtype = $SYSTEM_present_preprocess;
	$textnumber = $SYSTEM_present_text;

//	$SYSTEM_position_x_text_icon = 750;
//	$SYSTEM_position_y_text_icon = 550;

	$distext = "$boxtype" + "/text*";
	Delete("$distext");

	Request("$textnumber", Enter);
	WaitText("$textnumber", null);

	Request("$textnumber", UnLock);
}


..TypeBegin3
function TypeBegin3(秒数)
{
	$boxtype = $SYSTEM_present_preprocess;
	$textnumber = $SYSTEM_present_text;

	$SYSTEM_position_x_text_icon = -32768;
	$SYSTEM_position_y_text_icon = -32768;

	Request("$textnumber", Enter);
	WaitText("$textnumber", null);

	Fade("$textnumber", 秒数, 0, null, true);
	Request("$textnumber", UnLock);
	Request("$textnumber", Disused);
}

//■Fade系
//=============================================================================//
.//Fade系
//=============================================================================//

//透明度0からスタートする「CreateTexture」です
..CreateTextureEX
function CreateTextureEX("ナット名", 描画優先度, Ｘ座標, Ｙ座標, "イメージデータ")
{
	CreateTexture("ナット名", 描画優先度, Ｘ座標, Ｙ座標, "イメージデータ");
	Fade("ナット名", 0, 0, null, true);
}


//背景専用
..CreateBG
function CreateBG(描画優先度, 所要時間, Ｘ座標, Ｙ座標, "イメージデータ")
{
	if($BackGround=="back01"){$BackGround="back02";}
	else{$BackGround="back01";}

	CreateTexture("$BackGround", 描画優先度, Ｘ座標, Ｙ座標, "イメージデータ");
	Fade("$BackGround", 0, 0, null, true);
	Request("$BackGround", Lock);
	Fade("$BackGround", 所要時間, 1000, null, true);

	Delete("back*");
	Request("$BackGround", UnLock);
}

//背景専用
..CreateBG2
function CreateBG2(描画優先度, 所要時間, Ｘ座標, Ｙ座標, "イメージデータ")
{
	if($BackGround=="back01"){$BackGround="back02";}
	else{$BackGround="back01";}

	CreateTexture("$BackGround", 描画優先度, Ｘ座標, Ｙ座標, "イメージデータ");
	Request("$BackGround", Lock);

	Fade("back*", 所要時間, 0, null, false);
	Fade("$BackGround", 0, 1000, null, false);
	Wait(所要時間);

	Delete("back*");
	Request("$BackGround", UnLock);
}




..FadeDelete
function FadeDelete("ナット名", 所要時間, 待ち)
{
	Fade("ナット名", 所要時間, 0, null, 待ち);
	Request("ナット名", Disused);
}

..PrintBG
function PrintBG(描画優先度)
{
//	CreateBG(描画優先度, 0, 0, 0, "SCREEN");
	if($BackGround=="back01"){$BackGround="back02";}
	else{$BackGround="back01";}
	CreateTexture("$BackGround", 描画優先度, 0, 0, "SCREEN");
	Request("$BackGround", Lock);

	Delete("*");
	/*stand変数初期化*/
		$stand01_use="";
		$stand02_use="";
		$stand03_use="";
		$stand04_use="";
		$stand05_use="";
		$stand06_use="";
		$stand07_use="";
		$stand08_use="";
		$stand09_use="";
		$stand10_use="";
	Request("$BackGround", UnLock);
}


..ClearAll
function ClearAll(所要時間)
{
	CreateColor("黒", 30000, 0, 0, 800, 600, "BLACK");
	Fade("黒", 0, 0, null, true);
	Fade("黒", 所要時間, 1000, null, true);
	Delete("*");
	/*stand変数初期化*/
		$stand01_use="";
		$stand02_use="";
		$stand03_use="";
		$stand04_use="";
		$stand05_use="";
		$stand06_use="";
		$stand07_use="";
		$stand08_use="";
		$stand09_use="";
		$stand10_use="";
}


..FadeCross
function FadeCross("$ナット名１","$ナット名２", 所要時間)
{
	$ナット名 = "$ナット名１" + "$ナット名２";
	$ナット名アスタ = "$ナット名１" + "*";

	Fade("$ナット名", 所要時間, 1000, null, true);
	Request("$ナット名", Lock);
	Delete("$ナット名アスタ");
	Request("$ナット名", UnLock);
}



..MoveEX
function MoveEX("ナット名", 所要時間, $Ｘ座標, $Ｙ座標, テンポ, 待ち)
{
	$Ｘ座標プレ = - $Ｘ座標;
	$Ｙ座標プレ = - $Ｙ座標;

	$Ｘ座標マイナス = "@" + $Ｘ座標プレ;
	$Ｙ座標マイナス = "@" + $Ｙ座標プレ;

	$Ｘ座標プラス = "@" + $Ｘ座標;
	$Ｙ座標プラス = "@" + $Ｙ座標;

	Move("ナット名", 0, $Ｘ座標マイナス, $Ｙ座標マイナス, null, true);
	Move("ナット名", 所要時間, $Ｘ座標プラス, $Ｙ座標プラス, テンポ, 待ち);
}

..DeleteAll
function DeleteAll()
{
	Delete("*");
	/*stand変数初期化*/
		$stand01_use="";
		$stand02_use="";
		$stand03_use="";
		$stand04_use="";
		$stand05_use="";
		$stand06_use="";
		$stand07_use="";
		$stand08_use="";
		$stand09_use="";
		$stand10_use="";
}

//■cube系
//=============================================================================//
.//cube系
//=============================================================================//

..CubeRoom
function CubeRoom("ナット名", 描画優先度, 視野角度)
{
	$フォルダ名 = #SYSTEM_max_texture_size;
//	$フォルダ名 = 2048;

	$前面画像 = "cg/rv/星来覚醒前_明/" + "$フォルダ名" + "/" + "rv_cube_front" + ".jpg";
	$後面画像 = "cg/rv/星来覚醒前_明/" + "$フォルダ名" + "/" + "rv_cube_back" + ".jpg";
	$右面画像 = "cg/rv/星来覚醒前_明/" + "$フォルダ名" + "/" + "rv_cube_right" + ".jpg";
	$左面画像 = "cg/rv/星来覚醒前_明/" + "$フォルダ名" + "/" + "rv_cube_left" + ".jpg";
	$上面画像 = "cg/rv/星来覚醒前_明/" + "$フォルダ名" + "/" + "rv_cube_top" + ".jpg";
	$下面画像 = "cg/rv/星来覚醒前_明/" + "$フォルダ名" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("ナット名", 描画優先度, "$前面画像", "$後面画像", "$右面画像", "$左面画像", "$上面画像", "$下面画像");
	SetFov("キューブ１", 視野角度);
	Fade("ナット名", 0, 0, null, true);
}


..CubeRoom2
function CubeRoom2("ナット名", 描画優先度, 視野角度)
{
	$フォルダ名 = #SYSTEM_max_texture_size;
//	$フォルダ名 = 2048;

	$前面画像 = "cg/rv/星来覚醒後_明/" + "$フォルダ名" + "/" + "rv_cube_front" + ".jpg";
	$後面画像 = "cg/rv/星来覚醒後_明/" + "$フォルダ名" + "/" + "rv_cube_back" + ".jpg";
	$右面画像 = "cg/rv/星来覚醒後_明/" + "$フォルダ名" + "/" + "rv_cube_right" + ".jpg";
	$左面画像 = "cg/rv/星来覚醒後_明/" + "$フォルダ名" + "/" + "rv_cube_left" + ".jpg";
	$上面画像 = "cg/rv/星来覚醒後_明/" + "$フォルダ名" + "/" + "rv_cube_top" + ".jpg";
	$下面画像 = "cg/rv/星来覚醒後_明/" + "$フォルダ名" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("ナット名", 描画優先度, "$前面画像", "$後面画像", "$右面画像", "$左面画像", "$上面画像", "$下面画像");
	SetFov("キューブ１", 視野角度);
	Fade("ナット名", 0, 0, null, true);
}

..CubeRoom3
function CubeRoom3("ナット名", 描画優先度, 視野角度)
{
	$フォルダ名 = #SYSTEM_max_texture_size;
//	$フォルダ名 = 2048;

	$前面画像 = "cg/rv/星来覚醒前_暗/" + "$フォルダ名" + "/" + "rv_cube_front" + ".jpg";
	$後面画像 = "cg/rv/星来覚醒前_暗/" + "$フォルダ名" + "/" + "rv_cube_back" + ".jpg";
	$右面画像 = "cg/rv/星来覚醒前_暗/" + "$フォルダ名" + "/" + "rv_cube_right" + ".jpg";
	$左面画像 = "cg/rv/星来覚醒前_暗/" + "$フォルダ名" + "/" + "rv_cube_left" + ".jpg";
	$上面画像 = "cg/rv/星来覚醒前_暗/" + "$フォルダ名" + "/" + "rv_cube_top" + ".jpg";
	$下面画像 = "cg/rv/星来覚醒前_暗/" + "$フォルダ名" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("ナット名", 描画優先度, "$前面画像", "$後面画像", "$右面画像", "$左面画像", "$上面画像", "$下面画像");
	SetFov("キューブ１", 視野角度);
	Fade("ナット名", 0, 0, null, true);
}

..CubeRoom4
function CubeRoom4("ナット名", 描画優先度, 視野角度)
{
	$フォルダ名 = #SYSTEM_max_texture_size;
//	$フォルダ名 = 2048;

	$前面画像 = "cg/rv/星来覚醒後_暗/" + "$フォルダ名" + "/" + "rv_cube_front" + ".jpg";
	$後面画像 = "cg/rv/星来覚醒後_暗/" + "$フォルダ名" + "/" + "rv_cube_back" + ".jpg";
	$右面画像 = "cg/rv/星来覚醒後_暗/" + "$フォルダ名" + "/" + "rv_cube_right" + ".jpg";
	$左面画像 = "cg/rv/星来覚醒後_暗/" + "$フォルダ名" + "/" + "rv_cube_left" + ".jpg";
	$上面画像 = "cg/rv/星来覚醒後_暗/" + "$フォルダ名" + "/" + "rv_cube_top" + ".jpg";
	$下面画像 = "cg/rv/星来覚醒後_暗/" + "$フォルダ名" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("ナット名", 描画優先度, "$前面画像", "$後面画像", "$右面画像", "$左面画像", "$上面画像", "$下面画像");
	SetFov("キューブ１", 視野角度);
	Fade("ナット名", 0, 0, null, true);
}


//■妄想イン・アウトマクロ
//=============================================================================//
.//妄想in・out
//=============================================================================//

..DelusionIn
function DelusionIn()
{
	Move("レンズ１", 0, @0, @0, null, true);
	Request("レンズプロセス１", UnLock);
	Delete("レンズプロセス１");
	Request("レンズ１", UnLock);
	Delete("レンズ１");

	CreateColor("白１", 22000, 0, 0, 800, 600, "White");
	Fade("白１", 0, 0, null, false);

//画面エフェクト//妄想ＩＮエフェクト
	CreateMovie("妄想in", 21000, 0, 0, false, false, "dx/mvin.ngs");
	Request("妄想in", Play);

//ＳＥ//妄想ＩＮ
	CreateSE("SE100","SE_擬音_妄想IN");
	SoundPlay("SE100", 0, 1000, false);
	WaitAction("妄想in", null);

	Fade("白１", 300, 1000, null, true);
	Request("白１", Lock);
	Delete("妄想in");

		$SYSTEM_effect_lens_curvature = 8000;
		$SYSTEM_effect_lens_distance = 10;
		CreateEffect("レンズ１", 2100, -200, -300, 1200, 1200, "Lens");
		SetAlias("レンズ１", "レンズ１");
		CreateProcess("レンズプロセス１", 1000, 0, 0, "LensMove");

	Request("レンズ１", Lock);
	Request("レンズプロセス１", Lock);
	Wait(500);
	Request("レンズプロセス１", Start);
}

..DelusionIn2
function DelusionIn2()
{
	Request("白１", UnLock);
	Fade("白１", 1000, 0, null, true);
	Delete("白１");
}


..DelusionFakeIn
function DelusionFakeIn()
{
	CreateColor("白１", 22000, 0, 0, 800, 600, "White");
	Request("白１", Lock);
	Fade("白１", 0, 0, null, false);

//画面エフェクト//妄想ＩＮエフェクト
	CreateMovie("妄想in", 21000, 0, 0, false, false, "dx/mvin.ngs");
	Request("妄想in", Lock);
	Request("妄想in", Play);


//ＳＥ//妄想ＩＮ
	CreateSE("SE01","SE_擬音_妄想IN");
	SoundPlay("SE01", 0, 1000, false);
	WaitAction("SE01", null);

	Fade("白１", 300, 1000, null, true);
	Request("妄想in", UnLock);
	Delete("妄想in");
}

..DelusionFakeIn2
function DelusionFakeIn2()
{
	Request("白１", UnLock);
	Fade("白１", 1000, 0, null, true);
	Delete("白１");
}



..DelusionOut
function DelusionOut()
{

	Request("レンズ１", UnLock);
	Request("レンズプロセス１", UnLock);

	CreateColor("黒１", 22000, 0, 0, 800, 600, "Black");
	Request("黒１", Lock);
	Fade("黒１", 0, 0, null, false);

	Move("レンズ１", 0, @0, @0, null, true);
	Delete("レンズプロセス１");
	Delete("レンズ１");

//画面エフェクト//妄想ＯＵＴエフェクト
	CreateMovie("妄想out", 21000, 0, 0, false, false, "dx/mvout.ngs");
	Request("妄想out", Play);

//ＳＥ//妄想ＯＵＴ
	CreateSE("SE01","SE_擬音_妄想OUT");
	SoundPlay("SE01", 0, 1000, false);
	WaitAction("妄想out", null);

	Fade("黒１", 300, 1000, null, true);
	Delete("妄想out");
}

..DelusionOut2
function DelusionOut2()
{
	Wait(2000);

	Request("黒１", UnLock);
	Fade("黒１", 1000, 0, null, true);
	Delete("黒１");

}


//■インターミッション
//=============================================================================//
.//インターミッションIN
//=============================================================================//

..IntermissionIn
function IntermissionIn()
{
	CreateColor("インターミッション色", 25001, 0, 0, 800, 600, "black");
	Fade("インターミッション色", 0, 0, null, false);
	Request("インターミッション色", Lock);

	CreateMovie("インターミッションムービー１", 25000, 0, 0, false, true, "dx/mvアイキャッチ01.ngs");
	Request("インターミッションムービー１", Lock);
	WaitPlay("インターミッションムービー１", null);

	Fade("インターミッション色", 300, 1000, null, true);
}

..IntermissionIn2
function IntermissionIn2()
{

	Wait(500);

	CreateMovie("インターミッションムービー２", 25002, 0, 0, false, true, "dx/mvアイキャッチ02.ngs");

	Wait(400);

	Request("インターミッション色", UnLock);
	Request("インターミッションムービー１", UnLock);
	FadeDelete("インターミッション色", 100, false);
	FadeDelete("インターミッションムービー１", 100, true);

	WaitPlay("インターミッションムービー２", null);

	Delete("インターミッションムービー２");


}











//■音関係のマクロコマンド
//=============================================================================//
.//音関係
//=============================================================================//

// 定義
function CreateBGM("ナット名","$音楽データ")
{
	$場所指定 = "sound/bgm/" + "$音楽データ";

	CreateSound("ナット名", BGM, "$場所指定");
	SetVolume("ナット名", 0, 0, NULL);
	SetAlias("ナット名", "ナット名");
}

function CreateBGM2("ナット名","$音楽データ")
{
	$場所指定 = "sound/bgm/" + "$音楽データ";

	CreateSound("ナット名", SE, "$場所指定");
	SetVolume("ナット名", 0, 0, NULL);
	SetAlias("ナット名", "ナット名");
}

function CreateBGM3("ナット名","$音楽データ",開始ミリ秒,終了ミリ秒)
{
	$場所指定 = "sound/bgm/" + "$音楽データ";

	CreateSound("ナット名", BGM, "$場所指定");
	SetVolume("ナット名", 0, 0, NULL);
	SetAlias("ナット名", "ナット名");
	SetLoopPoint("ナット名",開始ミリ秒,終了ミリ秒);
}

function CreateSE("ナット名","$音楽データ")
{
	$場所指定 = "sound/se/" + "$音楽データ";

	CreateSound("ナット名", SE, "$場所指定");
	SetVolume("ナット名", 0, 0, NULL);
	SetAlias("ナット名", "ナット名");
}

function CreateVOICE("ナット名","$音楽データ")
{
	$場所指定 = "voice/" + "$音楽データ";

	CreateSound("ナット名", VOICE, "$場所指定");
	SetVolume("ナット名", 0, 0, NULL);
	SetAlias("ナット名", "ナット名");
}

function CreateVOICE2("ナット名","$音楽データ")
{
	$場所指定 = "voice/" + "$音楽データ";

	CreateSound("ナット名", SE, "$場所指定");
	SetVolume("ナット名", 0, 0, NULL);
	SetAlias("ナット名", "ナット名");
}

// 再生
function MusicStart("ナット名",秒数,ボリウム,再生方向,再生スピード,テンポ,ループ設定)
{
	Request("ナット名", "Play");

	SetFrequency("ナット名", 0, 再生スピード, NULL);
	SetPan("ナット名", 0, 再生方向, NULL);
	SetLoop("ナット名", ループ設定);
	SetVolume("ナット名", 秒数, ボリウム, テンポ);
	Request("ナット名", Disused);
}

function SoundPlay("ナット名",秒数,ボリウム,ループ設定)
{
	Request("ナット名", Play);
	SetLoop("ナット名", ループ設定);
	SetVolume("ナット名", 秒数, ボリウム, null);
	Request("ナット名", Disused);
}

function SoundStop("ナット名")
{
	SetVolume("ナット名", 3000, 0, NULL);
}

function SoundStop2("ナット名")
{
	Request("ナット名", Stop);
	Delete("ナット名");
}

//■BGMを纏めて定義
//=============================================================================//
.//BGM定義
//=============================================================================//

function InitBGM()
{
	//27
	CreateBGM3("CH00","CH00",2981,43832);
	CreateBGM("CH01","CH01");
	CreateBGM3("CH02","CH02",9766,74528);
	CreateBGM3("CH03","CH03",6645,184915);
	CreateBGM3("CH04","CH04",10010,150010);
	CreateBGM3("CH05","CH05",18757,98757);
	CreateBGM("CH06","CH06");
	CreateBGM3("CH07","CH07",11575,117341);
	CreateBGM3("CH08","CH08",19426,221486);
	CreateBGM("CH09","CH09");
	CreateBGM("CH10","CH10");
	CreateBGM3("CH11","CH11",4675,90009);
	CreateBGM3("CH12","CH12",0,159157);
	CreateBGM("CH13","CH13");
	CreateBGM3("CH14","CH14",7019,170424);
	CreateBGM3("CH15","CH15",21905,181905);
	CreateBGM("CH16","CH16");
//	CreateBGM("CH17","CH17");
//	CreateBGM("CH18","CH18");
//	CreateBGM("CH19","CH19");
	CreateBGM3("CH20","CH20",6997,198998);
	CreateBGM("CH21","CH21");
	CreateBGM3("CH22","CH22",3272,119393);
	CreateBGM3("CH23","CH23",19495,107257);
	CreateBGM3("CH24","CH24",4952,182225);
	CreateBGM3("CH25","CH25",5109,68939);
	CreateBGM("CH26","CH26");
//	CreateBGM("CH27","CH27");
	CreateBGM("CH28","CH28");
	CreateBGM("CH29","CH29");

	CreateBGM("CH_INS_FES_アカベラ","CH_INS_FES_アカベラ");
	CreateBGM("CH_INS_FES_ライヴ","CH_INS_FES_ライヴ");
	CreateBGM("CH_OP","CH_OP");
	CreateBGM("CH_OP_Instrumental","CH_OP_Instrumental");
	CreateBGM2("CH_ED_A","CH_ED_A");
	CreateBGM2("CH_ED_B","CH_ED_B");
	CreateBGM2("CH_ED_C","CH_ED_C");

	Request("@CH00",Lock);
	Request("@CH01",Lock);
	Request("@CH02",Lock);
	Request("@CH03",Lock);
	Request("@CH04",Lock);
	Request("@CH05",Lock);
	Request("@CH06",Lock);
	Request("@CH07",Lock);
	Request("@CH08",Lock);
	Request("@CH09",Lock);
	Request("@CH10",Lock);
	Request("@CH11",Lock);
	Request("@CH12",Lock);
	Request("@CH13",Lock);
	Request("@CH14",Lock);
	Request("@CH15",Lock);
	Request("@CH16",Lock);
//	Request("@CH17",Lock);
//	Request("@CH18",Lock);
//	Request("@CH19",Lock);
	Request("@CH20",Lock);
	Request("@CH21",Lock);
	Request("@CH22",Lock);
	Request("@CH23",Lock);
	Request("@CH24",Lock);
	Request("@CH25",Lock);
	Request("@CH26",Lock);
//	Request("@CH27",Lock);
	Request("@CH28",Lock);
	Request("@CH29",Lock);

	Request("@CH_INS_FES_アカベラ",Lock);
	Request("@CH_INS_FES_ライヴ",Lock);
	Request("@CH_OP",Lock);
	Request("@CH_OP_Instrumental",Lock);
	Request("@CH_ED_A",Lock);
	Request("@CH_ED_B",Lock);
	Request("@CH_ED_C",Lock);

}




function DebugGallery()
{
	#ev001_01_1_INT01近づく梨深_a=true;
	#ev013_01_1_拓巳笑い口UP_a=true;
	#ev013_02_1_拓巳笑い口UP_a=true;
	#ev013_03_1_拓巳笑い口UP_a=true;
	#ev005_01_3_杭貼付け_a=true;
	#ev006_01_3_グロ画像_a=true;
	#ev007_01_3_十字架杭拓巳_a=true;
	#ev007_02_6_十字架杭拓巳_a=true;
	#ev009_01_4_腕掴みミイラ_a=true;
	#ev010_01_4_腕掴み梨深_a=true;
	#ev012_01_1_星来横たわり_a=true;
	#ev008_01_4_INT02あやせ歌う_a=true;
	#ev801_01_1_七海来訪_a=true;
	#ev801_02_3_七海来訪_a=true;
	#ev015_01_1_七海妄想エロ_a=true;
	#ev015_02_1_七海妄想エロ_a=true;
	#ev802_01_1_七海コーラ死_a=true;
	#ev016_01_1_七海携帯萌_a=true;
	#ev803_01_3_優愛遭遇_a=true;
	#ev019_02_3_優愛妄想_a=true;
	#ev019_01_3_優愛妄想_a=true;
	#ev017_01_2_尾道_a=true;
	#ev017_02_2_尾道_a=true;
	#ev057_01_1_拓巳子供時代_a=true;
	#ev050_01_1_受診小学生拓巳_a=true;
	#ev022_01_1_星来キス妄想_a=true;
	#bg119_01_3_監視カメラ映像_a=true;
	#ev037_01_3_INT13優愛自室でメール書く_a=true;
	#ev023_01_3_INT06優愛寝転がる_a=true;
	#ev024_01_3_あやせライブシーン_a=true;
	#ev025_01_3_あやせ脱ぎ妄想_a=true;
	#ev110_01_3_セナ足_a=true;
	#ev026_01_2_七海ハンバーガー_a=true;
	#ev026_02_2_七海ハンバーガー_a=true;
	#ev027_01_3_見下ろしセナ_a=true;
	#ev028_01_0_あやせライブ三住切る_a=true;
	#ev029_01_2_梨深ソファ腰掛け_a=true;
	#ev030_01_2_七海バングル_a=true;
	#bg136_01_1_希ＶＩＰルーム_a=true;
	#ev031_01_0_梢転校_a=true;
	#ev052_01_3_将軍車椅子_a=true;
	#ev052_02_3_将軍車椅子_a=true;
	#ev032_01_3_梨深だきしめ_a=true;
	#ev033_01_1_INT12セナ会話意識集中_a=true;
	#ev034_01_6_梨深と将軍の出会い_a=true;
	#ev035_01_0_梢コケる_a=true;
	#ev036_01_2_セナ白い鎖_a=true;
	#ev038_01_3_優愛ROOM37乱入_a=true;
	#ev040_01_3_あやせディソード出す_a=true;
	#ev040_02_3_あやせディソード出す_a=true;
	#ev039_01_3_あやせ白下着_a=true;
	#ev039_02_3_あやせ白下着_a=true;
	#ev054_01_3_刑事二人_a=true;
	#ev042_01_2_梨深に介抱される_a=true;
	#ev042_02_2_梨深に介抱される_a=true;
	#ev043_01_2_梨深CD貸してくれたら_a=true;
	#ev044_01_2_梨深下着Yシャツ_a=true;
	#ev044_02_2_梨深下着Yシャツ_a=true;
	#ev044_03_2_梨深下着Yシャツ_a=true;
	#ev045_01_3_INT16セナ機械破壊_a=true;
	#ev055_01_1_刑事と探偵会話_a=true;
	#ev056_01_1_優愛へたりこみ電話_a=true;
	#ev056_02_1_優愛へたりこみ電話_a=true;
	#ev064_01_1_あやせ飛び降りようと_a=true;
	#ev065_01_1_あやせ投身_a=true;
	#ev065_02_1_あやせ投身_a=true;
	#ev066_01_1_あやせ花壇落ち_a=true;
	#ev057_01_3_Q－FrontTVモニター_a=true;
	#ev067_01_6_血溜り七海_a=true;
	#ev056_01_1_9歳七海拓巳に食事_a=true;
	#ev070_01_2_将軍と梨深in病院_a=true;
	#ev070_02_2_将軍と梨深in病院_a=true;
	#ev071_01_1_梢ディソード顕現_a=true;
	#ev072_01_1_梢と波多野Roft前_a=true;
	#ev062_01_1_プリクラ_a=true;
	#ev062_02_1_プリクラ_a=true;
	#ev077_01_3_野呂瀬社長室_a=true;
	#ev068_01_1_七海廊下後姿_a=true;
	#ev069_01_1_七海廊下後姿髪かきあげ_a=true;
	#ev076_01_4_美愛くま抱き_a=true;
	#ev059_01_6_あやせ拷問_a=true;
	#ev078_01_3_葉月ナースめがね_a=true;
	#ev078_02_3_葉月ナースめがね_a=true;
	#ev080_01_1_梨深七海ハイタッチ_a=true;
	#ev060_01_3_セナコンテナ登場_a=true;
	#ev060_02_3_セナコンテナ登場_a=true;
	#ev079_01_3_梨深セナ対決_a=true;
	#ev073_01_1_赤子を抱く母_a=true;
	#ev063_01_1_マジックミラー波多野_a=true;
	#ev063_02_1_マジックミラー波多野_a=true;
	#ev082_01_3_七海ゾンビ_a=true;
	#ev081_01_3_判死_a=true;
	#ev083_01_3_優愛ディソード_a=true;
	#ev084_01_3_ノアII_a=true;
	#ev085_01_3_七海ディソード_a=true;
	#ev086_01_6_梨深スポットライト膝抱え_a=true;
	#ev087_01_3_拓巳ディソード_a=true;
	#ev087_02_3_拓巳ディソード_a=true;
	#ev088_01_4_葉月救いあれ_a=true;
	#ev088_02_4_葉月救いあれ_a=true;
	#ev089_01_1_あやせ瓦礫下_a=true;
	#ev090_01_1_優愛瓦礫下_a=true;
	#ev091_01_1_七海と将軍_a=true;
	#ev092_01_1_拓巳振り返り_a=true;
	#ev092_02_1_拓巳振り返り_a=true;
	#ev092_03_1_拓巳振り返り_a=true;
	#ev093_01_1_セナ父殺し_a=true;
	#ev093_01_1_セナ父殺し_b=true;
	#ev094_01_1_セナ父首はね_a=true;
	#ev095_01_1_拓巳ダーツ逆転_a=true;
	#ev096_01_1_星来大群_a=true;
	#ev097_01_1_梨深はりつけ_a=true;
	#ev107_01_1_梨深祈る_a=true;
	#ev106_01_1_剣交え_a=true;
	#ev099_01_1_梨深レイプ_a=true;
	#ev108_02_1_串刺し_a=true;
	#ev100_06_1_６人祈る_a=true;
	#ev111_01_6_野呂瀬ラスト_a=true;
	#ev105_01_1_拓巳光と消える_a=true;
	#ev105_02_1_拓巳光と消える_a=true;
	#ev102_01_1_Ａエンド1_a=true;
	#ev103_01_1_Ａエンド2_a=true;
	#ev999_01_1_おめでとう=true;
	#bg006_01_1_コンテナ外観_a=true;
	#bg026_02_3_拓巳部屋_a=true;
	#ev014_01_1_スプー_a=true;
	#bg155_01_1_Ir2_a=true;
	#ev047_01_1_張り付け死体現場写真_a=true;
	#ev048_01_1_張り付けグロ絵アップ_a=true;
	#ev053_01_1_集団ダイブ現場写真_a=true;
	#bg142_01_3_ミュウツベ集団ダイブ_a=true;
	#ev046_01_1_妊娠男現場写真_a=true;
	#ev049_01_1_ヴァンパイ屋現場写真_a=true;
	#bg213_01_6_ニュースDQNパズル_a=true;
	#ev061_01_2_ニュージェネ犯人逮捕TV_a=true;
	#ev058_01_3_Q－Front殺到するキャスター_a=true;
	#ev051_01_3_清掃員_a=true;
	#bg011_01_1_検索欄UP_a=true;
	#bg040_01_2_優愛カバン_a=true;
	#bg041_01_2_優愛カバンぶちまけ_a=true;
	#bg127_01_2_ギョロリゲロカエルん_a=true;
	#bg181_01_3_捨てられたギョロリ_a=true;
	#bg147_01_2_ディソード想像_a=true;
	#bg073_01_1_TownVanguard店内_a=true;
	#bg012_01_1_ニュースサイト_a=true;
	#bg012_02_1_ニュースサイト_a=true;
	#bg117_01_3_PC画面ニュージェネ_a=true;
	#bg141_01_3_PC画面del検索結果_a=true;
	#bg183_01_3_PC画面ZACO‐DQN_a=true;
	#bg202_01_1_希グループｗｅｂサイト_a=true;
	#bg125_01_3_PC画面ファンタズム公式_a=true;
	#bg203_01_1_ゲロカエルん偽通販サイト_a=true;
	#bg160_03_3_ダンボール箱_a=true;
	#bg004_01_1_作文用紙その目_a=true;
	#bg022_01_0_黒板その目_a=true;
	#bg171_01_3_カルテその目_a=true;
	#bg144_01_1_生徒手帳その目_a=true;
	#bg159_02_1_PC画面メールソフト_a=true;
	#bg120_01_3_PC画面その目_a=true;
	#bg186_02_1_たなびくタオル_a=true;
	#bg180_01_1_アイスその目_a=true;
	#bg128_02_3_ネットオークション_a=true;
	#bg158_03_1_ニュース地震_a=true;
	#bg001_01_1_崩壊渋谷_a=true;
	#bg027_03_5_道玄坂_a=true;
	#bg009_03_5_107_a=true;
	#bg182_01_3_手描きのクマの絵_a=true;
	#bg185_01_1_あやせ下着_a=true;
	#ev074_01_1_洗脳部隊inモニター_a=true;
	#bg205_01_3_あやせディソードリアルブート_a=true;
	#bg211_01_5_黄色いバングル_a=true;
	#bg197_01_3_渋谷駅東口とプラネタリウム_a=true;
	#bg200_01_6_ノアIIのあるドーム内_a=true;
}


