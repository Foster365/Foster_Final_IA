using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : INode
{
    //Un delegate permite agregarle funciones que cumplan con la firma del delegate (el valor de retorno y los parametros del metodo)
    public delegate bool QuestionDelegate();
    private QuestionDelegate question;
    private INode trueNode;
    private INode falseNode;

    public QuestionNode(QuestionDelegate question, INode trueNode, INode falseNode)
    {
        this.question = question;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public void Execute()
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
