// ********************************************************************************************
// Detecci�n de colisiones y sonidos.
//
// API:
// + Funci�n "cc.audioEngine.playMusic": pone como m�sica de fondo presente en el fichero pasado. Como segundo
//     valor se le dice si se tiene que poner en bucle o s�lo una vez.
// + Funci�n "cc.audioEngine.playEffect: pone el sonido presente en el fichero pasado.
//
// + Los objetos de tipo nodo (escenas, sprites, ...) tienen:
//   - Funci�n "boundingBox": retorna el rect�ngulo alrededor del sprite. 
//
// + Funci�n "cc.rectIntersectsRect": se le pasan dos rect�ngulos y retorna "true" si interseccionan
//   o false si no lo hacen
// + Funci�n "cc.rectContainsPoint": se le pasa un rect�ngulo y un punto y retorna true si el punto est�
//   dentro del rect�ngulo y false si no lo est�.
//
// + En Javascript podemos generara un n�mero aleatorio entre 0 y 1 con la funci�n "Math.random"
// + En Javascript podemos redondear hacia abajo un n�mero con la funci�n "Math.floor".
// + EN Javascript podemos generar un n�mero entero entre 0 y "n - 1": Math.floor(Math.random() * n)
// ********************************************************************************************

function runEjercicio()
{
  var spritePlanes = [];
  var myScene;
  var updateFunction = function (sprite, perSecondPixelsUp, dt) {
    var ypos = sprite.getPositionY();
    ypos = ypos + perSecondPixelsUp * dt;
    sprite.setPositionY(ypos);

    if (cc.director.getWinSize().height + 20 < ypos) {
      sprite.removeFromParent();
      sprite.unscheduleUpdate();
    }
  }
  
  // Programamos la funci�n que se ejecutar� cuando el juego termina de arrancar
  cc.game.onStart = function()
    {
      myScene = cc.Scene.create();
      var bg = cc.LayerColor.create();
      bg.changeWidthAndHeight(cc.director.getWinSize().width, cc.director.getWinSize().height);
      bg.setColor(cc.color(200, 200, 200, 255 / 2));
      myScene.addChild(bg);
      var size = cc.director.getWinSize();
      var timeCounter = 1;
      myScene.scheduleUpdate();
      myScene.update = function (dt) {
        timeCounter = timeCounter + dt;

        // Spawn plane every second
        if (timeCounter > 1) {
          console.log("Creating plane");
          timeCounter = 0;

          var spritePlane = cc.Sprite.create("plane.png");
          spritePlane.setPosition(size.width * Math.random(), 0);
          spritePlane.update = updateFunction.bind(this, spritePlane, Math.random() * (1000 - 200) + 200);
          spritePlane.scheduleUpdate();
          myScene.addChild(spritePlane);
          spritePlanes.push(spritePlane);
        }
      }

      cc.audioEngine.playMusic("Ambiente.mp3", true);
    
      // Le decimos al "director" que ejecute la escena creada.
      cc.director.runScene(myScene);
    }
    
  // *********************************************************************
  // EVENTOS DE RAT�N
  // *********************************************************************
  var listenerMouse = cc.EventListener.create(
    {
	    event: cc.EventListener.MOUSE,
	    onMouseMove: function(event)
        {        
        },
	    onMouseUp: function(event)
        {
        },
	    onMouseDown: function (event) {
	      var removeElements = [];
	      spritePlanes.forEach(function (sprite, index) {
	        var rect = sprite.boundingBox();
	        if (sprite.removeFromParent && cc.rectContainsPoint(rect, event.getLocation())) {
	          cc.audioEngine.playEffect("bullet.mp3");
	          sprite.removeFromParent();
	          removeElements.push(index);
	        }
	      });

	      removeElements.forEach(function (element) {
	        spritePlanes.splice(element, 1);
	      });
      },
    });  
    
    cc.eventManager.addListener(listenerMouse, 1);
  // Arrancamos el juego
  cc.game.run("gameCanvas");
}

// ****************************************************************************
// EJERCICIO:
// Hacer un juego que:
// 1.- Salga aviones en la pantalla por la parte de abajo en posiciones aleatorias y que se muevan
//     hacia arriba en la pantalla con velocidades aleatorias.
// 2.- Si el jugador hace click sobre un avi�n se produzca un sonido y el avi�n desaparezca.
// ****************************************************************************
