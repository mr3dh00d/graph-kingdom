:'
class PlayerStats(models.Model):
    player=models.CharField(max_length=10)
    session=models.CharField(max_length=36)
    level=models.CharField(max_length=36)
    action=models.IntegerField()
    time=models.FloatField()
    state=models.TextField()
    timestamp=models.DateTimeField(auto_now_add=True)

player    identificador unico del player, ideal rut
session   identificador unico de la session de juego, usar UUID
level     identificador unico del nivel, usar nombre juego:número nivel
action    accion realizada 0: iniciar nivel, 1: resetear nivel, 2: terminar               nivel
time      tiempo en ms del reloj del juego en que ocurrió la acción
state     estado del juego al realizar la acción, por ejemplo guardar las vidas           o puntaje del jugador u otro dato que pueda servir para analizar
timestamp fecha y hora de la acción, yyyymmdd hh:mm:ss'

curl -X POST https://dashboard.gemusei.org/playerstat/ -H "Content-Type: application/json" -d '{"player": "Pablo", "session": "UUID", "level":"nivel", "action":0,"time":100,"state":"Solución usada u otro dato relevante", "timestamp":"20221107 09:44"}' 
