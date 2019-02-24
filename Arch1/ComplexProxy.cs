using System;

using System.Dynamic;


namespace Arch1
{
    public class ComplexProxy : DynamicObject
    {
        private readonly Complex origin;
        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = origin.GetType().GetProperty(binder.Name).GetValue(origin);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            switch (binder.Name)
            {
                case "Real":
                case "Imaginary":
                    throw new Exception("Attempted assignment of readonly property. Throwed by proxy.");
                default:
                    break;
            }
           
            try
            {
                var property = origin.GetType().GetProperty(binder.Name);

                property.SetValue(origin, value);

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                result = origin.GetType().GetMethod(binder.Name).Invoke(origin, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public ComplexProxy(Complex obj)
        {
            origin = obj;
        }
       
    }
}
