# interactivo
docker start -i compi
# usa de base mymono

# compila, corre y borra ejecutable
docker run -v $PWD:/clase -e FILES="hellomono.cs" --rm mymono2
# usa de base mymono2
