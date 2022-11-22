using UnityEngine;

public class Flee // : ISteeringBehaviour
{
    public bool move = false;
    public float speed;
    public float rotSpeed;

    Transform character;
    LineOfSight lineOfSight;
    Transform target;
    Vector3 direction;

    public Transform Target { get => target; set => target = value; }

    public Flee(float _speed, float _rotSpeed, Transform _character, LineOfSight _lineOfSight, Transform _target, Vector3 _direction)
    {

        //move = _move;
        speed = _speed;
        rotSpeed = _rotSpeed;

        character = _character;

        lineOfSight = _lineOfSight;
        target = _target;
        direction = _direction;
    }

    public void GetDir() //Void
    {
        target = lineOfSight.Target;

        if (target != null)
        {
            //Consigo el vector entre el objetivo y mi posición, y luego lo niego para alejarme de él.
            Vector3 deltaVector = -(target.transform.position - character.transform.position);
            deltaVector.y = 0;
            //Me guardo la dirección unicamente.
            direction = deltaVector.normalized;

            //Roto mi objeto hacia la dirección obtenida
            character.transform.forward = Vector3.Lerp(character.transform.forward, direction, Time.deltaTime * rotSpeed);
            //Muevo mi objeto
            //character.transform.position += character.transform.forward * speed * Time.deltaTime;
        }

        character.transform.position += character.transform.forward * speed * Time.deltaTime;
    }

}
