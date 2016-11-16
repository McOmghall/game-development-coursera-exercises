/**
* El juego descrito como objeto javascript
*/
const Game = function Game () {
	// **************************************************************************************
	// El avión
	// **************************************************************************************
	const plane = new Plane({x: 50, y: 50}, 300, this)
	// **************************************************************************************
	// Control de elementos de debug del juego
	// **************************************************************************************
	const isGameDebugOn = true
	const debugZone = document.createElement('div')
	debugZone.className = 'debug-zone'
	const timeStart = Date.now()
	var frames = 0
	var timeOfLastFrame = timeStart
	const defaultGameLoopDebugActions = function defaultGameLoopDebugActions () {
		timeOfLastFrame = Date.now()
		frames += 1
		debugZone.innerHTML = 'FPS: ' + (frames * 1000 / (timeOfLastFrame - timeStart)).toFixed(3)
		debugZone.innerHTML += '<br>plane: ' + JSON.stringify(plane)
	}
	const gameLoopDebug = (isGameDebugOn ? defaultGameLoopDebugActions : function () {})
	
	// **************************************************************************************
	// Initialization.
	// **************************************************************************************
	this.Init = function Init () {
		plane.image = getImage('plane')
		
		if (isGameDebugOn) {
			document.body.appendChild(debugZone)
		}
	}

	// **************************************************************************************
	// Game loop.
	// **************************************************************************************
	this.gameLoop = function gameLoop () {
		const diffLastFrameSeconds = (Date.now() - timeOfLastFrame) / 1000
		plane.movePlane(diffLastFrameSeconds)
		drawImage(plane.image, plane.coords.x, plane.coords.y)
		
		gameLoopDebug()
	}
}

// **************************************************************************************
// El avión
// **************************************************************************************
const Plane = function Plane (startXY, speedInPixelsPerSecond, game) {
	this.coords = {
		x : startXY.x,
		y : startXY.y
	}
	
	const coordList = [{x: 50, y: 50}, {x: 750, y: 50}, {x: 750, y: 550}, {x: 50, y: 550}, {x: 50, y: 50}, {x: 400, y: 300}]
	var coordListStep = 0
	this.computeMovementDirection = function () {
		const distance = {x: coordList[coordListStep].x - this.coords.x, y: coordList[coordListStep].y - this.coords.y}
		const magnitude = Math.sqrt(distance.x * distance.x + distance.y * distance.y)
		var rval = {x: 0, y: 0, magnitude: 0}
		if (magnitude != 0) {
			rval = {x: distance.x / magnitude, y: distance.y / magnitude, magnitude: magnitude}
		}
		return rval
	}
	
	this.speedInPixelsPerSecond = speedInPixelsPerSecond
	this.movePlane = function (timeDiff) {
		const movement = this.speedInPixelsPerSecond * timeDiff
		const movementDirection = this.computeMovementDirection()
		
		if (movementDirection.magnitude < 4) {
			this.coords.x = coordList[coordListStep].x
			this.coords.y = coordList[coordListStep].y
			coordListStep = (coordListStep == coordList.length - 1 ? coordListStep : coordListStep + 1)
		} else {
			this.coords.x += movementDirection.x * movement
			this.coords.y += movementDirection.y * movement
		}
	}
}

/*
* Declaración de globales para engine.js
*/
const program = new Game()
const Init = program.Init
const gameLoop = program.gameLoop