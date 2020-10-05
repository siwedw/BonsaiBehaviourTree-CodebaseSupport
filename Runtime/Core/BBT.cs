using Bonsai.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bonsai.CodeBased
{
  public static class BBT
  {
    public abstract class NodeContainer
    {
      public abstract BehaviourNode Node { get; }
    }

    public class CompositeContainer : NodeContainer
    {
      private readonly Composite composite;

      internal CompositeContainer(Composite composite)
      {
        this.composite = composite;
      }

      public override BehaviourNode Node => composite;

      public CompositeContainer Children(params NodeContainer[] children)
      {
        List<BehaviourNode> nodes = new List<BehaviourNode>();
        for (int i = 0; i < children.Length; i++)
        {
          NodeContainer node = children[i];
          if (node != null && node.Node != null)
          {
            nodes.Add(node.Node);
          }
        }
        composite.SetChildren(nodes.ToArray());
        return this;
      }
    }

    public class DecoratorContainer : NodeContainer
    {
      private readonly Decorator decorator;

      internal DecoratorContainer(Decorator decorator)
      {
        this.decorator = decorator;
      }

      public override BehaviourNode Node => decorator;

      public DecoratorContainer Child(NodeContainer child)
      {
        if (child != null && child.Node != null)
        {
          decorator.SetChild(child.Node);
        }
        return this;
      }
    }

    public class TaskContainer : NodeContainer
    {
      private readonly Task task;

      public override BehaviourNode Node => task;

      public TaskContainer(Task task)
      {
        this.task = task;
      }
    }

    public static CompositeContainer Composite<TComposite>(Action<TComposite> action = null) where TComposite : Composite
    {
      var result = ScriptableObject.CreateInstance<TComposite>();
      action?.Invoke(result);
      return new CompositeContainer(result);
    }

    public static DecoratorContainer Decorator<TDecorator>(Action<TDecorator> action = null) where TDecorator : Decorator
    {
      var result = ScriptableObject.CreateInstance<TDecorator>();
      action?.Invoke(result);
      return new DecoratorContainer(result);
    }

    public static TaskContainer Task<TTask>(Action<TTask> action = null) where TTask : Task
    {
      var result = ScriptableObject.CreateInstance<TTask>();
      action?.Invoke(result);
      return new TaskContainer(result);
    }
  }
}