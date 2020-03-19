using System;
using System.Linq;
using System.Reflection;
using UnitTestProject.Exceptions;

namespace UnitTestProject
{
    public static class AssertionHandler
    {
        public static Tuple<object, Type> Expect(this object assertedObject)
        {
            var type = assertedObject.GetType();
            return new Tuple<object, Type>(assertedObject, assertedObject.GetType());
        }

        public static bool Eq(this Tuple<object, Type> actual, object expected)
        {
            if (actual.Item1.Equals(expected))
                return true;
            
            throw new ExpectationFailedException();
        }
        
        public static bool Eq(this Tuple<object, bool> actual, object expected)
        {
            var t = actual.Item1;
            
            if (actual.Item1.Equals(expected))
                throw new ExpectationFailedException();
                
            return true;
        }

        public static bool Eq(this Tuple<PropertyInfo[], object> actual, object expected)
        {
            var expectedProperties = expected.GetType().GetProperties();

            for (var i = 0; i < actual.Item1.Length; i++)
            {
                if (expectedProperties[i].GetValue(expected) != actual.Item1[i].GetValue(actual.Item2))
                    throw new ExpectationFailedException();
            }

            return true;
        }

        public static bool IsGreater(this Tuple<object, Type> actual, object expected)
        {
            if (Convert.ToInt32(actual.Item1) > Convert.ToInt32(expected))
                return true;
            
            throw new ExpectationFailedException();
        }
        
        public static Tuple<object, bool> Not(this Tuple<object, Type> assertedValue)
        {
            return new Tuple<object, bool>(assertedValue.Item1, false);
        }

        public static bool RaiseError(this Tuple<object, Type> objectToAssert)
        {
            var action = objectToAssert.Item1 as Action;
            
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                return true;
            }
            
            throw new ExpectationFailedException();
        }

        public static Tuple<PropertyInfo[], object> Properties(this Tuple<object, Type> actual)
        {
            return new Tuple<PropertyInfo[], object>(actual.Item1.GetType().GetProperties(), actual.Item1);
        }

        public static Tuple<PropertyInfo[], object> PropertiesWithout(this Tuple<object, Type> actual,
            Func<dynamic, string> lambda)
        {
            var specifiedProperties = actual.Item1.GetType().GetProperties().Where(x => x.Name != lambda(actual.Item1));
            return new Tuple<PropertyInfo[], object>(specifiedProperties.ToArray(), actual.Item1);
        }
        
        public static Tuple<PropertyInfo[], object> PropertiesWithout(this Tuple<object, Type> actual,
            string propertyName)
        {
            var specifiedProperties = actual.Item1.GetType().GetProperties().Where(x => x.Name != propertyName);
            return new Tuple<PropertyInfo[], object>(specifiedProperties.ToArray(), actual.Item1);
        }
    }
}
