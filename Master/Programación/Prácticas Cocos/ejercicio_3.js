// ********************************************************************************************
// Escena que detecta eventos de teclado y de ratón
//
// API:
// + Función "cc.EventListener.create": crea un observador de eventos. Se le pasa un objeto
//   que tiene el tipo de eventos a escuchar y las funciones que serán llamadas cuando se
//   se produzcan dichos eventos.
// + Función "cc.eventManager.addListener": añade un observador al gestor de eventos para que
//   le avise cuando estos se produzcan. Adicionalmente se le pasa la prioridad con la que se le 
//   tiene que llamar.
// ********************************************************************************************

function runEjercicio()
{
  var spritePlanes = [];
  var myScene;
  var planeSprite = "plane.png"

  // MOVE A PLANE FUNCTION
  var updateFunction = function (sprite, perSecondPixelsUp, dt) {
    var ypos = sprite.getPositionY();
    ypos = ypos + perSecondPixelsUp * dt;
    sprite.setPositionY(ypos);

    if (cc.director.getWinSize().height + 20 < ypos) {
      sprite.removeFromParent();
      sprite.unscheduleUpdate();
    }
  }
  
  // Programamos la función que se ejecutará cuando el juego termina de arrancar
  cc.game.onStart = function()
    {
      // Creamos la escena y guardamos una referencia en la variable "myScene"
      myScene = cc.Scene.create();
      var bg = cc.LayerColor.create();
      bg.changeWidthAndHeight(cc.director.getWinSize().width, cc.director.getWinSize().height);
      bg.setColor(cc.color(200, 200, 200, 255 / 2));
      myScene.addChild(bg);
                    
      // Le decimos al "director" que ejecute la escena creada.
      cc.director.runScene(myScene);
    }

  // *********************************************************************
  // EVENTOS DEL TECLADO
  // *********************************************************************
  var listenerKeyboard = cc.EventListener.create(
    {
      event: cc.EventListener.KEYBOARD,
      onKeyPressed:  function(keyCode, event) {
        console.log("Tecla pulsada: " + keyCode);
        if (keyCode == 27) { //ESC
          spritePlanes.forEach(function (sprite) {
            if (sprite.removeFromParent) {
              sprite.removeFromParent();
            }
          });
          spritePlanes = [];
        }
      },
      onKeyReleased: function (keyCode, event) {
        console.log("Tecla soltada: " + keyCode);
      }
    });
                      
  cc.eventManager.addListener(listenerKeyboard, 1);
    
  // *********************************************************************
  // EVENTOS DE RATÓN
  // *********************************************************************
  var listenerMouse = cc.EventListener.create(
    {
	    event: cc.EventListener.MOUSE,
	    onMouseMove: function(event) {        
      },
	    onMouseUp: function (event) {
	      console.log("Mouse button up: " + event.getButton() + " (" + event.getLocationX() + ", " + event.getLocationY() + ")");
      },
	    onMouseDown: function (event) {
	      console.log("Mouse button down: " + event.getButton() + " (" + event.getLocationX() + ", " + event.getLocationY() + ")");
	      if (event.getButton() == 0) {
	        var plane = cc.Sprite.create(planeSprite);
	        plane.setPosition(event.getLocationX(), event.getLocationY());
	        myScene.addChild(plane);
	        spritePlanes.push(plane);
	        plane.scheduleUpdate();
	        plane.update = updateFunction.bind(this, plane, 200);
	      }
      }
    });  
    
    cc.eventManager.addListener(listenerMouse, 1);
  // Arrancamos el juego
  cc.game.run("gameCanvas");
}

// ****************************************************************************
// EJERCICIO:
// Modificar el programa para que:
// 1.- Cada vez que se pulse el botón izquierdo del ratón se pinte en esa posición
//     un avión nuevo.
// 2.- Los aviones pintados deberán moverse hacia arriba hasta desaparecer de la pantalla.
//     Momento en el que deberán ser eliminados de la escena. Mirar la documentación
//     de cocos para ver cómo se puede eliminar un nodo.
// 3.- Cuando se pulse la tecla "ESC" de eliminarán todos los aviones presentes en la escena y se
//     secará un mensaje en rojo en mitad de la pantalla indicando fin de partida.
//     Mirar en la documentación el funcionamiento del nodo de tipo "LabelTTF" para sacar el mensaje.
//     Usar la función create de "LabelTTF" para crear un nodo nuevo.
// ****************************************************************************
