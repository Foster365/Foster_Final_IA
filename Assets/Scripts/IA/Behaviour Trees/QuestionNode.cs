using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : BehaviourTreeNode
{
    //Un delegate permite agregarle funciones que cumplan con la firma del delegate (el valor de retorno y los parametros del metodo)
    public delegate bool QuestionDelegate();
    private QuestionDelegate question;
    private BehaviourTreeNode trueNode;
    private BehaviourTreeNode falseNode;

    public QuestionNode(QuestionDelegate question, BehaviourTreeNode trueNode, BehaviourTreeNode falseNode)
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
