# ChromeDinoAi
An attempt at creating an AI for the real dino game that runs in chrome when you have no internet connection


I didn't take the time to train this to see how good it could actually get at the game. It is curently coded for a two monitor setup, where one monitor has the code running and the other monitor is running the game. 

A simpler implementation would be to create the dino game yourself then just be able to access the object locations easily, instead of sending in difference pixel values into the neural net. 

If you try to run this yourself you will have to change the pixels being sent in, as it is hardcoded for my monitors resolution. 

I ended up having to garbage collect more often than normal because the amount of data I was saving I kept running out of RAM.
