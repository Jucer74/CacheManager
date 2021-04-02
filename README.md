# Introducción
Al momento de mejorar el rendimiento de nuestra aplicación, uno de los puntos que pensamos a primera instancia es en almacenar datos en Caché, por esto veremos brevemente el patrón Cache-Aside Pattern y su implementación usando .Net Core. para ayudarnos a resolver nuestro problema.


# Cache-Aside Pattern
Las aplicaciones utilizan el caché para mejorar el acceso repetitivo a los datos y no tener que estar realizando la consulta de los mismos a su fuente de información principal.

Este patrón es muy sencillo y simple. Cuando necesitamos datos los buscamos en el caché y si no estan ahí, los obtenemos de la fuente, los adicionamos al caché y luego los devolvemos. De esta forma la proxima vez que realicemos la consulta, los datos ya se encontrarán en el caché. 

![](https://github.com/Jucer74/CacheManager/blob/main/Images/cache-aside-diagram.png)

1. Validamos si el dato que necesitamos esta en el caché
2. Si No esta vamos a nuestra fuente y lo obtenemos
3. Lo guardamos en el caché y lo retornamos

Al adicionar al caché, debemos determinar cuanto tiempo deben almacenarse los datos en el caché o cuando deben refrescarse. A continuación vemos el algoritmo para clarificar un poco mas el patrón.

![](https://github.com/Jucer74/CacheManager/blob/main/Images/Cache-Aside-Pattern-Algorithm.png)

# Implementación
Nuestra solución consta de tres (3) proyectos:

![](https://github.com/Jucer74/CacheManager/blob/main/Images/Cache-Aside-Pattern-Solution.png)

1. **CacheStore**: Contiene la implementación simple del manejo del caché.
2. **CacheManager**: Contiene la implementación mejorada de este patrón.
3. **CacheWebApi**: Contiene una Web API en donde se muestra el uso de ambas librerías.

## Recursos
Se tiene una Base de datos de tipo SQLite como fuente de datos, que contiene la lista de paises del mundo segun el estandar [ISO-3166-1](https://www.iso.org/iso-3166-country-codes.html).




  
