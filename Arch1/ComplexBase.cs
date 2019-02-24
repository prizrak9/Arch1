namespace Arch1
{
    abstract class ComplexBase
    {
        protected readonly double real;
        protected readonly double imaginary;

        protected ComplexBase(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }
    }
}
