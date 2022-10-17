using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS<T>
{
    public delegate bool Satisfies(T curr);
    public delegate List<T> GetNeighbours(T curr);
    public List<T> Run(T start, Satisfies satisfies, GetNeighbours getNeighbours, int watchdog = 50)
    {
        //Creamos un linkeo de hijos-padres
        Dictionary<T, T> parents = new Dictionary<T, T>();
        //Creamos una cola (Una fila, en donde el primero que ingresa es el primero en salir)
        Queue<T> pending = new Queue<T>();
        //Creamos coleccion para tener todos los nodos que ya fueron visitados
        HashSet<T> visited = new HashSet<T>();
        //Agregamos el nodo start a pendientes
        pending.Enqueue(start);
        while (pending.Count != 0)
        {
            watchdog--;
            if (watchdog <= 0) return new List<T>();
            //Sacame el primero de pendientes
            T current = pending.Dequeue();
            //Compurevo si ese nodo me satisface
            if (satisfies(current))
            {
                List<T> path = new List<T>();
                path.Add(current);
                //Mientras que parents contenga la key del ultimo item del path
                while (parents.ContainsKey(path[path.Count - 1]))
                {
                    //se va a agregar al path el padre del ultimo item del path
                    var lastNode = path[path.Count - 1];
                    path.Add(parents[lastNode]);
                }
                //Damos vuelta la coleccion
                path.Reverse();
                //La retornamos
                return path;
            }
            //Agregamos ese nodo a visitados
            visited.Add(current);
            //Obtenemos sus vecinos
            List<T> neighbours = getNeighbours(current);
            for (int i = 0; i < neighbours.Count; i++)
            {
                T item = neighbours[i];
                if (visited.Contains(item) || pending.Contains(item))
                {
                    continue;
                }
                else
                {
                    //Agregamos sus vecinos en pendientes
                    pending.Enqueue(item);
                    //Realizamos el linkeo de sus vecinos con el nodo
                    parents[item] = current;
                }
            }
        }
        return new List<T>();
    }
}
