
//���}�N��������Ƀ}�N���ɂĂ܂Ƃ߂Ē�`
//=============================================================================//
.//�܂Ƃߒ�`
//=============================================================================//

..SystemInit
function SystemInit()
{

	$SYSTEM_spt_name = $ChapterName;
	$SYSTEM_text_interval = 34;


	//����x�ǂݍ��񂾂�ēx�ǂݍ���ł��܂�Ȃ��悤�ɁB
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


//���e�L�X�g�{�b�N�X�E�t�H���g�A�X�N���v�g���悭��`������̂��}�N���ɂĂ܂Ƃ߂Ē�`�B
//=============================================================================//
.//�{�b�N�X��`
//=============================================================================//
..Box
function LoadBox()
{
	//�V�l�X�R���ǂ�
	CreateColor("box11", 20000, 0, 0, 800, 50, "BLACK");
	CreateColor("box12", 20000, 0, 470, 800, 130, "BLACK");
	SetAlias("box11", "box11");
	SetAlias("box12", "box12");
	Fade("box11",0,0,null,false);
	Fade("box12",0,0,null,true);
	Request("box11", Lock);
	Request("box12", Lock);

	LoadFont("�t�H���g�P�`", "�l�r �S�V�b�N", 20, #FFFFFF, #000000, 500, LEFTDOWN, "�����������������������������������ĂƂȂɂʂ˂̂͂Ђӂւق܂݂ނ߂�������������񂪂����������������������Âłǂ΂тԂׂڂς҂Ղ؂ۂ���������������A�C�E�G�I�J�L�N�P�R�T�V�X�Z�\�^�`�c�e�g�i�j�k�l�m�n�q�t�w�z�}�~�����������������������������K�M�O�Q�S�U�W�Y�[�]�_�a�d�f�h�o�r�u�x�{�p�s�v�y�|�@�B�D�F�H�b�������A�B�[�c�I�H");
	Request("�t�H���g�P�`", Lock);
}



//���X�N���v�g�ɂ����ăe�L�X�g���`����}�N���R�}���h
//=============================================================================//
.//�e�L�X�g��`
//=============================================================================//

..SetText
function SetText("�{�b�N�X��","$�e�L�X�g��")
{
	SetFont("�l�r �S�V�b�N", 20, #FFFFFF, #000000, 500, LEFTDOWN);
	LoadText("$�\����","�{�b�N�X��","$�e�L�X�g��",720,130,0,29);

	Request("$�e�L�X�g��", Hideable);
	Request("$�e�L�X�g��", Lock);
	Request("$�e�L�X�g��", Erase);

	Move("$�e�L�X�g��",0,@40,@0,null,true);
}

//����`�����e�L�X�g�̈ʒu���܂Ƃ߂Ē���
//=============================================================================//
.//�e�L�X�g�ʒu�␳
//=============================================================================//


//���{�b�N�X��`�悷��̂ƃe�L�X�g���^�C�s���O���铮����܂Ƃ߂Ď��s����}�N��
//=============================================================================//
.//�^�C�s���O�}�N��
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
function TypeBegin3(�b��)
{
	$boxtype = $SYSTEM_present_preprocess;
	$textnumber = $SYSTEM_present_text;

	$SYSTEM_position_x_text_icon = -32768;
	$SYSTEM_position_y_text_icon = -32768;

	Request("$textnumber", Enter);
	WaitText("$textnumber", null);

	Fade("$textnumber", �b��, 0, null, true);
	Request("$textnumber", UnLock);
	Request("$textnumber", Disused);
}

//��Fade�n
//=============================================================================//
.//Fade�n
//=============================================================================//

//�����x0����X�^�[�g����uCreateTexture�v�ł�
..CreateTextureEX
function CreateTextureEX("�i�b�g��", �`��D��x, �w���W, �x���W, "�C���[�W�f�[�^")
{
	CreateTexture("�i�b�g��", �`��D��x, �w���W, �x���W, "�C���[�W�f�[�^");
	Fade("�i�b�g��", 0, 0, null, true);
}


//�w�i��p
..CreateBG
function CreateBG(�`��D��x, ���v����, �w���W, �x���W, "�C���[�W�f�[�^")
{
	if($BackGround=="back01"){$BackGround="back02";}
	else{$BackGround="back01";}

	CreateTexture("$BackGround", �`��D��x, �w���W, �x���W, "�C���[�W�f�[�^");
	Fade("$BackGround", 0, 0, null, true);
	Request("$BackGround", Lock);
	Fade("$BackGround", ���v����, 1000, null, true);

	Delete("back*");
	Request("$BackGround", UnLock);
}

//�w�i��p
..CreateBG2
function CreateBG2(�`��D��x, ���v����, �w���W, �x���W, "�C���[�W�f�[�^")
{
	if($BackGround=="back01"){$BackGround="back02";}
	else{$BackGround="back01";}

	CreateTexture("$BackGround", �`��D��x, �w���W, �x���W, "�C���[�W�f�[�^");
	Request("$BackGround", Lock);

	Fade("back*", ���v����, 0, null, false);
	Fade("$BackGround", 0, 1000, null, false);
	Wait(���v����);

	Delete("back*");
	Request("$BackGround", UnLock);
}




