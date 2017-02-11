// ********************************************************************************************
// Una escena con un sprite en el medio de la misma
// API:
//  + Objeto "cc.game":
//    - Variable "onStart": contiene la funci�n que se ejecutar� cuando el juego arranque
//    - Funci�n "run": se le llama para arrancar el juego.
//
//  + Funci�n "cc.Scene.create":      crea una nodo escena nuevo y lo retorna.
//  + Funci�n "cc.LayerColor.create": crea un nodo "layout" nuevo con el color indicado y lo retorna.
//  + Funci�n "cc.color":             crea un color con las cantidades RGB indicadas y lo retorna
//
//  + Objeto "cc.director":
//    - Funci�n "runScene":  pone como activa la escena pasada.
//    - Funci�n "getWinSize": retorna un objeto con las propiedades "width" y "height"
//      conteniendo las dimensiones de la pantalla del juego.
//
//  + Funci�n "cc.Sprite.create": crea un nodo de tipo "sprite" y lo retorna.
//
//  + Los objetos de tipo nodo (escenas, sprites, ...) tienen:
//    - Funci�n "addChild":    permite a�adir al nodo un nodo hijo.
//    - Funci�n "setPosition/getPosition":   permite establecer/consultar la posici�n del nodo en relaci�n a su padre
//    - Funci�n "setPositionX/getPositionX": permite establecer/consultar la posici�n "x" del nodo en relaci�n a su padre
//    - Funci�n "setPositionY/getPositionY": permite establecer/consultar la posici�n "y" del nodo en relaci�n a su padre
//    - Funci�n "setScale/getScale":         permite establecer/consultar el escalado en "x" e "y" del nodo.
//    - Funci�n "setScaleX/getScaleX":       permite establecer/consultar el escalado en "x" del nodo.
//    - Funci�n "setScaleY/getScaleY":       permite establecer/consultar el escalado en "y" del nodo.
//    - Funci�n "setRotation/getRotation":   permite establecer/consultar el giro de un nodo sobre el eje "z".
//    - Funci�n "setOpacity/getOpcaity":     permite establecer/consultar el grado de transparencia de un nodo.
// ********************************************************************************************

function runEjercicio() {
  var spritePlanes = [];
  
  // Programamos la funci�n que se ejecutar� cuando el juego termina de arrancar
  cc.game.onStart = function() {
      // Creamos la escena y guardamos una referencia en la variable "myScene"
      // Llamaos a la funci�n del "director" para conseguir y guardamos el tama�o de la pantalla
      var myScene = cc.Scene.create();
      var size = cc.director.getWinSize();
                    
      var bg = cc.LayerColor.create();
      bg.changeWidthAndHeight(size.width, size.height);
      bg.setColor(cc.color(200, 200, 200, 255 / 2));
      myScene.addChild(bg);

      // Avi�n 1
      // Creamos el sprite del avi�n
      // Lo posicionamos en medio de la pantalla.
      // Lo a�adimos como hijo de las escena.
      var planeSprite = "plane.png"
      var plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, (size.height * 3) / 5);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avi�n doble grande (EJERCICIO 1)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, (size.height * 4) / 5);
      plane.setScale(2);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avi�n mitad grande (EJERCICIO 1)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, (size.height * 2) / 5);
      plane.setScale(0.5);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avi�n mitad grande (EJERCICIO 2)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition((size.width * 2) / 5, (size.height * 3) / 5);
      plane.setRotation(90);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avi�n mitad grande (EJERCICIO 3)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition((size.width * 3) / 5, (size.height * 3) / 5);
      plane.setRotation(-45);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avi�n mitad grande (EJERCICIO 4)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, size.height / 5);
      plane.setOpacity(plane.getOpacity() / 2);
      myScene.addChild(plane);
      spritePlanes.push(plane);
      
      // Le decimos al "director" que ejecute la escena creada.
      cc.director.runScene(myScene);
    }
  
  // Arrancamos el juego
  cc.game.run("gameCanvas");
}

// ****************************************************************************
// EJERCICIO:
// Modificar el programa para que adem�s de lo que se pinta ahora se muestre en la pantalla:
// 1.- Un avi�n el doble de grande y otro avi�n la mitad de peque�o
// 2.- Un avi�n girado 90 grados en el sentido de la agujas del reloj.
// 3.- Un avi�n girado 45 grados en el sentido contrario de la agujas del reloj.
// 4.- Un avi�n semitransparente.
// 5.- Poner a la escena un color de fondo usando una nodo de tipo LayerColor.
// ****************************************************************************