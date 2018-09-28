# AI Space Navigator

The Space Navigator is a Unity simulation that finds the optimal path from Earth to Mars, depending on their relative positions.

The code provided creates a solar system consisting of the Sun, Earth, Moon, and Mars -- but its relative size is not the same as our solar system. The spaceship is also assumed to be already in orbit around the Earth, and to be traveling at a constant speed towards Mars. The program considers the effects of gravity and potential collision between objects, but not energy in orbits.

The optimal path is found for different relative positions between Earth and Mars, and the program finds the path involving the shortest time. However, the result is slightly different from the Hohmann transfer, as the program does not consider energy levels for different orbits. For instance, if a straight line is the fastest way from Earth to Mars, the spaceship will travel in a straight line, although it is theoretically highly improbable that the spaceship will move in such a manner.

If there’s any bug or idea for improvements, please feel free to add an issue, or send me an email at jwkim000124@gmail.com. Thank you.
