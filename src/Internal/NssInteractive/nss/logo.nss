#include "nss/function.nss"
#include "nss/function_select.nss"


//=============================================================================//
//������������������������������������������������������������������������������
.//���V�Y�~���o�[�W����1.00
//������������������������������������������������������������������������������
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

		//���F�t���O������
		InitTrigger();

		//���F��`
		SystemInit();

		if($GameContiune == 1)
		{
			#play_speed_plus = 3;
			$GameContiune = 0;
			Delete("*");
			call_chapter nss/boot_�J�n�X�N���v�g.nss;
		}

		//���F�V�X�e���ϐ��n�̃N���A
		#play_speed_plus += 0;
		if(#play_speed_plus != 0)
		{
			/*
				����N�����ł͂Ȃ��Ƃ��́A�v���C���x���o�b�N�A�b�v
			*/
			#play_speed_plus = #SYSTEM_play_speed;
		}
		else
		{
			/*
				����N�����́A�v���C���x�̃o�b�N�A�b�v���R�ɌŒ�
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

		//���^�C�g���ŉ���I���������̃��Z�b�g
		$TitleSelect = 0;

		CreateColor("�^�C�g���J���[", 1000, 0, 0, 800, 600, "BLACK");
		Fade("�^�C�g���J���[", 0, 0, null, true);
		Fade("@box11",0,0,null,false);
		Fade("@box12",0,0,null,true);

		//���F���S�ƃG�L�X�g��BGM����
		TitleLogo();
		//���F�^�C�g����`
		
		while (1)
		{
			Wait(1);
		}

	}
	//->end while

}

//============================================================================//
..//�����S���聡
//============================================================================//
function TitleLogo()
{
//���F��x�ς���Q�[�����͏o���Ȃ��悤�ɂ��锻��

	$Logo += 0;

	if($Logo == 0)
	{
		CreateTexture("�^�C�g���j�g���v���X", 100, 0, 0, "cg/sys/title/boot_nitroplus.jpg");
		CreateTexture("�^�C�g��5GK", 100, 0, 0, "cg/sys/title/boot_5gk.jpg");
		CreateTexture("�^�C�g�����ӎ���", 100, 0, 0, "cg/sys/title/���ӎ���.jpg");
		Fade("�^�C�g��*", 0, 0, null, true);

		Fade("�^�C�g���j�g���v���X", 800, 1000, null, true);
		WaitKey(3000);
		Fade("�^�C�g���j�g���v���X", 800, 0, null, true);

		CreateSE("�^�C�g���O�T�E���h�P","SE_����_PC_�n�[�h�f�B�X�N_Loop");
		SoundPlay("�^�C�g���O�T�E���h�P",0,1000,true);

		Fade("�^�C�g�����ӎ���", 800, 1000, null, true);
		WaitKey(10000);

		CreateSE("�^�C�g���O�T�E���h�Q","SE_����_PC_�}�E�X�N���b�N");
		SoundPlay("�^�C�g���O�T�E���h�Q",0,1000,false);
		SetVolume("�^�C�g���O�T�E���h�P", 100, 0, NULL);

		Fade("�^�C�g�����ӎ���", 800, 0, null, true);

		Delete("�^�C�g���j�g���v���X");
		Delete("�^�C�g��5GK");
		Delete("�^�C�g�����ӎ���");
	}


	if($�G�L�X�g���^�C�g�� == 1)
	{
		if($�G�L�X�g���a�f�l != "@CH01")
		{
			//���a�f�l�v���C
			SetVolume("@CH*", 1000, 0, NULL);
			SoundPlay("@CH01",3000,1000,true);
		}
		$�G�L�X�g���^�C�g�� = 0;
	}
	else
	{
		//���a�f�l�v���C
		SoundPlay("@CH01",0,1000,true);
	}


}
//============================================================================//






