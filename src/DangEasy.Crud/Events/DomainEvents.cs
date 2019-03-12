using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DangEasy.Crud.Events
{
    // http://udidahan.com/2009/06/14/domain-events-salvation/
    // https://benfoster.io/blog/deferred-domain-events
    // https://enterprisecraftsmanship.com/2017/10/03/domain-events-simple-and-reliable-solution/

    public interface IDomainEvent { }

    public interface IHandles<TEvent> where TEvent : IDomainEvent
    {
        EventResult Handle(TEvent args);
    }

    public static class DomainEvents
    {
        [ThreadStatic]
        private static List<IHandles<IDomainEvent>> Container;

        public static void Register<T>(IHandles<IDomainEvent> handler) where T : IDomainEvent
        {
            if (Container == null)
            {
                Container = new List<IHandles<IDomainEvent>>();
            }

            Container.Add(handler);
        }

        public static void Remove(Type type)
        {
            if (Container != null)
            {
                Container = Container.Where(x => x.GetType() == type).ToList();
            }
        }


        public static void Clear()
        {
            Container = null;
        }


        //Raises the given domain event
        public static EventResult Raise<TEvent>(TEvent args) where TEvent : IDomainEvent
        {
            if (Container != null)
            {
                foreach (var handler in Container)
                {
                    if (handler is TEvent)
                    {
                        return handler.Handle(args);
                    }
                }
            }

            return new EventResult { ExitMethod = false };
        }
    }
}
