using System;
using System.Linq;
using System.Reflection;
using UnitTestProject.Exceptions;

namespace UnitTestProject
{
    public static class AssertionHandler
    {
        public static object Expect(this object assertedObject)
        {
            return assertedObject;
        }

        public static bool Eq(this object actual, object expected)
        {
            if (actual.Equals(expected))
                return true;
            
            throw new ExpectationFailedException();
        }
        
        public static bool Eq(this Tuple<object, bool> actual, object expected)
        {
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

        public static bool IsGreater(this object actual, object expected)
        {
            if (Convert.ToInt32(actual) > Convert.ToInt32(expected))
                return true;
            
            throw new ExpectationFailedException();
        }
        
        public static Tuple<object, bool> Not(this object assertedValue)
        {
            return new Tuple<object, bool>(assertedValue, false);
        }

        public static bool RaiseError(this object objectToAssert)
        {
            var action = objectToAssert as Action;
            
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

        public static Tuple<PropertyInfo[], object> Properties(this object actual)
        {
            return new Tuple<PropertyInfo[], object>(actual.GetType().GetProperties(), actual);
        }

        public static Tuple<PropertyInfo[], object> PropertiesWithout(this object actual,
            Func<dynamic, string> lambda)
        {
            var specifiedProperties = actual.GetType().GetProperties().Where(x => x.Name != lambda(actual));
            return new Tuple<PropertyInfo[], object>(specifiedProperties.ToArray(), actual);
        }
        
        public static Tuple<PropertyInfo[], object> PropertiesWithout(this object actual,
            string propertyName)
        {
            var specifiedProperties = actual.GetType().GetProperties().Where(x => x.Name != propertyName);
            return new Tuple<PropertyInfo[], object>(specifiedProperties.ToArray(), actual);
        }
    }
}
