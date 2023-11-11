
public class ActionNode : INode
{
    //Un delegate permite agregarle funciones que cumplan con la firma del delegate (el valor de retorno y los parametros del metodo)
    public delegate void ActionDelegate();
    private ActionDelegate action;

    //Constructor que recibirá un delegate
    public ActionNode(ActionDelegate action)
    {
        this.action = action;
    }

    public void Execute()
    {
        action();
    }
}