..FadeDelete
function FadeDelete("�i�b�g��", ���v����, �҂�)
{
	Fade("�i�b�g��", ���v����, 0, null, �҂�);
	Request("�i�b�g��", Disused);
}

..PrintBG
function PrintBG(�`��D��x)
{
//	CreateBG(�`��D��x, 0, 0, 0, "SCREEN");
	if($BackGround=="back01"){$BackGround="back02";}
	else{$BackGround="back01";}
	CreateTexture("$BackGround", �`��D��x, 0, 0, "SCREEN");
	Request("$BackGround", Lock);

	Delete("*");
	/*stand�ϐ�������*/
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
function ClearAll(���v����)
{
	CreateColor("��", 30000, 0, 0, 800, 600, "BLACK");
	Fade("��", 0, 0, null, true);
	Fade("��", ���v����, 1000, null, true);
	Delete("*");
	/*stand�ϐ�������*/
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
function FadeCross("$�i�b�g���P","$�i�b�g���Q", ���v����)
{
	$�i�b�g�� = "$�i�b�g���P" + "$�i�b�g���Q";
	$�i�b�g���A�X�^ = "$�i�b�g���P" + "*";

	Fade("$�i�b�g��", ���v����, 1000, null, true);
	Request("$�i�b�g��", Lock);
	Delete("$�i�b�g���A�X�^");
	Request("$�i�b�g��", UnLock);
}



..MoveEX
function MoveEX("�i�b�g��", ���v����, $�w���W, $�x���W, �e���|, �҂�)
{
	$�w���W�v�� = - $�w���W;
	$�x���W�v�� = - $�x���W;

	$�w���W�}�C�i�X = "@" + $�w���W�v��;
	$�x���W�}�C�i�X = "@" + $�x���W�v��;

	$�w���W�v���X = "@" + $�w���W;
	$�x���W�v���X = "@" + $�x���W;

	Move("�i�b�g��", 0, $�w���W�}�C�i�X, $�x���W�}�C�i�X, null, true);
	Move("�i�b�g��", ���v����, $�w���W�v���X, $�x���W�v���X, �e���|, �҂�);
}

..DeleteAll
function DeleteAll()
{
	Delete("*");
	/*stand�ϐ�������*/
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

//��cube�n
//=============================================================================//
.//cube�n
//=============================================================================//

..CubeRoom
function CubeRoom("�i�b�g��", �`��D��x, ����p�x)
{
	$�t�H���_�� = #SYSTEM_max_texture_size;
//	$�t�H���_�� = 2048;

	$�O�ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_front" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_back" + ".jpg";
	$�E�ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_right" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_left" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_top" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("�i�b�g��", �`��D��x, "$�O�ʉ摜", "$��ʉ摜", "$�E�ʉ摜", "$���ʉ摜", "$��ʉ摜", "$���ʉ摜");
	SetFov("�L���[�u�P", ����p�x);
	Fade("�i�b�g��", 0, 0, null, true);
}


