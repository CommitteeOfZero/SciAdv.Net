//======================================================================//
//　選択肢用の設定
//======================================================================//

//======================================================================//
//■妄想トリガーフラグ
//======================================================================//
//初期化
function InitTrigger()
{

	$妄想トリガー通過１ = 1;
	$妄想トリガー通過２ = 1;
	$妄想トリガー通過３ = 1;
	$妄想トリガー通過４ = 1;
	$妄想トリガー通過５ = 1;
	$妄想トリガー通過６ = 1;
	$妄想トリガー通過７ = 1;
	$妄想トリガー通過８ = 1;
	$妄想トリガー通過９ = 1;
	$妄想トリガー通過１０ = 1;
	$妄想トリガー通過１１ = 1;
	$妄想トリガー通過１２ = 1;
	$妄想トリガー通過１３ = 1;
	$妄想トリガー通過１４ = 1;
	$妄想トリガー通過１５ = 1;
	$妄想トリガー通過１６ = 1;
	$妄想トリガー通過１７ = 1;
	$妄想トリガー通過１８ = 1;
	$妄想トリガー通過１９ = 1;
	$妄想トリガー通過２０ = 1;
	$妄想トリガー通過２１ = 1;
	$妄想トリガー通過２２ = 1;
	$妄想トリガー通過２３ = 1;
	$妄想トリガー通過２４ = 1;
	$妄想トリガー通過２５ = 1;
	$妄想トリガー通過２６ = 1;
	$妄想トリガー通過２７ = 1;
	$妄想トリガー通過２８ = 1;
	$妄想トリガー通過２９ = 1;
	$妄想トリガー通過３０ = 1;
	$妄想トリガー通過３１ = 1;
	$妄想トリガー通過３２ = 1;
	$妄想トリガー通過３３ = 1;
	$妄想トリガー通過３４ = 1;
	$妄想トリガー通過３５ = 1;
	$妄想トリガー通過３６ = 1;
	$妄想トリガー通過３７ = 1;
	$妄想トリガー通過３８ = 1;
	$妄想トリガー通過３９ = 1;
	$妄想トリガー通過４０ = 1;
	$妄想トリガー通過４１ = 1;
	$妄想トリガー通過４２ = 1;
	$妄想トリガー通過４３ = 1;
	$妄想トリガー通過４４ = 1;
}

//======================================================================//
//■妄想トリガー選択肢
//======================================================================//
//定義

