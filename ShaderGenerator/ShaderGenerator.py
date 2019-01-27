print "hello world"

myFile = open("output.txt", "w")
xSize = 1.0/10.0
ySize = 1.0/10.0


def write1(i, j):
    myFile.write("\nfloat4 planetsx" + str(i) + "y" + str(j) + "[100];")


def write2(i, j):
    myFile.write("\nint numberOfPlanetsx" + str(i) + "y" + str(j) + ";")


def write3(i, j):
    myFile.write("\nelse if (fragInput.screenPos.x >= "+str(i*xSize)+" && fragInput.screenPos.x <= "+str((i+1)*xSize)+" && fragInput.screenPos.y >= "+str(j*ySize)+" && fragInput.screenPos.y <= "+str((j+1)*ySize)+")")
    myFile.write("\n{")
    myFile.write("\n\tcolor = fragHelper(fragInput, planetsx"+str(x)+"y"+str(y)+", numberOfPlanetsx"+str(x)+"y"+str(y)+");")
    myFile.write("\n}")

for x in range(0, 10):
    for y in range(0, 10):
        write3(x, y)

myFile.close()