..CubeRoom2
function CubeRoom2("�i�b�g��", �`��D��x, ����p�x)
{
	$�t�H���_�� = #SYSTEM_max_texture_size;
//	$�t�H���_�� = 2048;

	$�O�ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_front" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_back" + ".jpg";
	$�E�ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_right" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_left" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_top" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("�i�b�g��", �`��D��x, "$�O�ʉ摜", "$��ʉ摜", "$�E�ʉ摜", "$���ʉ摜", "$��ʉ摜", "$���ʉ摜");
	SetFov("�L���[�u�P", ����p�x);
	Fade("�i�b�g��", 0, 0, null, true);
}

..CubeRoom3
function CubeRoom3("�i�b�g��", �`��D��x, ����p�x)
{
	$�t�H���_�� = #SYSTEM_max_texture_size;
//	$�t�H���_�� = 2048;

	$�O�ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_front" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_back" + ".jpg";
	$�E�ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_right" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_left" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_top" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o���O_��/" + "$�t�H���_��" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("�i�b�g��", �`��D��x, "$�O�ʉ摜", "$��ʉ摜", "$�E�ʉ摜", "$���ʉ摜", "$��ʉ摜", "$���ʉ摜");
	SetFov("�L���[�u�P", ����p�x);
	Fade("�i�b�g��", 0, 0, null, true);
}

..CubeRoom4
function CubeRoom4("�i�b�g��", �`��D��x, ����p�x)
{
	$�t�H���_�� = #SYSTEM_max_texture_size;
//	$�t�H���_�� = 2048;

	$�O�ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_front" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_back" + ".jpg";
	$�E�ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_right" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_left" + ".jpg";
	$��ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_top" + ".jpg";
	$���ʉ摜 = "cg/rv/�����o����_��/" + "$�t�H���_��" + "/" + "rv_cube_bottom" + ".jpg";

	CreateCube("�i�b�g��", �`��D��x, "$�O�ʉ摜", "$��ʉ摜", "$�E�ʉ摜", "$���ʉ摜", "$��ʉ摜", "$���ʉ摜");
	SetFov("�L���[�u�P", ����p�x);
	Fade("�i�b�g��", 0, 0, null, true);
}


//���ϑz�C���E�A�E�g�}�N��
//=============================================================================//
.//�ϑzin�Eout
//=============================================================================//

..DelusionIn
function DelusionIn()
{
	Move("�����Y�P", 0, @0, @0, null, true);
	Request("�����Y�v���Z�X�P", UnLock);
	Delete("�����Y�v���Z�X�P");
	Request("�����Y�P", UnLock);
	Delete("�����Y�P");

	CreateColor("���P", 22000, 0, 0, 800, 600, "White");
	Fade("���P", 0, 0, null, false);

//��ʃG�t�F�N�g//�ϑz�h�m�G�t�F�N�g
	CreateMovie("�ϑzin", 21000, 0, 0, false, false, "dx/mvin.ngs");
	Request("�ϑzin", Play);

//�r�d//�ϑz�h�m
	CreateSE("SE100","SE_�[��_�ϑzIN");
	SoundPlay("SE100", 0, 1000, false);
	WaitAction("�ϑzin", null);

	Fade("���P", 300, 1000, null, true);
	Request("���P", Lock);
	Delete("�ϑzin");

		$SYSTEM_effect_lens_curvature = 8000;
		$SYSTEM_effect_lens_distance = 10;
		CreateEffect("�����Y�P", 2100, -200, -300, 1200, 1200, "Lens");
		SetAlias("�����Y�P", "�����Y�P");
		CreateProcess("�����Y�v���Z�X�P", 1000, 0, 0, "LensMove");

	Request("�����Y�P", Lock);
	Request("�����Y�v���Z�X�P", Lock);
	Wait(500);
	Request("�����Y�v���Z�X�P", Start);
}