function SetTrigger("$妄想トリガー名")
{

	WaitAction("@エンドトリガー");
	
	if($妄想トリガー名 == "１")
	{
		$妄想トリガー通過１ = 1;
	}
	else if($妄想トリガー名 == "２")
	{
		$妄想トリガー通過２ = 1;
	}
	else if($妄想トリガー名 == "３")
	{
		$妄想トリガー通過３ = 1;
	}
	else if($妄想トリガー名 == "４")
	{
		$妄想トリガー通過４ = 1;
	}
	else if($妄想トリガー名 == "５")
	{
		$妄想トリガー通過５ = 1;
	}
	else if($妄想トリガー名 == "６")
	{
		$妄想トリガー通過６ = 1;
	}
	else if($妄想トリガー名 == "７")
	{
		$妄想トリガー通過７ = 1;
	}
	else if($妄想トリガー名 == "８")
	{
		$妄想トリガー通過８ = 1;
	}
	else if($妄想トリガー名 == "９")
	{
		$妄想トリガー通過９ = 1;
	}
	else if($妄想トリガー名 == "１０")
	{
		$妄想トリガー通過１０ = 1;
	}
	else if($妄想トリガー名 == "１１")
	{
		$妄想トリガー通過１１ = 1;
	}
	else if($妄想トリガー名 == "１２")
	{
		$妄想トリガー通過１２ = 1;
	}
	else if($妄想トリガー名 == "１３")
	{
		$妄想トリガー通過１３ = 1;
	}
	else if($妄想トリガー名 == "１４")
	{
		$妄想トリガー通過１４ = 1;
	}
	else if($妄想トリガー名 == "１５")
	{
		$妄想トリガー通過１５ = 1;
	}
	else if($妄想トリガー名 == "１６")
	{
		$妄想トリガー通過１６ = 1;
	}
	else if($妄想トリガー名 == "１７")
	{
		$妄想トリガー通過１７ = 1;
	}
	else if($妄想トリガー名 == "１８")
	{
		$妄想トリガー通過１８ = 1;
	}
	else if($妄想トリガー名 == "１９")
	{
		$妄想トリガー通過１９ = 1;
	}
	else if($妄想トリガー名 == "２０")
	{
		$妄想トリガー通過２０ = 1;
	}
	else if($妄想トリガー名 == "２１")
	{
		$妄想トリガー通過２１ = 1;
	}
	else if($妄想トリガー名 == "２２")
	{
		$妄想トリガー通過２２ = 1;
	}
	else if($妄想トリガー名 == "２３")
	{
		$妄想トリガー通過２３ = 1;
	}
	else if($妄想トリガー名 == "２４")
	{
		$妄想トリガー通過２４ = 1;
	}
	else if($妄想トリガー名 == "２５")
	{
		$妄想トリガー通過２５ = 1;
	}
	else if($妄想トリガー名 == "２６")
	{
		$妄想トリガー通過２６ = 1;
	}
	else if($妄想トリガー名 == "２７")
	{
		$妄想トリガー通過２７ = 1;
	}
	else if($妄想トリガー名 == "２８")
	{
		$妄想トリガー通過２８ = 1;
	}
	else if($妄想トリガー名 == "２９")
	{
		$妄想トリガー通過２９ = 1;
	}
	else if($妄想トリガー名 == "３０")
	{
		$妄想トリガー通過３０ = 1;
	}
	else if($妄想トリガー名 == "３１")
	{
		$妄想トリガー通過３１ = 1;
	}
	else if($妄想トリガー名 == "３２")
	{
		$妄想トリガー通過３２ = 1;
	}
	else if($妄想トリガー名 == "３３")
	{
		$妄想トリガー通過３３ = 1;
	}
	else if($妄想トリガー名 == "３４")
	{
		$妄想トリガー通過３４ = 1;
	}
	else if($妄想トリガー名 == "３５")
	{
		$妄想トリガー通過３５ = 1;
	}
	else if($妄想トリガー名 == "３６")
	{
		$妄想トリガー通過３６ = 1;
	}
	else if($妄想トリガー名 == "３７")
	{
		$妄想トリガー通過３７ = 1;
	}
	else if($妄想トリガー名 == "３８")
	{
		$妄想トリガー通過３８ = 1;
	}
	else if($妄想トリガー名 == "３９")
	{
		$妄想トリガー通過３９ = 1;
	}
	else if($妄想トリガー名 == "４０")
	{
		$妄想トリガー通過４０ = 1;
	}
	else if($妄想トリガー名 == "４１")
	{
		$妄想トリガー通過４１ = 1;
	}
	else if($妄想トリガー名 == "４２")
	{
		$妄想トリガー通過４２ = 1;
	}
	else if($妄想トリガー名 == "４３")
	{
		$妄想トリガー通過４３ = 1;
	}
	else if($妄想トリガー名 == "４４")
	{
		$妄想トリガー通過４４ = 1;
	}
	
	CreateTextureSP("posi1", 20100, 0, 0, "cg/sys/trigger/left-001.png");
	CreateTextureSP("posi2", 20100, 0, 0, "cg/sys/trigger/left-002.png");
	CreateTextureSP("posi3", 20100, 0, 0, "cg/sys/trigger/left-003.png");
	CreateTextureSP("posi4", 20100, 0, 0, "cg/sys/trigger/left-004.png");
	CreateTextureSP("posi5", 20100, 0, 0, "cg/sys/trigger/left-005.png");
	CreateTextureSP("posi6", 20100, 0, 0, "cg/sys/trigger/left-006.png");
	CreateTextureSP("posi7", 20100, 0, 0, "cg/sys/trigger/left-007.png");
	CreateTextureSP("posi8", 20100, 0, 0, "cg/sys/trigger/left-008.png");
	CreateTextureSP("posi9", 20100, 0, 0, "cg/sys/trigger/left-009.png");
	CreateTextureSP("posi10", 20100, 0, 0, "cg/sys/trigger/left-010.png");
	CreateTextureSP("posi11", 20100, 0, 0, "cg/sys/trigger/left-011.png");
	CreateTextureSP("posi12", 20100, 0, 0, "cg/sys/trigger/left-012.png");
	CreateTextureSP("posi13", 20100, 0, 0, "cg/sys/trigger/left-013.png");
	CreateTextureSP("posi14", 20100, 0, 0, "cg/sys/trigger/left-014.png");
	CreateTextureSP("posi15", 20100, 0, 0, "cg/sys/trigger/left-015.png");
	CreateTextureSP("posi16", 20100, 0, 0, "cg/sys/trigger/left-016.png");
	CreateTextureSP("posi17", 20100, 0, 0, "cg/sys/trigger/left-017.png");
	CreateTextureSP("posi18", 20100, 0, 0, "cg/sys/trigger/left-018.png");
	CreateTextureSP("posi19", 20100, 0, 0, "cg/sys/trigger/left-019.png");
	CreateTextureSP("posi20", 20100, 0, 0, "cg/sys/trigger/left-020.png");
	CreateTextureSP("posi21", 20100, 0, 0, "cg/sys/trigger/left-021.png");
	CreateTextureSP("posi22", 20100, 0, 0, "cg/sys/trigger/left-022.png");
	CreateTextureSP("posi23", 20100, 0, 0, "cg/sys/trigger/left-023.png");
	CreateTextureSP("posi24", 20100, 0, 0, "cg/sys/trigger/left-024.png");
	CreateTextureSP("posi25", 20100, 0, 0, "cg/sys/trigger/left-025.png");
	CreateTextureSP("posi26", 20100, 0, 0, "cg/sys/trigger/left-026.png");
	CreateTextureSP("posi27", 20100, 0, 0, "cg/sys/trigger/left-027.png");
	CreateTextureSP("posi28", 20100, 0, 0, "cg/sys/trigger/left-028.png");
	CreateTextureSP("posi29", 20100, 0, 0, "cg/sys/trigger/left-029.png");
	CreateTextureSP("posi30", 20100, 0, 0, "cg/sys/trigger/left-030.png");
	CreateTextureSP("posi31", 20100, 0, 0, "cg/sys/trigger/left-031.png");
	CreateTextureSP("posi32", 20100, 0, 0, "cg/sys/trigger/left-032.png");
	CreateTextureSP("posi33", 20100, 0, 0, "cg/sys/trigger/left-033.png");
	CreateTextureSP("posi34", 20100, 0, 0, "cg/sys/trigger/left-034.png");
	CreateTextureSP("posi35", 20100, 0, 0, "cg/sys/trigger/left-035.png");
	CreateTextureSP("posi36", 20100, 0, 0, "cg/sys/trigger/left-036.png");
	CreateTextureSP("posi37", 20100, 0, 0, "cg/sys/trigger/left-037.png");
	CreateTextureSP("posi38", 20100, 0, 0, "cg/sys/trigger/left-038.png");

	Fade("posi*", 0, 0, null, true);

	CreateTextureSP("nega1", 20100, 640, 0, "cg/sys/trigger/right-001.png");
	CreateTextureSP("nega2", 20100, 640, 0, "cg/sys/trigger/right-002.png");
	CreateTextureSP("nega3", 20100, 640, 0, "cg/sys/trigger/right-003.png");
	CreateTextureSP("nega4", 20100, 640, 0, "cg/sys/trigger/right-004.png");
	CreateTextureSP("nega5", 20100, 640, 0, "cg/sys/trigger/right-005.png");
	CreateTextureSP("nega6", 20100, 640, 0, "cg/sys/trigger/right-006.png");
	CreateTextureSP("nega7", 20100, 640, 0, "cg/sys/trigger/right-007.png");
	CreateTextureSP("nega8", 20100, 640, 0, "cg/sys/trigger/right-008.png");
	CreateTextureSP("nega9", 20100, 640, 0, "cg/sys/trigger/right-009.png");
	CreateTextureSP("nega10", 20100, 640, 0, "cg/sys/trigger/right-010.png");
	CreateTextureSP("nega11", 20100, 640, 0, "cg/sys/trigger/right-011.png");
	CreateTextureSP("nega12", 20100, 640, 0, "cg/sys/trigger/right-012.png");
	CreateTextureSP("nega13", 20100, 640, 0, "cg/sys/trigger/right-013.png");
	CreateTextureSP("nega14", 20100, 640, 0, "cg/sys/trigger/right-014.png");
	CreateTextureSP("nega15", 20100, 640, 0, "cg/sys/trigger/right-015.png");
	CreateTextureSP("nega16", 20100, 640, 0, "cg/sys/trigger/right-016.png");
	CreateTextureSP("nega17", 20100, 640, 0, "cg/sys/trigger/right-017.png");
	CreateTextureSP("nega18", 20100, 640, 0, "cg/sys/trigger/right-018.png");
	CreateTextureSP("nega19", 20100, 640, 0, "cg/sys/trigger/right-019.png");
	CreateTextureSP("nega20", 20100, 640, 0, "cg/sys/trigger/right-020.png");
	CreateTextureSP("nega21", 20100, 640, 0, "cg/sys/trigger/right-021.png");
	CreateTextureSP("nega22", 20100, 640, 0, "cg/sys/trigger/right-022.png");
	CreateTextureSP("nega23", 20100, 640, 0, "cg/sys/trigger/right-023.png");
	CreateTextureSP("nega24", 20100, 640, 0, "cg/sys/trigger/right-024.png");
	CreateTextureSP("nega25", 20100, 640, 0, "cg/sys/trigger/right-025.png");
	CreateTextureSP("nega26", 20100, 640, 0, "cg/sys/trigger/right-026.png");
	CreateTextureSP("nega27", 20100, 640, 0, "cg/sys/trigger/right-027.png");
	CreateTextureSP("nega28", 20100, 640, 0, "cg/sys/trigger/right-028.png");
	CreateTextureSP("nega29", 20100, 640, 0, "cg/sys/trigger/right-029.png");
	CreateTextureSP("nega30", 20100, 640, 0, "cg/sys/trigger/right-030.png");
	CreateTextureSP("nega31", 20100, 640, 0, "cg/sys/trigger/right-031.png");
	CreateTextureSP("nega32", 20100, 640, 0, "cg/sys/trigger/right-032.png");
	CreateTextureSP("nega33", 20100, 640, 0, "cg/sys/trigger/right-033.png");
	CreateTextureSP("nega34", 20100, 640, 0, "cg/sys/trigger/right-034.png");
	CreateTextureSP("nega35", 20100, 640, 0, "cg/sys/trigger/right-035.png");
	CreateTextureSP("nega36", 20100, 640, 0, "cg/sys/trigger/right-036.png");
	CreateTextureSP("nega37", 20100, 640, 0, "cg/sys/trigger/right-037.png");
	CreateTextureSP("nega38", 20100, 640, 0, "cg/sys/trigger/right-038.png");

	Fade("nega*", 0, 0, null, true);

	CreateProcess("トリガープロセス１", 150, 0, 0, "ProcessTrigger");
	SetAlias("トリガープロセス１", "トリガープロセス１");

	CreateSE("サウンド０","SE_擬音_妄想トリガーB_Loop");
	MusicStart("サウンド０",2000,700,0,1000,null,true);

	Request("トリガープロセス１", Start);
	Request("トリガープロセス１", Disused);
}



function EndTrigger()
{
	SetVolume("サウンド０", 5000, 0, NULL);

	CreateProcess("トリガープロセス４", 150, 0, 0, "ProcessEndTrigger");
	SetAlias("トリガープロセス４", "エンドトリガー");
	
	Request("トリガープロセス４", Start);
	Request("トリガープロセス４", Disused);
}

function ProcessEndTrigger()
{
	CreateColor("トリガー色", 20200, 0, 0, 800, 50, "BLACK");
	Fade("トリガー色", 0, 0, null, true);
	begin:
	Fade("トリガー色", 500, 1000, null, true);

	Delete("@nega*");
	Delete("@posi*");
	Delete("@トリガー*");
	Delete("@選択肢*");
	Delete("トリガー色");
}

function CreateTextureSP("ナット名", 画像優先度, x, y, "絵")
{
	CreateTexture("ナット名", 画像優先度, x, y, "絵");
	Fade("ナット名", 0, 0, null, true);
	SetAlias("ナット名", "ナット名");
}


