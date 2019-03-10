using System;
using System.Collections.Generic;
using System.ComponentModel;
using DangEasy.Crud.Interfaces;
using DangEasy.Interfaces.Database;

namespace DangEasy.Crud.Events
{
    //http://udidahan.com/2009/06/14/domain-events-salvation/

    public interface IDomainEvent { }

    public interface Handles<T> where T : IDomainEvent
    {
        ICrudResponse Handle(T args);
    }


    public static class DomainEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static List<Delegate> actions;
        public static IContainer Container { get; set; } //as before

        //Registers a callback for the given domain event
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new List<Delegate>();
            }

            actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            actions = null;
        }

        //Raises the given domain event
        public static ICrudResponse Raise<T>(T args) where T : IDomainEvent
        {
            ICrudResponse response = null;
            if (Container != null)
            {
                foreach (var handler in Container.Components)
                {
                    var h = handler as Handles<T>;
                    response = h.Handle(args);
                }
            }
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }

            return response;
        }
    }


    public class CreateConflictEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public TEntity Entity { get; set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }

}