..DelusionIn2
function DelusionIn2()
{
	Request("���P", UnLock);
	Fade("���P", 1000, 0, null, true);
	Delete("���P");
}


..DelusionFakeIn
function DelusionFakeIn()
{
	CreateColor("���P", 22000, 0, 0, 800, 600, "White");
	Request("���P", Lock);
	Fade("���P", 0, 0, null, false);

//��ʃG�t�F�N�g//�ϑz�h�m�G�t�F�N�g
	CreateMovie("�ϑzin", 21000, 0, 0, false, false, "dx/mvin.ngs");
	Request("�ϑzin", Lock);
	Request("�ϑzin", Play);


//�r�d//�ϑz�h�m
	CreateSE("SE01","SE_�[��_�ϑzIN");
	SoundPlay("SE01", 0, 1000, false);
	WaitAction("SE01", null);

	Fade("���P", 300, 1000, null, true);
	Request("�ϑzin", UnLock);
	Delete("�ϑzin");
}

..DelusionFakeIn2
function DelusionFakeIn2()
{
	Request("���P", UnLock);
	Fade("���P", 1000, 0, null, true);
	Delete("���P");
}



..DelusionOut
function DelusionOut()
{

	Request("�����Y�P", UnLock);
	Request("�����Y�v���Z�X�P", UnLock);

	CreateColor("���P", 22000, 0, 0, 800, 600, "Black");
	Request("���P", Lock);
	Fade("���P", 0, 0, null, false);

	Move("�����Y�P", 0, @0, @0, null, true);
	Delete("�����Y�v���Z�X�P");
	Delete("�����Y�P");

//��ʃG�t�F�N�g//�ϑz�n�t�s�G�t�F�N�g
	CreateMovie("�ϑzout", 21000, 0, 0, false, false, "dx/mvout.ngs");
	Request("�ϑzout", Play);

//�r�d//�ϑz�n�t�s
	CreateSE("SE01","SE_�[��_�ϑzOUT");
	SoundPlay("SE01", 0, 1000, false);
	WaitAction("�ϑzout", null);

	Fade("���P", 300, 1000, null, true);
	Delete("�ϑzout");
}

..DelusionOut2
function DelusionOut2()
{
	Wait(2000);

	Request("���P", UnLock);
	Fade("���P", 1000, 0, null, true);
	Delete("���P");

}


//���C���^�[�~�b�V����
//=============================================================================//
.//�C���^�[�~�b�V����IN
//=============================================================================//

..IntermissionIn
function IntermissionIn()
{
	CreateColor("�C���^�[�~�b�V�����F", 25001, 0, 0, 800, 600, "black");
	Fade("�C���^�[�~�b�V�����F", 0, 0, null, false);
	Request("�C���^�[�~�b�V�����F", Lock);

	CreateMovie("�C���^�[�~�b�V�������[�r�[�P", 25000, 0, 0, false, true, "dx/mv�A�C�L���b�`01.ngs");
	Request("�C���^�[�~�b�V�������[�r�[�P", Lock);
	WaitPlay("�C���^�[�~�b�V�������[�r�[�P", null);

	Fade("�C���^�[�~�b�V�����F", 300, 1000, null, true);
}

..IntermissionIn2
function IntermissionIn2()
{

	Wait(500);

	CreateMovie("�C���^�[�~�b�V�������[�r�[�Q", 25002, 0, 0, false, true, "dx/mv�A�C�L���b�`02.ngs");

	Wait(400);

	Request("�C���^�[�~�b�V�����F", UnLock);
	Request("�C���^�[�~�b�V�������[�r�[�P", UnLock);
	FadeDelete("�C���^�[�~�b�V�����F", 100, false);
	FadeDelete("�C���^�[�~�b�V�������[�r�[�P", 100, true);

	WaitPlay("�C���^�[�~�b�V�������[�r�[�Q", null);

	Delete("�C���^�[�~�b�V�������[�r�[�Q");


}











