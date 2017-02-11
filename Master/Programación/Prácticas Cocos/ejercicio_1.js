// ********************************************************************************************
// Una escena con un sprite en el medio de la misma
// API:
//  + Objeto "cc.game":
//    - Variable "onStart": contiene la función que se ejecutará cuando el juego arranque
//    - Función "run": se le llama para arrancar el juego.
//
//  + Función "cc.Scene.create":      crea una nodo escena nuevo y lo retorna.
//  + Función "cc.LayerColor.create": crea un nodo "layout" nuevo con el color indicado y lo retorna.
//  + Función "cc.color":             crea un color con las cantidades RGB indicadas y lo retorna
//
//  + Objeto "cc.director":
//    - Función "runScene":  pone como activa la escena pasada.
//    - Función "getWinSize": retorna un objeto con las propiedades "width" y "height"
//      conteniendo las dimensiones de la pantalla del juego.
//
//  + Función "cc.Sprite.create": crea un nodo de tipo "sprite" y lo retorna.
//
//  + Los objetos de tipo nodo (escenas, sprites, ...) tienen:
//    - Función "addChild":    permite añadir al nodo un nodo hijo.
//    - Función "setPosition/getPosition":   permite establecer/consultar la posición del nodo en relación a su padre
//    - Función "setPositionX/getPositionX": permite establecer/consultar la posición "x" del nodo en relación a su padre
//    - Función "setPositionY/getPositionY": permite establecer/consultar la posición "y" del nodo en relación a su padre
//    - Función "setScale/getScale":         permite establecer/consultar el escalado en "x" e "y" del nodo.
//    - Función "setScaleX/getScaleX":       permite establecer/consultar el escalado en "x" del nodo.
//    - Función "setScaleY/getScaleY":       permite establecer/consultar el escalado en "y" del nodo.
//    - Función "setRotation/getRotation":   permite establecer/consultar el giro de un nodo sobre el eje "z".
//    - Función "setOpacity/getOpcaity":     permite establecer/consultar el grado de transparencia de un nodo.
// ********************************************************************************************

function runEjercicio() {
  var spritePlanes = [];
  
  // Programamos la función que se ejecutará cuando el juego termina de arrancar
  cc.game.onStart = function() {
      // Creamos la escena y guardamos una referencia en la variable "myScene"
      // Llamaos a la función del "director" para conseguir y guardamos el tamaño de la pantalla
      var myScene = cc.Scene.create();
      var size = cc.director.getWinSize();
                    
      var bg = cc.LayerColor.create();
      bg.changeWidthAndHeight(size.width, size.height);
      bg.setColor(cc.color(200, 200, 200, 255 / 2));
      myScene.addChild(bg);

      // Avión 1
      // Creamos el sprite del avión
      // Lo posicionamos en medio de la pantalla.
      // Lo añadimos como hijo de las escena.
      var planeSprite = "plane.png"
      var plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, (size.height * 3) / 5);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avión doble grande (EJERCICIO 1)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, (size.height * 4) / 5);
      plane.setScale(2);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avión mitad grande (EJERCICIO 1)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition(size.width / 2, (size.height * 2) / 5);
      plane.setScale(0.5);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avión mitad grande (EJERCICIO 2)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition((size.width * 2) / 5, (size.height * 3) / 5);
      plane.setRotation(90);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avión mitad grande (EJERCICIO 3)
      plane = cc.Sprite.create(planeSprite);
      plane.setPosition((size.width * 3) / 5, (size.height * 3) / 5);
      plane.setRotation(-45);
      myScene.addChild(plane);
      spritePlanes.push(plane);

      // Avión mitad grande (EJERCICIO 4)
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
// Modificar el programa para que además de lo que se pinta ahora se muestre en la pantalla:
// 1.- Un avión el doble de grande y otro avión la mitad de pequeño
// 2.- Un avión girado 90 grados en el sentido de la agujas del reloj.
// 3.- Un avión girado 45 grados en el sentido contrario de la agujas del reloj.
// 4.- Un avión semitransparente.
// 5.- Poner a la escena un color de fondo usando una nodo de tipo LayerColor.
// ****************************************************************************