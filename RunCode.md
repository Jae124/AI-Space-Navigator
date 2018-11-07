# How to use the Space Navigator

### Step-by-Step Process

1. Open the Unity project

Under "Academy," set "Brain type" to "External." This is the part where Red circle is.
![Alt text](/project_images/1.png?raw=true "Title")

2. Open "Anaconda Prompt"

This is the screen you will see when first opening the prompt. After opening the command line prompt, use the cd command to move to the file folder where the Unity project is in.
Now, we could start the machine learning.
When running, input "Python python\learn.py –run-id="learning model name" –-train". I used "Python python\learn.py –run-id=univtrain –-train".

You will probably encounter a UnicodeEncodeError. In this case, set the unicode to "chcp 65001". Now, there's no more error in running the machine learning code.
After this input, you will see a screen similar to this one.

3. Open the Unity Project again

After you see the screen above, press the play button in the center top of the Unity window. The training looks like this.

The training is finished with 50000 steps, and the model is saved.

4. Take a look at the trained data

In Unity, go the "Project", "Assets", and then "learning data" to view the learning data. If you turn the data inspector on, you will be able to see 4 lines of information. The first line is the starting time, second line is success or failure, thrid is time in minutes, and fourth is time in seconds.