function ProcessPosi()
{
	while(1)
	{
		$PrePosiCount = $PosiCount;
		if($PosiCount == 38)
		{
			$PosiCount = 0;
		}
		$PosiCount += 1;
		$PosiNat = "@posi" + "$PosiCount";
		$PrePosiNat = "@posi" + "$PrePosiCount";

		Fade("$PosiNat", 10, 1000, null, 10);
		Fade("$PrePosiNat", 0, 0, null, 0);

	}
}


function ProcessNega()
{
	while(1)
	{
		$PreNegaCount = $NegaCount;
		if($NegaCount == 38)
		{
			$NegaCount = 0;
		}
		$NegaCount += 1;
		$NegaNat = "@nega" + "$NegaCount";
		$PreNegaNat = "@nega" + "$PreNegaCount";

		Fade("$NegaNat", 10, 1000, null, 10);
		Fade("$PreNegaNat", 0, 0, null, 0);
	}
}


function ProcessTrigger()
{
	CreateChoice("選択肢１");
	SetAlias("選択肢１", "選択肢１");
	CreateTexture("選択肢１/MouseUsual/選択範囲１", 100, 0, 0, "cg/sys/trigger/left-001.png");
	SetAlias("選択肢１/MouseUsual/選択範囲１", "選択肢１/MouseUsual/選択範囲１");
	CreateTexture("選択肢１/MouseOver/選択範囲２", 100, 0, 0, "cg/sys/trigger/left-001.png");
	SetAlias("選択肢１/MouseOver/選択範囲２", "選択肢１/MouseUsual/選択範囲２");
	CreateTexture("選択肢１/MouseClick/選択範囲３", 100, 0, 0, "cg/sys/trigger/left-001.png");
	SetAlias("選択肢１/MouseOver/選択範囲３", "選択肢１/MouseUsual/選択範囲３");
//	CreateSound("選択肢１/MouseOver/効果音１", SE, "sound/se/se01.wav");
//	CreateSound("選択肢１/MouseClick/効果音２", SE, "sound/se/se02.wav");
	Request("選択肢１/MouseUsual/選択範囲*", Erase);

	CreateChoice("選択肢２");
	SetAlias("選択肢２", "選択肢２");
	CreateTexture("選択肢２/MouseUsual/選択範囲１", 100, 640, 0, "cg/sys/trigger/right-001.png");
	SetAlias("選択肢２/MouseUsual/選択範囲１", "選択肢２/MouseUsual/選択範囲１");
	CreateTexture("選択肢２/MouseOver/選択範囲２", 100, 640, 0, "cg/sys/trigger/right-001.png");
	SetAlias("選択肢２/MouseUsual/選択範囲２", "選択肢２/MouseUsual/選択範囲２");
	CreateTexture("選択肢２/MouseClick/選択範囲３", 100, 640, 0, "cg/sys/trigger/right-001.png");
	SetAlias("選択肢２/MouseUsual/選択範囲３", "選択肢２/MouseUsual/選択範囲３");
//	CreateSound("選択肢２/MouseOver/効果音１", SE, "sound/se/se01.wav");
//	CreateSound("選択肢２/MouseClick/効果音２", SE, "sound/se/se02.wav");
	Request("選択肢２/MouseUsual/選択範囲*", Erase);

	CreateProcess("トリガープロセス２", 150, 0, 0, "ProcessPosi");
	SetAlias("トリガープロセス２", "トリガープロセス２");
	CreateProcess("トリガープロセス３", 150, 0, 0, "ProcessNega");
	SetAlias("トリガープロセス３", "トリガープロセス３");
	Request("トリガープロセス２", Start);
	Request("トリガープロセス３", Start);

	begin:

	if($妄想トリガー名 == "１")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "４")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー４ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー４ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "５")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー５ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー５ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "６")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー６ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー６ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "７")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー７ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー７ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "８")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー８ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー８ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "９")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー９ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー９ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１０")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１０ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１０ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１１")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１１ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１１ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１２")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１２ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１２ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１３")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１３ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１３ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１４")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１４ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１４ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１５")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１５ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１５ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１６")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１６ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１６ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１７")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１７ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１７ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１８")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１８ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１８ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "１９")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー１９ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー１９ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２０")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２０ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２０ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２１")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２１ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２１ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２２")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２２ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２２ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２３")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２３ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２３ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２４")
	{
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２４ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２４ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２５")
	{
		$妄想トリガー通過２５ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２５ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２５ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２６")
	{
		$妄想トリガー通過２６ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２６ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２６ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２７")
	{
		$妄想トリガー通過２７ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２７ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２７ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２８")
	{
		$妄想トリガー通過２８ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２８ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２８ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "２９")
	{
		$妄想トリガー通過２９ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー２９ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー２９ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３０")
	{
		$妄想トリガー通過３０ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３０ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３０ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３１")
	{
		$妄想トリガー通過３１ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３１ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３１ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３２")
	{
		$妄想トリガー通過３２ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３２ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３２ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３３")
	{
		$妄想トリガー通過３３ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３３ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３３ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３４")
	{
		$妄想トリガー通過３４ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３４ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３４ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３５")
	{
		$妄想トリガー通過３５ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３５ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３５ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３６")
	{
		$妄想トリガー通過３６ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３６ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３６ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３７")
	{
		$妄想トリガー通過３７ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３７ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３７ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３８")
	{
		$妄想トリガー通過３８ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３８ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３８ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "３９")
	{
		$妄想トリガー通過３９ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー３９ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー３９ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "４０")
	{
		$妄想トリガー通過４０ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー４０ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー４０ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "４１")
	{
		$妄想トリガー通過４１ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー４１ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー４１ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "４２")
	{
		$妄想トリガー通過４２ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー４２ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー４２ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "４３")
	{
		$妄想トリガー通過４３ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー４３ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー４３ = 1;
			}
		}
	}
	else if($妄想トリガー名 == "４４")
	{
		$妄想トリガー通過４４ = 1;
		select
		{
			case 選択肢１:
			{
				$妄想トリガー４４ = 2;
			}
			case 選択肢２:
			{
				$妄想トリガー４４ = 1;
			}
		}
	}
	



	SetVolume("@サウンド０", 5000, 0, NULL);

	CreateColor("トリガー色", 20200, 0, 0, 800, 50, "BLACK");
	Fade("トリガー色", 0, 0, null, true);
	Fade("トリガー色", 500, 1000, null, true);

	Delete("@nega*");
	Delete("@posi*");

	Delete("トリガープロセス２");
	Delete("トリガープロセス３");

	Delete("@選択肢*");
	Delete("トリガー色");
}






















//======================================================================//
//■ＹＥＳ・ＮＯ選択肢２
//======================================================================//
// ＹＥＳ・ＮＯを表示
function StartWhich02()
{
	$skip_log=$SYSTEM_skip;
	$auto_log=$SYSTEM_text_auto;
	$SYSTEM_backlog_lock = 1;
	$SYSTEM_text_erase_lock = true;
	$SYSTEM_skip_lock = true;


	CreateTexture("セカイ背景", 10000, 0, 0, "cg/sys/select2/back.png");
	Request("セカイ背景", Smoothing);
	Fade("セカイ背景", 0, 0, null, true);

	CreateTexture("セカイドア", 10000, 0, 0, "cg/sys/select2/yesno.png");
	Fade("セカイドア", 0, 0, null, true);
	Zoom("セカイドア", 0, 200, 200, null, true);
	SetAlias("セカイドア","セカイドア");
	Request("セカイドア", Smoothing);

	Fade("セカイ背景", 500, 1000, null, true);
	Fade("セカイドア", 500, 1000, null, true);


	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;

}


function PreWhich02()
{
	CreateTexture("セカイ背景", 9500, 0, 0, "cg/sys/select2/back.png");
	Request("セカイ背景", Smoothing);
	CreateTexture("セカイドア", 10000, 0, 0, "cg/sys/select2/yesno.png");
	Zoom("セカイドア", 0, 1, 1, null, true);
	SetAlias("セカイドア","セカイドア");
	Request("セカイドア", Smoothing);

	DrawTransition("世界スクリーン", 500, 1000, 0, 100, Axl2, "cg/data/zoom2.png", true);
	Delete("世界スクリーン");

	Zoom("セカイドア", 500, 200, 200, Dxl2, true);

	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;
}


function FadeWhich02()
{
	CreateTexture("セカイ乗算", 10350, 0, 0, "cg/sys/select2/jyousan.png");
	Fade("セカイ乗算", 0, 0, null, true);
	SetAlias("セカイ乗算","セカイ乗算");
	Request("セカイ乗算", Smoothing);
	Request("セカイ乗算", AddRender);

	CreateSE("セカイサウンド１","SE_擬音_YesNo選択_IN");
	MusicStart("セカイサウンド１",1000,1000,0,1000,null,false);

	Zoom("セカイドア", 800, 1000, 1000, AxlDxl, false);
	Fade("セカイ乗算", 800, 200, null, true);
}


function EndWhich02()
{
	DrawTransition("世界スクリーン", 500, 1000, 0, 100, Axl2, "cg/data/zoom2.png", true);

	Delete("世界スクリーン");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;
	
	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock =false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

// ＹＥＳ・ＮＯを表示
function SetWhich02()
{
	CreateChoice("選択肢１");
	SetAlias("選択肢１","選択肢１");

	CreateTexture("@選択肢１/MouseUsual/選択範囲１", 10100, 160, 100, "cg/sys/select2/room.png");
	CreateTexture("@選択肢１/MouseOver/選択範囲２", 10100, 0, 0, "cg/sys/select2/yes.png");
	CreateTexture("@選択肢１/MouseClick/選択範囲３", 10100, 0, 0, "cg/sys/select2/yes.png");
	Request("@選択肢１/MouseUsual/選択範囲１", Erase);
	Request("@選択肢１/選択肢０１", Erase);

	CreateChoice("選択肢２");
	SetAlias("選択肢２","選択肢２");

	CreateTexture("@選択肢２/MouseUsual/選択範囲１", 10100, 430, 100, "cg/sys/select2/room.png");
	CreateTexture("@選択肢２/MouseOver/選択範囲２", 10100, 0, 0, "cg/sys/select2/no.png");
	CreateTexture("@選択肢２/MouseClick/選択範囲３", 10100, 0, 0, "cg/sys/select2/no.png");
	Request("@選択肢２/MouseUsual/選択範囲１", Erase);
	Request("@選択肢２/選択肢０１", Erase);

	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", UP);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", DOWN);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", LEFT);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", RIGHT);

	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", UP);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", DOWN);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", LEFT);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", RIGHT);

	Fade("@選択肢*/*/*",0,0,null,true);

	$SYSTEM_show_cursor = true;
}



function YES02()
{

	CreateSE("サウンド１","SE_擬音_YesNo選択_クリック");
	SoundPlay("サウンド１",0,1000,false);

	Fade("box00/*",500,0,null,false);
	Fade("@選択肢１/MouseClick/選択範囲３",500,0,null,true);

	CreateColor("セカイ色１", 15000, 0, 0, 800, 600, "WHITE");
	Request("セカイ色１", AddRender);
	Fade("セカイ色１", 0, 0, null, true);
	SetVertex("セカイドア", 265, middle);

	Fade("セカイ色１", 1000, 1000, null, false);
	Move("セカイドア", 1000, @135, @0, Axl2, false);
	Zoom("セカイドア", 1000, 5000, 5000, Axl2, true);

	CreateTexture("世界スクリーン", 20550, 0, 0, "SCREEN");

	$テキストデータバックログ１ = "【YES】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("box00/*");
	Delete("@選択肢*");
	Delete("選択肢*");
	Delete("セカイ*");
}




function NO02()
{
	CreateSE("サウンド１","SE_擬音_YesNo選択_クリック");
	SoundPlay("サウンド１",0,1000,false);

	Fade("box00/*",500,0,null,false);
	Fade("@選択肢２/MouseClick/選択範囲３",500,0,null,true);

	CreateColor("セカイ色１", 15000, 0, 0, 800, 600, "WHITE");
	Request("セカイ色１", AddRender);
	Fade("セカイ色１", 0, 0, null, true);
	SetVertex("セカイドア", 535, middle);

	Fade("セカイ色１", 1000, 1000, null, false);
	Move("セカイドア", 1000, @-135, @0, Axl2, false);
	Zoom("セカイドア", 1000, 5000, 5000, Axl2, true);

	CreateTexture("世界スクリーン", 20550, 0, 0, "SCREEN");

	$テキストデータバックログ１ = "【NO】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("box00/*");
	Delete("@選択肢*");
	Delete("選択肢*");
	Delete("セカイ*");
}











//======================================================================//
//■ＹＥＳ・ＮＯ選択肢
//======================================================================//
// ＹＥＳ・ＮＯを表示
function StartWhich03()
{
	$skip_log=$SYSTEM_skip;
	$auto_log=$SYSTEM_text_auto;
	$SYSTEM_backlog_lock = 1;
	$SYSTEM_text_erase_lock = true;
	$SYSTEM_skip_lock = true;


	CreateTexture("セカイ背景", 9500, 0, 0, "cg/sys/select/back.png");
	Request("セカイ背景", Smoothing);
	Fade("セカイ背景", 0, 0, null, true);

	CreateTexture("セカイドア", 10000, 160, 100, "cg/sys/select/yesno.png");
	Fade("セカイドア", 0, 0, null, true);
	Zoom("セカイドア", 0, 200, 200, null, true);
	SetAlias("セカイドア","セカイドア");
	Request("セカイドア", Smoothing);

	Fade("セカイ背景", 500, 500, null, true);
	Fade("セカイドア", 500, 1000, null, true);


	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;

}


function PreWhich03()
{
	CreateTexture("セカイ背景", 10000, 0, 0, "cg/sys/select/back.png");
	Request("セカイ背景", Smoothing);
	CreateTexture("セカイドア", 10000, 160, 100, "cg/sys/select/yesno.png");
	Zoom("セカイドア", 0, 1, 1, null, true);
	SetAlias("セカイドア","セカイドア");
	Request("セカイドア", Smoothing);

	Fade("セカイ背景", 0, 500, null, true);

	DrawTransition("世界スクリーン", 500, 1000, 0, 100, Axl2, "cg/data/zoom2.png", true);
	Delete("世界スクリーン");

	Zoom("セカイドア", 500, 200, 200, Dxl2, true);

	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;
}


function FadeWhich03()
{
	CreateTexture("セカイ乗算", 10350, 0, 0, "cg/sys/select/jyousan.png");
	Fade("セカイ乗算", 0, 0, null, true);
	SetAlias("セカイ乗算","セカイ乗算");
	Request("セカイ乗算", Smoothing);
	Request("セカイ乗算", AddRender);

	CreateSE("セカイサウンド１","SE_擬音_YesNo選択_IN");
	MusicStart("セカイサウンド１",1000,1000,0,1000,null,false);

	Zoom("セカイドア", 1000, 1000, 1000, AxlDxl, false);
	Fade("セカイ乗算", 1000, 500, null, true);
}


function EndWhich03()
{
	DrawTransition("世界スクリーン", 500, 1000, 0, 100, Axl2, "cg/data/zoom2.png", true);

	Delete("世界スクリーン");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;
	
	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock =false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

// ＹＥＳ・ＮＯを表示
function SetWhich03()
{
	CreateChoice("選択肢１");
	SetAlias("選択肢１","選択肢１");
	CreateTexture("@選択肢１/MouseUsual/選択範囲１", 10100, 160, 100, "cg/sys/select/room.png");
	CreateTexture("@選択肢１/MouseOver/選択範囲２", 10100, 160, 100, "cg/sys/select/yes.png");
	CreateTexture("@選択肢１/MouseClick/選択範囲３", 10100, 160, 100, "cg/sys/select/yes.png");
	Request("@選択肢１/MouseUsual/選択範囲１", Erase);

	CreateChoice("選択肢２");
	SetAlias("選択肢２","選択肢２");
	CreateTexture("@選択肢２/MouseUsual/選択範囲１", 10100, 430, 100, "cg/sys/select/room.png");
	CreateTexture("@選択肢２/MouseOver/選択範囲２", 10100, 430, 100, "cg/sys/select/no.png");
	CreateTexture("@選択肢２/MouseClick/選択範囲３", 10100, 430, 100, "cg/sys/select/no.png");
	Request("@選択肢２/MouseUsual/選択範囲１", Erase);


	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", UP);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", DOWN);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", LEFT);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", RIGHT);

	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", UP);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", DOWN);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", LEFT);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", RIGHT);

	Fade("@選択肢*/*/*",0,0,null,true);

	$SYSTEM_show_cursor = true;
}



function YES03()
{
	CreateTexture("選択肢１＿００", 10100, 130, 0, "cg/sys/select/yes000.png");
	CreateTexture("選択肢１＿０１", 10100, 130, 0, "cg/sys/select/yes001.png");
	CreateTexture("選択肢１＿０２", 10100, 130, 0, "cg/sys/select/yes002.png");
	CreateTexture("選択肢１＿０３", 10100, 130, 0, "cg/sys/select/yes003.png");
	CreateTexture("選択肢１＿０４", 10100, 130, 0, "cg/sys/select/yes004.png");
	CreateTexture("選択肢１＿０５", 10100, 130, 0, "cg/sys/select/yes005.png");
	CreateTexture("選択肢１＿０６", 10100, 130, 0, "cg/sys/select/yes006.png");
	CreateTexture("選択肢１＿０７", 10100, 130, 0, "cg/sys/select/yes007.png");
	CreateTexture("選択肢１＿０８", 10100, 130, 0, "cg/sys/select/yes008.png");
	CreateTexture("選択肢１＿０９", 10100, 130, 0, "cg/sys/select/yes009.png");
	CreateTexture("選択肢１＿１０", 10100, 130, 0, "cg/sys/select/yes010.png");
	CreateTexture("選択肢１＿１１", 10100, 400, 0, "cg/sys/select/no000.png");
	CreateTexture("選択肢１奥", 20550, 160, 100, "cg/sys/select/room.png");
	SetVertex("選択肢１奥", 70, middle);
	Request("選択肢１＿１１", Smoothing);
	Request("選択肢１奥", Smoothing);

	Fade("選択肢１＿*",0,0,null,false);
	Fade("選択肢１奥",0,0,null,true);

	Fade("box00/*",500,0,null,false);
	Fade("@選択肢１/MouseClick/選択範囲３",500,0,null,true);

	CreateSE("サウンド１","SE_擬音_YesNo選択_クリック");
	SoundPlay("サウンド１",0,1000,false);

	Fade("選択肢１＿００",0,1000,null,false);
	Fade("選択肢１＿１１",0,1000,null,true);
	Fade("セカイドア",0,0,null,true);

	Fade("選択肢１＿０１",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿００",30,0,null,false);
	Fade("選択肢１＿０２",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０１",30,0,null,false);
	Fade("選択肢１＿０３",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０２",30,0,null,false);
	Fade("選択肢１＿０４",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０３",30,0,null,false);
	Fade("選択肢１＿０５",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０４",30,0,null,false);
	Fade("選択肢１＿０６",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０５",30,0,null,false);
	Fade("選択肢１＿０７",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０６",30,0,null,false);
	Fade("選択肢１＿０８",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０７",30,0,null,false);
	Fade("選択肢１＿０９",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０８",30,0,null,false);
	Fade("選択肢１＿１０",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０９",30,0,null,false);

	Fade("セカイ背景", 600, 1000, null, false);
	Fade("選択肢１奥",300,1000,null,300);
	Fade("選択肢１＿１０",300,0,null,300);

	Zoom("セカイ乗算", 1000, 2000, 2000, Axl2, false);
	Zoom("セカイ背景", 1000, 2000, 2000, Axl2, false);

	Move("選択肢１＿１１", 1000, @1500, @0, Axl2, false);
	Zoom("選択肢１＿１１", 1000, 5000, 5000, Axl2, false);
	Zoom("選択肢１奥", 1000, 5000, 5000, Axl2, true);

	CreateTexture("世界スクリーン", 20550, 0, 0, "SCREEN");

	$テキストデータバックログ１ = "【YES】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("box00/*");
	Delete("@選択肢*");
	Delete("選択肢*");
	Delete("セカイ*");
}




function NO03()
{
	CreateTexture("選択肢２＿００", 10100, 400, 0, "cg/sys/select/no000.png");
	CreateTexture("選択肢２＿０１", 10100, 400, 0, "cg/sys/select/no001.png");
	CreateTexture("選択肢２＿０２", 10100, 400, 0, "cg/sys/select/no002.png");
	CreateTexture("選択肢２＿０３", 10100, 400, 0, "cg/sys/select/no003.png");
	CreateTexture("選択肢２＿０４", 10100, 400, 0, "cg/sys/select/no004.png");
	CreateTexture("選択肢２＿０５", 10100, 400, 0, "cg/sys/select/no005.png");
	CreateTexture("選択肢２＿０６", 10100, 400, 0, "cg/sys/select/no006.png");
	CreateTexture("選択肢２＿０７", 10100, 400, 0, "cg/sys/select/no007.png");
	CreateTexture("選択肢２＿０８", 10100, 400, 0, "cg/sys/select/no008.png");
	CreateTexture("選択肢２＿０９", 10100, 400, 0, "cg/sys/select/no009.png");
	CreateTexture("選択肢２＿１０", 10100, 400, 0, "cg/sys/select/no010.png");
	CreateTexture("選択肢２＿１１", 10100, 130, 0, "cg/sys/select/yes000.png");
	CreateTexture("選択肢２奥", 20550, 430, 100, "cg/sys/select/room.png");
	SetVertex("選択肢２奥", 140, middle);
	Request("選択肢２＿１１", Smoothing);
	Request("選択肢２奥", Smoothing);

	Fade("選択肢２＿*",0,0,null,false);
	Fade("選択肢２奥",0,0,null,true);

	Fade("box00/*",500,0,null,false);
	Fade("@選択肢２/MouseClick/選択範囲３",500,0,null,true);

	CreateSE("サウンド１","SE_擬音_YesNo選択_クリック");
	SoundPlay("サウンド１",0,1000,false);

	Fade("選択肢２＿００",0,1000,null,false);
	Fade("選択肢２＿１１",0,1000,null,true);
	Fade("セカイドア",0,0,null,true);

	Fade("選択肢２＿０１",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿００",30,0,null,false);
	Fade("選択肢２＿０２",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０１",30,0,null,false);
	Fade("選択肢２＿０３",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０２",30,0,null,false);
	Fade("選択肢２＿０４",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０３",30,0,null,false);
	Fade("選択肢２＿０５",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０４",30,0,null,false);
	Fade("選択肢２＿０６",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０５",30,0,null,false);
	Fade("選択肢２＿０７",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０６",30,0,null,false);
	Fade("選択肢２＿０８",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０７",30,0,null,false);
	Fade("選択肢２＿０９",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０８",30,0,null,false);
	Fade("選択肢２＿１０",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０９",30,0,null,false);

	Fade("セカイ背景", 600, 1000, null, false);
	Fade("選択肢２奥",300,1000,null,300);
	Fade("選択肢２＿１０",300,0,null,300);

	Zoom("セカイ乗算", 1000, 2000, 2000, Axl2, false);
	Zoom("セカイ背景", 1000, 2000, 2000, Axl2, false);

	Move("選択肢２＿１１", 1000, @-1500, @0, Axl2, false);
	Zoom("選択肢２＿１１", 1000, 5000, 5000, Axl2, false);
	Zoom("選択肢２奥", 1000, 5000, 5000, Axl2, true);

	CreateTexture("世界スクリーン", 20550, 0, 0, "SCREEN");

	$テキストデータバックログ１ = "【NO】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("box00/*");
	Delete("@選択肢*");
	Delete("選択肢*");
	Delete("セカイ*");
}






























//======================================================================//
//■ＹＥＳ・ＮＯ選択肢
//======================================================================//
// ＹＥＳ・ＮＯを表示
function StartWhich()
{
	$skip_log=$SYSTEM_skip;
	$auto_log=$SYSTEM_text_auto;
	$SYSTEM_backlog_lock = 1;
	$SYSTEM_text_erase_lock = true;
	$SYSTEM_skip_lock = true;

	CreateTexture("セカイ背景", 9500, 0, 0, "cg/sys/select/back.png");
	Request("セカイ背景", Smoothing);
	Fade("セカイ背景", 0, 0, null, true);

	CreateTexture("セカイドア", 10000, 160, 100, "cg/sys/select/yesno.png");
	Fade("セカイドア", 0, 0, null, true);
	Zoom("セカイドア", 0, 200, 200, null, true);
	SetAlias("セカイドア","セカイドア");
	Request("セカイドア", Smoothing);

	Fade("セカイ背景", 500, 500, null, true);
	Fade("セカイドア", 500, 1000, null, true);


	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;

}



function PreWhich()
{
	CreateTexture("セカイ背景", 9500, 0, -130, "cg/sys/select/back.png");
	Request("セカイ背景", Smoothing);
	CreateTexture("セカイドア", 10000, 160, 70, "cg/sys/select/yesno.png");
	Zoom("セカイドア", 0, 200, 200, null, true);
	SetAlias("セカイドア","セカイドア");
	Request("セカイドア", Smoothing);

	Fade("セカイ背景", 0, 500, null, true);

	Move("セカイ背景", 1000, @0, @130, Dxl2, false);
	Move("セカイドア", 1000, @0, @30, Dxl2, false);

	DrawTransition("世界スクリーン", 500, 1000, 0, 100, Axl2, "cg/data/zoom2.png", true);
	Delete("世界スクリーン");

//	Zoom("セカイドア", 500, 200, 200, Dxl2, false);
	Wait(500);

	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;
}


function FadeWhich()
{
	CreateTexture("セカイ乗算", 10350, 0, 0, "cg/sys/select/jyousan.png");
	Fade("セカイ乗算", 0, 0, null, true);
	SetAlias("セカイ乗算","セカイ乗算");
	Request("セカイ乗算", Smoothing);
	Request("セカイ乗算", AddRender);

	CreateSE("セカイサウンド１","SE_擬音_YesNo選択_IN");
	MusicStart("セカイサウンド１",1000,1000,0,1000,null,false);

	Zoom("セカイドア", 1000, 1000, 1000, AxlDxl, false);
	Fade("セカイ乗算", 1000, 500, null, true);
}


function EndWhich()
{
	DrawTransition("世界スクリーン", 500, 1000, 0, 100, Axl2, "cg/data/zoom2.png", true);

	Delete("世界スクリーン");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;
	
	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock =false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

// ＹＥＳ・ＮＯを表示
function SetWhich()
{
	CreateChoice("選択肢１");
	SetAlias("選択肢１","選択肢１");
	CreateTexture("@選択肢１/MouseUsual/選択範囲１", 10100, 160, 100, "cg/sys/select/room.png");
	CreateTexture("@選択肢１/MouseOver/選択範囲２", 10100, 160, 100, "cg/sys/select/yes.png");
	CreateTexture("@選択肢１/MouseClick/選択範囲３", 10100, 160, 100, "cg/sys/select/yes.png");
	Request("@選択肢１/MouseUsual/選択範囲１", Erase);

	CreateChoice("選択肢２");
	SetAlias("選択肢２","選択肢２");
	CreateTexture("@選択肢２/MouseUsual/選択範囲１", 10100, 430, 100, "cg/sys/select/room.png");
	CreateTexture("@選択肢２/MouseOver/選択範囲２", 10100, 430, 100, "cg/sys/select/no.png");
	CreateTexture("@選択肢２/MouseClick/選択範囲３", 10100, 430, 100, "cg/sys/select/no.png");
	Request("@選択肢２/MouseUsual/選択範囲１", Erase);


	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", UP);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", DOWN);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", LEFT);
	SetNextFocus("@選択肢１/MouseUsual/選択範囲１", "@選択肢２/MouseUsual/選択範囲１", RIGHT);

	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", UP);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", DOWN);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", LEFT);
	SetNextFocus("@選択肢２/MouseUsual/選択範囲１", "@選択肢１/MouseUsual/選択範囲１", RIGHT);

	Fade("@選択肢*/*/*",0,0,null,true);

	$SYSTEM_show_cursor = true;
}



function YES()
{
	CreateTexture("選択肢１＿００", 10100, 130, 0, "cg/sys/select/yes000.png");
	CreateTexture("選択肢１＿０１", 10100, 130, 0, "cg/sys/select/yes001.png");
	CreateTexture("選択肢１＿０２", 10100, 130, 0, "cg/sys/select/yes002.png");
	CreateTexture("選択肢１＿０３", 10100, 130, 0, "cg/sys/select/yes003.png");
	CreateTexture("選択肢１＿０４", 10100, 130, 0, "cg/sys/select/yes004.png");
	CreateTexture("選択肢１＿０５", 10100, 130, 0, "cg/sys/select/yes005.png");
	CreateTexture("選択肢１＿０６", 10100, 130, 0, "cg/sys/select/yes006.png");
	CreateTexture("選択肢１＿０７", 10100, 130, 0, "cg/sys/select/yes007.png");
	CreateTexture("選択肢１＿０８", 10100, 130, 0, "cg/sys/select/yes008.png");
	CreateTexture("選択肢１＿０９", 10100, 130, 0, "cg/sys/select/yes009.png");
	CreateTexture("選択肢１＿１０", 10100, 130, 0, "cg/sys/select/yes010.png");
	CreateTexture("選択肢１＿１１", 10100, 400, 0, "cg/sys/select/no000.png");
	CreateTexture("選択肢１奥", 20550, 160, 100, "cg/sys/select/room.png");
	SetVertex("選択肢１奥", 70, middle);
	Request("選択肢１＿１１", Smoothing);
	Request("選択肢１奥", Smoothing);

	Fade("選択肢１＿*",0,0,null,false);
	Fade("選択肢１奥",0,0,null,true);

	Fade("box00/*",500,0,null,false);
	Fade("@選択肢１/MouseClick/選択範囲３",500,0,null,true);

	CreateSE("サウンド１","SE_擬音_YesNo選択_クリック");
	SoundPlay("サウンド１",0,1000,false);

	Fade("選択肢１＿００",0,1000,null,false);
	Fade("選択肢１＿１１",0,1000,null,true);
	Fade("セカイドア",0,0,null,true);

	Fade("選択肢１＿０１",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿００",30,0,null,false);
	Fade("選択肢１＿０２",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０１",30,0,null,false);
	Fade("選択肢１＿０３",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０２",30,0,null,false);
	Fade("選択肢１＿０４",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０３",30,0,null,false);
	Fade("選択肢１＿０５",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０４",30,0,null,false);
	Fade("選択肢１＿０６",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０５",30,0,null,false);
	Fade("選択肢１＿０７",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０６",30,0,null,false);
	Fade("選択肢１＿０８",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０７",30,0,null,false);
	Fade("選択肢１＿０９",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０８",30,0,null,false);
	Fade("選択肢１＿１０",30,1000,null,false);
	Wait(30);
	Fade("選択肢１＿０９",30,0,null,false);

	Fade("セカイ背景", 600, 1000, null, false);

	Fade("選択肢１奥",300,1000,null,300);
	Fade("選択肢１＿１０",300,0,null,300);


	Zoom("セカイ乗算", 1000, 2000, 2000, Axl2, false);
	Zoom("セカイ背景", 1000, 2000, 2000, Axl2, false);

	Move("選択肢１＿１１", 1000, @1500, @0, Axl2, false);
	Zoom("選択肢１＿１１", 1000, 5000, 5000, Axl2, false);
	Zoom("選択肢１奥", 1000, 5000, 5000, Axl2, true);

	CreateTexture("世界スクリーン", 20550, 0, 0, "SCREEN");

	$テキストデータバックログ１ = "【YES】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("box00/*");
	Delete("@選択肢*");
	Delete("選択肢*");
	Delete("セカイ*");
}


function NO()
{
	CreateTexture("選択肢２＿００", 10100, 400, 0, "cg/sys/select/no000.png");
	CreateTexture("選択肢２＿０１", 10100, 400, 0, "cg/sys/select/no001.png");
	CreateTexture("選択肢２＿０２", 10100, 400, 0, "cg/sys/select/no002.png");
	CreateTexture("選択肢２＿０３", 10100, 400, 0, "cg/sys/select/no003.png");
	CreateTexture("選択肢２＿０４", 10100, 400, 0, "cg/sys/select/no004.png");
	CreateTexture("選択肢２＿０５", 10100, 400, 0, "cg/sys/select/no005.png");
	CreateTexture("選択肢２＿０６", 10100, 400, 0, "cg/sys/select/no006.png");
	CreateTexture("選択肢２＿０７", 10100, 400, 0, "cg/sys/select/no007.png");
	CreateTexture("選択肢２＿０８", 10100, 400, 0, "cg/sys/select/no008.png");
	CreateTexture("選択肢２＿０９", 10100, 400, 0, "cg/sys/select/no009.png");
	CreateTexture("選択肢２＿１０", 10100, 400, 0, "cg/sys/select/no010.png");
	CreateTexture("選択肢２＿１１", 10100, 130, 0, "cg/sys/select/yes000.png");
	CreateTexture("選択肢２奥", 20550, 430, 100, "cg/sys/select/room.png");
	SetVertex("選択肢２奥", 140, middle);
	Request("選択肢２＿１１", Smoothing);
	Request("選択肢２奥", Smoothing);

	Fade("選択肢２＿*",0,0,null,false);
	Fade("選択肢２奥",0,0,null,true);

	Fade("box00/*",500,0,null,false);
	Fade("@選択肢２/MouseClick/選択範囲３",500,0,null,true);

	CreateSE("サウンド１","SE_擬音_YesNo選択_クリック");
	SoundPlay("サウンド１",0,1000,false);

	Fade("選択肢２＿００",0,1000,null,false);
	Fade("選択肢２＿１１",0,1000,null,true);
	Fade("セカイドア",0,0,null,true);

	Fade("選択肢２＿０１",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿００",30,0,null,false);
	Fade("選択肢２＿０２",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０１",30,0,null,false);
	Fade("選択肢２＿０３",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０２",30,0,null,false);
	Fade("選択肢２＿０４",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０３",30,0,null,false);
	Fade("選択肢２＿０５",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０４",30,0,null,false);
	Fade("選択肢２＿０６",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０５",30,0,null,false);
	Fade("選択肢２＿０７",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０６",30,0,null,false);
	Fade("選択肢２＿０８",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０７",30,0,null,false);
	Fade("選択肢２＿０９",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０８",30,0,null,false);
	Fade("選択肢２＿１０",30,1000,null,false);
	Wait(30);
	Fade("選択肢２＿０９",30,0,null,false);

	Fade("セカイ背景", 600, 1000, null, false);

	Fade("選択肢２奥",300,1000,null,300);
	Fade("選択肢２＿１０",300,0,null,300);

	Zoom("セカイ乗算", 1000, 2000, 2000, Axl2, false);
	Zoom("セカイ背景", 1000, 2000, 2000, Axl2, false);

	Move("選択肢２＿１１", 1000, @-1500, @0, Axl2, false);
	Zoom("選択肢２＿１１", 1000, 5000, 5000, Axl2, false);
	Zoom("選択肢２奥", 1000, 5000, 5000, Axl2, true);

	CreateTexture("世界スクリーン", 20550, 0, 0, "SCREEN");

	$テキストデータバックログ１ = "【NO】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("box00/*");
	Delete("@選択肢*");
	Delete("選択肢*");
	Delete("セカイ*");
}





















//======================================================================//
//■ジャンゴ選択肢
//======================================================================//
// ２択の選択肢ボタンを表示
function SetChoice02($テキストデータ１,$テキストデータ２)
{
	$skip_log=$SYSTEM_skip;
	$auto_log=$SYSTEM_text_auto;
	$SYSTEM_backlog_lock = 1;
	$SYSTEM_text_erase_lock = true;
	$SYSTEM_skip_lock = true;

	SetFont("ＭＳ ゴシック", 22, #FFFFFF, #000000, 500, LEFTDOWN);

	LoadImage("select_img","cg/sys/select/Select001.png");

	CreateTexture("選択肢板１",10400,138,200,"select_img");
	SetAlias("選択肢板１","選択肢板１");
	CreateTexture("選択肢板２",10410,138,300,"select_img");
	SetAlias("選択肢板２","選択肢板２");

	CreateText("選択肢板１/選択肢文字列１",10401,inherit, inherit,auto,auto,$テキストデータ１);
	SetAlias("選択肢板１/選択肢文字列１","選択肢文字列１");
	CreateText("選択肢板２/選択肢文字列２",10411,inherit, inherit,auto,auto,$テキストデータ２);
	SetAlias("選択肢板２/選択肢文字列２","選択肢文字列２");
	Request("@選択肢文字列*",PushText);
	Request("@選択肢文字列*",NoLog);

	CreateChoice("選択肢１");
	SetAlias("選択肢１","選択肢１");
	CreateTexture("@選択肢１/MouseUsual/選択肢１板１",10400,138,200,"select_img");
	CreateTexture("@選択肢１/MouseOver/選択肢１板２", 10400,138,200,"cg/sys/select/Select002.png");
	CreateTexture("@選択肢１/MouseClick/選択肢１板３",10400,138,200,"cg/sys/select/Select003.png");

	CreateChoice("選択肢２");
	SetAlias("選択肢２","選択肢２");
	CreateTexture("@選択肢２/MouseUsual/選択肢２板１",10410,138,300,"select_img");
	CreateTexture("@選択肢２/MouseOver/選択肢２板２", 10410,138,300,"cg/sys/select/Select002.png");
	CreateTexture("@選択肢２/MouseClick/選択肢２板３",10410,138,300,"cg/sys/select/Select003.png");

	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢２/MouseUsual/選択肢２板１", UP);
	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢２/MouseUsual/選択肢２板１", DOWN);
	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢２/MouseUsual/選択肢２板１", LEFT);
	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢２/MouseUsual/選択肢２板１", RIGHT);

	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢１/MouseUsual/選択肢１板１", UP);
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢１/MouseUsual/選択肢１板１", DOWN);
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢１/MouseUsual/選択肢１板１", LEFT);
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢１/MouseUsual/選択肢１板１", RIGHT);

	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;

	Fade("@選択肢板*",0,0,null,false);
	Fade("@選択肢*/*/*",0,0,null,false);
	Fade("@選択肢文字列*",0,0,null,false);
	Fade("@選択肢板*",300,1000,null,false);
	Fade("@選択肢文字列*",300,1000,null,false);
	Fade("@選択肢*/MouseUsual/*",300,1000,null,true);
	
	Draw();
	
	$SYSTEM_show_cursor = true;
	$SYSTEM_last_text = $テキストデータ１ + "<BR>　　"+$テキストデータ２;
}

// ２択の１番目が選ばれた後の処理
function ChoiceA02()
{
	Fade("@選択肢板２",300,0,null,false);
	Fade("@選択肢文字列２",300,0,null,false);
	Fade("@選択肢２/*",300,0,null,false);
	Fade("@選択肢２/*/*",300,0,null,false);
	Wait(500);
	Fade("@選択肢板１",1000,0,null,false);
	Fade("@選択肢文字列１",1000,0,null,false);
	Fade("@選択肢１/*",1000,0,null,false);
	Fade("@選択肢１/*/*",1000,0,null,false);

	WaitAction("@選択肢*");
	WaitAction("@選択肢*/*");
	WaitAction("@選択肢*/*/*");
			
	$テキストデータバックログ１ = "【" + $テキストデータ１ + "】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("選択*");
	Delete("@選択*");
	Delete("select_*");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;
	
	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock =false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

// ２択の２番目が選ばれた後の処理
function ChoiceB02()
{
	Fade("@選択肢板１",300,0,null,false);
	Fade("@選択肢文字列１",300,0,null,false);
	Fade("@選択肢１/*",300,0,null,false);
	Fade("@選択肢１/*/*",300,0,null,false);
	Wait(500);
	Fade("@選択肢板２",1000,0,null,false);
	Fade("@選択肢文字列２",1000,0,null,false);
	Fade("@選択肢２/*",1000,0,null,false);
	Fade("@選択肢２/*/*",1000,0,null,false);

	WaitAction("@選択肢*");
	WaitAction("@選択肢*/*");
	WaitAction("@選択肢*/*/*");
	
	//SetFont("ＭＳ ゴシック", 22, #FFFFFF, #00000, 500,DOWN);

	$テキストデータバックログ２ = "【" + $テキストデータ２ + "】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ２, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("選択*");
	Delete("@選択*");
	Delete("select_*");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;

	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock = false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}










// ３択の選択肢ボタンを表示
function SetChoice03($テキストデータ１,$テキストデータ２,$テキストデータ３)
{
	$skip_log=$SYSTEM_skip;
	$auto_log=$SYSTEM_text_auto;
	$SYSTEM_backlog_lock = 1;
	$SYSTEM_text_erase_lock = true;
	$SYSTEM_skip_lock = true;

	SetFont("ＭＳ ゴシック", 22, #FFFFFF, #000000, 500, LEFTDOWN);

	LoadImage("select_img","cg/sys/select/Select001.png");

	CreateTexture("選択肢板１",10400,138,150,"select_img");
	SetAlias("選択肢板１","選択肢板１");
	CreateTexture("選択肢板２",10410,138,250,"select_img");
	SetAlias("選択肢板２","選択肢板２");
	CreateTexture("選択肢板３",10420,138,350,"select_img");
	SetAlias("選択肢板３","選択肢板３");

	CreateText("選択肢板１/選択肢文字列１",10401,inherit, inherit,auto,auto,$テキストデータ１);
	SetAlias("選択肢板１/選択肢文字列１","選択肢文字列１");
	CreateText("選択肢板２/選択肢文字列２",10411,inherit, inherit,auto,auto,$テキストデータ２);
	SetAlias("選択肢板２/選択肢文字列２","選択肢文字列２");
	CreateText("選択肢板３/選択肢文字列３",10421,inherit, inherit,auto,auto,$テキストデータ３);
	SetAlias("選択肢板３/選択肢文字列３","選択肢文字列３");
	Request("@選択肢文字列*",PushText);
	Request("@選択肢文字列*",NoLog);

	CreateChoice("選択肢１");
	SetAlias("選択肢１","選択肢１");
	CreateTexture("@選択肢１/MouseUsual/選択肢１板１",10400,138,150,"select_img");
	CreateTexture("@選択肢１/MouseOver/選択肢１板２", 10400,138,150,"cg/sys/select/Select002.png");
	CreateTexture("@選択肢１/MouseClick/選択肢１板３",10400,138,150,"cg/sys/select/Select003.png");

	CreateChoice("選択肢２");
	SetAlias("選択肢２","選択肢２");
	CreateTexture("@選択肢２/MouseUsual/選択肢２板１",10410,138,250,"select_img");
	CreateTexture("@選択肢２/MouseOver/選択肢２板２", 10410,138,250,"cg/sys/select/Select002.png");
	CreateTexture("@選択肢２/MouseClick/選択肢２板３",10410,138,250,"cg/sys/select/Select003.png");

	CreateChoice("選択肢３");
	SetAlias("選択肢３","選択肢３");
	CreateTexture("@選択肢３/MouseUsual/選択肢３板１",10420,138,350,"select_img");
	CreateTexture("@選択肢３/MouseOver/選択肢３板２", 10420,138,350,"cg/sys/select/Select002.png");
	CreateTexture("@選択肢３/MouseClick/選択肢３板３",10420,138,350,"cg/sys/select/Select003.png");

	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢３/MouseUsual/選択肢３板１", UP);
	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢２/MouseUsual/選択肢２板１", DOWN);
	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢３/MouseUsual/選択肢３板１", LEFT);
	SetNextFocus("@選択肢１/MouseUsual/選択肢１板１", "@選択肢２/MouseUsual/選択肢２板１", RIGHT);
	
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢１/MouseUsual/選択肢１板１", UP);
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢３/MouseUsual/選択肢３板１", DOWN);
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢１/MouseUsual/選択肢１板１", LEFT);
	SetNextFocus("@選択肢２/MouseUsual/選択肢２板１", "@選択肢３/MouseUsual/選択肢３板１", RIGHT);
	
	SetNextFocus("@選択肢３/MouseUsual/選択肢３板１", "@選択肢２/MouseUsual/選択肢２板１", UP);
	SetNextFocus("@選択肢３/MouseUsual/選択肢３板１", "@選択肢１/MouseUsual/選択肢１板１", DOWN);
	SetNextFocus("@選択肢３/MouseUsual/選択肢３板１", "@選択肢２/MouseUsual/選択肢２板１", LEFT);
	SetNextFocus("@選択肢３/MouseUsual/選択肢３板１", "@選択肢１/MouseUsual/選択肢１板１", RIGHT);
	
	$SYSTEM_skip=false;
	$SYSTEM_text_auto=false;
//	$SYSTEM_backselect_lock = true;

	Fade("@選択肢板*",0,0,null,false);
	Fade("@選択肢*/*/*",0,0,null,false);
	Fade("@選択肢文字列*",0,0,null,false);
	Fade("@選択肢板*",300,1000,null,false);
	Fade("@選択肢文字列*",300,1000,null,false);
	Fade("@選択肢*/MouseUsual/*",300,1000,null,true);
	
	Draw();
	
	$SYSTEM_show_cursor = true;
	$SYSTEM_last_text = $テキストデータ１ + "<BR>　　"+$テキストデータ２ + "<BR>　　"+$テキストデータ３;
}

// ３択の１番目が選ばれた後の処理
function ChoiceA03()
{
	Fade("@選択肢板２",300,0,null,false);
	Fade("@選択肢文字列２",300,0,null,false);
	Fade("@選択肢２/*",300,0,null,false);
	Fade("@選択肢２/*/*",300,0,null,false);
	Fade("@選択肢板３",300,0,null,false);
	Fade("@選択肢文字列３",300,0,null,false);
	Fade("@選択肢３/*",300,0,null,false);
	Fade("@選択肢３/*/*",300,0,null,false);
	Wait(500);
	Fade("@選択肢板１",1000,0,null,false);
	Fade("@選択肢文字列１",1000,0,null,false);
	Fade("@選択肢１/*",1000,0,null,false);
	Fade("@選択肢１/*/*",1000,0,null,false);
	WaitAction("@選択肢*");
	WaitAction("@選択肢*/*");
	WaitAction("@選択肢*/*/*");
			
	$テキストデータバックログ１ = "【" + $テキストデータ１ + "】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ１, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("選択*");
	Delete("@選択*");
	Delete("select_*");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;
	
	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock =false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

// ３択の２番目が選ばれた後の処理
function ChoiceB03()
{
	Fade("@選択肢板１",300,0,null,false);
	Fade("@選択肢文字列１",300,0,null,false);
	Fade("@選択肢１/*",300,0,null,false);
	Fade("@選択肢１/*/*",300,0,null,false);
	Fade("@選択肢板３",300,0,null,false);
	Fade("@選択肢文字列３",300,0,null,false);
	Fade("@選択肢３/*",300,0,null,false);
	Fade("@選択肢３/*/*",300,0,null,false);
	Wait(500);
	Fade("@選択肢板２",1000,0,null,false);
	Fade("@選択肢文字列２",1000,0,null,false);
	Fade("@選択肢２/*",1000,0,null,false);
	Fade("@選択肢２/*/*",1000,0,null,false);
	WaitAction("@選択肢*");
	WaitAction("@選択肢*/*");
	WaitAction("@選択肢*/*/*");
	
	//SetFont("ＭＳ ゴシック", 22, #FFFFFF, #00000, 500,DOWN);

	$テキストデータバックログ２ = "【" + $テキストデータ２ + "】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ２, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("選択*");
	Delete("@選択*");
	Delete("select_*");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;

	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock = false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

// ３択の３番目が選ばれた後の処理
function ChoiceC03()
{
	Fade("@選択肢板１",300,0,null,false);
	Fade("@選択肢文字列１",300,0,null,false);
	Fade("@選択肢１/*",300,0,null,false);
	Fade("@選択肢１/*/*",300,0,null,false);
	Fade("@選択肢板２",300,0,null,false);
	Fade("@選択肢文字列２",300,0,null,false);
	Fade("@選択肢２/*",300,0,null,false);
	Fade("@選択肢２/*/*",300,0,null,false);
	Wait(500);
	Fade("@選択肢板３",1000,0,null,false);
	Fade("@選択肢文字列３",1000,0,null,false);
	Fade("@選択肢３/*",1000,0,null,false);
	Fade("@選択肢３/*/*",1000,0,null,false);
	WaitAction("@選択肢*");
	WaitAction("@選択肢*/*");
	WaitAction("@選択肢*/*/*");
	
	//SetFont("ＭＳ ゴシック", 22, #FFFFFF, #00000, 500,DOWN);

	$テキストデータバックログ３ = "【" + $テキストデータ３ + "】";

	SetBacklog("　", "null", null);//★バクログ
	SetBacklog($テキストデータバックログ３, "null", null);//★バクログ
	SetBacklog("　", "null", null);//★バクログ

	Delete("選択*");
	Delete("@選択*");
	Delete("select_*");

	$SYSTEM_backlog_lock = 0;
	$SYSTEM_skip_lock=false;

	if(#keep_auto_and_skip){
		if($skip_log){
			$SYSTEM_skip=true;
		}else if($auto_log){
			$SYSTEM_text_auto=true;
		}
	}
//	$SYSTEM_backselect_lock = false;
	$SYSTEM_show_cursor = false;
	$SYSTEM_text_erase_lock = false;
}

