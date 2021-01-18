using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : Node
{
    //Un delegate permite agregarle funciones que cumplan con la firma del delegate (el valor de retorno y los parametros del metodo)
    public delegate bool QuestionDelegate();
    private QuestionDelegate question;
    private Node trueNode;
    private Node falseNode;

    public QuestionNode(QuestionDelegate question, Node trueNode, Node falseNode)
    {
        this.question = question;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public override void Execute()
    {
        if (question())
        {
            trueNode.Execute();
        }
        else
        {
            falseNode.Execute();
        }
    }
}
