// there needs to be something before chapter main
// (whitespace, comment, whatever)
// or the game CTDs
chapter main
{
	TitleLogo();
	CreateTexture("frog", 100, 0, 0, "bg127.jpg");
	Fade("frog", 0, 0, null, true);
	Fade("frog", 500, 1000, null, true);
	CreateText("hello", 200, 50, 50, 700, 500, "I'm all out of memes");
	Request("hello", PushText);
	Fade("hello", 0, 0, null, true);
	Fade("hello", 500, 1000, null, true);
	// the loop works
	// I just don't want to deal with optimising the run() loop for busy-waits
	// uncomment it if you don't believe me
	//while(1)
	//{
	//	// game hangs without a loop that does *something*
	//	Wait(1);
	//}
}

function TitleLogo()
{
		CreateTexture("タイトルニトロプラス", 100, 0, 0, "cg/sys/title/nitroplus.jpg");
		CreateTexture("タイトル5GK", 100, 0, 0, "cg/sys/title/5gk.jpg");
		
		//Fade("タイトル*", 0, 0, null, true);
		Fade("タイトルニトロプラス", 0, 0, null, true);
		Fade("タイトル5GK", 0, 0, null, true);

		Fade("タイトルニトロプラス", 800, 1000, null, true);
		WaitKey(3000);
		Fade("タイトルニトロプラス", 800, 0, null, true);
		Fade("タイトル5GK", 800, 1000, null, true);
		WaitKey(3000);
		Fade("タイトル5GK", 800, 0, null, true);
		Wait(500);

		Delete("タイトルニトロプラス");
		Delete("タイトル5GK");
}