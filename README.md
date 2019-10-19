# EndGameVisualizer

This is an open source project that was implemented to visualize the Artificial Intelligence project (CSEN 901).

Anyone can download this repo and visualize his Algorithm in the project easily.

 
## Installation Steps

### Step 1:
Extract Visualizer.zip in any folder other than the repo (this is the build folder)

### Step 2:

Import these libraries in your EndGameProblem.java Class

```java
import java.awt.datatransfer.Clipboard;
import java.awt.datatransfer.StringSelection;
import java.awt.Toolkit;
```


### Step 3:
In your EndGameProblem.java Class, add this snippet of code to the part where you print your Path.

```java
 //Visualizing
    	 String copiedString = GridString + "_" + solution;
    	 StringSelection stringSelection = new StringSelection(copiedString);
    	 Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();
    	 clipboard.setContents(stringSelection, null);
 ////////////
```

In this part you will formulate a string that will be sent to the Visualizer using the Clipboard

The String format is:
```java
String copiedString = "The_grid_string" + "_" + "Your_output_path"
```

There are no restrictions on The grid string as it is provided in the test cases.

As for Your_output_path the format should be as follows:

```java
String solution = "Kill , Right , Left"
```

The only restriction is to start the string with an action and let all the other actions separated by ',' 

And here are some examples:
```java
String solution1 = "DOWN(1) , RIGHT(1) , DOWN(4) , KILL(10) , DOWN(10)"

String solution2 = "Down , Right , Left , Kill , Down"

String solution3 = "down(1) , right(1) , down(4) , kill(10) , down(10)"

String solution4 = "down-(dmg10) , right-(dmg10) , down-(dmg10)"
```



## Step 4:
Import these libraries in your Main.java Class 
```java
import java.io.File;
import java.io.IOException;

```

## Step 5:
Add this snippet of code in your Main.java Class after calling your Solve() function
```java
String path = "path_To_exeFile"+"\\EndGameVisualization.exe";
		File file = new File(path);
		if (! file.exists()) {
		   throw new IllegalArgumentException("The file " + path + " does not exist");
		}
		try {
			Process p = Runtime.getRuntime().exec(file.getAbsolutePath());
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
```
path should be the path to the .exe file that is found in the build folder in this repo 


## Step 6: 
Run your Main.java Class and the visualized should run immediately.

Enjoy! 
## Contributing
All changes are welcomed, your code must be reviewed before merging with the master.
