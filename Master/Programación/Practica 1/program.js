// Imagen del avi�n
var g_imgPlane 	= null;

// Variables con la posici�n del avi�n.
var g_posX = 400;
var g_posY = 300;

// **************************************************************************************
// Initialization.
// **************************************************************************************
function Init()
{
	g_imgPlane = getImage("plane");
}

// **************************************************************************************
// Game loop.
// **************************************************************************************
function gameLoop()
{
	drawImage(g_imgPlane, g_posX, g_posY);
}