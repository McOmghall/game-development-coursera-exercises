// ********************************************************************************************
// Escena de un sprite movi�ndose por la pantalla
//
// API
//  + Los objetos de tipo nodo (escenas, sprites, ...) tienen:
//    - Funci�n "scheduleUpdate": sirve para indicar que se quiere que el motor llame a 
//      la funci�n almacenada en la variable del nodo "update" en bucle de manera indefinida (game loop).
//    - Variable "update": contiene la funci�n que ser� llamada constantemente si se activa llamando a 
//      la funci�n "scheduleUpdate".
//    - Variable "width": almacena la anchura actual en pixels del nodo.
//    - Variable "height": almacena la altura en pixels del nodo.
// ********************************************************************************************

function runEjercicio() {
  var spritePlane;
  
  // Programamos la funci�n que se ejecutar� cuando el juego termina de arrancar
  cc.game.onStart = function() {
      // Creamos la escena y guardamos una referencia en la variable "myScene"
      var myScene = cc.Scene.create();
    
      // Llamaos a la funci�n del "director" para conseguir y guardamos el tama�o de la pantalla
      var size = cc.director.getWinSize();
                    
      // Creamos el sprite del avi�n
      spritePlane = cc.Sprite.create("plane.png");
      spritePlane.setAnchorPoint(cc.v2f(0, 0));
      
      // Lo posicionamos en medio de la pantalla.
      spritePlane.setPosition(size.width / 2, 0);
      
      // Lo a�adimos como hijo de las escena.
      myScene.addChild(spritePlane, 0);

      // ROTATE A PLANE FUNCTION
      var updateFunction = function (sprite, perSecondDegrees, dt) {
        var rot = sprite.getRotation();
        rot = rot + perSecondDegrees * dt;
        sprite.setRotation(rot);
      }

      // LEFT PLANE
      var planeSmall1 = cc.Sprite.create("plane.png");
      planeSmall1.setPosition(0, 0);
      planeSmall1.setScale(0.5);
      spritePlane.addChild(planeSmall1);
      planeSmall1.scheduleUpdate();
      planeSmall1.update = updateFunction.bind(this, planeSmall1, 90);

      // RIGHT PLANE
      var planeSmall2 = cc.Sprite.create("plane.png");
      planeSmall1.setPosition(61, 0);
      planeSmall2.setScale(0.5);
      spritePlane.addChild(planeSmall2);
      planeSmall2.scheduleUpdate();
      planeSmall2.update = updateFunction.bind(this, planeSmall2, -90);

      // Configuramos el "game loop"
      myScene.scheduleUpdate();
      myScene.update = function(dt) {
          var currPosY = spritePlane.getPositionY();
          currPosY = currPosY + 1;
          if (currPosY > size.height)
          {
            currPosY = 0;
          }
          spritePlane.setPositionY(currPosY);        
        }
      
      // Le decimos al "director" que ejecute la escena creada.
      cc.director.runScene(myScene);
    }
  
  // Arrancamos el juego
  cc.game.run("gameCanvas");
}

// ****************************************************************************
// EJERCICIO:
// Modificar el programa para que:
// 1.- Aparezcan dos aviones la mitad de grandes, uno pegado a la izquierda y otro a la derecha
//     del original que se muevan solos (sin utilizar para ellos "setPosition") junto con el primero.
// 2.- Hacer que los aviones de los lados giren constantemente sobre si mismos uno en el sentido de 
//     las agujas del reloj y el otro en sentido contrario.
// ****************************************************************************