//�����֌W�̃}�N���R�}���h
//=============================================================================//
.//���֌W
//=============================================================================//

// ��`
function CreateBGM("�i�b�g��","$���y�f�[�^")
{
	$�ꏊ�w�� = "sound/bgm/" + "$���y�f�[�^";

	CreateSound("�i�b�g��", BGM, "$�ꏊ�w��");
	SetVolume("�i�b�g��", 0, 0, NULL);
	SetAlias("�i�b�g��", "�i�b�g��");
}

function CreateBGM2("�i�b�g��","$���y�f�[�^")
{
	$�ꏊ�w�� = "sound/bgm/" + "$���y�f�[�^";

	CreateSound("�i�b�g��", SE, "$�ꏊ�w��");
	SetVolume("�i�b�g��", 0, 0, NULL);
	SetAlias("�i�b�g��", "�i�b�g��");
}

function CreateBGM3("�i�b�g��","$���y�f�[�^",�J�n�~���b,�I���~���b)
{
	$�ꏊ�w�� = "sound/bgm/" + "$���y�f�[�^";

	CreateSound("�i�b�g��", BGM, "$�ꏊ�w��");
	SetVolume("�i�b�g��", 0, 0, NULL);
	SetAlias("�i�b�g��", "�i�b�g��");
	SetLoopPoint("�i�b�g��",�J�n�~���b,�I���~���b);
}

function CreateSE("�i�b�g��","$���y�f�[�^")
{
	$�ꏊ�w�� = "sound/se/" + "$���y�f�[�^";

	CreateSound("�i�b�g��", SE, "$�ꏊ�w��");
	SetVolume("�i�b�g��", 0, 0, NULL);
	SetAlias("�i�b�g��", "�i�b�g��");
}

function CreateVOICE("�i�b�g��","$���y�f�[�^")
{
	$�ꏊ�w�� = "voice/" + "$���y�f�[�^";

	CreateSound("�i�b�g��", VOICE, "$�ꏊ�w��");
	SetVolume("�i�b�g��", 0, 0, NULL);
	SetAlias("�i�b�g��", "�i�b�g��");
}

function CreateVOICE2("�i�b�g��","$���y�f�[�^")
{
	$�ꏊ�w�� = "voice/" + "$���y�f�[�^";

	CreateSound("�i�b�g��", SE, "$�ꏊ�w��");
	SetVolume("�i�b�g��", 0, 0, NULL);
	SetAlias("�i�b�g��", "�i�b�g��");
}

