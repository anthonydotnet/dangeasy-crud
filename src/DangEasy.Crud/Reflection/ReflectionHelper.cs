using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DangEasy.Crud.Attributes;

namespace DangEasy.Crud.Reflection
{
    public static class ReflectionHelper
    {
        public static string TryGetIdProperty<T>(Expression<Func<T, object>> idNameFactory)
        {
            Type entityType = typeof(T);
            var properties = entityType.GetProperties();

            // search for idNameFactory
            if (idNameFactory != null)
            {
                var expr = GetMemberExpression(idNameFactory);
                MemberInfo customPropertyInfo = expr.Member;

                return customPropertyInfo.Name;
            }

            // search for id property in entity
            var idProperty = properties.SingleOrDefault(p => p.Name == "id");

            if (idProperty != null)
            {
                return idProperty.Name;
            }

            // search for Id property in entity
            idProperty = properties.SingleOrDefault(p => p.Name == "Id");

            if (idProperty != null)
            {
                return idProperty.Name;
            }

            // identity property not found;
            throw new ArgumentException("Unique identity property not found. Create \"id\" property for your entity or use different property name with JsonAttribute with PropertyName set to \"id\"");
        }


        public static PropertyInfo GetFirstPropertyWith<TEntity>(Type attributeType)
        {
            Type entityType = typeof(TEntity);
            var properties = entityType.GetProperties();

            foreach (var prop in properties)
            {
                var attributes = prop.GetCustomAttributes(attributeType, true);
                if (attributes.Any())
                {
                    return prop;
                }
            }

            return null;
        }



        public static TEntity SetAutoDateProperties<TEntity>(TEntity entity, Type autoDateAttributeType, DateTime date)
        {
            Type entityType = typeof(TEntity);
            var properties = entityType.GetProperties();

            foreach (var prop in properties)
            {
                var attributes = prop.GetCustomAttributes(autoDateAttributeType, true);
                if (attributes.Any())
                {
                    prop.SetValue(entity, date);
                }
            }
            return entity;
        }




        public static void SetValue<T, TV>(string propertyName, T item, TV value)
        {
            MethodInfo method = typeof(T).GetProperty(propertyName).GetSetMethod();
            Action<T, TV> setter = (Action<T, TV>)Delegate.CreateDelegate(typeof(Action<T, TV>), method);
            setter(item, value);
        }




        public static object GetPropertyValue<T>(T entity, string propertyName)
        {
            var p = Expression.Parameter(typeof(T), "x");
            Expression body = Expression.Property(p, propertyName);
            if (body.Type.IsValueType)
            {
                body = Expression.Convert(body, typeof(object));
            }
            var exp = Expression.Lambda<Func<T, object>>(body, p);
            return exp.Compile()(entity);
        }



        private static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expr)
        {
            var member = expr.Body as MemberExpression;
            var unary = expr.Body as UnaryExpression;
            return member ?? (unary != null ? unary.Operand as MemberExpression : null);
        }

    }
}
