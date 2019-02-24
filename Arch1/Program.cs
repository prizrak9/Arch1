using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ImpromptuInterface;


namespace Arch1
{

    class Program
    {
        static void Main(string[] args)
        {
            var c = new Complex(-1, 5);

            PrintRepresentations(c);

            InvokeMethods(c);

            DescribeType(c);
            //DescribeType(new ComplexProxy(c));

            ProxyTest();
           
            Console.ReadKey();
        }

        private static void PrintRepresentations(Complex c)
        {
            Console.WriteLine("-----------------Representations start--------------");
            Console.WriteLine("Complex form:");
            Console.WriteLine(c.ToComplexForm());
            Console.WriteLine("Exponential form:");
            Console.WriteLine(c.ToExponentialForm());
            Console.WriteLine("Exponential full form:");
            Console.WriteLine(c.ToExponentialForm(Form.Full));
            Console.WriteLine("-----------------Representations end--------------");
        }

        private static void ProxyTest()
        {
            Console.WriteLine(@"
----------Proxy test--------------------
Property 'Real' is but a crutch for demonstration purpose only.
Real value is stored in field 'real' which is protected readonly.
For IntelliSense support we will use ImpromptuInterface
that allows us to use types with behaviours of chosen interfaces.

Test scenario:
1)create original class
2)read value directly
3)write value directly
4)create proxy
5)read value via dynamic
6)write value via dynamic
7)create proxy via ImpromptuInterface ActLike<T>()
8)read value directly
9)write value directly
---------------Start test-----------------------
");
            Console.WriteLine("1)");
            dynamic origin = new Complex(0, 7);

            Console.WriteLine("2)");
            Console.WriteLine(origin.Real);

            Console.WriteLine("3)");
            try
            {
                Console.WriteLine(origin.Real = 5);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("4)");
            dynamic proxy = new ComplexProxy(new Complex(0, 7));

            Console.WriteLine("5)");
            try
            {
                Console.WriteLine(proxy.Real);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("6)");
            try
            {
                Console.WriteLine(proxy.Real = 5);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("7)");
            var impromptuProxy = new ComplexProxy(new Complex(0, 7)).ActLike<IComplex>();

            Console.WriteLine("8)");
            try
            {
                Console.WriteLine(impromptuProxy.Real);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("9)");
            try
            {
                Console.WriteLine(impromptuProxy.Real = 5);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("------------------End test---------------------");
        }

        private static void InvokeMethods(object o)
        {
            Console.WriteLine("-----------------Reflection invocation start--------------");

            var methods = o.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(n => n.GetCustomAttributes(true).Any(m => m is MyAttribute));


            foreach (var method in methods)
            {
                if (method.GetParameters().Count() > 0)
                {
                    Console.WriteLine($"method '{method.Name}' has parameters thus invoking with default values.");

                    var res = method.Invoke(o, new object[method.GetParameters().Count()]);

                    Console.WriteLine(res);

                    Console.WriteLine();

                    continue;
                }

                Console.WriteLine($"invoking '{method.Name}'");

                if (method.ReturnType == typeof(void))
                {
                    method.Invoke(o, null);
                }
                else
                {
                    Console.WriteLine($"result = {method.Invoke(o, null)}");
                }

                Console.WriteLine();

            }

            Console.WriteLine("-----------------Reflection invocation end--------------");
        }

        private static void DescribeType(object o)
        {
            Console.WriteLine("-----------------Reflection type description start--------------");

            var type = o.GetType();

            Console.WriteLine($"assembly qualified name: {type.AssemblyQualifiedName}");
            Console.WriteLine();
            
            Console.WriteLine($"{(type.IsClass ? "class" : type.IsInterface ? "interface" : "struct")} {type.Name} : {type.BaseType.Name}");
            Console.WriteLine($"{{\n{string.Join("", MethodsToString(type).Select(n=>"\t" + n))}\n}}");

            Console.WriteLine("-----------------Reflection type description end--------------");

        }

        private static IEnumerable<string> MethodsToString(Type @class)
        {
            var buffer = new List<string>();

            foreach (var method in @class.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attributes = method.GetCustomAttributes(true).Select(n => n.GetType());

                foreach (var attribute in attributes)
                {
                    a(AttributeToString(attribute));
                    e();
                }

                var parameters = method.GetParameters().Select(n => (n.ParameterType, n.Name, n.HasDefaultValue, n.DefaultValue));

                var str = parameters.Select(n => $"{n.ParameterType.Name} {n.Name}{(n.HasDefaultValue ? $" = {n.DefaultValue}":string.Empty)}");

                var access = method.IsPrivate ? "private" : method.IsPublic ? "public" : "other";

                var methodString = $"{access} {method.ReturnType.Name} {method.Name}({string.Join(", ", str)})";

                a(methodString);
                e();
                e();
            }

            void a(string s) => buffer.Add(s);
            void e() => buffer.Add("\n");

            return buffer;
        }
       
        private static string AttributeToString(Type attribute)
        {
            var constructors = attribute.GetConstructors();
            var buffer = "";

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters().Select(n => (n.ParameterType, n.Name));

                var parametersString = string.Join(", ", parameters.Select(n => $"{n.ParameterType} {n.Name}"));

                buffer += $"[{attribute.Name}({parametersString})]";
            }

            return buffer;
        }
    }
}
