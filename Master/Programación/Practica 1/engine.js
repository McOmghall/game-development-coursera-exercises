// **************************************************************************************
// Global variables
// **************************************************************************************
var g_canvas 			= null;
var g_ctx		 		 	= null;
var g_screenWidth = 0;
var g_screenHeight= 0;

// **************************************************************************************
// Initialization.
// **************************************************************************************
function InitEngine()
{
	g_canvas 				= document.getElementById("myCanvas");
	g_ctx						= g_canvas.getContext("2d");
	g_screenWidth 	= g_canvas.width;
	g_screenHeight 	= g_canvas.height;
	
	g_canvas.addEventListener('click', 	onEngineClick);
	g_canvas.addEventListener('keydown',onEngineKeyDown);
	g_canvas.addEventListener('keyup', 	onEngineKeyUp);
	
	g_ctx.fillStyle = '#99D9EA'; 
	g_ctx.font      = "bold 16px Arial";
	
	g_canvas.focus();
	
	Init();
	
	setInterval(engineLoop, 1000/60);
}

// **************************************************************************************
//
// **************************************************************************************
function getScreenWidth()
{
	return g_screenWidth;
}
function getScreenHeight()
{
	return g_screenHeight;
}

// **************************************************************************************
// log
// **************************************************************************************
function log(str)
{
	console.log(str);
}

// **************************************************************************************
// onClick
// **************************************************************************************
function onEngineClick(event)
{
  var posX = event.pageX - g_canvas.offsetLeft;
  var posY = event.pageY - g_canvas.offsetTop;
	onClick(posX, posY);
}
// **************************************************************************************
// KeyDown
// **************************************************************************************
function onEngineKeyDown(event)
{
	onKeyDown(event.keyCode);
}
// **************************************************************************************
// KeyUp
// **************************************************************************************
function onEngineKeyUp(event)
{
	onKeyUp(event.keyCode);
}

// **************************************************************************************
//
// **************************************************************************************
function getImage(name)
{
	return document.getElementById(name);
}

// **************************************************************************************
//
// **************************************************************************************
function  getSound(name)
{
	return document.getElementById(name);
}

// **************************************************************************************
//
// **************************************************************************************
function engineLoop()
{
	clearScreen();
	gameLoop();
}

// **************************************************************************************
// Clear Screen
// **************************************************************************************
function clearScreen()
{
	g_ctx.fillStyle = '#99D9EA'; 
	g_ctx.fillRect(0, 0, g_screenWidth, g_screenHeight);
}

// **************************************************************************************
// Pintado de una imagen
// **************************************************************************************
function drawImage(image, posX, posY)
{
	this.g_ctx.drawImage(image, posX - image.width/2, posY - image.height/2);
}

// **************************************************************************************
// Pintado de una imagen
// **************************************************************************************
function drawText(text, posX, posY)
{
	g_ctx.fillStyle = 'black'; 
	this.g_ctx.fillText(text, posX, posY);
}

// **************************************************************************************
// Reporducción de un sonido
// **************************************************************************************
function playSound(sonido)
{
	sonido.currentTime = 0;
	sonido.play();
}

// **************************************************************************************
// Detección de colisión
// **************************************************************************************
function collide(img1, posx1, posy1, img2, posx2, posy2)
{
	var ret = false;

	var x1Ini = posx1 - img1.width  / 2;
	var x1Fin = x1Ini + img1.width;
	var y1Ini = posy1 - img1.height / 2;
	var y1Fin = y1Ini + img1.height;

	var x2Ini = posx2 - img2.width  / 2;
	var x2Fin = x2Ini + img2.width;
	var y2Ini = posy2 - img2.height / 2;
	var y2Fin = y2Ini + img2.height;
	
	if ((x1Ini < x2Fin) &&
      (x1Fin > x2Ini) &&
      (y1Ini < y2Fin) &&
      (y1Fin > y2Ini))
		ret = true;
		
	return ret;
}

// **************************************************************************************
// Detección de punto dentro de imagen
// **************************************************************************************
function isInside(img, posx, posy, px, py)
{
	var ret = false;
	
	var xIni = posx - img.width  / 2;
	var xFin = xIni + img.width;
	var yIni = posy - img.height / 2;
	var yFin = yIni + img.height;
	
	if ((xIni <= px) &&
			(xFin >= px) &&
			(yIni <= py) &&
			(yFin >= py))
		ret = true;
	
	return ret;
}