// �Đ�
function MusicStart("�i�b�g��",�b��,�{���E��,�Đ�����,�Đ��X�s�[�h,�e���|,���[�v�ݒ�)
{
	Request("�i�b�g��", "Play");

	SetFrequency("�i�b�g��", 0, �Đ��X�s�[�h, NULL);
	SetPan("�i�b�g��", 0, �Đ�����, NULL);
	SetLoop("�i�b�g��", ���[�v�ݒ�);
	SetVolume("�i�b�g��", �b��, �{���E��, �e���|);
	Request("�i�b�g��", Disused);
}

function SoundPlay("�i�b�g��",�b��,�{���E��,���[�v�ݒ�)
{
	Request("�i�b�g��", Play);
	SetLoop("�i�b�g��", ���[�v�ݒ�);
	SetVolume("�i�b�g��", �b��, �{���E��, null);
	Request("�i�b�g��", Disused);
}

function SoundStop("�i�b�g��")
{
	SetVolume("�i�b�g��", 3000, 0, NULL);
}

function SoundStop2("�i�b�g��")
{
	Request("�i�b�g��", Stop);
	Delete("�i�b�g��");
}

//��BGM��Z�߂Ē�`
//=============================================================================//
.//BGM��`
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

	CreateBGM("CH_INS_FES_�A�J�x��","CH_INS_FES_�A�J�x��");
	CreateBGM("CH_INS_FES_���C��","CH_INS_FES_���C��");
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

	Request("@CH_INS_FES_�A�J�x��",Lock);
	Request("@CH_INS_FES_���C��",Lock);
	Request("@CH_OP",Lock);
	Request("@CH_OP_Instrumental",Lock);
	Request("@CH_ED_A",Lock);
	Request("@CH_ED_B",Lock);
	Request("@CH_ED_C",Lock);

}




function DebugGallery()
{
	#ev001_01_1_INT01�߂Â����[_a=true;
	#ev013_01_1_�񖤏΂���UP_a=true;
	#ev013_02_1_�񖤏΂���UP_a=true;
	#ev013_03_1_�񖤏΂���UP_a=true;
	#ev005_01_3_�Y�\�t��_a=true;
	#ev006_01_3_�O���摜_a=true;
	#ev007_01_3_�\���ˍY��_a=true;
	#ev007_02_6_�\���ˍY��_a=true;
	#ev009_01_4_�r�͂݃~�C��_a=true;
	#ev010_01_4_�r�͂ݗ��[_a=true;
	#ev012_01_1_�����������_a=true;
	#ev008_01_4_INT02���₹�̂�_a=true;
	#ev801_01_1_���C���K_a=true;
	#ev801_02_3_���C���K_a=true;
	#ev015_01_1_���C�ϑz�G��_a=true;
	#ev015_02_1_���C�ϑz�G��_a=true;
	#ev802_01_1_���C�R�[����_a=true;
	#ev016_01_1_���C�g�іG_a=true;
	#ev803_01_3_�D������_a=true;
	#ev019_02_3_�D���ϑz_a=true;
	#ev019_01_3_�D���ϑz_a=true;
	#ev017_01_2_����_a=true;
	#ev017_02_2_����_a=true;
	#ev057_01_1_�񖤎q������_a=true;
	#ev050_01_1_��f���w����_a=true;
	#ev022_01_1_�����L�X�ϑz_a=true;
	#bg119_01_3_�Ď��J�����f��_a=true;
	#ev037_01_3_INT13�D�������Ń��[������_a=true;
	#ev023_01_3_INT06�D���Q�]����_a=true;
	#ev024_01_3_���₹���C�u�V�[��_a=true;
	#ev025_01_3_���₹�E���ϑz_a=true;
	#ev110_01_3_�Z�i��_a=true;
	#ev026_01_2_���C�n���o�[�K�[_a=true;
	#ev026_02_2_���C�n���o�[�K�[_a=true;
	#ev027_01_3_�����낵�Z�i_a=true;
	#ev028_01_0_���₹���C�u�O�Z�؂�_a=true;
	#ev029_01_2_���[�\�t�@���|��_a=true;
	#ev030_01_2_���C�o���O��_a=true;
	#bg136_01_1_��u�h�o���[��_a=true;
	#ev031_01_0_���]�Z_a=true;
	#ev052_01_3_���R�Ԉ֎q_a=true;
	#ev052_02_3_���R�Ԉ֎q_a=true;
	#ev032_01_3_���[��������_a=true;
	#ev033_01_1_INT12�Z�i��b�ӎ��W��_a=true;
	#ev034_01_6_���[�Ə��R�̏o�_a=true;
	#ev035_01_0_���R�P��_a=true;
	#ev036_01_2_�Z�i������_a=true;
	#ev038_01_3_�D��ROOM37����_a=true;
	#ev040_01_3_���₹�f�B�\�[�h�o��_a=true;
	#ev040_02_3_���₹�f�B�\�[�h�o��_a=true;
	#ev039_01_3_���₹������_a=true;
	#ev039_02_3_���₹������_a=true;
	#ev054_01_3_�Y����l_a=true;
	#ev042_01_2_���[�ɉ�������_a=true;
	#ev042_02_2_���[�ɉ�������_a=true;
	#ev043_01_2_���[CD�݂��Ă��ꂽ��_a=true;
	#ev044_01_2_���[����Y�V���c_a=true;
	#ev044_02_2_���[����Y�V���c_a=true;
	#ev044_03_2_���[����Y�V���c_a=true;
	#ev045_01_3_INT16�Z�i�@�B�j��_a=true;
	#ev055_01_1_�Y���ƒT���b_a=true;
	#ev056_01_1_�D���ւ��肱�ݓd�b_a=true;
	#ev056_02_1_�D���ւ��肱�ݓd�b_a=true;
	#ev064_01_1_���₹��э~��悤��_a=true;
	#ev065_01_1_���₹���g_a=true;
	#ev065_02_1_���₹���g_a=true;
	#ev066_01_1_���₹�Ԓd����_a=true;
	#ev057_01_3_Q�|FrontTV���j�^�[_a=true;
	#ev067_01_6_�����莵�C_a=true;
	#ev056_01_1_9�Ύ��C�񖤂ɐH��_a=true;
	#ev070_01_2_���R�Ɨ��[in�a�@_a=true;
	#ev070_02_2_���R�Ɨ��[in�a�@_a=true;
	#ev071_01_1_���f�B�\�[�h����_a=true;
	#ev072_01_1_���Ɣg����Roft�O_a=true;
	#ev062_01_1_�v���N��_a=true;
	#ev062_02_1_�v���N��_a=true;
	#ev077_01_3_��C���В���_a=true;
	#ev068_01_1_���C�L����p_a=true;
	#ev069_01_1_���C�L����p����������_a=true;
	#ev076_01_4_�������ܕ���_a=true;
	#ev059_01_6_���₹����_a=true;
	#ev078_01_3_�t���i�[�X�߂���_a=true;
	#ev078_02_3_�t���i�[�X�߂���_a=true;
	#ev080_01_1_���[���C�n�C�^�b�`_a=true;
	#ev060_01_3_�Z�i�R���e�i�o��_a=true;
	#ev060_02_3_�Z�i�R���e�i�o��_a=true;
	#ev079_01_3_���[�Z�i�Ό�_a=true;
	#ev073_01_1_�Ԏq�������_a=true;
	#ev063_01_1_�}�W�b�N�~���[�g����_a=true;
	#ev063_02_1_�}�W�b�N�~���[�g����_a=true;
	#ev082_01_3_���C�]���r_a=true;
	#ev081_01_3_����_a=true;
	#ev083_01_3_�D���f�B�\�[�h_a=true;
	#ev084_01_3_�m�AII_a=true;
	#ev085_01_3_���C�f�B�\�[�h_a=true;
	#ev086_01_6_���[�X�|�b�g���C�g�G����_a=true;
	#ev087_01_3_�񖤃f�B�\�[�h_a=true;
	#ev087_02_3_�񖤃f�B�\�[�h_a=true;
	#ev088_01_4_�t���~������_a=true;
	#ev088_02_4_�t���~������_a=true;
	#ev089_01_1_���₹���I��_a=true;
	#ev090_01_1_�D�����I��_a=true;
	#ev091_01_1_���C�Ə��R_a=true;
	#ev092_01_1_�񖤐U��Ԃ�_a=true;
	#ev092_02_1_�񖤐U��Ԃ�_a=true;
	#ev092_03_1_�񖤐U��Ԃ�_a=true;
	#ev093_01_1_�Z�i���E��_a=true;
	#ev093_01_1_�Z�i���E��_b=true;
	#ev094_01_1_�Z�i����͂�_a=true;
	#ev095_01_1_�񖤃_�[�c�t�]_a=true;
	#ev096_01_1_������Q_a=true;
	#ev097_01_1_���[�͂��_a=true;
	#ev107_01_1_���[�F��_a=true;
	#ev106_01_1_������_a=true;
	#ev099_01_1_���[���C�v_a=true;
	#ev108_02_1_���h��_a=true;
	#ev100_06_1_�U�l�F��_a=true;
	#ev111_01_6_��C�����X�g_a=true;
	#ev105_01_1_�񖤌��Ə�����_a=true;
	#ev105_02_1_�񖤌��Ə�����_a=true;
	#ev102_01_1_�`�G���h1_a=true;
	#ev103_01_1_�`�G���h2_a=true;
	#ev999_01_1_���߂łƂ�=true;
	#bg006_01_1_�R���e�i�O��_a=true;
	#bg026_02_3_�񖤕���_a=true;
	#ev014_01_1_�X�v�[_a=true;
	#bg155_01_1_Ir2_a=true;
	#ev047_01_1_����t�����̌���ʐ^_a=true;
	#ev048_01_1_����t���O���G�A�b�v_a=true;
	#ev053_01_1_�W�c�_�C�u����ʐ^_a=true;
	#bg142_01_3_�~���E�c�x�W�c�_�C�u_a=true;
	#ev046_01_1_�D�P�j����ʐ^_a=true;
	#ev049_01_1_���@���p�C������ʐ^_a=true;
	#bg213_01_6_�j���[�XDQN�p�Y��_a=true;
	#ev061_01_2_�j���[�W�F�l�Ɛl�ߕ�TV_a=true;
	#ev058_01_3_Q�|Front�E������L���X�^�[_a=true;
	#ev051_01_3_���|��_a=true;
	#bg011_01_1_������UP_a=true;
	#bg040_01_2_�D���J�o��_a=true;
	#bg041_01_2_�D���J�o���Ԃ��܂�_a=true;
	#bg127_01_2_�M�������Q���J�G����_a=true;
	#bg181_01_3_�̂Ă�ꂽ�M������_a=true;
	#bg147_01_2_�f�B�\�[�h�z��_a=true;
	#bg073_01_1_TownVanguard�X��_a=true;
	#bg012_01_1_�j���[�X�T�C�g_a=true;
	#bg012_02_1_�j���[�X�T�C�g_a=true;
	#bg117_01_3_PC��ʃj���[�W�F�l_a=true;
	#bg141_01_3_PC���del��������_a=true;
	#bg183_01_3_PC���ZACO�]DQN_a=true;
	#bg202_01_1_��O���[�v�������T�C�g_a=true;
	#bg125_01_3_PC��ʃt�@���^�Y������_a=true;
	#bg203_01_1_�Q���J�G����U�ʔ̃T�C�g_a=true;
	#bg160_03_3_�_���{�[����_a=true;
	#bg004_01_1_�앶�p�����̖�_a=true;
	#bg022_01_0_�����̖�_a=true;
	#bg171_01_3_�J���e���̖�_a=true;
	#bg144_01_1_���k�蒠���̖�_a=true;
	#bg159_02_1_PC��ʃ��[���\�t�g_a=true;
	#bg120_01_3_PC��ʂ��̖�_a=true;
	#bg186_02_1_���Ȃт��^�I��_a=true;
	#bg180_01_1_�A�C�X���̖�_a=true;
	#bg128_02_3_�l�b�g�I�[�N�V����_a=true;
	#bg158_03_1_�j���[�X�n�k_a=true;
	#bg001_01_1_����a�J_a=true;
	#bg027_03_5_������_a=true;
	#bg009_03_5_107_a=true;
	#bg182_01_3_��`���̃N�}�̊G_a=true;
	#bg185_01_1_���₹����_a=true;
	#ev074_01_1_���]����in���j�^�[_a=true;
	#bg205_01_3_���₹�f�B�\�[�h���A���u�[�g_a=true;
	#bg211_01_5_���F���o���O��_a=true;
	#bg197_01_3_�a�J�w�����ƃv���l�^���E��_a=true;
	#bg200_01_6_�m�AII�̂���h�[����_a=true;
}


